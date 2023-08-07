// ------------------------------------------------------------------------------
// <copyright file="MosaicManager.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Infrastructure
{
    using System.Collections.Generic;
    using System.Linq;
    using Mosaic.Infrastructure.Collections;
    using Mosaic.Infrastructure.Config;

    public partial class MosaicManager
    {
        private readonly ConcurrentLoopingQueue<MediaEntry> loopingQueue = new();

        public int SourceCount => this.loopingQueue.Count;

        public void SetConfig(IEnumerable<MediaEntry>? entries = null)
        {
            this.loopingQueue.Clear();
            this.loopingQueue.EnqueueRange(entries ?? Enumerable.Empty<MediaEntry>());
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
                tile.PlayVideo(source);
            }
        }
    }
}
