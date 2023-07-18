// ------------------------------------------------------------------------------
// <copyright file="VideoPlayerTile.xaml.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Controls
{
    using System;
    using LibVLCSharp.Platforms.Windows;
    using LibVLCSharp.Shared;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Mosaic.Infrastructure;
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

        public void SetLabelVisibility(bool visible) => this.Label.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;

        public bool PlayVideo(Uri mrl, string? label = null)
        {
            if (this.mediaPlayer is null)
            {
                return false;
            }

            this.StopVideo();
            this.VideoView.Opacity = 1;

            using var media = new Media(this.libVlc!, mrl);
            this.Label.Text = label ?? media.Meta(MetadataType.Title) ?? mrl.AbsoluteUri;
            if (this.mediaPlayer.Play(media))
            {
                this.Root.ContextFlyout = this.TileFlyout;
                return true;
            }

            return false;
        }

        public void SetPause(bool pause)
            => this.mediaPlayer?.SetPause(pause);

        public void StopVideo()
        {
            this.Root.ContextFlyout?.Hide();
            this.Root.ContextFlyout = null;
            this.mediaPlayer?.Stop();
            this.VideoView.Opacity = 0;
            this.Label.Text = string.Empty;
        }

        private void VideoView_Initialized(object? sender, InitializedEventArgs e)
        {
            this.libVlc = new LibVLC(e.SwapChainOptions);
            this.mediaPlayer = new MediaPlayer(this.libVlc)
            {
                Mute = true,
                EnableHardwareDecoding = true
            };

            this.Initalized?.Invoke(this, EventArgs.Empty);
        }

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
}
