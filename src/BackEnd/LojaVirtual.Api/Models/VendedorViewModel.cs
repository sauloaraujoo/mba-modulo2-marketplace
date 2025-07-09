using LojaVirtual.Business.Entities;

namespace LojaVirtual.Api.Models
{
    public class VendedorViewModel(Guid id, string nome, string email)
    {
        public Guid Id { get; } = id;
        public string Nome { get; } = nome;
        public string Email { get; } = email;

        public static VendedorViewModel FromVendedor(Vendedor vendedor) =>
            new(
                vendedor.Id,
                vendedor.Nome,
                vendedor.Email
            );
    }
}
