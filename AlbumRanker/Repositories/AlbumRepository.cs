using AlbumRanker.Data;
using AlbumRanker.Entities;
using AlbumRanker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AlbumRanker.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly ApplicationDbContext _context;

        public AlbumRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Album>> GetAll()
        {
            return await _context.Albumes
                .Include(a => a.Artista)
                .Include(a => a.Canciones)
                .ToListAsync();
        }

        public async Task<Album?> GetById(int id)
        {
            return await _context.Albumes
                .Include(a => a.Artista)
                .Include(a => a.Canciones)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task Add(Album album)
        {
            _context.Albumes.Add(album);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Album album)
        {
            _context.Albumes.Update(album);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Album album)
        {
           _context.Remove(album); 
            await _context.SaveChangesAsync();
        }
    }
}
