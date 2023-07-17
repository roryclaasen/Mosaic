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

    public partial class MosaicManager
    {
        private readonly WeightedBag<SourceConfig> weightedBag = new();

        private MosaicConfig? config;

        public int SourceCount => this.weightedBag.Count;

        public void SetConfig(MosaicConfig config)
        {
            this.config = config;
            this.weightedBag.Clear();
            this.weightedBag.AddRange(config.Sources);
        }

        public void StartTile(IVideoPlayerTile tile)
        {
            if (tile.IsPlaying)
            {
                return;
            }

            if (this.weightedBag?.TryGetNext(out var source) ?? false)
            {
                tile.PlayVideo(new Uri(source.Source));
            }
        }

        public void SwapTiles(IEnumerable<IVideoPlayerTile> videoPlayerTiles)
        {
            //if (!(this.weightedBag?.CanSwap() ?? false))
            //{
            //    return;
            //}

            //var activeIndex = this.weightedBag.NextSwapIndex;
            //var activeTile = videoPlayerTiles?.ElementAt(activeIndex);
            //if (activeTile is null)
            //{
            //    return;
            //}

            //var activeSource = this.config?.Sources.FirstOrDefault(s => s.Source.Equals(activeTile.Mrl));
            //if (activeSource is null)
            //{
            //    return;
            //}

            //var nextSource = this.weightedBag.Swap(activeSource);
            //if (nextSource is not null)
            //{
            //    activeTile.PlayVideo(new Uri(nextSource.Source));
            //}
        }
    }
}
