// ------------------------------------------------------------------------------
// <copyright file="ConfigLoader{T}.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Infrastructure.Config.Loader
{
    using System.IO;
    using System.Text.Json;

    public class ConfigLoader<T> : IConfigLoader<T>
        where T : MosaicConfig
    {
        private readonly JsonSerializerOptions options;

        public ConfigLoader()
            : this(null)
        {
        }

        public ConfigLoader(JsonSerializerOptions? options)
        {
            this.options = options ?? new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public T? LoadConfigFile(string file)
        {
            using (var stream = File.Open(file, FileMode.Open, FileAccess.Read))
            using (var streamReader = new StreamReader(stream))
            {
                return JsonSerializer.Deserialize<T>(stream, this.options);
            }
        }
    }
}
