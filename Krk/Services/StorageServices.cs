using Microsoft.Azure.Cosmos;

public class StorageService
{
    private CosmosClient _client;
    private Container _container;

    public StorageService(string connectionString, string databaseName, string containerName)
    {
        _client = new CosmosClient(connectionString);
        _container = _client.GetContainer(databaseName, containerName);
    }

    public async Task AddItemAsync<T>(T item)
    {
        await _container.CreateItemAsync(item);
    }
}