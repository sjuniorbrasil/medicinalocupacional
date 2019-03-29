using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaudeWeb.Models
{
    public class ContaCorrenteCaixa
    {
        [Key, Column(Order = 0)]
        public int ID { get; set; }

        [Key, Column(Order = 1)]
        public int EmpresaID { get; set; }

        [Required]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        
        public int? Banco { get; set; }

        [Display(Name = "Agência")]
        public string Agencia { get; set; }

        [Required]
        [Display(Name = "Número da Conta")]
        public string NumeroConta { get; set; }

        [Display(Name = "Conta Padrão")]
        public int? ContaPadrao { get; set; }

        public string Carteira { get; set; }
        public string Modalidade { get; set; }
        public string CodigoBeneficiario { get; set; }




        public class ListaBanco
        {
            public string ID { get; set; }
            public string Descricao { get; set; }
            public List<ListaBanco> MetodoListaBanco()
            {
                return new List<ListaBanco>
                {                    
                    new ListaBanco { ID = "001", Descricao = "001 - BANCO DO BRASIL" },
                    new ListaBanco { ID = "033", Descricao = "033 - SANTANDER" },
                    new ListaBanco { ID = "084", Descricao = "084 - UNIPRIME" },
                    new ListaBanco { ID = "104", Descricao = "104 - CAIXA ECONÔMICA" },
                    new ListaBanco { ID = "237", Descricao = "237 - BRADESCO" },
                    new ListaBanco { ID = "341", Descricao = "341 - ITAÚ" },
                    new ListaBanco { ID = "356", Descricao = "084 - REAL SANTANDER" },
                    new ListaBanco { ID = "399", Descricao = "104 - HSBC" },
                    new ListaBanco { ID = "748", Descricao = "237 - SICREDI" },
                    new ListaBanco { ID = "756", Descricao = "341 - SICOOB" },
                    new ListaBanco { ID = "999", Descricao = "OUTRO" }
                };
            }
        }
    }
}