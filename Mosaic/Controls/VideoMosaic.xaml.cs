// ------------------------------------------------------------------------------
// <copyright file="VideoMosaic.xaml.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CommunityToolkit.WinUI.UI;
    using CommunityToolkit.WinUI.UI.Controls;
    using LibVLCSharp.Shared;
    using Microsoft.UI.Dispatching;
    using Mosaic.Infrastructure;

    public sealed partial class VideoMosaic : UniformGrid
    {
        private const int DEFAULTSIZE = 3;

        private readonly DispatcherQueueTimer regenerateGridTimer;

        private event EventHandler? SizingComplete;

        public VideoMosaic()
        {
            this.Rows = DEFAULTSIZE;
            this.Columns = DEFAULTSIZE;

            this.InitializeComponent();
            Core.Initialize();

            this.regenerateGridTimer = DispatcherQueue.GetForCurrentThread().CreateTimer();

            this.SizingComplete += this.SizeUpdated;
            this.Loaded += (_, _) => this.regenerateGridTimer.Debounce(this.TriggerResize, TimeSpan.FromMilliseconds(400));
            this.Unloaded += (_, _) => this.Stop();
        }

        public MosaicManager MosaicManager { get; set; }

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

        public IEnumerable<VideoPlayerTile> Tiles => this.Children.OfType<VideoPlayerTile>();

        public void SetPause(bool pause)
        {
            this.IsPaused = pause;
            foreach (var tile in this.Tiles)
            {
                tile.SetPause(pause);
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

            // if (!this.canStartPlaying)
            // {
            //     return;
            // }

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
                this.Children.RemoveAt(this.Tiles.Count() - 1);
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
                    initialzedCount++;
                    if (initialzedCount == count)
                    {
                        this.SizingComplete?.Invoke(this, EventArgs.Empty);
                    }
                };
                newVideoPlayer.MediaChangeRequested += (sender, args) =>
                {
                    if (sender is IVideoPlayerTile videoPlayerTile)
                    {
                        // TODO: Trigger a new video to be played
                        this.MosaicManager.StartTile(videoPlayerTile);
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
    }
}
