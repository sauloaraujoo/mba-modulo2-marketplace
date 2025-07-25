﻿using LojaVirtual.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LojaVirtual.Data.Mappings
{
    public class VendedorMapping : IEntityTypeConfiguration<Vendedor>
    {
        public void Configure(EntityTypeBuilder<Vendedor> builder)
        {
            builder.ToTable("Vendedor");

            builder.HasKey(x => x.Id);

            builder.Property(p => p.Email)
               .IsRequired();


            builder.Property(p => p.Ativo)
               .IsRequired();

            builder.HasMany(c => c.Produtos)
                .WithOne(p => p.Vendedor)
                .HasForeignKey(c => c.VendedorId);
        }
    }
}
