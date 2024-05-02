using System.Text.Json;
using Newtonsoft.Json;

namespace Krk.Models;

public class QueueItem
{
    [JsonProperty(PropertyName = "id")]
    public required string Id { get; set; }

    [JsonProperty(PropertyName = "user")]
    public required string User { get; set; }

    [JsonProperty(PropertyName = "song")]
    public required Song Song { get; set; }

    [JsonProperty(PropertyName = "order")]
    public required long Order { get; set; }
}