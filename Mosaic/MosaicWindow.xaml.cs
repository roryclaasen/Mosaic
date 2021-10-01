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
    using Mosaic.Player;

    public partial class MosaicWindow : Window, IDisposable
    {
        private readonly MosaicManager mosaicManager;

        public MosaicWindow()
        {
            InitializeComponent();
            Core.Initialize();

            this.mosaicManager = new MosaicManager(new LibVLC(), this.GetVideoViews());

            this.Closing += MosaicView_Closing;
            this.KeyUp += MosaicView_KeyUp;

            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += (o, e) =>
            {
                this.mosaicManager.Update();
            };
            timer.IsEnabled = true;
        }

        public void InitializeSources(IEnumerable<string> sources) => this.mosaicManager.Initialize(sources);

        private void MosaicView_Closing(object sender, CancelEventArgs e) => this.Dispose();

        private void MosaicView_KeyUp(object sender, KeyEventArgs e) => this.mosaicManager.TogglePause();


        private IEnumerable<VideoView> GetVideoViews()
        {
            return this.VideoGrid.Children.OfType<VideoView>();
        }

        public void Dispose()
        {
            foreach (var view in this.GetVideoViews())
            {
                view.Dispose();
            }
        }
    }
}
