using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaudeWeb.Models
{
    public class Baixa
    {
        [Key, Column(Order = 0)]
        public int ID { get; set; }

        [Key, Column(Order = 1)]
        public int EmpresaID { get; set; }

        [Key, Column(Order = 2)]
        public int FinanceiroID { get; set; }

        [Key, Column(Order = 3)]
        public int ParcelaID { get; set; }
        
        public decimal? Valor { get; set; }        

        public decimal? Juros { get; set; }

        public decimal? Desconto { get; set; }

        
        [Display(Name ="Data da Baixa")]
        public DateTime? DataBaixa { get; set; }

        [Display(Name = "Observações")]
        public string Obs { get; set; }

        public decimal? Multa { get; set; }

        public decimal? Total { get; set; }

        public decimal? JMD { get; set; }
    }
}