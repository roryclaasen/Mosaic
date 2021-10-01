namespace Mosaic.Player
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using LibVLCSharp.Shared;

    public sealed class MosaicManager
    {
        private readonly LibVLC LlibVLC;
        private IEnumerable<IVideoView> VideoViews;

        public bool Paused { get; private set; } = false;

        private QueueSwapper<Media> QueueSwapper;

        public MosaicManager(LibVLC libVLC, IEnumerable<IVideoView> videoViews)
        {
            this.LlibVLC = libVLC ?? throw new ArgumentNullException(nameof(libVLC));
            this.VideoViews = videoViews ?? throw new ArgumentNullException(nameof(videoViews));
        }

        public void Initialize(IEnumerable<string> sources)
        {
            this.SetupVideoViews();

            var views = this.VideoViews.ToArray();

            this.QueueSwapper = new QueueSwapper<Media>(views.Length, sources.Select(source => new Media(this.LlibVLC, source, FromType.FromLocation)));

            for (var i = 0; i < views.Length; i++)
            {
                if (this.QueueSwapper.TryDequeue(out var media))
                {
                    views[i].MediaPlayer.Play(media);
                }
            }
        }

        private void SetupVideoViews()
        {
            foreach (var view in this.VideoViews)
            {
                var mediaPlayer = new MediaPlayer(this.LlibVLC);
                mediaPlayer.Mute = true;
                view.MediaPlayer = mediaPlayer;
            }
        }

        public void SwapViews()
        {
            var viewIndex = this.QueueSwapper.NextSwapIndex;

            var view = this.VideoViews.ToArray()[viewIndex];

            var oldMedia = view.MediaPlayer.Media.Duplicate();
            var newMedia = this.QueueSwapper.Swap(oldMedia);

            view.MediaPlayer.Play(newMedia);
        }

        public void Pause()
        {
            this.Paused = true;

            foreach (var view in this.VideoViews)
            {
                view.MediaPlayer.Pause();
            }
        }

        public void Resume()
        {
            this.Paused = false;

            foreach (var view in this.VideoViews)
            {
                view.MediaPlayer.Play();
            }
        }

        public void TogglePause()
        {
            if (this.Paused) this.Resume();
            else this.Pause();
        }
    }
}
