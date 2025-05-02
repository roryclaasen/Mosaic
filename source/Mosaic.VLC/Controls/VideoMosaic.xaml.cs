// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace Mosaic.Controls;

using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.WinUI.UI;
using CommunityToolkit.WinUI.UI.Controls;
using LibVLCSharp.Shared;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Mosaic.Infrastructure;

public sealed partial class VideoMosaic : UniformGrid
{
    private const int DEFAULTSIZE = 3;

    private readonly DispatcherQueueTimer regenerateGridTimer;
    private readonly DispatcherTimer swapTimer;

    private DateTime lastIntervalChange = DateTime.MinValue;

    public VideoMosaic()
    {
        this.Rows = DEFAULTSIZE;
        this.Columns = DEFAULTSIZE;

        this.InitializeComponent();
        Core.Initialize();

        this.regenerateGridTimer = DispatcherQueue.GetForCurrentThread().CreateTimer();
        this.swapTimer = new DispatcherTimer();
        this.swapTimer.Tick += (_, _) => this.SwapTimerTick();

        this.SizingComplete += this.SizeUpdated;
        this.Loaded += (_, _) => this.regenerateGridTimer.Debounce(this.TriggerResize, TimeSpan.FromMilliseconds(400));
        this.Unloaded += (_, _) => this.Stop();
    }

    private event EventHandler? SizingComplete;

    public MosaicManager? MosaicManager { get; set; }

    public int MosaicWidth
    {
        get => this.Columns;
        set
        {
            this.Columns = value;
            this.regenerateGridTimer.Debounce(this.TriggerResize, TimeSpan.FromMilliseconds(600));
            if (!this.regenerateGridTimer.IsRunning)
            {
                this.regenerateGridTimer.Start();
            }
        }
    }

    public int MosaicHeight
    {
        get => this.Rows;
        set
        {
            this.Rows = value;
            this.regenerateGridTimer.Debounce(this.TriggerResize, TimeSpan.FromMilliseconds(600));
            if (!this.regenerateGridTimer.IsRunning)
            {
                this.regenerateGridTimer.Start();
            }
        }
    }

    public bool IsPlaying { get; private set; } = false;

    public bool IsPaused { get; private set; } = false;

    public bool ShowLabels { get; private set; } = false;

    public bool MuteVideos { get; private set; } = true;

    public TimeSpan IntervalDelay { get; private set; } = TimeSpan.FromSeconds(5);

    public IEnumerable<VideoPlayerTile> Tiles => this.Children.OfType<VideoPlayerTile>();

    public void SetPause(bool pause)
    {
        this.IsPaused = pause;
        foreach (var tile in this.Tiles)
        {
            tile.SetPause(pause);
        }

        if (pause)
        {
            this.swapTimer.Stop();
        }
        else
        {
            this.swapTimer.Start();
        }
    }

    public void SetMute(bool mute)
    {
        this.MuteVideos = mute;
        foreach (var tile in this.Tiles)
        {
            tile.SetMute(mute);
        }
    }

    public void Play()
    {
        if (this.MosaicManager is null)
        {
            throw new Exception("MosaicManager is null");
        }

        if (this.IsPlaying && this.IsPaused)
        {
            this.SetPause(false);
        }

        this.IsPlaying = true;

        var i = 0;
        foreach (var videoTile in this.Tiles)
        {
            if (i++ >= this.MosaicManager.SourceCount)
            {
                break;
            }

            this.MosaicManager.StartTile(videoTile);
        }

        this.swapTimer.Start();
    }

    public void Stop()
    {
        this.IsPaused = false;
        this.IsPlaying = false;

        foreach (var tile in this.Tiles)
        {
            tile.StopVideo();
        }

        this.regenerateGridTimer.Stop();
        this.swapTimer.Stop();

        this.lastIntervalChange = DateTime.MinValue;
    }

    public void SetShowLabels(bool showLabels)
    {
        this.ShowLabels = showLabels;
        foreach (var tile in this.Tiles)
        {
            tile.SetLabelVisibility(showLabels);
        }
    }

    public void TriggerResize()
    {
        var requiredTiles = this.MosaicWidth * this.MosaicHeight;
        var countOfCurrentTiles = this.Tiles.Count();

        if (countOfCurrentTiles == requiredTiles)
        {
            this.SizingComplete?.Invoke(this, EventArgs.Empty);
        }
        else if (countOfCurrentTiles < requiredTiles)
        {
            this.AddTiles(requiredTiles - countOfCurrentTiles);
        }
        else
        {
            this.RemoveTiles(countOfCurrentTiles - requiredTiles);
        }
    }

    private void RemoveTiles(int count)
    {
        for (var i = 0; i < count; i++)
        {
            var index = this.Tiles.Count() - 1;
            var tile = this.Tiles.ElementAt(index);
            this.MosaicManager!.RemoveTile(tile);
            this.Children.Remove(tile);
        }

        this.SizingComplete?.Invoke(this, EventArgs.Empty);
    }

    private void AddTiles(int count)
    {
        var initialzedCount = 0;
        for (var i = 0; i < count; i++)
        {
            var newVideoPlayer = new VideoPlayerTile();
            newVideoPlayer.SetLabelVisibility(this.ShowLabels);
            newVideoPlayer.Initalized += (sender, args) =>
            {
                newVideoPlayer.SetMute(this.MuteVideos);
                if (++initialzedCount == count)
                {
                    this.SizingComplete?.Invoke(this, EventArgs.Empty);
                }
            };
            newVideoPlayer.MediaChangeRequested += (sender, args) =>
            {
                if (sender is IVideoPlayerTile videoPlayerTile)
                {
                    this.MosaicManager!.TriggerNextVideo(videoPlayerTile);
                }
            };

            this.Children.Add(newVideoPlayer);
        }
    }

    private void SizeUpdated(object? sender, EventArgs e)
    {
        this.UpdateLayout();
        if (this.regenerateGridTimer.IsRunning)
        {
            this.regenerateGridTimer.Stop();
        }

        if (this.IsPlaying && !this.IsPaused)
        {
            this.Play();
        }
    }

    private void TriggerNextTile(VideoPlayerTile? tile)
    {
        if (!this.IsPlaying || this.IsPaused || tile is null)
        {
            return;
        }

        this.lastIntervalChange = DateTime.UtcNow;
        this.MosaicManager!.TriggerNextVideo(tile);
    }

    private void SwapTimerTick()
    {
        if (!this.IsPlaying || this.IsPaused)
        {
            return;
        }

        var nextTileToSwap = this.Tiles.MinBy(this.MosaicManager!.GetPlayCount);
        if (nextTileToSwap is not null)
        {
            var timeDiff = DateTime.UtcNow - this.lastIntervalChange;
            nextTileToSwap.SetProgress(timeDiff.Ticks, this.IntervalDelay.Ticks);
            if (timeDiff > this.IntervalDelay)
            {
                this.TriggerNextTile(nextTileToSwap);
            }
        }
    }
}
