using LojaVirtual.Core.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LojaVirtual.Core.Infra.Mappings
{
    public class ProdutoMapping : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("Produto");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome)
                .IsRequired();

            builder.Property(p => p.Descricao)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(p => p.Imagem)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(p => p.Preco)
               .IsRequired();

            builder.Property(p => p.Estoque)
               .IsRequired();
        }
    }
}
