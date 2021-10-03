namespace Mosaic.Infrastructure
{
    using Newtonsoft.Json;

    public class ConfigLoader : ConfigLoader<LibraryConfig>
    {
        public ConfigLoader(JsonSerializer serializer)
            : base(serializer)
        {
        }
    }
}
