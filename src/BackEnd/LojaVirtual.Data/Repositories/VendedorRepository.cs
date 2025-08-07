﻿using LojaVirtual.Business.Entities;
using LojaVirtual.Business.Interfaces;
using LojaVirtual.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LojaVirtual.Data.Repositories
{
    public class VendedorRepository : IVendedorRepository
    {
        private readonly LojaVirtualContext _context;

        public VendedorRepository(LojaVirtualContext context)
        {
            _context = context;
        }
        public async Task Insert(Vendedor request, CancellationToken cancellationToken)
        {
            
            var claimProduto = new IdentityUserClaim<string>
            {
                UserId = request.Id.ToString(),
                ClaimType = "Produtos",
                ClaimValue = "AD,VI,ED,EX,ATUALIZAR_STATUS"
            };

            await _context.UserClaims.AddRangeAsync(claimProduto);
            await _context.VendedorSet.AddAsync(request, cancellationToken);
        }

        public async Task<int> SaveChanges(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task<Vendedor> GetById(Guid id, CancellationToken cancellationToken)
        {            
            return await _context
                    .VendedorSet
                    .AsNoTracking()
                    .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
        }
        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<IList<Vendedor>> ListAsNoTracking(CancellationToken cancellationToken)
        {
            return await _context
                           .VendedorSet
                           .AsNoTracking()
                           .ToListAsync(cancellationToken);
        }

        public Task Edit(Vendedor vendedor, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.VendedorSet.Update(vendedor));
        }

        public async Task<bool> Exists(string nome, CancellationToken cancellationToken)
        {
            return await _context.VendedorSet.AnyAsync(c => c.Nome == nome, cancellationToken);
        }
    }
}
