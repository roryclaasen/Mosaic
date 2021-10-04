// ------------------------------------------------------------------------------
// <copyright file="IConfigLoader{T}.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Infrastructure.Config.Loader
{
    public interface IConfigLoader<T>
    {
        T LoadConfigFile(string file);
    }
}
