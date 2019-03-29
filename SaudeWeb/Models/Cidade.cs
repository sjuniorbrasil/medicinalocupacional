using System.ComponentModel.DataAnnotations;

namespace SaudeWeb.Models
{
    public class Cidade
    {
        public int ID { get; set; }

        [Display(Name = "Cidade")]
        [StringLength(100), Required(ErrorMessage = "Campo de Preenchimento obrigatório", AllowEmptyStrings = false)]
        public string Descricao { get; set; }
        [Display(Name = "UF")]
        [StringLength(2)]
        public string UF { get; set; }

        public virtual string CidadeUF
        {
            get
            {
                return Descricao + " - " + UF;
            }
        }

        public Cidade()
        {

        }
    }
}