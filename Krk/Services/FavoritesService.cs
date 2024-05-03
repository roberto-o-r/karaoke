using Krk.Models;
using Krk.Repositories;

namespace Krk.Services;

public class FavoritesService
{
    private readonly FavoritesRepository favoritesRepository;

    public FavoritesService(FavoritesRepository favoritesRepository)
    {
        this.favoritesRepository = favoritesRepository;
    }

    public async Task AddSongToFavorites(string user, Song song)
    {
        var favorite = new Favorite() { Id = Guid.NewGuid().ToString(), User = user, Song = song, Order = DateTime.UtcNow.Ticks };
        await favoritesRepository.AddSongToFavorites(favorite);
    }

    public async Task<List<Favorite>> GetUserFavorites(string user)
    {
        return await favoritesRepository.GetUserFavorites(user);
    }

    public async Task RemoveSongFromFavorites(string id)
    {
        await favoritesRepository.RemoveSongFromFavorites(id);
    }
}