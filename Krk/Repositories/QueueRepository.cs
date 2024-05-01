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

    public async Task<List<QueueItem>> GetUserQueue(string user)
    {
        var query = _container.GetItemQueryIterator<QueueItem>(new QueryDefinition($"SELECT * FROM c WHERE c.user = '{user}'"));
        List<QueueItem> results = new List<QueueItem>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response.ToList());
        }

        return results;
    }

    public async Task<List<QueueItem>> GetQueue()
    {
        var query = _container.GetItemQueryIterator<QueueItem>(new QueryDefinition("SELECT * FROM c"));
        List<QueueItem> results = new List<QueueItem>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response.ToList());
        }

        return results;
    }

    public async Task ClearQueue(string user)
    {
        var query = new QueryDefinition($"SELECT * FROM c WHERE c.user = '{user}'");
        var iterator = _container.GetItemQueryIterator<QueueItem>(query);

        var tasks = new List<Task>();

        while (iterator.HasMoreResults)
        {
            foreach (var item in await iterator.ReadNextAsync())
            {
                tasks.Add(_container.DeleteItemAsync<QueueItem>(item.Id, new PartitionKey(item.Id)));
            }
        }

        await Task.WhenAll(tasks);
    }
}