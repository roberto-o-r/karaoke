using Karaoke.Server.Data;
using Karaoke.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Karaoke.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SongsController : ControllerBase
    {
        private readonly SongService songService;

        public SongsController(SongService songService)
        {
            this.songService = songService;
        }

        [HttpGet]
        public IEnumerable<Song> Get(string searchText)
        {
            var filteredSongs = songService.SearchSongs(searchText);
            return filteredSongs;
        }

        [HttpPost]
        public bool Post()
        {
            songService.LoadSongs();
            return true;
        }
    }
}
