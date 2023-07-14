// ------------------------------------------------------------------------------
// <copyright file="HomePage.xaml.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Views
{
    using System;
    using System.Linq;
    using System.Threading;
    using LibVLCSharp.Shared;
    using Microsoft.UI.Xaml.Controls;
    using Mosaic.Controls;

    public sealed partial class HomePage : Page
    {
        private bool showLabels = true;

        public HomePage()
        {
            this.InitializeComponent();
            Core.Initialize();

            this.Loaded += (sender, args) =>
            {
                var size = 3;
                for (var i = 0; i < size * size; i++)
                {
                    var newVideoPlayer = new VideoPlayerTile();
                    newVideoPlayer.Initalized += (sender, args) =>
                    {
                        newVideoPlayer.PlayVideo(new Uri("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4"));
                    };

                    this.MosaicGrid.Children.Add(newVideoPlayer);
                }
            };
        }

        private void AppBar_ToggleLabels(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            foreach (var videoPlayer in this.MosaicGrid.Children.OfType<VideoPlayerTile>())
            {
                if (this.showLabels)
                {
                    videoPlayer.HideLabel();
                }
                else
                {
                    videoPlayer.ShowLabel();
                }
            }

            this.showLabels = !this.showLabels;
        }
    }
}
