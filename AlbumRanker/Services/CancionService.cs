using AlbumRanker.DTOs.Canciones;
using AlbumRanker.Repositories.Interfaces;

namespace AlbumRanker.Services
{
    public class CancionService
    {
        private readonly ICancionRepository _cancionRepository;

        public CancionService(ICancionRepository cancionRepository)
        {
            _cancionRepository = cancionRepository;
        }

        public async Task<bool> Update(int id, ActualizarCancionDTO dto)
        {
            ValidarCancion(dto);

            var cancion = await _cancionRepository.GetById(id);
            if (cancion == null) 
                return false;

            cancion.NumeroPista = dto.NumeroPista;
            cancion.Titulo = dto.Titulo;
            cancion.DuracionSegundos = dto.DuracionSegundos;
            cancion.Puntuacion = dto.Puntuacion;
            cancion.Nota = dto.Nota;

            await _cancionRepository.Update(cancion);
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var cancion = await _cancionRepository.GetById(id);
            if (cancion == null) 
                return false;

            await _cancionRepository.Delete(cancion);
            return true;
        }

        private void ValidarCancion(ActualizarCancionDTO dto)
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
