using AlbumRanker.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlbumRanker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtistasController : ControllerBase
    {
        private readonly ArtistaService _artistaService;

        public ArtistasController(ArtistaService artistaService)
        {
            _artistaService = artistaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var artistas = await _artistaService.GetAll();

            return Ok(artistas);
        }

        [HttpGet("{id}/albumes")]
        public async Task<IActionResult> GetByIdConAlbumes(int id)
        {
            var artista = await _artistaService.GetByIdConAlbumes(id);

            if (artista == null)
                return NotFound();

            return Ok(artista);
        }
    }
}