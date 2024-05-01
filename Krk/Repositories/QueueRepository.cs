using Krk.Models;
using Microsoft.Azure.Cosmos;

namespace Krk.Repositories;

public class QueueRepository
{
    private readonly string containerName = "SongsQueue";
    private CosmosClient _client;
    private Container _container;

    public QueueRepository(string connectionString, string databaseName)
    {
        _client = new CosmosClient(connectionString);
        _container = _client.GetContainer(databaseName, containerName);
    }

    public async Task AddSongToQueue(QueueItem item)
    {
        await _container.CreateItemAsync(item);
    }

    public async Task DeleteSongFromQueue(string id)
    {
        await _container.DeleteItemAsync<QueueItem>(id, new PartitionKey(id));
    }

    public async Task<List<QueueItem>> GetUserQueue(string username)
    {
        var query = _container.GetItemQueryIterator<QueueItem>(new QueryDefinition($"SELECT * FROM c WHERE c.UserName = '{username}'"));
        List<QueueItem> results = new List<QueueItem>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response.ToList());
        }

        return results;
    }
}