// ------------------------------------------------------------------------------
// <copyright file="HomePage.xaml.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LibVLCSharp.Shared;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Mosaic.Controls;
    using Mosaic.Infrastructure;
    using Windows.Foundation.Metadata;

    public sealed partial class HomePage : Page
    {
        private readonly MosaicManager mosaicManager;

        private bool canStartPlaying = false;

        public HomePage()
        {
            this.mosaicManager = new MosaicManager();
            var sources = new List<SourceConfig>();
            for (var i = 0; i < 9; i++)
            {
                sources.Add(new SourceConfig
                {
                    Source = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4"
                });
            }

            this.mosaicManager.SetConfig(new MosaicConfig
            {
                Sources = sources
            });

            this.InitializeComponent();
            Core.Initialize();

            this.Loaded += (sender, args) =>
            {
                var totalTiles = this.MosaicWidth * this.MosaicHeight;
                var initialzedCount = 0;
                for (var i = 0; i < totalTiles; i++)
                {
                    var newVideoPlayer = new VideoPlayerTile
                    {
                        Tag = $"Tile {i}"
                    };
                    newVideoPlayer.SetLabelVisibility(this.ShowLabels);
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

        public bool ShowLabels { get; private set; } = false;

        private IEnumerable<VideoPlayerTile> VideoPlayerTiles => this.MosaicGrid.Children.OfType<VideoPlayerTile>();

        private void CommandBar_ToggleLabels(object sender, RoutedEventArgs e)
        {
            this.ShowLabels = !this.ShowLabels;
            foreach (var videoPlayer in this.VideoPlayerTiles)
            {
                videoPlayer.SetLabelVisibility(this.ShowLabels);
            }
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
            if (this.IsPlaying || !this.canStartPlaying)
            {
                return;
            }

            this.IsPlaying = true;
            foreach (var videoTile in this.VideoPlayerTiles)
            {
                this.mosaicManager.StartTile(videoTile);
            }
        }

        private void CommandBar_Stop(object sender, RoutedEventArgs e)
        {
            this.IsPlaying = false;
            foreach (var videoTile in this.VideoPlayerTiles)
            {
                videoTile.StopVideo();
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
