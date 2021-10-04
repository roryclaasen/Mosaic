// ------------------------------------------------------------------------------
// <copyright file="ConfigLoader.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Infrastructure.Config.Loader
{
    using Newtonsoft.Json;

    public class ConfigLoader : ConfigLoader<MosaicConfig>, IConfigLoader<MosaicConfig>
    {
        public ConfigLoader(JsonSerializer serializer)
            : base(serializer)
        {
        }
    }
}
