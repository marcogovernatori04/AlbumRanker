using AlbumRanker.DTOs.Canciones;

namespace AlbumRanker.DTOs.Albumes
{
    public class AlbumDetalleDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Artista { get; set; } = string.Empty;
        public int? Anio { get; set; }
        public string? UrlPortada { get; set; }
        public decimal? Promedio { get; set; }
        public List<CancionDTO> Canciones { get; set; } = new();
    }
}
