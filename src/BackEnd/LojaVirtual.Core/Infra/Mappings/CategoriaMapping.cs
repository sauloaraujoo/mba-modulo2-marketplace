using LojaVirtual.Core.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LojaVirtual.Core.Infra.Mappings
{
    public class CategoriaMapping : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("Categoria");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nome)
                .IsRequired();

            builder.Property(c => c.Descricao)
                .HasMaxLength(1000)
                .IsRequired();

            builder.HasMany(c => c.Produtos)
                .WithOne(p => p.Categoria)
                .HasForeignKey(c => c.CategoriaId);
        }
    }
}
