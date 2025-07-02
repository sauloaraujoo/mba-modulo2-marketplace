using LojaVirtual.Business.Entities;

namespace LojaVirtual.Api.Models
{
    public class FavoritoViewModel
    {
        public FavoritoViewModel(Guid produtoId, string nome, string imagem, decimal preco, string categoria, Guid vendedorId, string vendedor)
        {
            ProdutoId = produtoId;
            Nome = nome;
            Imagem = imagem;
            Preco = preco;
            Categoria = categoria;
            VendedorId = vendedorId;
            Vendedor = vendedor;
        }

        public Guid ProdutoId { get; set; }
        public string Nome { get; set; }
        public string Imagem { get; set; }
        public decimal Preco { get; set; }
        public string Categoria { get; private set; }
        public Guid VendedorId { get; set; }
        public string Vendedor { get; private set; }

        public static FavoritoViewModel FromFavorito(Favorito favorito) =>
            new(
                favorito.ProdutoId,
                favorito.Produto.Nome,
                favorito.Produto.Imagem,
                favorito.Produto.Preco,
                favorito.Produto.Categoria.Nome,
                favorito.Produto.Vendedor.Id,
                favorito.Produto.Vendedor.Nome
            );
    }
}
