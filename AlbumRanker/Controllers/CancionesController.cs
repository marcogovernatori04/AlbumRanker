using AlbumRanker.DTOs.Canciones;
using AlbumRanker.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlbumRanker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CancionesController : ControllerBase
    {
        private readonly CancionService _cancionService;

        public CancionesController(CancionService cancionService)
        {
            _cancionService = cancionService;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ActualizarCancionDTO dto)
        {
            try
            {
                var success = await _cancionService.Update(id, dto);
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
            var success = await _cancionService.Delete(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
