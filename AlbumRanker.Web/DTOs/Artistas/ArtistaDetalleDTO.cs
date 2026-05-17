using AlbumRanker.Web.DTOs.Albums;

namespace AlbumRanker.Web.DTOs.Artistas
{
    public class ArtistaDetalleDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public List<AlbumListadoDTO> Albums { get; set; } = new();
    }

}
