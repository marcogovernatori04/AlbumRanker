using AlbumRanker.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlbumRanker.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Artista> Artistas { get; set; }
        public DbSet<Album> Albumes { get; set; }
        public DbSet<Cancion> Canciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Artista>()
                .HasMany(a => a.Albumes)
                .WithOne(al => al.Artista)
                .HasForeignKey(al => al.ArtistaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Album>()
                .HasMany(al => al.Canciones)
                .WithOne(c => c.Album)
                .HasForeignKey(c => c.AlbumId)
                .OnDelete(DeleteBehavior.Cascade);  
        }
    }
}
