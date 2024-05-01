using System.Text.Json;
using Newtonsoft.Json;

namespace Krk.Models;

public class QueueItem
{
    [JsonProperty(PropertyName = "id")]
    public required string Id { get; set; }

    public required string UserName { get; set; }

    public required Song Song { get; set; }

    public required long Order { get; set; }
}