using AlbumRanker.Data;
using AlbumRanker.Entities;
using AlbumRanker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AlbumRanker.Repositories
{
    public class ArtistaRepository : IArtistaRepository
    {
        private readonly ApplicationDbContext _context;

        public ArtistaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Artista?> GetByNombre(string nombre)
        {
            return await _context.Artistas.FirstOrDefaultAsync(a => a.Nombre.ToLower() == nombre.ToLower());
        }

        public async Task<List<Artista>> GetAll()
        {
           return await _context.Artistas.ToListAsync();
        }

        public async Task<Artista?> GetByIdConAlbumes(int id)
        {
            return await _context.Artistas
                .Include(a => a.Albumes)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task Add(Artista artista)
        {
            _context.Artistas.Add(artista);
            await _context.SaveChangesAsync();
        }
    }
}
