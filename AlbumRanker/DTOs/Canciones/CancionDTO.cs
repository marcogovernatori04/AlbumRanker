namespace AlbumRanker.DTOs.Canciones
{
    public class CancionDTO
    {
        public int Id { get; set; }
        public int NumeroPista { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public int DuracionSegundos { get; set; }
        public decimal? Puntuacion { get; set; }
        public string? Nota { get; set; }
    }
}
