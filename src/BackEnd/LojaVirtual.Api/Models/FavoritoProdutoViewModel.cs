namespace LojaVirtual.Api.Models
{
    public class FavoritoProdutoViewModel
    {
        public Guid ProdutoId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
        public decimal Preco { get; set; }
    }
}
