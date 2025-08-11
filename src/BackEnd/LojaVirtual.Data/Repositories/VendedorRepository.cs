using LojaVirtual.Business.Entities;
using LojaVirtual.Business.Interfaces;
using LojaVirtual.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Data.Repositories
{
    public class VendedorRepository : IVendedorRepository
    {
        private readonly LojaVirtualContext _context;

        public VendedorRepository(LojaVirtualContext context)
        {
            _context = context;
        }
        public async Task Inserir(Vendedor request, CancellationToken tokenDeCancelamento)
        {
            
            var claimProduto = new IdentityUserClaim<string>
            {
                UserId = request.Id.ToString(),
                ClaimType = "Produtos",
                ClaimValue = "ADICIONAR,VISUALIZAR,EDITAR,EXCLUIR,ATUALIZAR_STATUS"
            };

            await _context.UserClaims.AddRangeAsync(claimProduto);
            await _context.VendedorSet.AddAsync(request, tokenDeCancelamento);
        }

        public async Task<int> SalvarMudancas(CancellationToken tokenDeCancelamento)
        {
            return await _context.SaveChangesAsync(tokenDeCancelamento);
        }
        public async Task<Vendedor> ObterPorIdSemContexto(Guid id, CancellationToken tokenDeCancelamento)
        {            
            return await _context
                    .VendedorSet
                    .AsNoTracking()
                    .FirstOrDefaultAsync(v => v.Id == id, tokenDeCancelamento);
        }
        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<IList<Vendedor>> ListarSemContexto(CancellationToken tokenDeCancelamento)
        {
            return await _context
                           .VendedorSet
                           .AsNoTracking()
                           .ToListAsync(tokenDeCancelamento);
        }

        public Task Editar(Vendedor vendedor, CancellationToken tokenDeCancelamento)
        {
            return Task.FromResult(_context.VendedorSet.Update(vendedor));
        }

        public async Task<bool> Existe(string nome, CancellationToken tokenDeCancelamento)
        {
            return await _context.VendedorSet.AnyAsync(c => c.Nome == nome, tokenDeCancelamento);
        }
    }
}
