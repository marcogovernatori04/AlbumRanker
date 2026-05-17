using AlbumRanker.Entities;

namespace AlbumRanker.Repositories.Interfaces
{
    public interface IAlbumRepository
    {
        public Task<List<Album>> GetAll();
        public Task<Album?> GetById(int id);
        public Task Add(Album album);
        public Task Update(Album album);
        public Task Delete(Album album);
    }
}
