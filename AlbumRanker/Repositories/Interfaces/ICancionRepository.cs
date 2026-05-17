using AlbumRanker.Entities;

namespace AlbumRanker.Repositories.Interfaces
{
    public interface ICancionRepository
    {
        Task<Cancion?> GetById(int id);
        Task Add(Cancion cancion);
        Task Update(Cancion cancion);
        Task Delete(Cancion cancion);
    }
}
