namespace Mosaic.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using LibVLCSharp.Shared;

    public sealed class MosaicManager
    {
        private readonly LibVLC LlibVLC;
        private readonly IVideoView[] VideoTiles;

        public bool Paused { get; private set; } = false;

        private QueueSwapper<Media> QueueSwapper;

        public MosaicManager(LibVLC libVLC, IEnumerable<IVideoView> videoTiles)
        {
            this.LlibVLC = libVLC ?? throw new ArgumentNullException(nameof(libVLC));
            this.VideoTiles = videoTiles?.ToArray() ?? throw new ArgumentNullException(nameof(videoTiles));
        }

        public void Initialize(IEnumerable<string> sources)
        {
            this.SetupVideoViews();

            var tiles = this.VideoTiles;

            this.QueueSwapper = new QueueSwapper<Media>(tiles.Length, sources.Select(source => new Media(this.LlibVLC, source, FromType.FromLocation)));

            for (var i = 0; i < tiles.Length; i++)
            {
                if (this.QueueSwapper.TryDequeue(out var media))
                {
                    tiles[i].MediaPlayer.Play(media);
                }
            }
        }

        private void SetupVideoViews()
        {
            foreach (var tile in this.VideoTiles)
            {
                var mediaPlayer = new MediaPlayer(this.LlibVLC);
                mediaPlayer.Mute = true;
                tile.MediaPlayer = mediaPlayer;
            }
        }

        public void SwapViews()
        {
            var tileIndex = this.QueueSwapper.NextSwapIndex;

            var tile = this.VideoTiles[tileIndex];

            var oldMedia = tile.MediaPlayer.Media.Duplicate();
            var newMedia = this.QueueSwapper.Swap(oldMedia);

            tile.MediaPlayer.Play(newMedia);
        }

        public void Pause()
        {
            this.Paused = true;

            foreach (var view in this.VideoTiles)
            {
                view.MediaPlayer.Pause();
            }
        }

        public void Resume()
        {
            this.Paused = false;

            foreach (var tile in this.VideoTiles)
            {
                tile.MediaPlayer.Play();
            }
        }

        public void TogglePause()
        {
            if (this.Paused) this.Resume();
            else this.Pause();
        }
    }
}
