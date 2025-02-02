using Microsoft.EntityFrameworkCore;

namespace ClientePolizasAPI.Models
{
    public class PolizaDbContext : DbContext
    {
        public PolizaDbContext(DbContextOptions<PolizaDbContext> options) : base(options)
        {
        }

        public DbSet<Poliza> Polizas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar la relación con Cliente
            modelBuilder.Entity<Poliza>()
                .HasOne<Cliente>()
                .WithMany()
                .HasForeignKey(p => p.IdCliente)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
