using Microsoft.Azure.Cosmos;

namespace Krk.Repositories;

public class QueueRepository
{
    private CosmosClient _client;
    private Container _container;

    public QueueRepository(string connectionString, string databaseName, string containerName)
    {
        _client = new CosmosClient(connectionString);
        _container = _client.GetContainer(databaseName, containerName);
    }

    public async Task AddItemAsync<T>(T item)
    {
        await _container.CreateItemAsync(item);
    }

    public async Task<List<T>> GetItemsAsync<T>(string queryString)
    {
        var query = _container.GetItemQueryIterator<T>(new QueryDefinition(queryString));
        List<T> results = new List<T>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();

            results.AddRange(response.ToList());
        }

        return results;
    }
}