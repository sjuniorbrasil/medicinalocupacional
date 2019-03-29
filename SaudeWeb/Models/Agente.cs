using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaudeWeb.Models
{
    public class Agente
    {
        public Agente()
        {

        }
        [Key, Column(Order = 0)]
        public int Id { get; set; }

        [Display(Name = "Filial")]
        [Key, Column(Order = 1)]
        public int EmpresaID { get; set; }

        [MaxLength(100)]
        [Required]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Risco")]
        public int RiscoId { get; set; }
        [Display(Name = "Risco")]
        public virtual string RiscoDesc
        {
            get
            {
                DataContext db = new DataContext();
                var risco = db.Riscos.Find(RiscoId);
                return risco.Descricao;
            }
            
        }

        [MaxLength(150)]
        [Display(Name = "Danos à Saúde")]
        public string DanosSaude { get; set; }

        [MaxLength(500)]
        [Display(Name = "Recomendação Médica")]
        public string RecomendacaoMedica { get; set; }

        public Empresa Empresa { get; set; }
                
        public Risco Riscos { get; set; }
    }
}