namespace Mosaic.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using LibVLCSharp.Shared;

    public class MosaicManager
    {
        private readonly LibVLC LlibVLC;
        private readonly IVideoView[] VideoTiles;

        public bool Paused { get; private set; } = false;

        private MosaicConfig Config;
        private QueueSwapper<SourceConfig> QueueSwapper;

        public event EventHandler TileChanged;

        public MosaicManager(LibVLC libVLC, IEnumerable<IVideoView> videoTiles)
        {
            this.LlibVLC = libVLC ?? throw new ArgumentNullException(nameof(libVLC));
            this.VideoTiles = videoTiles?.ToArray() ?? throw new ArgumentNullException(nameof(videoTiles));
        }

        public void Initialize(MosaicConfig config)
        {
            this.Config = config;
            this.QueueSwapper = new QueueSwapper<SourceConfig>(this.VideoTiles.Length, config.Sources);
            this.SetupVideoTiles();
        }

        private void SetupVideoTiles()
        {
            foreach (var tile in this.VideoTiles)
            {
                tile.MediaPlayer = new MediaPlayer(this.LlibVLC)
                {
                    Mute = true
                };

                if (this.QueueSwapper.TryDequeue(out var source))
                {
                    using (var media = this.CreateMedia(source))
                    {
                        tile.MediaPlayer.Play(media);
                    }
                }
            }
        }

        public void SwapTiles()
        {
            if (this.QueueSwapper.CanSwap())
            {
                var currentTileIndex = this.QueueSwapper.NextSwapIndex;
                var currentTile = this.VideoTiles[currentTileIndex];

                var oldMedia = currentTile.MediaPlayer.Media;
                var oldSource = this.Config.Sources.FirstOrDefault(s => s.Source.Equals(oldMedia.Mrl));
                if (oldSource != null)
                {
                    var newSource = this.QueueSwapper.Swap(oldSource);
                    if (newSource != null)
                    {
                        using (var media = this.CreateMedia(newSource))
                        {
                            currentTile.MediaPlayer.Play(media);
                        }

                        this.OnTileChanged(this, new TileSwapEventArgs(currentTileIndex, newSource));
                    }
                }
            }
        }

        private Media CreateMedia(SourceConfig config) => new Media(this.LlibVLC, config.Source, FromType.FromLocation);

        public void Pause()
        {
            this.Paused = true;

            foreach (var title in this.VideoTiles)
            {
                title.MediaPlayer.Pause();
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

        protected void OnTileChanged(object sender, TileSwapEventArgs args)
        {
            if (this.TileChanged != null)
            {
                this.TileChanged(sender, args);
            }
        }
    }
}
