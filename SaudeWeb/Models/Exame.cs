using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SaudeWeb.Models
{
    public class Exame
    {
        public Exame()
        {

        }
        [Display(Name = "Controle")]
        [Key, Column(Order = 0)]
        public int ID { get; set; }

        [Display(Name = "Filial")]
        [Key, Column(Order = 1)]
        public int EmpresaID { get; set; }
        public Empresa Empresa { get; set; }

        
        [Display(Name = "Exame")]
        [StringLength(100), Required(ErrorMessage = "Campo de preenchimento obrigatório")]
        public string Nome { get; set; }

        
        [Display(Name = "Valor do Exame")]
        public decimal? ValorExame { get; set; }

        [Display(Name = "Valor do Repasse")]
        public decimal? ValorRepasse { get; set; }
    }
}