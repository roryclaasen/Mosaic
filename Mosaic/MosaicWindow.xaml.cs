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

        public MosaicWindow()
        {
            InitializeComponent();
            Core.Initialize();

            this.MosaicManager = new MosaicManager(new LibVLC(), this.GetVideoViews());

            this.SwapTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            this.SwapTimer.Tick += (o, e) => this.MosaicManager.SwapViews();
            this.SwapTimer.IsEnabled = true;

            this.KeyUp += MosaicView_KeyUp;
            this.Closing += MosaicView_Closing;
        }

        public void InitializeSources(IEnumerable<string> sources) => this.MosaicManager.Initialize(sources);

        private IEnumerable<VideoView> GetVideoViews() => this.VideoGrid.Children.OfType<VideoView>();

        private void MosaicView_KeyUp(object sender, KeyEventArgs e)
        {
            this.MosaicManager.TogglePause();
            this.SwapTimer.IsEnabled = this.MosaicManager.Paused;
        }

        private void MosaicView_Closing(object sender, CancelEventArgs e) => this.Dispose();

        public void Dispose()
        {
            foreach (var view in this.GetVideoViews())
            {
                view.Dispose();
            }
        }
    }
}
