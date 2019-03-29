using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SaudeWeb.Models
{
    public class TabelaPreco
    {
        [Key, Column(Order = 0)]
        public int ID { get; set; }
        [Key, Column(Order = 1)]
        public int EmpresaID { get; set; }

        [Required]
        public string Descricao { get; set; }

        public List<TabelaPrecoExame> TabelaPrecos { get; set; }

        public class TabelaPrecoExame
        {
            [Key, Column(Order = 0)]
            public int TabID { get; set; }

            [Key, Column(Order = 1)]
            public int EmpresaID { get; set; }

            [Display(Name ="Exame")]
            [Key, Column(Order = 2)]
            public int ExameID { get; set; }

            public virtual string ExameDesc
            {
                get
                {
                    DataContext db = new DataContext();
                    var exame = db.Exames.Find(ExameID, EmpresaID);
                    if(exame != null)
                    {
                        return exame.Nome;
                    }
                    else
                    {
                        return "";
                    }
                    
                }
            }

            [Display(Name = "Valor do Exame")]
            public decimal? ValorExame { get; set; }

            [Display(Name = "Repasse Encaminhamento %")]
            public decimal? RepasseEncaminhadoPerc { get; set; }

            [Display(Name = "Repasse Encaminhamento Valor")]
            public decimal? RepasseEncaminhadoValor { get; set; }

            [Display(Name = "Repasse Médico Valor")]
            public decimal? RepasseMedicoPerc { get; set; }

            [Display(Name = "Repasse Médico %")]
            public decimal? RepasseMedicoValor { get; set; }
        }

        public static decimal GetValorExame(string empresaID, string exameID, string clienteID)
        {
            DataContext db = new DataContext();
            decimal retorno = 0;
            int empresaid = Convert.ToInt32(empresaID);
            int exameid = Convert.ToInt32(exameID);
            int clienteid = Convert.ToInt32(clienteID);
            var temTabela = db.Pessoas.Where(x => x.ID == clienteid && x.EmpresaID == empresaid && x.TabelaID != null).FirstOrDefault();
            if(temTabela != null)
            {
                var ex = db.TabExame.Where(x => x.EmpresaID == empresaid && x.ExameID == exameid).FirstOrDefault();
                if (ex != null)
                {
                    retorno = Convert.ToDecimal(ex.ValorExame);
                }
                else
                {
                    var exame = db.Exames.Where(x => x.ID == exameid && x.EmpresaID == empresaid).FirstOrDefault();
                    if(exame != null)
                    {
                        retorno = Convert.ToDecimal(exame.ValorExame);
                    }
                    else
                    {
                        retorno = 0;
                    }                    
                }
            }
            else
            {
                var ex = db.Exames.Where(x => x.ID == exameid && x.EmpresaID == empresaid).FirstOrDefault();
                if (ex != null)
                {
                    retorno = Convert.ToDecimal(ex.ValorExame);
                }
                else
                {
                    retorno = 0;
                }                
            }            
            return retorno;
        }
    }
}