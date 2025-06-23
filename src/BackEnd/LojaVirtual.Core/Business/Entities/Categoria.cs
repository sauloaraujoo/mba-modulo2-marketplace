namespace LojaVirtual.Core.Business.Entities
{
    public class Categoria : Entity
    {
        protected Categoria() { }
        public Categoria(string nome, string descricao)
        {
            Nome = nome.Trim();
            Descricao = descricao.Trim();
            _produtos = new List<Produto>();
        }

        public string Nome { get; set; }
        public string Descricao { get; set; }
        private readonly List<Produto> _produtos;
        public IReadOnlyCollection<Produto> Produtos => _produtos;

        public void Edit(string nome, string descricao)
        {
            Nome = nome.Trim();
            Descricao = descricao.Trim();
        }
        public void AddProduto(Produto produto)
        {
            _produtos.Add(produto);
        }
    }
}
