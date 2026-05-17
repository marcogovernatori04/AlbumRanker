namespace AlbumRanker.Entities
{
    public class Artista
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public List<Album> Albumes { get; set; } = new();
    }
}
