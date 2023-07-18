// ------------------------------------------------------------------------------
// <copyright file="MosaicManager.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using Mosaic.Infrastructure.Collections;
    using Mosaic.Infrastructure.Config;

    public partial class MosaicManager
    {
        private readonly ConcurrentLoopingQueue<MediaEntry> weightedBag = new();

        private MosaicConfig? config;

        public int SourceCount => this.weightedBag.Count;

        public void SetConfig(MosaicConfig config)
        {
            this.config = config;
            this.weightedBag.Clear();
            this.weightedBag.EnqueueRange(config.Sources);
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
            if (this.weightedBag?.TryDequeue(out var source) ?? false)
            {
                tile.PlayVideo(new Uri(source.Source));
            }
        }
    }
}
