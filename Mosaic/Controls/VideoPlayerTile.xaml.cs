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

    public sealed partial class VideoPlayerTile : UserControl, IVideoPlayerTile
    {
        private LibVLC libVlc;
        private MediaPlayer mediaPlayer;

        public EventHandler Initalized;

        public VideoPlayerTile()
        {
            this.InitializeComponent();

            this.VideoView.Initialized += this.VideoView_Initialized;
            this.Unloaded += this.VideoPlayerTile_Unloaded;
        }

        public string? Mrl => this.mediaPlayer?.Media?.Mrl;

        public bool IsPlaying => this.mediaPlayer?.IsPlaying ?? false;

        public void SetLabelVisibility(bool visible) => this.Label.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;

        public bool PlayVideo(Uri mrl, string? label = null)
        {
            if (this.mediaPlayer is null)
            {
                return false;
            }

            this.mediaPlayer.Stop();

            using var media = new Media(this.libVlc, mrl);
            this.Label.Text = label ?? media.Meta(MetadataType.Title) ?? mrl.AbsoluteUri;
            return this.mediaPlayer.Play(media);
        }

        public void SetPause(bool pause)
            => this.mediaPlayer?.SetPause(pause);

        public void StopVideo()
        {
            this.mediaPlayer?.Stop();
            this.Label.Text = string.Empty;
        }

        private void VideoView_Initialized(object sender, InitializedEventArgs e)
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
    }
}
