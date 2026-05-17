namespace AlbumRanker.Entities
{
    public class Cancion
    {
        public int Id { get; set; }

        public int AlbumId { get; set; }
        public Album Album { get; set; } = null!;

        public int NumeroPista { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public int DuracionSegundos { get; set; }
        public decimal? Puntuacion { get; set; }
        public string? Nota { get; set; }

    }
}
