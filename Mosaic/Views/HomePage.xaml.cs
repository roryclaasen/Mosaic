// ------------------------------------------------------------------------------
// <copyright file="HomePage.xaml.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Views
{
    using System;
    using System.Linq;
    using LibVLCSharp.Shared;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Mosaic.Controls;
    using Windows.Foundation.Metadata;

    public sealed partial class HomePage : Page
    {
        private bool showLabels = true;

        private bool canStartPlaying = false;

        public HomePage()
        {
            this.InitializeComponent();
            Core.Initialize();

            this.Loaded += (sender, args) =>
            {
                var totalTiles = this.MosaicWidth * this.MosaicHeight;
                var initialzedCount = 0;
                for (var i = 0; i < totalTiles; i++)
                {
                    var newVideoPlayer = new VideoPlayerTile();
                    newVideoPlayer.Initalized += (sender, args) =>
                    {
                        initialzedCount++;
                        if (initialzedCount >= totalTiles)
                        {
                            this.canStartPlaying = true;
                        }
                    };

                    this.MosaicGrid.Children.Add(newVideoPlayer);
                }
            };
        }

        public int MosaicWidth { get; set; } = 4;

        public int MosaicHeight { get; set; } = 3;

        public bool IsPlaying { get; private set; } = false;

        private void CommandBar_ToggleLabels(object sender, RoutedEventArgs e)
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

        private async void CommandBar_ShowAbout(object sender, RoutedEventArgs e)
        {
            var dialog = new AboutDialog();
            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 8))
            {
                dialog.XamlRoot = this.XamlRoot;
            }

            await dialog.ShowAsync();
        }

        private void CommandBar_Play(object sender, RoutedEventArgs e)
        {
            if (this.IsPlaying)
            {
                return;
            }

            this.IsPlaying = true;
            foreach (var videoPlayer in this.MosaicGrid.Children.OfType<VideoPlayerTile>())
            {
                videoPlayer.PlayVideo(new Uri("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4"));
            }
        }

        private void CommandBar_Stop(object sender, RoutedEventArgs e)
        {
            this.IsPlaying = false;
            foreach (var videoPlayer in this.MosaicGrid.Children.OfType<VideoPlayerTile>())
            {
                videoPlayer.StopVideo();
            }
        }

        private void CommandBar_ChangeTheme(object sender, RoutedEventArgs e)
        {
            if (sender is RadioMenuFlyoutItem button && App.Current.StartupWindow?.Content is FrameworkElement frameworkElement)
            {
                frameworkElement.RequestedTheme = button.Tag switch
                {
                    "light" => ElementTheme.Light,
                    "dark" => ElementTheme.Dark,
                    _ => ElementTheme.Default,
                };
            }
        }
    }
}
