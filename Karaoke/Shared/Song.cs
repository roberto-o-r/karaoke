using Newtonsoft.Json;

namespace Karaoke.Shared
{
    public class Song
    {
        [JsonProperty("interprete")]
        public string Artist { get; set; }
        [JsonProperty("cancion")]
        public string Name { get; set; }
    }
}
