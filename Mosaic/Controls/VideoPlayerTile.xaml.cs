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

    public sealed partial class VideoPlayerTile : UserControl
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

        public string VideoLabel { get; set; } = "testing";

        public void HideLabel() => this.Label.Visibility = Visibility.Collapsed;

        public void ShowLabel() => this.Label.Visibility = Visibility.Visible;

        public bool PlayVideo(Uri video)
        {
            if (this.mediaPlayer is null)
            {
                return false;
            }

            this.mediaPlayer.Stop();

            using var media = new Media(this.libVlc, video);
            return this.mediaPlayer.Play(media);
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

        private void VideoPlayerTile_Unloaded(object sender, RoutedEventArgs e)
        {
            this.mediaPlayer?.Stop();
        }
    }
}
