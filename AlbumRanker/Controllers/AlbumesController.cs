using AlbumRanker.DTOs.Albumes;
using AlbumRanker.DTOs.Canciones;
using AlbumRanker.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlbumRanker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class AlbumesController : ControllerBase
    {
        private readonly AlbumService _albumService;

        public AlbumesController(AlbumService albumService)
        {
            _albumService = albumService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var albums = await _albumService.GetAll();
            return Ok(albums);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var album = await _albumService.GetById(id);

            if (album == null)
                return NotFound();

            return Ok(album);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CrearAlbumDTO dto)
        {
            try
            {
                var album = await _albumService.Create(dto);
                return CreatedAtAction(nameof(GetById), new { id = album.Id }, album);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("{albumId}/canciones")]
        public async Task<IActionResult> AddCancion(int albumId, [FromBody] CrearCancionDTO dto)
        {
            try
            {
                var cancion = await _albumService.AddCancion(albumId, dto);
                if (cancion == null)
                    return NotFound();

                return CreatedAtAction(nameof(GetById), new { id = albumId }, cancion);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ActualizarAlbumDTO dto)
        {
            try
            {
                var success = await _albumService.Update(id, dto);
                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _albumService.Delete(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
