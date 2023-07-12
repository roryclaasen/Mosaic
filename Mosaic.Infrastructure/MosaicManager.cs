// ------------------------------------------------------------------------------
// <copyright file="MosaicManager.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using LibVLCSharp.Shared;
    using Mosaic.Infrastructure.Config;
    using Mosaic.Infrastructure.Events;

    public partial class MosaicManager
    {
        private readonly IVideoView[] videoTiles;

        private MosaicConfig config;
        private IQueueSwapper<SourceConfig> queueSwapper;

        public MosaicManager(IEnumerable<IVideoView> videoTiles)
        {
            this.videoTiles = videoTiles?.ToArray() ?? throw new ArgumentNullException(nameof(videoTiles));
        }

        public bool Paused { get; private set; } = false;

        public void Initialize(MosaicConfig config)
        {
            this.config = config;
            this.queueSwapper = new QueueSwapper<SourceConfig>(this.videoTiles.Length, config.Sources);
        }

        public void StartTile(LibVLC libVLC, IVideoView tile)
        {
            if (this.queueSwapper.TryDequeue(out var source))
            {
                using (var media = this.CreateMedia(libVLC, source))
                {
                    tile.MediaPlayer.Play(media);
                }

                this.OnTileStarted(this, tile, source);
            }
        }

        public void SwapTiles(Func<IVideoView, LibVLC> getLibVlc)
        {
            if (this.queueSwapper.CanSwap())
            {
                var currentTileIndex = this.queueSwapper.NextSwapIndex;
                var currentTile = this.videoTiles.ElementAt(currentTileIndex);
                var libVlc = getLibVlc(currentTile);

                var oldMedia = currentTile.MediaPlayer.Media;
                var oldSource = this.config.Sources.FirstOrDefault(s => s.Source.Equals(oldMedia.Mrl));
                if (oldSource is not null)
                {
                    var newSource = this.queueSwapper.Swap(oldSource);
                    if (newSource is not null)
                    {
                        using (var media = this.CreateMedia(libVlc, newSource))
                        {
                            currentTile.MediaPlayer.Play(media);
                        }

                        var eventArgs = new TileSourceEventArgs(currentTileIndex, newSource);
                        this.OnTileStarted(this, eventArgs);
                        this.OnTileChanged(this, eventArgs);
                    }
                }
            }
        }

        public void Pause()
        {
            this.Paused = true;

            foreach (var tile in this.videoTiles)
            {
                tile.MediaPlayer.Pause();
            }
        }

        public void Resume()
        {
            this.Paused = false;

            foreach (var tile in this.videoTiles)
            {
                tile.MediaPlayer.Play();
            }
        }

        public void TogglePause()
        {
            if (this.Paused)
            {
                this.Resume();
            }
            else
            {
                this.Pause();
            }
        }

        private Media CreateMedia(LibVLC libVLC, SourceConfig config) => new(libVLC, config.Source, FromType.FromLocation);
    }
}
