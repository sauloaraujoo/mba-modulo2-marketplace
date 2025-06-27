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

        public Guid ClienteId { get; protected set; }
        public Cliente Cliente { get; protected set; }
        public Guid ProdutoId { get; protected set; }
        public Produto Produto { get; protected set; }
    }
}
