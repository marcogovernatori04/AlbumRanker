namespace AlbumRanker.Web.DTOs.Albums
{
    public class ActualizarAlbumDTO
    {
        public string Titulo { get; set; } = string.Empty;
        public string NombreArtista { get; set; } = string.Empty;
        public int? Anio { get; set; }
        public string? UrlPortada { get; set; }
    }


}
