using Krk.Models;
using Microsoft.Azure.Cosmos;

namespace Krk.Repositories;

public class FavoritesRepository
{
    private readonly string containerName = "Favorites";
    private CosmosClient _client;
    private Container _container;

    public FavoritesRepository(string connectionString, string databaseName)
    {
        _client = new CosmosClient(connectionString);
        _container = _client.GetContainer(databaseName, containerName);
    }

    public async Task AddSongToFavorites(Favorite item)
    {
        await _container.CreateItemAsync(item);
    }

    public async Task<List<Favorite>> GetUserFavorites(string user)
    {
        var query = _container.GetItemQueryIterator<Favorite>(new QueryDefinition($"SELECT * FROM c WHERE c.user = '{user}' ORDER BY c.song.Artist ASC"));
        List<Favorite> results = new List<Favorite>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response.ToList());
        }

        return results;
    }

    public async Task RemoveSongFromFavorites(string id)
    {
        await _container.DeleteItemAsync<Favorite>(id, new PartitionKey(id));
    }
}