// ------------------------------------------------------------------------------
// <copyright file="MosaicApplicationConfig.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic
{
    using Mosaic.Infrastructure;
    using Newtonsoft.Json;

    public class MosaicApplicationConfig : MosaicConfig
    {
        [JsonProperty("fullScreen")]
        public bool FullScreen { get; set; }
    }
}
