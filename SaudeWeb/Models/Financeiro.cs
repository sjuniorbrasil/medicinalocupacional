using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;


namespace SaudeWeb.Models
{
    public class Financeiro
    {
        [Key, Column(Order = 0)]
        public int ID { get; set; }

        [Key, Column(Order = 1)]
        public int EmpresaID { get; set; }

        [Key, Column(Order = 2)]
        public int ParcelaID { get; set; }

        [Required]
        public int Tipo { get; set; }//1 recebimento 2 pagamento

        
        [Required]
        [Display(Name = "Data de Emissão")]
        [DataType(DataType.Date, ErrorMessage = "Informe somente data válida.")]
        public DateTime? DataEmissao { get; set; }
        
        [Required]
        [Display(Name = "Data de Vencimento")]
        [DataType(DataType.Date, ErrorMessage = "Informe somente data válida.")]
        public DateTime DataVencimento { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Informe somente data válida.")]
        [Display(Name ="Data de Liquidação")]
        public DateTime? DataLiquidacao { get; set; }

        public string Competencia { get; set; }

        [Required]
        public decimal Valor { get; set; }

        public decimal Juros { get; set; }

        public decimal Multa { get; set; }

        public decimal Desconto { get; set; }

        [Display(Name = "Valor Baixado")]
        public decimal ValorBaixado { get; set; }

        [Display(Name = "Tipo")]
        public virtual string TipoDesc
        {
            get
            {
                if(Tipo == 1)
                {
                    return "RECEBIMENTO";
                }
                else if (Tipo == 2)
                {
                    return "PAGAMENTO";
                }
                else
                {
                    return "";
                }

            }
        }

        [Display(Name = "Tipo")]
        public virtual string IDParcela
        {
            get
            {
                return Convert.ToString(ID) + "/" + Convert.ToString(ParcelaID);

            }
        }

        [Display(Name = "Cadastro")]
        public virtual string ClienteDesc
        {
            get
            {
                if(PessoaID != null)
                {
                    DataContext db = new DataContext();
                    var pessoa = db.Pessoas.Where(x => x.EmpresaID == EmpresaID && x.ID == PessoaID).FirstOrDefault();
                    if (pessoa != null)
                    {
                        return pessoa.Razao;
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

        public class Situacao
        {
            public int ID { get; set; }
            public string Descricao { get; set; }
            public List<Situacao> MetodoListaSituacao()
            {
                return new List<Situacao>
                {
                    new Situacao { ID = 1000, Descricao = "TODOS" },
                    new Situacao { ID = 0, Descricao = "PENDENTE" },
                    new Situacao { ID = 1, Descricao = "LIQUIDADO" }
                };
            }
        }

        public class TipoF
        {
            public int ID { get; set; }
            public string Descricao { get; set; }
            public List<TipoF> MetodoListaTipo()
            {
                return new List<TipoF>
                {
                    new TipoF { ID = 1000, Descricao = "TODOS" },
                    new TipoF { ID = 1, Descricao = "RECEBIMENTO" },
                    new TipoF { ID = 2, Descricao = "PAGAMENTO" }
                };
            }
        }

        [Display(Name = "Data de Vencimento")]
        public virtual string VencimentoFormatado
        {
            get
            {
                if (DataVencimento != null)
                {
                    DateTime dt = Convert.ToDateTime(DataVencimento);
                    
                    return dt.ToShortDateString();
                }
                else
                {
                    return "";
                }

            }
        }

        [Display(Name = "Data de Emissão")]
        public virtual string EmissaoFormatado
        {
            get
            {
                if (DataEmissao != null)
                {
                    DateTime dt = Convert.ToDateTime(DataEmissao);

                    return dt.ToShortDateString();
                }
                else
                {
                    return "";
                }

            }
        }

        [Display(Name = "Data de Liquidação")]
        public virtual string LiquidacaoFormatado
        {
            get
            {
                if (DataLiquidacao != null)
                {
                    DateTime dt = Convert.ToDateTime(DataLiquidacao);
                    return dt.ToShortDateString();
                }
                else
                {
                    return "";
                }

            }
        }


        [Display(Name = "Valor Total")]
        public virtual decimal ValorTotal
        {
            get
            {
                return Valor + Juros + Multa - Desconto;
            }
        }

        [Display(Name = "JMD")]
        public virtual decimal JMD
        {
            get
            {
                return Juros + Multa - Desconto;
            }
        }

        [Display(Name = "Valor Pendente")]
        public virtual decimal ValorPendente
        {
            get
            {
                return Valor - ValorBaixado;
            }
        }

        [Display(Name = "Cadastro")]
        [Required]
        public int PessoaID { get; set; }

        [Display(Name = "Documento")]
        public string NumeroDocumento { get; set; }


        public int? ConsultaID { get; set; }

        [Display(Name = "Observação")]
        public string Observacao { get; set; }

        [Required]
        [Display(Name = "Plano de Contas")]
        public int? CategoriaID { get; set; }

        public virtual string PessoaDesc
        {
            get
            {
                DataContext db = new DataContext();
                var retorno = "";
                if (PessoaID != null)
                {
                    var pessoa = db.Pessoas.Find(PessoaID, EmpresaID);
                    if(pessoa != null)
                    {
                        retorno = pessoa.Razao;
                    }
                }
                return retorno;
            }
        }

        public virtual string CategoriaDesc {
            get
            {
                DataContext db = new DataContext();
                var categoria = db.PlanoContas.Where(x => x.ID == CategoriaID && x.EmpresaID == EmpresaID).FirstOrDefault();
                if(categoria != null)
                {
                    return categoria.Conta + " - " + categoria.Descricao;
                }
                else
                {
                    return "";
                }
            }
        }

        public virtual bool EhFaturamento
        {
            get
            {
                DataContext db = new DataContext();
                var consulta = db.Consultas.Where(x => x.EmpresaID == EmpresaID && x.FinanceiroID == ID && x.Fatura != 0 && x.Fatura != null).FirstOrDefault();
                if(consulta != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public class TipoCompetencia
        {
            public int ID { get; set; }
            public string Descricao { get; set; }
            public List<TipoCompetencia> MetodoLista()
            {
                return new List<TipoCompetencia>
                {                    
                    new TipoCompetencia { ID = 0, Descricao = "FIXA" },
                    new TipoCompetencia { ID = 1, Descricao = "MENSAL" }
                };
            }
        }

        public static dynamic SituacaoRegistro(int empresaid, int id, int parcelaid)
        {
            dynamic obj = new ExpandoObject();            
            DataContext db = new DataContext();
            var financeiro = db.RecPag.Where(x => x.EmpresaID == empresaid && x.ID == id && x.ParcelaID == parcelaid).FirstOrDefault();
            obj.ID = financeiro.IDParcela;            
            if((financeiro.ValorBaixado + financeiro.Desconto) -(financeiro.Juros + financeiro.Multa) >= financeiro.Valor)
            {
                obj.Situacao = "REGISTRO LIQUIDADO";
            }
            else if(((financeiro.ValorBaixado + financeiro.Desconto) - (financeiro.Juros + financeiro.Multa) > 0) && ((financeiro.ValorBaixado + financeiro.Desconto) - (financeiro.Juros + financeiro.Multa) < financeiro.Valor))
            {
                obj.Situacao = "REGISTRO LIQUIDADO PARCIALMENTE";
            }
            else
            {
                obj.Situacao = "REGISTRO PENDENTE DE LIQUIDAÇÃO";
            }            
            return obj;
        }
    }
}