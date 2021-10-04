// ------------------------------------------------------------------------------
// <copyright file="MosaicApplicationConfig.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Infrastructure.Config
{
    using Newtonsoft.Json;

    public class MosaicApplicationConfig : MosaicConfig
    {
        [JsonProperty("fullScreen")]
        public bool FullScreen { get; set; }

        [JsonProperty("showTitles")]
        public bool ShowTitles { get; set; }
    }
}
