using AlbumRanker.Entities;

namespace AlbumRanker.Repositories.Interfaces
{
    public interface IArtistaRepository
    {
        Task<Artista?> GetByNombre(string nombre);
        Task<List<Artista>> GetAll();
        Task<Artista?> GetByIdConAlbumes(int id);
        Task Add(Artista artista);
    }
}
