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
