// ------------------------------------------------------------------------------
// <copyright file="ConfigLoader.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Infrastructure
{
    using Newtonsoft.Json;

    public class ConfigLoader : ConfigLoader<MosaicConfig>
    {
        public ConfigLoader(JsonSerializer serializer)
            : base(serializer)
        {
        }
    }
}
