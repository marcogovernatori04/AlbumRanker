using AlbumRanker.Data;
using AlbumRanker.Entities;
using AlbumRanker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AlbumRanker.Repositories
{
    public class CancionRepository : ICancionRepository
    {
        private readonly ApplicationDbContext _context;

        public CancionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cancion?> GetById(int id)
        {
           return await _context.Canciones.FirstOrDefaultAsync(c => c.Id == id); 
        }
        public async Task Add(Cancion cancion)
        {
            _context.Canciones.Add(cancion);
            await _context.SaveChangesAsync();
        }
        public async Task Update(Cancion cancion)
        {
            _context.Canciones.Update(cancion);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Cancion cancion)
        {
            _context.Canciones.Remove(cancion);
            await _context.SaveChangesAsync();
        }
    }
}
