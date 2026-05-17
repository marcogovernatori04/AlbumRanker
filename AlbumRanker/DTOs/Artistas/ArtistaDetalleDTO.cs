using AlbumRanker.DTOs.Albumes;

namespace AlbumRanker.DTOs.Artistas
{
    public class ArtistaDetalleDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public List<AlbumListadoDTO> Albumes { get; set; } = new();
    }

}
