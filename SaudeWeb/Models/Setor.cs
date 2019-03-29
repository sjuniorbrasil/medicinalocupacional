using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaudeWeb.Models
{
    public class Setor
    {
        public Setor()
        {

        }
        [Key, Column(Order = 0)]
        public int Id { get; set; }

        [Display(Name = "Filial")]
        [Key, Column(Order = 1)]
        public int EmpresaID { get; set; }
        public Empresa Empresa { get; set; }

        [MaxLength(100)]
        [Required]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        

    }
}