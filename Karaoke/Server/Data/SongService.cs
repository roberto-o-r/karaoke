using Karaoke.Shared;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Karaoke.Server.Data
{
    public class SongService
    {
        private readonly IWebHostEnvironment env;
        private List<Song> songs;

        public SongService(IWebHostEnvironment env)
        {
            this.env = env;
            LoadSongs();
        }

        public void LoadSongs()
        {
            using (StreamReader r = new StreamReader(env.ContentRootPath + "/data/canciones.json", Encoding.UTF8))
            {
                string json = r.ReadToEnd();
                songs = JsonConvert.DeserializeObject<List<Song>>(json);
            }
        }

        public List<Song> SearchSongs(string searchText = null)
        {
            var filteredSongs = songs;

            if (!string.IsNullOrEmpty(searchText))
            {
                filteredSongs = filteredSongs.Where(s => s.Artist.ToLower().Contains(searchText) || s.Name.ToLower().Contains(searchText)).ToList();
            }

            return filteredSongs;
        }

        public bool UpdateSongs(string data)
        {
            try
            {
                var songsData = JsonConvert.DeserializeObject<List<Song>>(data);
                using (StreamWriter file = File.CreateText(env.ContentRootPath + "/data/canciones.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, songsData);
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
}
