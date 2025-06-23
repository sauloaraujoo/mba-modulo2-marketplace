using LojaVirtual.Mvc.Extensions;
using System.ComponentModel;

namespace LojaVirtual.Mvc.Models
{
    public class ProdutoVitrineViewModel
    {        
        public Guid Id { get; set; }
     
        public string Nome { get; set; }

        [DisplayName("Descrição")]
        public string Descricao { get; set; }

        public string? Imagem { get; set; }
        
        [DisplayName("Preço")]
        [Moeda]
        public decimal Preco { get; set; }

        public int Estoque { get; set; }

        [DisplayName("Categoria")]
        public string Categoria { get; set; }        
        public string Vendedor { get; set; }
    }
}
