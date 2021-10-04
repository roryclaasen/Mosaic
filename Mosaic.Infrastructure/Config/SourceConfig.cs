// ------------------------------------------------------------------------------
// <copyright file="SourceConfig.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Infrastructure.Config
{
    using Newtonsoft.Json;

    public class SourceConfig
    {
        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
    }
}
