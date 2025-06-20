using LojaVirtual.Core.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LojaVirtual.Core.Infra.Mappings
{
    public class VendedorMapping : IEntityTypeConfiguration<Vendedor>
    {
        public void Configure(EntityTypeBuilder<Vendedor> builder)
        {
            builder.ToTable("Vendedor");

            builder.HasKey(x => x.Id);

            builder.Property(p => p.Email)
               .IsRequired();

        }
    }
}
