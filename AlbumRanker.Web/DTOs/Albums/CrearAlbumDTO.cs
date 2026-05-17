using AlbumRanker.Web.DTOs.Canciones;

namespace AlbumRanker.Web.DTOs.Albums
{
    public class CrearAlbumDTO
    {
        public string Titulo { get; set; } = string.Empty;
        public string NombreArtista { get; set; } = string.Empty;
        public int? Anio { get; set; }
        public string? UrlPortada { get; set; }

        public List<CrearCancionDTO> Canciones { get; set; } = new();
    }
}
