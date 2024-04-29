namespace Krk.Models;

public class QueueItem
{
    public required string Id { get; set; }

    public required string UserName { get; set; }

    public required Song Song { get; set; }
}