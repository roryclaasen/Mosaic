namespace Mosaic.Infrastructure
{
    using System;

    public class TileSwapEventArgs : EventArgs
    {
        public int TileIndex { get; private set; }
        public SourceConfig SourceConfig { get; private set; }

        public TileSwapEventArgs(int tileIndex, SourceConfig sourceConfig)
        {
            this.TileIndex = tileIndex;
            this.SourceConfig = sourceConfig;
        }
    }
}
