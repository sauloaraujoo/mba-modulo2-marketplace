namespace LojaVirtual.Business.Entities
{
    public class Categoria : Entity
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        private readonly List<Produto> _produtos;
        public IReadOnlyCollection<Produto> Produtos => _produtos;

        protected Categoria() { }

        public Categoria(string nome, string descricao)
        {
            Nome = nome.Trim();
            Descricao = descricao.Trim();
            _produtos = new List<Produto>();
        }

        public void Editar(string nome, string descricao)
        {
            Nome = nome.Trim();
            Descricao = descricao.Trim();
        }

        public void AdicionarProduto(Produto produto)
        {
            _produtos.Add(produto);
        }
    }
}
