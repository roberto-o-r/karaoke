using Krk.Models;
using Krk.Repositories;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace Krk.Services;

public class SongService
{
    private readonly IWebHostEnvironment env;
    private readonly QueueRepository songsRepository;
    private List<Song>? songs;

    public SongService(IWebHostEnvironment env, QueueRepository songsRepository)
    {
        this.env = env;
        this.songsRepository = songsRepository;
        LoadSongs();
    }

    public void LoadSongs()
    {
        try
        {
            using (StreamReader r = new StreamReader(env.ContentRootPath + "/data/canciones.json", Encoding.UTF8))
            {
                string json = r.ReadToEnd();
                songs = JsonSerializer.Deserialize<List<Song>>(json);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            songs = new List<Song>();
        }
    }

    public List<Song>? SearchSongs(string? searchText = null)
    {
        var filteredSongs = songs;
        if (filteredSongs != null && !string.IsNullOrEmpty(searchText))
        {
            var compareInfo = CultureInfo.InvariantCulture.CompareInfo;
            filteredSongs = filteredSongs.Where(s =>
                compareInfo.IndexOf(s.Artist.ToLower(), searchText.ToLower(), CompareOptions.IgnoreNonSpace) > -1 ||
                compareInfo.IndexOf(s.Name.ToLower(), searchText.ToLower(), CompareOptions.IgnoreNonSpace) > -1).ToList();
        }

        return filteredSongs;
    }

    public async Task AddSongToQueue(string user, Song song)
    {
        var item = new QueueItem() { Id = Guid.NewGuid().ToString(), User = user, Song = song, Order = DateTime.UtcNow.Ticks };
        await songsRepository.AddSongToQueue(item);
    }

    public async Task RemoveSongFromQueue(string id)
    {
        await songsRepository.DeleteSongFromQueue(id);
    }

    public async Task<List<QueueItem>> GetUserQueue(string user)
    {
        return await songsRepository.GetUserQueue(user);
    }

    public async Task<List<QueueItem>> GetQueue()
    {
        return await songsRepository.GetQueue();
    }

    public Task ClearQueue(string user)
    {
        return songsRepository.ClearQueue(user);
    }
}
