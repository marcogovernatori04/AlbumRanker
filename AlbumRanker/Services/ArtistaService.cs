using AlbumRanker.DTOs.Albumes;
using AlbumRanker.DTOs.Artistas;
using AlbumRanker.Entities;
using AlbumRanker.Repositories.Interfaces;

namespace AlbumRanker.Services
{
    public class ArtistaService
    {
        private readonly IArtistaRepository _artistaRepository;

        public ArtistaService(IArtistaRepository artistaRepository)
        {
            _artistaRepository = artistaRepository;
        }

        public async Task<List<ArtistaDTO>> GetAll()
        {
            var artistas = await _artistaRepository.GetAll();

            return artistas.Select(a => new ArtistaDTO
            {
                Id = a.Id,
                Nombre = a.Nombre
            }).ToList();
        }

        public async Task<ArtistaDetalleDTO?> GetByIdConAlbumes(int id)
        {
            var artista = await _artistaRepository.GetByIdConAlbumes(id);

            if (artista == null)
                return null;

            return new ArtistaDetalleDTO
            {
                Id = artista.Id,
                Nombre = artista.Nombre,
                Albumes = artista.Albumes
                .OrderBy(a => a.Anio)
                .Select(a => new AlbumListadoDTO
                {
                    Id = a.Id,
                    Titulo = a.Titulo,
                    Artista = artista.Nombre,
                    Anio = a.Anio,
                    UrlPortada = a.UrlPortada,
                    CantidadCanciones = a.Canciones.Count,
                    Promedio = CalcularPromedio(a.Canciones)
                }).ToList()
            };
        }

        private decimal? CalcularPromedio(List<Cancion> canciones)
        {
            var puntuadas = canciones
                .Where(c => c.Puntuacion.HasValue)
                .ToList();

            if (!puntuadas.Any())
                return null;

            return Math.Round(puntuadas.Average(c => c.Puntuacion!.Value), 1);
        }
    }
}
