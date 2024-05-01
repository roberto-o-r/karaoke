using System.Text.Json.Serialization;

namespace Krk.Models;

public class Song
{
    [JsonPropertyName("interprete")]
    public required string Artist { get; set; }

    [JsonPropertyName("cancion")]
    public required string Name { get; set; }

    [JsonPropertyName("favorite")]
    public bool Favorite { get; set; }
}