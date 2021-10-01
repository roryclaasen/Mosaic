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
        private readonly MosaicManager mosaicManager;
        private readonly DispatcherTimer swapTimer;

        public MosaicWindow()
        {
            InitializeComponent();
            Core.Initialize();

            this.mosaicManager = new MosaicManager(new LibVLC(), this.GetVideoViews());

            this.Closing += MosaicView_Closing;
            this.KeyUp += MosaicView_KeyUp;

            swapTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            swapTimer.Tick += (o, e) => this.mosaicManager.SwapViews();
            swapTimer.IsEnabled = true;
        }

        public void InitializeSources(IEnumerable<string> sources) => this.mosaicManager.Initialize(sources);

        private IEnumerable<VideoView> GetVideoViews() => this.VideoGrid.Children.OfType<VideoView>();

        private void MosaicView_Closing(object sender, CancelEventArgs e) => this.Dispose();

        private void MosaicView_KeyUp(object sender, KeyEventArgs e) => this.mosaicManager.TogglePause();

        public void Dispose()
        {
            foreach (var view in this.GetVideoViews())
            {
                view.Dispose();
            }
        }
    }
}
