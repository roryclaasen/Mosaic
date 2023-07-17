// ------------------------------------------------------------------------------
// <copyright file="MosaicConfig.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Infrastructure
{
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;

    public record MosaicConfig
    {
        [JsonProperty("sources")]
        public IEnumerable<SourceConfig> Sources { get; set; } = Enumerable.Empty<SourceConfig>();
    }

    public record SourceConfig
    {
        [JsonProperty("source")]
        public string Source { get; set; } = string.Empty;

        [JsonProperty("displayName")]
        public string DisplayName { get; set; } = string.Empty;
    }
}
