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
        public IEnumerable<MediaEntry> Sources { get; set; } = Enumerable.Empty<MediaEntry>();
    }

    public record MediaEntry(string Source, string? DisplayName = null);
}
