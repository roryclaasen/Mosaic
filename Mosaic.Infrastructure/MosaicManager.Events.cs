// ------------------------------------------------------------------------------
// <copyright file="MosaicManager.Events.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Infrastructure
{
    using System;

    public partial class MosaicManager
    {
        public event EventHandler TileChanged;

        protected void OnTileChanged(object sender, TileSwapEventArgs args)
        {
            if (this.TileChanged != null)
            {
                this.TileChanged(sender, args);
            }
        }
    }
}
