namespace Mosaic.Infrastructure
{
    using System.Collections.Generic;

    public class LibraryConfig
    {
        public IEnumerable<SourceConfig> Sources { get; set; }
    }
}
