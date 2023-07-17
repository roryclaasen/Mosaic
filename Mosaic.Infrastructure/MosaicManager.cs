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

    public partial class MosaicManager
    {
        private MosaicConfig? config;
        private IQueueSwapper<SourceConfig>? queueSwapper;

        public void SetConfig(MosaicConfig config)
        {
            this.config = config;
            this.queueSwapper = new QueueSwapper<SourceConfig>(config.Sources);
        }

        public void StartTile(IVideoPlayerTile tile)
        {
            if (tile.IsPlaying)
            {
                return;
            }

            if (this.queueSwapper?.TryDequeue(out var source) ?? false)
            {
                tile.PlayVideo(new Uri(source.Source));
            }
        }

        public void SwapTiles(IEnumerable<IVideoPlayerTile> videoPlayerTiles)
        {
            if (!(this.queueSwapper?.CanSwap() ?? false))
            {
                return;
            }

            var activeIndex = this.queueSwapper.NextSwapIndex;
            var activeTile = videoPlayerTiles?.ElementAt(activeIndex);
            if (activeTile is null)
            {
                return;
            }

            var activeSource = this.config?.Sources.FirstOrDefault(s => s.Source.Equals(activeTile.Mrl));
            if (activeSource is null)
            {
                return;
            }

            var nextSource = this.queueSwapper.Swap(activeSource);
            if (nextSource is not null)
            {
                activeTile.PlayVideo(new Uri(nextSource.Source));
            }
        }
    }
}
