﻿namespace LojaVirtual.Business.Entities
{
    public class Produto : Entity
    {
        protected Produto()
        { }
        public Produto(
            string nome,
            string descricao,
            string imagem,
            decimal preco,
            int estoque,
            bool ativo,
            Guid categoriaId)
        {
            Nome = nome;
            Descricao = descricao;
            Imagem = imagem;
            Preco = preco;
            Estoque = estoque;
            Ativo = ativo;
            CategoriaId = categoriaId;
        }

        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public string Imagem { get; private set; }
        public decimal Preco { get; private set; }
        public int Estoque { get; private set; }
        public bool Ativo { get; private set; }
        public Guid CategoriaId { get; private set; }
        public Categoria Categoria { get; private set; }
        public Guid VendedorId { get; private set; }
        public Vendedor Vendedor { get; private set; }

        public IReadOnlyCollection<Favorito> Favoritos;

        public void Editar(string nome,
            string descricao,
            string imagem,
            decimal preco,
            int estoque,
            bool ativo,
            Guid categoriaId)
        {
            Nome = nome;
            Descricao = descricao;
            Imagem = imagem;
            Preco = preco;
            Estoque = estoque;
            Ativo = ativo;
            CategoriaId = categoriaId;
        }
        public void VinculaVendedor(Guid vendedorId)
        {
            VendedorId = vendedorId;
        }

        public void AlterarStatus()
        {
            Ativo = !Ativo;
        }


    }
}
