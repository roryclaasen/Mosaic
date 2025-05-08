// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace Mosaic.Controls;

using System;
using System.Threading.Tasks;
using LibVLCSharp.Platforms.Windows;
using LibVLCSharp.Shared;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Mosaic.Infrastructure;
using Mosaic.Infrastructure.Config;
using Windows.ApplicationModel.DataTransfer;

public sealed partial class VideoPlayerTile : UserControl, IVideoPlayerTile
{
    private LibVLC? libVlc;
    private MediaPlayer? mediaPlayer;

    public VideoPlayerTile()
    {
        this.InitializeComponent();

        this.VideoView.Initialized += this.VideoView_Initialized;
        this.Unloaded += this.VideoPlayerTile_Unloaded;

        this.VideoView.Opacity = 0;
    }

    public event EventHandler? Initalized;

    public event EventHandler? MediaChangeRequested;

    public string? Mrl => this.mediaPlayer?.Media?.Mrl;

    public bool IsPlaying => this.mediaPlayer?.IsPlaying ?? false;

    public double Progress { get; private set; }

    public void SetProgress(double value)
        => this.ProgressBar.Value = value;

    public void SetProgress(long currentValue, long maxValue)
    {
        this.Progress = (currentValue * 100) / maxValue;
        this.SetProgress(this.Progress);
    }

    public void SetLabelVisibility(bool visible) => this.Label.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;

    public bool PlayVideo(MediaEntry entry)
    {
        if (this.mediaPlayer is null)
        {
            return false;
        }

        this.StopVideo();
        this.VideoView.Opacity = 1;

        using var media = new Media(this.libVlc!, entry.Mrl);
        media.AddOption(":avcodec-hw=any");
        media.AddOption(":avcodec-threads=1");
        this.Label.Text = entry.DisplayLabel ?? media.Meta(MetadataType.Title) ?? entry.Mrl.AbsoluteUri;
        if (this.mediaPlayer.Play(media))
        {
            this.Root.ContextFlyout = this.TileFlyout;
            return true;
        }

        return false;
    }

    public void SetPause(bool pause)
        => this.mediaPlayer?.SetPause(pause);

    public void SetMute(bool mute)
    {
        if (this.mediaPlayer is not null)
        {
            this.mediaPlayer.Mute = mute;
        }
    }

    public void StopVideo()
    {
        this.Root.ContextFlyout?.Hide();
        this.Root.ContextFlyout = null;
        this.mediaPlayer?.Stop();
        this.VideoView.Opacity = 0;
        this.Label.Text = string.Empty;
        this.SetProgress(0);
    }

    private void VideoView_Initialized(object? sender, InitializedEventArgs e) => Task.Run(() =>
    {
        this.libVlc = new LibVLC(["--aout=directsound", .. e.SwapChainOptions]);
        this.mediaPlayer = new MediaPlayer(this.libVlc)
        {
            Mute = true,
            EnableHardwareDecoding = true
        };

        this.Initalized?.Invoke(this, EventArgs.Empty);
    });

    private void VideoPlayerTile_Unloaded(object sender, RoutedEventArgs e) => this.StopVideo();

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
}
