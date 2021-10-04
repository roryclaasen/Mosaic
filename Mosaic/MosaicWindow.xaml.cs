// ------------------------------------------------------------------------------
// <copyright file="MosaicWindow.xaml.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Threading;
    using LibVLCSharp.Shared;
    using LibVLCSharp.WPF;
    using Mosaic.Infrastructure;
    using Mosaic.Infrastructure.Config;
    using Mosaic.Infrastructure.Events;

    public partial class MosaicWindow : Window, IDisposable
    {
        private readonly MosaicManager mosaicManager;
        private readonly DispatcherTimer swapTimer;

        private MosaicApplicationConfig config;

        public MosaicWindow()
        {
            this.InitializeComponent();
            Core.Initialize();

            this.mosaicManager = new MosaicManager(new LibVLC(), this.GetVideoTiles());
            this.mosaicManager.TileStarted += this.MosaicManager_TileStarted;

            this.swapTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            this.swapTimer.Tick += (o, e) => this.mosaicManager.SwapTiles();

            this.KeyUp += this.MosaicView_KeyUp;
            this.Closing += this.MosaicView_Closing;
        }

        public void Dispose()
        {
            foreach (var tile in this.GetVideoTiles())
            {
                tile.Dispose();
            }
        }

        public void InitializeConfig(MosaicApplicationConfig config)
        {
            this.config = config;
            this.mosaicManager.Initialize(config);

            this.SetFullScreen();
            this.SetShowTitles();

            this.swapTimer.IsEnabled = true;
        }

        private IEnumerable<VideoView> GetVideoTiles() => this.VideoGrid.Children.OfType<VideoView>();

        private IEnumerable<TextBlock> GetTextTiles() => this.VideoGrid.Children.OfType<TextBlock>();

        private void MosaicView_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Space:
                    this.mosaicManager.TogglePause();
                    this.swapTimer.IsEnabled = this.mosaicManager.Paused;
                    break;
                case Key.F:
                    this.SetFullScreen(!this.config.FullScreen);
                    break;
                case Key.T:
                    this.SetShowTitles(!this.config.ShowTitles);
                    break;
                default:
                    break;
            }
        }

        private void MosaicManager_TileStarted(object sender, EventArgs e)
        {
            if (e is TileSourceEventArgs tileSwap)
            {
                this.SetTileTitle(tileSwap.TileIndex, tileSwap.SourceConfig);
            }
        }

        private void MosaicView_Closing(object sender, CancelEventArgs e) => this.Dispose();

        private void SetFullScreen(bool fullScreen)
        {
            this.config.FullScreen = fullScreen;
            this.SetFullScreen();
        }

        private void SetFullScreen()
        {
            if (this.config.FullScreen)
            {
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowStyle = WindowStyle.SingleBorderWindow;
                this.WindowState = WindowState.Normal;
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
    }
}
