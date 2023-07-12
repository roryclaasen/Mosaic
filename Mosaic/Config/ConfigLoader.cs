// ------------------------------------------------------------------------------
// <copyright file="ConfigLoader.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic
{
    using Mosaic.Infrastructure;
    using Newtonsoft.Json;

    public class ConfigLoader : ConfigLoader<MosaicConfig>, IConfigLoader<MosaicConfig>
    {
        public ConfigLoader(JsonSerializer serializer)
            : base(serializer)
        {
        }
    }
}
