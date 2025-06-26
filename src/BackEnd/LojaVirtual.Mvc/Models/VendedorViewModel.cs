using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LojaVirtual.Mvc.Models
{
    public class VendedorViewModel
    {
        [Key]
        public Guid Id { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public bool Ativo { get; set; }
    }
}
