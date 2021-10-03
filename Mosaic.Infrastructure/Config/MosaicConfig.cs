namespace Mosaic.Infrastructure
{
    using System.Collections.Generic;

    public class MosaicConfig
    {
        public IEnumerable<SourceConfig> Sources { get; set; }
    }
}
