// ------------------------------------------------------------------------------
// <copyright file="MosaicConfigLoader.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Infrastructure.Config
{
    using Mosaic.Infrastructure.Config.Loader;

    public class MosaicConfigLoader : ConfigLoader<MosaicConfig>, IConfigLoader<MosaicConfig>
    {
        public MosaicConfigLoader()
            : base()
        {
        }
    }
}
