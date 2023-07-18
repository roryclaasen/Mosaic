// ------------------------------------------------------------------------------
// <copyright file="MosaicConfig.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Infrastructure.Config
{
    using System.Collections.Generic;
    using System.Linq;

    public record MosaicConfig
    {
        public IEnumerable<SourceConfig> Sources { get; set; } = Enumerable.Empty<SourceConfig>();
    }

    public record SourceConfig
    {
        public string Source { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;
    }
}
