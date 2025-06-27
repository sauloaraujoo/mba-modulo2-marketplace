using LojaVirtual.Business.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Data.Context
{
    public class LojaVirtualContext : IdentityDbContext
    {
        public LojaVirtualContext(DbContextOptions<LojaVirtualContext> options) : base(options)
        {
            //ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            //ChangeTracker.AutoDetectChangesEnabled = false;
        }
        public DbSet<Vendedor> VendedorSet { get; set; }
        public DbSet<Categoria> CategoriaSet { get; set; }
        public DbSet<Produto> ProdutoSet { get; set; }
        public DbSet<Cliente> ClienteSet { get; set; }
        public DbSet<Favorito> FavoritoSet { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(255)");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LojaVirtualContext).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientCascade;

            modelBuilder.Entity<Favorito>(e =>
            {
                e.ToTable("Favoritos");
                e.HasKey(f => new { f.ClienteId, f.ProdutoId });
                e.HasOne(f => f.Cliente).WithMany().HasForeignKey(f => f.ClienteId).OnDelete(DeleteBehavior.ClientCascade);
                e.HasOne(f => f.Produto).WithMany().HasForeignKey(f => f.ProdutoId).OnDelete(DeleteBehavior.ClientCascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
