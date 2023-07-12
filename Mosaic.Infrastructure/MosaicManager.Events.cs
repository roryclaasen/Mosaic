// ------------------------------------------------------------------------------
// <copyright file="MosaicManager.Events.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Infrastructure
{
    using System;
    using LibVLCSharp.Shared;

    public partial class MosaicManager
    {
        public event EventHandler TileStarted;

        public event EventHandler TileChanged;

        protected void OnTileStarted(object sender, IVideoView tile, SourceConfig sourceConfig)
        {
            var index = Array.IndexOf(this.videoTiles, tile);
            this.OnTileStarted(sender, new TileSourceEventArgs(index, sourceConfig));
        }

        protected void OnTileStarted(object sender, TileSourceEventArgs args)
        {
            if (this.TileStarted != null)
            {
                this.TileStarted(sender, args);
            }
        }

        protected void OnTileChanged(object sender, IVideoView tile, SourceConfig sourceConfig)
        {
            var index = Array.IndexOf(this.videoTiles, tile);
            this.OnTileChanged(sender, new TileSourceEventArgs(index, sourceConfig));
        }

        protected void OnTileChanged(object sender, TileSourceEventArgs args)
        {
            if (this.TileChanged != null)
            {
                this.TileChanged(sender, args);
            }
        }
    }
}
