using LojaVirtual.Core.Business.Entities;
using LojaVirtual.Core.Business.Interfaces;
using LojaVirtual.Core.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Core.Infra.Repositories
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
    }
}
