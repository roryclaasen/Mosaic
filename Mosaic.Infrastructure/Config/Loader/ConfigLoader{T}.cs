// ------------------------------------------------------------------------------
// <copyright file="ConfigLoader{T}.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Infrastructure.Config.Loader
{
    using System.IO;
    using Newtonsoft.Json;

    public class ConfigLoader<T> : IConfigLoader<T>
        where T : MosaicConfig
    {
        private readonly JsonSerializer serializer;

        public ConfigLoader(JsonSerializer serializer)
        {
            this.serializer = serializer;
        }

        public T LoadConfigFile(string file)
        {
            using (var stream = File.Open(file, FileMode.Open, FileAccess.Read))
            using (var streamReader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                return this.serializer.Deserialize<T>(jsonReader);
            }
        }
    }
}
