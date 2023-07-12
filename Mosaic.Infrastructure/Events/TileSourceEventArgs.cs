// ------------------------------------------------------------------------------
// <copyright file="TileSourceEventArgs.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Infrastructure
{
    using System;

    public class TileSourceEventArgs : EventArgs
    {
        public TileSourceEventArgs(int tileIndex, SourceConfig sourceConfig)
        {
            this.TileIndex = tileIndex;
            this.SourceConfig = sourceConfig;
        }

        public int TileIndex { get; private set; }

        public SourceConfig SourceConfig { get; private set; }
    }
}
