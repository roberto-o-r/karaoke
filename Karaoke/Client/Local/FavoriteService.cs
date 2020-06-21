using Blazored.LocalStorage;
using Karaoke.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Karaoke.Client.Local
{
    public class FavoriteService
    {
        private const string favoritesKey = "favorites";
        private readonly ILocalStorageService localStorageService;

        public FavoriteService(ILocalStorageService localStorageService)
        {
            this.localStorageService = localStorageService;
        }

        public async Task<List<Song>> GetFavorites()
        {
            var favorites = await localStorageService.GetItemAsync<List<Song>>(favoritesKey);
            if (favorites == null)
            {
                favorites = new List<Song>();
            }
            return favorites.OrderBy(f => f.Artist).ToList();
        }

        public async Task AddFavorite(Song song)
        {
            var favorites = await GetFavorites();
            favorites.Add(song);
            await localStorageService.SetItemAsync(favoritesKey, favorites);
        }

        public async Task RemoveFavorite(Song song)
        {
            var favorites = await GetFavorites();
            favorites.RemoveAll(f => f.Artist == song.Artist && f.Name == song.Name);
            await localStorageService.SetItemAsync(favoritesKey, favorites);
        }

    }
}
