using LojaVirtual.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LojaVirtual.Data.Mappings
{
    public class FavoritoMapping : IEntityTypeConfiguration<Favorito>
    {
        public void Configure(EntityTypeBuilder<Favorito> builder)
        {
            builder.ToTable("Favorito");
            builder.HasKey(f => new { f.ClienteId, f.ProdutoId });

            builder
            .HasOne(f => f.Cliente)
            .WithMany(c => c.Favoritos)
            .HasForeignKey(f => f.ClienteId)
            .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(f => f.Produto)
                .WithMany(p => p.Favoritos)
                .HasForeignKey(f => f.ProdutoId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
