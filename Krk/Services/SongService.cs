using Krk.Models;
using System.Text;
using System.Text.Json;

namespace Krk.Services;

public class SongService
{
    private readonly IWebHostEnvironment env;
    private List<Song>? songs;

    public SongService(IWebHostEnvironment env)
    {
        this.env = env;
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
}
