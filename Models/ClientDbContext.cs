using Microsoft.EntityFrameworkCore;

namespace ClientePolizasAPI.Models
{
    public class ClienteDbContext : DbContext
    {
        public ClienteDbContext(DbContextOptions<ClienteDbContext> options) : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define la restricción de unicidad en DNI
            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.DNI)
                .IsUnique();
        }
    }
}
