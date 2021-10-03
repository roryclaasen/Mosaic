namespace Mosaic
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;
    using LibVLCSharp.Shared;
    using LibVLCSharp.WPF;
    using Mosaic.Infrastructure;

    public partial class MosaicWindow : Window, IDisposable
    {
        private readonly MosaicManager MosaicManager;
        private readonly DispatcherTimer SwapTimer;

        private ApplicationConfig Config;

        public MosaicWindow()
        {
            InitializeComponent();
            Core.Initialize();

            this.MosaicManager = new MosaicManager(new LibVLC(), this.GetVideoTiles());

            this.SwapTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            this.SwapTimer.Tick += (o, e) => this.MosaicManager.SwapViews();
            this.SwapTimer.IsEnabled = true;

            this.KeyUp += MosaicView_KeyUp;
            this.Closing += MosaicView_Closing;
        }

        public void InitializeConfig(ApplicationConfig config)
        {
            this.Config = config;
            this.MosaicManager.Initialize(config.Sources.Select(s => s.Source));

            this.SetFullScreen();
        }

        private IEnumerable<VideoView> GetVideoTiles() => this.VideoGrid.Children.OfType<VideoView>();

        private void MosaicView_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Space:
                    this.MosaicManager.TogglePause();
                    this.SwapTimer.IsEnabled = this.MosaicManager.Paused;
                    break;
                case Key.F:
                    this.SetFullScreen(!this.Config.FullScreen);
                    break;
                default:
                    break;
            }
        }

        private void MosaicView_Closing(object sender, CancelEventArgs e) => this.Dispose();

        private void SetFullScreen(bool fullScreen)
        {
            this.Config.FullScreen = fullScreen;
            this.SetFullScreen();
        }

        private void SetFullScreen()
        {
            if (this.Config.FullScreen)
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

        public void Dispose()
        {
            foreach (var view in this.GetVideoTiles())
            {
                view.Dispose();
            }
        }
    }
}
