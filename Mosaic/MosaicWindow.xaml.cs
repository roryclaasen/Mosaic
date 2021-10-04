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
    using System.Windows.Media;
    using System.Windows.Threading;
    using LibVLCSharp.Shared;
    using LibVLCSharp.WPF;
    using Mosaic.Infrastructure;

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
            this.mosaicManager.TileChanged += this.MosaicManager_TileChanged;

            this.swapTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            this.swapTimer.Tick += (o, e) => this.mosaicManager.SwapTiles();
            this.swapTimer.IsEnabled = true;

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
        }

        private IEnumerable<VideoView> GetVideoTiles() => this.VideoGrid.Children.OfType<VideoView>();

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
                default:
                    break;
            }
        }

        private void MosaicManager_TileChanged(object sender, EventArgs e)
        {
            if (e is TileSwapEventArgs tileSwap)
            {
                // FIXME Text does not appear in correct location
                // var tile = this.GetVideoTiles().ToArray()[tileSwap.TileIndex];
                // tile.Content = new TextBlock
                // {
                //     Text = tileSwap.SourceConfig.DisplayName,
                //     Foreground = Brushes.Yellow
                // };
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
    }
}
