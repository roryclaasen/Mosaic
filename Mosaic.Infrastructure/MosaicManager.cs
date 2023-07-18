// ------------------------------------------------------------------------------
// <copyright file="MosaicManager.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Infrastructure
{
    using System;
    using Mosaic.Infrastructure.Collections;
    using Mosaic.Infrastructure.Config;

    public partial class MosaicManager
    {
        private readonly ConcurrentLoopingQueue<MediaEntry> loopingQueue = new();

        private MosaicConfig? config;

        public int SourceCount => this.loopingQueue.Count;

        public void SetConfig(MosaicConfig config)
        {
            this.config = config;
            this.loopingQueue.Clear();
            this.loopingQueue.EnqueueRange(config.Sources);
        }

        public void StartTile(IVideoPlayerTile tile)
        {
            if (tile.IsPlaying)
            {
                return;
            }

            this.TriggerNextVideo(tile);
        }

        public void TriggerNextVideo(IVideoPlayerTile tile)
        {
            if (this.loopingQueue.TryDequeue(out var source))
            {
                tile.PlayVideo(new Uri(source.Source));
            }
        }
    }
}
