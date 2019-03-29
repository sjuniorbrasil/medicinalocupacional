using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaudeWeb.Models
{
    public class Agenda
    {
        [Key, Column(Order = 0)]
        public int ID { get; set; }

        [Key, Column(Order = 1)]
        public int EmpresaID { get; set; }

        [Display(Name ="Cliente")]
        public int? ClienteID { get; set; }

        [Display(Name = "Funcionário")]
        public int? FuncionarioID { get; set; }

        [Display(Name = "Situação")]
        public int? Situacao { get; set; }

        public DateTime DataAgendamento { get; set; }

        public DateTime? DataConclusao { get; set; }

        [Required(ErrorMessage = "Campo de preenchimento obrigatório")]
        [DataType(DataType.Date, ErrorMessage = "Informe somente data válida.")]
        [Display(Name = "Data")]
        public DateTime DataAgendado { get; set; }
        
        [MaxLength(20)]
        [Required(ErrorMessage = "Campo de preenchimento obrigatório")]
        [Display(Name = "Hora")]
        public string Hora { get; set; }

        public virtual string DataHoraFormatada
        {
            get
            {
                return DataAgendado.ToShortDateString() + " - " + Hora;
            }
        }

        [MaxLength(100)]
        [Required(ErrorMessage = "Campo de preenchimento obrigatório")]
        [Display(Name = "Funcionario")]
        public string NomeFuncionario { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "Campo de preenchimento obrigatório")]
        [Display(Name = "Cliente")]
        public string NomeCliente { get; set; }

        [MaxLength(500)]
        [Display(Name = "Observações")]
        public string Observacoes { get; set; }


        [Display(Name = "Cliente")]
        public virtual string ClienteDesc
        {
            get
            {
                if (ClienteID != null)
                {
                    DataContext db = new DataContext();
                    var pessoa = db.Pessoas.Find(ClienteID, EmpresaID);
                    return pessoa.Razao;
                }
                else
                {
                    return "Não informado";
                }
            }
        }


        [Display(Name = "Status")]
        public virtual int Status
        {
            get
            {
                if (DataAgendado !=  Convert.ToDateTime("01 / 01 / 0001 00:00:00") && (!string.IsNullOrEmpty(Hora)))
                {
                    var dt = DataAgendado.ToShortDateString();
                    var f = dt + " " + Hora + ":00";
                    var retorno = Convert.ToDateTime(f);
                    if (DataConclusao == null)
                    {
                        if (DateTime.Now > retorno)
                        {
                            return 1;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    else
                    {
                        return 100;
                    }
                }
                else
                {
                    return 1000;
                }

            }
        }

        [Display(Name = "Funcionário")]
        public virtual string FuncionarioDesc
        {
            get
            {
                if (FuncionarioID != null)
                {

                    DataContext db = new DataContext();
                    var funcionario = db.Funcionarios.Find(FuncionarioID, EmpresaID);
                    return funcionario.Nome;
                }
                else
                {
                    return "Não informado";
                }
            }
        }

        public class ListaSituacaoAgenda
        {
            public int ID { get; set; }
            public string Descricao { get; set; }
            public List<ListaSituacaoAgenda> MetodoListaAgenda()
            {
                return new List<ListaSituacaoAgenda>
                {
                    new ListaSituacaoAgenda { ID = 0, Descricao = "PENDENTE" },
                    new ListaSituacaoAgenda { ID = 1, Descricao = "FINALIZADO" },
                    new ListaSituacaoAgenda { ID = 1000, Descricao = "TODOS" }



                };
            }
        }
    }
}