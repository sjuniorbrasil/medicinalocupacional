using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SaudeWeb.Models
{
    public class PlanoConta
    {
        [Key, Column(Order = 0)]
        public int ID { get; set; }

        [Key, Column(Order = 1)]
        public int EmpresaID { get; set; }

        [Required]
        [Display(Name ="Descrição")]
        public string Descricao { get; set; }

        [Required]
        [Display(Name = "Conta")]
        public string Conta { get; set; }

        [Required]
        [Display(Name = "Operação")]
        public int? Operacao { get; set; }// 1 - credito 2 - debito

        [Display(Name = "Nível")]
        public int Nivel { get; set; }

        [Display(Name = "Nível Superior")]
        public int? NivelSuperior { get; set; }

        [Display(Name = "Nível Superior")]
        public virtual string NivelSuperiorDesc
        {
            get
            {
                if ((Nivel != 0) && (Nivel != null))
                {
                    DataContext db = new DataContext();
                    var plconta = db.PlanoContas.Where(x => x.ID == NivelSuperior && x.EmpresaID == EmpresaID).FirstOrDefault();
                    if (plconta != null)
                    {
                        return plconta.Descricao;
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }

            }
        }

        [Display(Name = "Operação")]
        public virtual string OperacaoDesc
        {
            get
            {
                if (Operacao == 1)
                {
                    return "RECEITAS";
                }
                else
                {
                    return "DESPESAS";
                }
            }
        }       

        public virtual string PlanoContaDesc
        {
            get
            {
                if(!string.IsNullOrEmpty(Descricao) || (!string.IsNullOrEmpty(Conta)))
                {
                    return Conta + " - " + Descricao;
                }
                else
                {
                    return "";
                }
                
            }
        }

        public List<PlanoConta> SubCategorias { get; set; }
        public class ListaSOperacao
        {
            public int ID { get; set; }
            public string Descricao { get; set; }
            public List<ListaSOperacao> MetodoListaOperacao()
            {
                return new List<ListaSOperacao>
                {
                    new ListaSOperacao { ID = 0, Descricao = "RECEITAS" },
                    new ListaSOperacao { ID = 1, Descricao = "DESPESAS" }
                };
            }
        }

    }
}