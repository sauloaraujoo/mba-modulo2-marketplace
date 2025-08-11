using LojaVirtual.Mvc.Extensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LojaVirtual.Mvc.Models
{
    public class ProdutoViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(255, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]        
        [StringLength(1000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        [DisplayName("Descrição")]
        public string Descricao { get; set; }

        [DisplayName("Imagem do Produto")]
        [NotMapped]
        public IFormFile? ImagemUpload { get; set; }

        public string? Imagem { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Preço")]
        [Moeda]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Range(0, int.MaxValue, ErrorMessage = "O campo {0} tem que ser maior que {1}")]
        public int Estoque { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Categoria")]
        public Guid CategoriaId { get; set; }

        public bool Ativo { get; set; }

        [DisplayName("Categoria")]
        public string NomeCategoria { get; set; } = string.Empty;
        public IEnumerable<CategoriaViewModel> Categorias { get; set; } =  Enumerable.Empty<CategoriaViewModel>();

        [DisplayName("Vendedor")]
        public string NomeVendedor { get; set; } = string.Empty;
    }
}
