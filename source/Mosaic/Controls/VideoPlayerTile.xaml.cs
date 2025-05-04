// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace Mosaic.Controls;

using System;
using FlyleafLib;
using FlyleafLib.MediaPlayer;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Mosaic.Infrastructure;
using Mosaic.Infrastructure.Config;
using Windows.ApplicationModel.DataTransfer;

public sealed partial class VideoPlayerTile : UserControl, IVideoPlayerTile
{
    public VideoPlayerTile()
    {
        this.Config = new Config();
        this.Config.Video.VideoAcceleration = true;
        this.Config.Video.Enabled = true;

        this.Player = new Player(this.Config);

        this.InitializeComponent();

        this.Unloaded += this.VideoPlayerTile_Unloaded;
    }

    public event EventHandler? MediaChangeRequested;

    public Player Player { get; set; }

    public Config Config { get; set; }

    public string? Mrl => null;

    public bool IsPlaying => false;

    public double Progress { get; private set; }

    public void SetProgress(double value)
        => this.ProgressBar.Value = value;

    public void SetProgress(long currentValue, long maxValue)
    {
        this.Progress = currentValue * 100 / maxValue;
        this.SetProgress(this.Progress);
    }

    public void SetLabelVisibility(bool visible) => this.Label.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;

    public bool PlayVideo(MediaEntry entry)
    {
        this.StopVideo();

        this.Label.Text = entry.DisplayLabel;

        var result = this.Player.Open(entry.Mrl.ToString());
        if (result.Success)
        {
            this.NoMedia.Opacity = 0;
            return true;
        }

        return false;
    }

    public void SetPause(bool pause)
    {
        if (pause)
        {
            this.Player.Pause();
        }
        else
        {
            this.Player.Play();
        }
    }

    public void SetMute(bool mute)
    {
        this.Player.Audio.Mute = mute;
    }

    public void StopVideo()
    {
        this.Root.ContextFlyout?.Hide();
        this.Root.ContextFlyout = null;

        this.Player.Stop();

        this.NoMedia.Opacity = 1;

        this.Label.Text = string.Empty;
        this.SetProgress(0);
    }

    private void AppBarButton_Copy(object sender, RoutedEventArgs e)
    {
        if (sender is AppBarButton button)
        {
            var copyData = button.Tag switch
            {
                "label" => this.Label.Text,
                "source" => this.Mrl,
                _ => throw new Exception("Unknown tag")
            };

            if (!string.IsNullOrEmpty(copyData))
            {
                var dataPackage = new DataPackage
                {
                    RequestedOperation = DataPackageOperation.Copy
                };
                dataPackage.SetText(copyData);
                Clipboard.SetContent(dataPackage);
            }
        }
    }

    private void AppBarButton_Next(object sender, RoutedEventArgs e)
        => this.MediaChangeRequested?.Invoke(this, EventArgs.Empty);

    private void VideoPlayerTile_Unloaded(object sender, RoutedEventArgs e) => this.Player.Stop();
}
