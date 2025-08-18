﻿namespace LojaVirtual.Business.Entities
{
    public class Vendedor : Entity
    {
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public bool Ativo { get; private set; }
        private readonly List<Produto> _produtos;
        public IReadOnlyCollection<Produto> Produtos => _produtos;

        protected Vendedor()
        {
            _produtos = new List<Produto>();
        }

        public Vendedor(Guid id, 
                        string nome, 
                        string email)
        {
            Id = id;
            Nome = nome;
            Email = email;
            Ativo = true;
            _produtos = new List<Produto>();
        }

        public void AlterarStatus()
        {
            Ativo = !Ativo;
            foreach (var produto in _produtos)
            {
                produto.AlterarStatus();
            }
        }
    }
}
