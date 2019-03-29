using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;

namespace SaudeWeb.Models
{
    public class BancoCaixa
    {
        [Key, Column(Order = 0)]
        public int ID { get; set; }

        [Key, Column(Order = 1)]
        public int BaixaID { get; set; }
        
        [Key, Column(Order = 2)]
        public int EmpresaID { get; set; }

        [Required]
        public int Tipo { get; set; }//1 recebimento 2 pagamento

        [Display(Name = "D/C")]
        public virtual string TipoDesc
        {
            get
            {
                if(Tipo == 1)
                {
                    return "C";
                }
                else
                {
                    return "D";
                }
            }
        }

        public class ListaTipo
        {
            public int ID { get; set; }
            public string Descricao { get; set; }
            public List<ListaTipo> MetodoListaOpcao()
            {
                return new List<ListaTipo>
                {
                    new ListaTipo { ID = 1, Descricao = "CRÉDITO" },
                    new ListaTipo { ID = 2, Descricao = "DÉBITO" }
                };
            }
        }

        [Required]
        [Display(Name = "Conta")]
        public int ContaID { get; set; }

        [Required]
        [Display(Name = "Tipo Documento")]
        public int TpDocto { get; set; } // 1 - dinheiro, 2 Cheque, 99 - Outros

        public class ListaTipoDocumento
        {
            public int ID { get; set; }
            public string Descricao { get; set; }
            public List<ListaTipoDocumento> MetodoListaOpcao()
            {
                return new List<ListaTipoDocumento>
                {
                    new ListaTipoDocumento { ID = 1, Descricao = "DINHEIRO" },
                    new ListaTipoDocumento { ID = 2, Descricao = "CHEQUE" },
                    new ListaTipoDocumento { ID = 99, Descricao = "OUTROS" },
                };
            }
        }

        [Required]
        [Display(Name = "Cadastro")]
        public int? PessoaId { get; set; }

        public virtual string PessoaDesc
        {
            get
            {
                string retorno = "";
                if(PessoaId != null)
                {
                    DataContext db = new DataContext();
                    var pessoa = db.Pessoas.Where(x => x.EmpresaID == EmpresaID && x.ID == PessoaId).FirstOrDefault();
                    if(pessoa != null)
                    {
                        retorno = pessoa.Razao;
                    }
                }
                return retorno;
            }
        }

        [DataType(DataType.Date, ErrorMessage = "Informe somente data válida.")]
        [Required]
        [Display(Name = "Emissão")]
        public DateTime? DataEmissao { get; set; }


        [Display(Name = "Emissão")]
        public virtual string DataEmissaoF
        {
            get
            {
                if(DataEmissao != null)
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

        [Display(Name = "Vencimento")]
        public virtual string DataVencimentoF
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

        [Display(Name = "Conciliação")]
        public virtual string DataConciliacaoF
        {
            get
            {
                if (DataConciliacao != null)
                {
                    DateTime dt = Convert.ToDateTime(DataConciliacao);
                    return dt.ToShortDateString();
                }
                else
                {
                    return "";
                }
            }
        }

        [DataType(DataType.Date, ErrorMessage = "Informe somente data válida.")]
        [Required]
        [Display(Name = "Vencimento")]
        public DateTime? DataVencimento { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Informe somente data válida.")]
        [Display(Name = "Conciliação")]
        public DateTime? DataConciliacao { get; set; }


        [Display(Name = "NR.Documento")]
        public string NumeroDocumento { get; set; }

        public string Banco { get; set; }

        [Display(Name = "Agência")]
        public string Agencia { get; set; }

        [Display(Name = "Observação")]
        public string Obs { get; set; }

        [Required]
        [Display(Name = "Valor")]
        public decimal valor { get; set; }

        public virtual string ValorF
        {
            get
            {
                return valor.ToString("C", CultureInfo.CurrentCulture).Replace("R$","");
            }
        }


        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal DebCred { get; set; }

        public string Cheque { get; set; }

        public int? Transferencia { get; set; }

        public string Emitente { get; set; }

        public string Conta { get; set; }

        [Required]
        public int PlanoContaID { get; set; }

        public virtual string PlanoContaDesc
        {
            get
            {
                string retorno = "";
                if (PlanoContaID != null)
                {
                    int planoConta = Convert.ToInt32(PlanoContaID);
                    DataContext db = new DataContext();
                    var plconta = db.PlanoContas.Where(x => x.EmpresaID == EmpresaID && x.ID == planoConta).FirstOrDefault();
                    if(plconta != null)
                    {
                        retorno = plconta.Conta + " - " + plconta.Descricao;
                    }
                }
                return retorno;
            }
        }

        public class ListaOpcao
        {
            public int ID { get; set; }
            public string Descricao { get; set; }
            public List<ListaOpcao> MetodoListaOpcao()
            {
                return new List<ListaOpcao>
                {                 
                    new ListaOpcao { ID = 1, Descricao = "EMISSÃO" },
                    new ListaOpcao { ID = 2, Descricao = "VENCIMENTO" },
                    new ListaOpcao { ID = 3, Descricao = "CONCILIAÇÃO" }

                };
            }
        }

        public class ListaSituacao
        {
            public int ID { get; set; }
            public string Descricao { get; set; }
            public List<ListaSituacao> MetodoListaSituacao()
            {
                return new List<ListaSituacao>
                {
                    new ListaSituacao { ID = 1000, Descricao = "TODOS" },
                    new ListaSituacao { ID = 1, Descricao = "NÃO CONCILIADOS" },
                    new ListaSituacao { ID = 2, Descricao = "CONCILIADOS" }

                };
            }
        }
    }
}