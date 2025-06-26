namespace LojaVirtual.Core.Business.Entities
{
    public class Vendedor: Entity
    {
        public Vendedor(Guid id, string nome, string email, bool ativo)
        {
            Id = id;
            Nome = nome;
            Email = email;
            Ativo = ativo;
        }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public bool Ativo { get; private set; }
    }
}
