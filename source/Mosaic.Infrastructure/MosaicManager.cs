// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace Mosaic.Infrastructure;

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Mosaic.Infrastructure.Collections;
using Mosaic.Infrastructure.Config;

public class MosaicManager
{
    private readonly ConcurrentLoopingQueue<MediaEntry> loopingQueue = new();
    private readonly ConcurrentDictionary<IVideoPlayerTile, int> playCount = new();

    public int SourceCount => this.loopingQueue.Count;

    public int MinPlayCount => this.playCount.IsEmpty ? 0 : this.playCount.Min(x => x.Value);

    public void SetConfig(IEnumerable<MediaEntry>? entries = null)
    {
        this.loopingQueue.Clear();
        this.loopingQueue.EnqueueRange(entries ?? []);
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

            var playCount = this.MinPlayCount;
            this.playCount.AddOrUpdate(tile, playCount, (_, count) => count + 1);
        }
    }

    public void RemoveTile(IVideoPlayerTile tile)
        => this.playCount.TryRemove(tile, out _);

    public int GetPlayCount(IVideoPlayerTile tile)
        => this.playCount.TryGetValue(tile, out var count) ? count : this.MinPlayCount;
}
