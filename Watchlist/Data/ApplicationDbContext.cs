using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Watchlist.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        // Construteur
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        } 
        // Classe Film
        public DbSet<Film> Films { get; set; } 
        // Classe FilmsUtilisateur
        public DbSet<FilmUtilisateur> FilmsUtilisateur { get; set; }
        // Surcharge de la méthode OnModelCreating pour migrer
        // objet avec relation many-to-many (association).
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // crée l'Entity Framework de FilmUtilisateur
            // // en indiquant sa clé composite.
            modelBuilder.Entity<FilmUtilisateur>()
            .HasKey(t => new { t.IdUtilisateur, t.IdFilm }); 
        }
    }
}