namespace LojaVirtual.Business.Entities
{
    public class Favorito
    {
        public Favorito()
        {

        }

        public Favorito(Guid clienteId, Guid produtoId)
        {
            ClienteId = clienteId;
            ProdutoId = produtoId;
        }

        public Guid ClienteId { get; private set; }
        public Cliente Cliente { get; private set; }
        public Guid ProdutoId { get; private set; }
        public Produto Produto { get; private set; }
    }
}
