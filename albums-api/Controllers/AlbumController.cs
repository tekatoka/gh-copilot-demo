using albums_api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using System.Text;

namespace albums_api.Controllers
{
    [Route("albums")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        // GET: api/albums - Get all albums
        [HttpGet]
        public IActionResult Get()
        {
            var albums = Album.GetAll();
            return Ok(albums);
        }

        // GET: api/albums/5 - Get album by id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var album = Album.GetById(id);
            if (album == null)
            {
                return NotFound(new { message = $"Album with id {id} not found" });
            }
            return Ok(album);
        }

        // GET: api/albums/search/year?year=2020 - Search albums by year
        [HttpGet("search/year")]
        public IActionResult SearchByYear(int year)
        {
            var albums = Album.GetByYear(year);
            if (!albums.Any())
            {
                return NotFound(new { message = $"No albums found for year {year}" });
            }
            return Ok(albums);
        }

        // GET: api/albums/sorted?sortBy=title - Get sorted albums
        [HttpGet("sorted")]
        public IActionResult GetSorted(string sortBy)
        {
            var albums = Album.GetAlbums();
            switch (sortBy.ToLower())
            {
                case "title":
                    albums = albums.OrderBy(a => a.Title).ToList();
                    break;
                case "artist":
                    albums = albums.OrderBy(a => a.Artist).ToList();
                    break;
                case "price":
                    albums = albums.OrderBy(a => a.Price).ToList();
                    break;
                default:
                    return BadRequest("Invalid sort parameter. Use 'title', 'artist', or 'price'.");
            }
            return Ok(albums);
        }

        // POST: api/albums - Create a new album
        [HttpPost]
        public IActionResult Create([FromBody] Album album)
        {
            if (album == null)
            {
                return BadRequest("Album data is required");
            }

            if (string.IsNullOrWhiteSpace(album.Title) || string.IsNullOrWhiteSpace(album.Artist))
            {
                return BadRequest("Title and Artist are required fields");
            }

            if (album.Price < 0)
            {
                return BadRequest("Price cannot be negative");
            }

            var createdAlbum = Album.Add(album);
            return CreatedAtAction(nameof(Get), new { id = createdAlbum.Id }, createdAlbum);
        }

        // PUT: api/albums/5 - Update an album
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Album album)
        {
            if (album == null)
            {
                return BadRequest("Album data is required");
            }

            if (string.IsNullOrWhiteSpace(album.Title) || string.IsNullOrWhiteSpace(album.Artist))
            {
                return BadRequest("Title and Artist are required fields");
            }

            if (album.Price < 0)
            {
                return BadRequest("Price cannot be negative");
            }

            var existingAlbum = Album.GetById(id);
            if (existingAlbum == null)
            {
                return NotFound(new { message = $"Album with id {id} not found" });
            }

            var updatedAlbum = Album.Update(id, album);
            return Ok(updatedAlbum);
        }

        // DELETE: api/albums/5 - Delete an album
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var album = Album.GetById(id);
            if (album == null)
            {
                return NotFound(new { message = $"Album with id {id} not found" });
            }

            Album.Delete(id);
            return NoContent();
        }
    }
}
