using Krk.Models;
using Krk.Repositories;
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
            filteredSongs = filteredSongs.Where(s => s.Artist.ToLower().Contains(searchText) || s.Name.ToLower().Contains(searchText)).ToList();
        }

        return filteredSongs;
    }

    public bool UpdateSongs(string data)
    {
        try
        {
            var songsData = JsonSerializer.Serialize(data);
            using (StreamWriter file = File.CreateText(env.ContentRootPath + "/data/canciones.json"))
            {
                file.Write(songsData);
            }
            LoadSongs();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task AddSongToQueue(string userName, Song song)
    {
        var item = new { id = Guid.NewGuid().ToString(), UserName = userName, Song = song, Order = DateTime.UtcNow.Ticks };
        await songsRepository.AddItemAsync(item);
    }

    public async Task<List<Song>> GetUserQueue(string userName)
    {
        var queue =  await songsRepository.GetItemsAsync<QueueItem>($"SELECT * FROM c");
        return queue.Select(x => x.Song).ToList();
    }
}
