namespace Mosaic.Infrastructure
{
    using System.IO;
    using Newtonsoft.Json;

    public class ConfigLoader<T> where T : LibraryConfig
    {
        private readonly JsonSerializer Serializer;

        public ConfigLoader(JsonSerializer serializer)
        {
            this.Serializer = serializer;
        }

        public T LoadConfigFile(string file)
        {
            using (var stream = File.Open(file, FileMode.Open, FileAccess.Read))
            using (var streamReader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                return this.Serializer.Deserialize<T>(jsonReader);
            }
        }
    }
}
