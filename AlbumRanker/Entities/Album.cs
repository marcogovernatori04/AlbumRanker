namespace AlbumRanker.Entities
{
    public class Album
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public int? Anio { get; set; }
        public string? UrlPortada { get; set; }

        public int ArtistaId { get; set; }
        public Artista Artista { get; set; } = null!;

        public List<Cancion> Canciones { get; set; } = new();
    }
}
