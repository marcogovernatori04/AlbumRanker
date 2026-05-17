using AlbumRanker.DTOs.Albumes;
using AlbumRanker.DTOs.Canciones;
using AlbumRanker.Entities;
using AlbumRanker.Repositories.Interfaces;

namespace AlbumRanker.Services
{
    public class AlbumService
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly IArtistaRepository _artistaRepository;
        private readonly ICancionRepository _cancionRepository;

        public AlbumService(IAlbumRepository albumRepository, IArtistaRepository artistaRepository, ICancionRepository cancionRepository)
        {
            _albumRepository = albumRepository;
            _artistaRepository = artistaRepository;
            _cancionRepository = cancionRepository;
        }

        public async Task<List<AlbumListadoDTO>> GetAll()
        {
            var albums = await _albumRepository.GetAll();

            return albums.Select(a => new AlbumListadoDTO
            {
                Id = a.Id,
                Titulo = a.Titulo,
                Artista = a.Artista.Nombre,
                Anio = a.Anio,
                UrlPortada = a.UrlPortada,
                CantidadCanciones = a.Canciones.Count,
                Promedio = CalcularPromedio(a.Canciones)
            }).ToList();
        }

        public async Task<AlbumDetalleDTO?> GetById(int id)
        {
            var album = await _albumRepository.GetById(id);

            if (album == null)
                return null;

            return new AlbumDetalleDTO
            {
                Id = album.Id,
                Titulo = album.Titulo,
                Artista = album.Artista.Nombre,
                Anio = album.Anio,
                UrlPortada = album.UrlPortada,
                Promedio = CalcularPromedio(album.Canciones),
                Canciones = album.Canciones
                    .OrderBy(c => c.NumeroPista)
                    .Select(c => new CancionDTO
                {
                    Id = c.Id,
                    NumeroPista = c.NumeroPista,
                    Titulo = c.Titulo,
                    DuracionSegundos = c.DuracionSegundos,
                    Puntuacion = c.Puntuacion,
                    Nota = c.Nota
                }).ToList()
            };
        }

        public async Task<AlbumDetalleDTO> Create(CrearAlbumDTO dto)
        {
            ValidarAlbum(dto);

            var artista = await _artistaRepository.GetByNombre(dto.NombreArtista);

            if (artista == null)
            {
                artista = new Artista
                {
                    Nombre = dto.NombreArtista
                };

                await _artistaRepository.Add(artista);
            }

            var album = new Album
            {
                Titulo = dto.Titulo,
                Anio = dto.Anio,
                UrlPortada = dto.UrlPortada,
                Artista = artista,
                Canciones = dto.Canciones.Select(c => new Cancion
                {
                    NumeroPista = c.NumeroPista,
                    Titulo = c.Titulo,
                    DuracionSegundos = c.DuracionSegundos,
                    Puntuacion = c.Puntuacion,
                    Nota = c.Nota
                }).ToList()
            };

            await _albumRepository.Add(album);

            return new AlbumDetalleDTO
            {
                Id = album.Id,
                Titulo = album.Titulo,
                Artista = album.Artista.Nombre,
                Anio = album.Anio,
                UrlPortada = album.UrlPortada,
                Promedio = CalcularPromedio(album.Canciones),
                Canciones = album.Canciones
                    .OrderBy(c => c.NumeroPista)
                    .Select(c => new CancionDTO
                    {
                        Id = c.Id,
                        NumeroPista = c.NumeroPista,
                        Titulo = c.Titulo,
                        DuracionSegundos = c.DuracionSegundos,
                        Puntuacion = c.Puntuacion,
                        Nota = c.Nota
                    }).ToList()
            };
        }

        public async Task<CancionDTO?> AddCancion(int albumId, CrearCancionDTO dto)
        {
            ValidarCancion(dto);
            var album = await _albumRepository.GetById(albumId);

            if (album == null)
                return null;

            var cancion = new Cancion
            {
                AlbumId = albumId,
                NumeroPista = dto.NumeroPista,
                Titulo = dto.Titulo,
                DuracionSegundos = dto.DuracionSegundos,
                Puntuacion = dto.Puntuacion,
                Nota = dto.Nota
            };

            await _cancionRepository.Add(cancion);

            return new CancionDTO
            {
                Id = cancion.Id,
                NumeroPista = cancion.NumeroPista,
                Titulo = cancion.Titulo,
                DuracionSegundos = cancion.DuracionSegundos,
                Puntuacion = cancion.Puntuacion,
                Nota = cancion.Nota
            };
        }

        public async Task<bool> Update(int id, ActualizarAlbumDTO dto)
        {
            ValidarAlbum(dto);
            var album = await _albumRepository.GetById(id);

            if (album == null)
                return false;

            var artista = await _artistaRepository.GetByNombre(dto.NombreArtista);

            if (artista == null)
            {
                artista = new Artista
                {
                    Nombre = dto.NombreArtista
                };

                await _artistaRepository.Add(artista);
            }

            album.Titulo = dto.Titulo;
            album.Anio = dto.Anio;
            album.UrlPortada = dto.UrlPortada;
            album.Artista = artista;

            await _albumRepository.Update(album);
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var album = await _albumRepository.GetById(id);

            if (album == null)
                return false;

            await _albumRepository.Delete(album);
            return true;
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

        private void ValidarAlbum(CrearAlbumDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Titulo))
                throw new ArgumentException("El título del álbum es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.NombreArtista))
                throw new ArgumentException("El nombre del artista es obligatorio.");

            if (dto.Anio.HasValue && dto.Anio.Value < 0)
                throw new ArgumentException("El año no puede ser negativo.");

            foreach (var cancion in dto.Canciones)
            {
                ValidarCancion(cancion);
            }

            var pistasDuplicadas = dto.Canciones
                .GroupBy(c => c.NumeroPista)
                .Any(g => g.Count() > 1);

            if (pistasDuplicadas)
                throw new ArgumentException("No puede haber canciones con el mismo número de pista.");
        }

        private void ValidarAlbum(ActualizarAlbumDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Titulo))
                throw new ArgumentException("El título del álbum es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.NombreArtista))
                throw new ArgumentException("El nombre del artista es obligatorio.");

            if (dto.Anio.HasValue && dto.Anio.Value < 0)
                throw new ArgumentException("El año no puede ser negativo.");
        }

        private void ValidarCancion(CrearCancionDTO dto)
        {
            if (dto.NumeroPista <= 0)
                throw new ArgumentException("El número de pista debe ser mayor a 0.");

            if (string.IsNullOrWhiteSpace(dto.Titulo))
                throw new ArgumentException("El título de la canción es obligatorio.");

            if (dto.DuracionSegundos < 0)
                throw new ArgumentException("La duración no puede ser negativa.");

            if (dto.Puntuacion.HasValue && (dto.Puntuacion < 0 || dto.Puntuacion > 10))
                throw new ArgumentException("La puntuación debe estar entre 0 y 10.");
        }
    }
}
