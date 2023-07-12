// ------------------------------------------------------------------------------
// <copyright file="MosaicWindow.xaml.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Mosaic
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices.WindowsRuntime;
    using LibVLCSharp.Platforms.Windows;
    using LibVLCSharp.Shared;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Mosaic.Infrastructure;
    using Windows.UI.Core;

    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MosaicWindow : Window
    {
        private readonly ConcurrentDictionary<IVideoView, LibVLC> libVlcDictionary = new();
        private readonly MosaicManager mosaicManager;
        private readonly DispatcherTimer swapTimer;

        private MosaicApplicationConfig config;

        public MosaicWindow()
        {
            this.InitializeComponent();
            Core.Initialize();

            this.Closed += this.MainWindow_Closed;

            this.mosaicManager = new MosaicManager(this.GetVideoTiles());
            this.mosaicManager.TileStarted += this.MosaicWindow_TileStarted;

            this.swapTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            this.swapTimer.Tick += (o, e) => this.mosaicManager.SwapTiles(this.GetLibVLC);

            foreach (var tile in this.GetVideoTiles())
            {
                tile.Initialized += this.ViewView_Initailized;
            }
        }

        public void InitializeConfig(MosaicApplicationConfig config)
        {
            this.config = config;
            this.mosaicManager.Initialize(config);

            this.SetFullScreen();
            this.SetShowTitles();

            this.swapTimer.Start();
        }

        private void ViewView_Initailized(object sender, InitializedEventArgs e)
        {
            if (sender is VideoView videoView)
            {
                var libVlc = new LibVLC(e.SwapChainOptions);
                videoView.MediaPlayer = new MediaPlayer(libVlc)
                {
                    Mute = true,
                    EnableHardwareDecoding = true
                };

                this.libVlcDictionary.AddOrUpdate(videoView, libVlc, (k, v) => libVlc);
                this.mosaicManager.StartTile(libVlc, videoView);
            }
        }

        private IEnumerable<VideoView> GetVideoTiles() => this.VideoGrid.Children.OfType<VideoView>();

        private IEnumerable<TextBlock> GetTextTiles() => this.VideoGrid.Children.OfType<TextBlock>();

        private void MosaicView_KeyUp(object sender, KeyEventArgs e)
        {
            //switch (e.Key)
            //{
            //    case Key.Space:
            //        this.mosaicManager.TogglePause();
            //        this.swapTimer.IsEnabled = this.mosaicManager.Paused;
            //        break;
            //    case Key.F:
            //        this.SetFullScreen(!this.config.FullScreen);
            //        break;
            //    case Key.T:
            //        this.SetShowTitles(!this.config.ShowTitles);
            //        break;
            //    default:
            //        break;
            //}
        }

        private void MosaicWindow_TileStarted(object sender, EventArgs e)
        {
            if (e is TileSourceEventArgs tileSwap)
            {
                this.SetTileTitle(tileSwap.TileIndex, tileSwap.SourceConfig);
            }
        }

        private void MainWindow_Closed(object sender, WindowEventArgs e)
        {
            foreach (var tile in this.GetVideoTiles())
            {
                if (tile.MediaPlayer is not null)
                {
                    tile.MediaPlayer.Stopped += (s, e) => this.VideoGrid.Children.Remove(tile);
                    tile.MediaPlayer.Stop();
                }
            }
        }

        private void SetFullScreen(bool fullScreen)
        {
            this.config.FullScreen = fullScreen;
            this.SetFullScreen();
        }

        private void SetFullScreen()
        {
            if (this.config.FullScreen)
            {
                // this.WindowStyle = WindowStyle.None;
                // this.WindowState = WindowState.Maximized;
            }
            else
            {
                // this.WindowStyle = WindowStyle.SingleBorderWindow;
                // this.WindowState = WindowState.Normal;
            }
        }

        private void SetShowTitles(bool showTitles)
        {
            this.config.ShowTitles = showTitles;
            this.SetShowTitles();
        }

        private void SetShowTitles()
        {
            foreach (var title in this.GetTextTiles())
            {
                title.Visibility = this.config.ShowTitles ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void SetTileTitle(int tileIndex, SourceConfig sourceConfig)
        {
            var tile = this.GetTextTiles().ToArray()[tileIndex];
            tile.Text = !string.IsNullOrWhiteSpace(sourceConfig.DisplayName) ? sourceConfig.DisplayName : sourceConfig.Source;
        }

        private LibVLC GetLibVLC(IVideoView videoView)
        {
            if (this.libVlcDictionary.TryGetValue(videoView, out var libVlc))
            {
                return libVlc;
            }

            return null;
        }
    }
}
