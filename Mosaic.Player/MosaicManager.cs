namespace Mosaic.Player
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using LibVLCSharp.Shared;

    public sealed class MosaicManager
    {
        private readonly LibVLC libVLC;
        private IEnumerable<IVideoView> videoViews;

        public bool Paused { get; private set; } = false;





        private Queue<Media> MediaList;



        public MosaicManager(LibVLC libVLC, IEnumerable<IVideoView> videoViews)
        {
            this.libVLC = libVLC ?? throw new ArgumentNullException(nameof(libVLC));
            this.videoViews = videoViews ?? throw new ArgumentNullException(nameof(videoViews));
        }

        public void Initialize(IEnumerable<string> sources)
        {
            this.SetupVideoViews();

            this.MediaList = new Queue<Media>(sources.Select(source => new Media(this.libVLC, source, FromType.FromLocation)));

            var views = this.videoViews.ToArray();
            for (var i = 0; i < views.Length; i++)
            {
                if (this.MediaList.TryDequeue(out var media))
                {
                    views[i].MediaPlayer.Play(media);
                }
            }

            this.LastTime = DateTime.UtcNow;
        }

        private void SetupVideoViews()
        {
            foreach (var view in this.videoViews)
            {
                var mediaPlayer = new MediaPlayer(this.libVLC);
                mediaPlayer.Mute = true;
                view.MediaPlayer = mediaPlayer;
            }
        }

        private DateTime LastTime;
        private TimeSpan Length = TimeSpan.FromSeconds(10);
        private int SwapIndex = 0;

        public void Update()
        {
            if (DateTime.UtcNow - this.LastTime > Length && this.MediaList.Count > 0)
            {
                var views = this.videoViews.ToArray();
                var view = views[this.SwapIndex];

                this.MediaList.Enqueue(view.MediaPlayer.Media.Duplicate());
                view.MediaPlayer.Play(this.MediaList.Dequeue());

                this.SwapIndex++;
                if (this.SwapIndex >= views.Length) this.SwapIndex = 0;
                this.LastTime = DateTime.UtcNow;
            }
        }

        public void Pause()
        {
            this.Paused = true;

            foreach (var view in this.videoViews)
            {
                view.MediaPlayer.Pause();
            }
        }

        public void Resume()
        {
            this.Paused = false;

            foreach (var view in this.videoViews)
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
