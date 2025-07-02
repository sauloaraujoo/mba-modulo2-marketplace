namespace LojaVirtual.Api.Models
{
    public class FavoritoViewModel
    {
        public Guid ClienteId { get; set; }
        public Guid ProdutoId { get; set; }
        public string ProdutoNome { get; set; }
        public string ProdutoImagem { get; set; }
        public decimal ProdutoPreco { get; set; }
    }
}
