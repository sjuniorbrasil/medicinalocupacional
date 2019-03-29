using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SaudeWeb.Models
{
    public class Consulta
    {
        public Consulta()
        {


        }       
       

        public List<ConsultaExame> ExamesC { get; set; }

        public List<ConsultaAgente> AgentesC { get; set; }
        
        public virtual Empresa Empresa { get; set; }
        
        [Display(Name ="Tipo Consulta")]
        public virtual string TipoAso
        {
            get
            {
                if (TipoConsulta == 0)
                {
                    return "ADMISSIONAL";
                }
                else if (TipoConsulta == 1)
                {
                    return "PERIÓDICO, CONFORME PLANEJAMENTO DO PCMSO";
                }
                else if (TipoConsulta == 2)
                {
                    return "RETORNO AO TRABALHO";
                }
                else if (TipoConsulta == 3)
                {
                    return "MUDANÇA DE FUNÇÃO";
                }
                else if (TipoConsulta == 4)
                {
                    return "MONITORAÇÃO PONTUAL";
                }
                else if (TipoConsulta == 8)
                {
                    return "DEMISSIONAL";
                }
                else
                {
                    return "";
                }
            }
        }

        [Key, Column(Order = 0)]
        public int Id { get; set; }

        [Display(Name = "Filial")]
        [Key, Column(Order = 1)]
        public int EmpresaID { get; set; }

        public int? FinanceiroID { get; set; }
        [Display(Name = "Funcionário")]
        public virtual string funcionarioDesc 
        {
            get
            {
                if (FuncionarioId != null)
                {
                    
                    DataContext db = new DataContext();
                    var funcionario = db.Funcionarios.Find(FuncionarioId, EmpresaID);
                    if(funcionario != null)
                    {
                        return funcionario.Nome;
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

        public int? Fatura { get; set; }//0 não é faturamento - 1 - liberado para faturamento, 2 faturado

        [Display(Name = "Médico")]
        public virtual string MedicoDesc
        {
            get
            {
                if (MedicoExaminadorId != null)
                {
                    DataContext db = new DataContext();
                    var medico = db.Pessoas.Find(MedicoExaminadorId, EmpresaID);
                    if (medico != null)
                    {
                        return medico.Razao;
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

        [Display(Name = "Cliente")]
        public virtual string ClienteDesc
        {
            get
            {
                if (PessoaId != null)
                {
                    DataContext db = new DataContext();
                    var pessoa = db.Pessoas.Find(PessoaId, EmpresaID);
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

        [Display(Name = "Tipo Consulta")]
        public int? TipoConsulta { get; set; }
        public class ListaTipoConsulta
        {
            public int ID { get; set; }
            public string Descricao { get; set; }
            public List<ListaTipoConsulta> MetodoListaConsulta()
            {
                return new List<ListaTipoConsulta>
                {
                    new ListaTipoConsulta { ID = 0, Descricao = "ADMISSIONAL" },
                    new ListaTipoConsulta { ID = 1, Descricao = "PERIÓDICO" },
                    new ListaTipoConsulta { ID = 2, Descricao = "RETORNO AO TRABALHO" },
                    new ListaTipoConsulta { ID = 3, Descricao = "MUDANÇA DE FUNÇÃO" },
                    new ListaTipoConsulta { ID = 4, Descricao = "MONITORAÇÃO PONTUAL" },
                    new ListaTipoConsulta { ID = 8, Descricao = "DEMISSIONAL" }


                };
            }
        }

        public class ListaTipoConsultaF
        {
            public int ID { get; set; }
            public string Descricao { get; set; }
            public List<ListaTipoConsultaF> MetodoListaConsulta()
            {
                return new List<ListaTipoConsultaF>
                {
                    new ListaTipoConsultaF { ID = 1000, Descricao = "TODOS" },
                    new ListaTipoConsultaF { ID = 0, Descricao = "ADMISSIONAL" },
                    new ListaTipoConsultaF { ID = 1, Descricao = "PERIÓDICO" },
                    new ListaTipoConsultaF { ID = 2, Descricao = "RETORNO AO TRABALHO" },
                    new ListaTipoConsultaF { ID = 3, Descricao = "MUDANÇA DE FUNÇÃO" },
                    new ListaTipoConsultaF { ID = 4, Descricao = "MONITORAÇÃO PONTUAL" },
                    new ListaTipoConsultaF { ID = 8, Descricao = "DEMISSIONAL" }
                    


                };
            }
        }

        public int? Garbage { get; set; }

        [Display(Name = "Cliente")]
        public int? PessoaId { get; set; }

        [Display(Name = "Funcionário")]
        public int? FuncionarioId { get; set; }

        [Display(Name = "Setor")]
        public int? SetorId { get; set; }

        [Display(Name = "Função")]
        public int? FuncaoId { get; set; }

        [Display(Name = "Data Consulta")]
        [DataType(DataType.Date, ErrorMessage = "Informe somente data válida.")]
        public DateTime? DataConsulta { get; set; }
        public virtual string DataConclusaoF
        {
            get
            {
                if (DataConclusao != null)
                {

                    var dt = Convert.ToDateTime(DataConclusao);
                    return dt.ToShortDateString();
                }
                else
                {
                    return "";
                }
                
            }
        }

        [Display(Name = "Data Conclusão")]
        [DataType(DataType.Date, ErrorMessage = "Informe somente data válida.")]
        public DateTime? DataConclusao { get; set; }

        [Display(Name = "Próxima Consulta")]
        [DataType(DataType.Date, ErrorMessage = "Informe somente data válida.")]
        public DateTime? ProximaConsulta { get; set; }

        [MaxLength(50)]
        [Display(Name = "Último Emprego")]
        public string UltimoEmprego { get; set; }

        [MaxLength(100)]
        [Display(Name = "Última Função Exercida")]
        public string UltimaFuncaoExercida { get; set; }

        [MaxLength(50)]
        [Display(Name = "Tempo Permanência ")]
        public string TempoPermanenciaUltiMoEmprego { get; set; }

        [MaxLength(10)]
        public string Peso { get; set; }

        [MaxLength(10)]
        public string Altura { get; set; }

        [MaxLength(10)]
        public string Temperatura { get; set; }

        [MaxLength(10)]
        [Display(Name = "Pressão Arterial")]
        public string PressaoArterial { get; set; }

        [MaxLength(15)]
        [Display(Name = "Tempo Após Último Emprego")]
        public string TempoAposUltimoEmprego { get; set; }


        [Display(Name = "Médico Examinador")]
        public int? MedicoExaminadorId { get; set; }

        [Display(Name = "Médico Liberação")]
        public int? MedicoLiberacaoId { get; set; }

        [MaxLength(100)]
        [Display(Name = "Encaminhamento Especialista")]
        public string EncaMinhamentoEspecialista { get; set; }

        [Display(Name = "Data Encaminhamento")]
        [DataType(DataType.Date, ErrorMessage = "Informe somente data válida.")]
        public DateTime? DataEncaminhado { get; set; }

        [Display(Name = "Situação Consulta")]
        public int SituacaoConsulta { get; set; } //0 pendente, 1 apto, 2 inapto, 3 apto com restrição, 4 homologado, 5 encaminhado
        public class ListaSituacaoConsulta
        {
            public int ID { get; set; }
            public string Descricao { get; set; }
            public List<ListaSituacaoConsulta> MetodoListaConsulta()
            {
                return new List<ListaSituacaoConsulta>
                {
                    new ListaSituacaoConsulta { ID = 0, Descricao = "PENDENTE" },
                    new ListaSituacaoConsulta { ID = 1, Descricao = "APTO" },
                    new ListaSituacaoConsulta { ID = 2, Descricao = "INAPTO" }         


                };
            }
        }

        public class ListaSituacaoConsultaF
        {
            public int ID { get; set; }
            public string Descricao { get; set; }
            public List<ListaSituacaoConsultaF> MetodoListaConsulta()
            {
                return new List<ListaSituacaoConsultaF>
                {
                    new ListaSituacaoConsultaF { ID = 10, Descricao = "TODOS" },
                    new ListaSituacaoConsultaF { ID = 0, Descricao = "PENDENTE" },
                    new ListaSituacaoConsultaF { ID = 1, Descricao = "APTO" },
                    new ListaSituacaoConsultaF { ID = 2, Descricao = "INAPTO" }
                    


                };
            }
        }

        public class ListaTipoCadastro
        {
            public int ID { get; set; }
            public string Descricao { get; set; }
            public List<ListaTipoCadastro> MetodoLista()
            {
                return new List<ListaTipoCadastro>
                {
                    new ListaTipoCadastro { ID = 0, Descricao = "TODOS" },
                    new ListaTipoCadastro { ID = 1, Descricao = "CLIENTE" },
                    new ListaTipoCadastro { ID = 2, Descricao = "MÉDICO" }
                    


                };
            }
        }

        [Display(Name = "Conclusão")]
        public string AptoInapto
        {
            get
            {
                if (SituacaoConsulta == 0)
                {
                    return "PENDENTE DE CONCLUSÃO";
                }
                else if (SituacaoConsulta == 1)
                {
                    return "APTO";
                }
                else
                {
                    return "INAPTO";
                }
            }
        }

        public decimal? ValorConsulta { get; set; }

        public decimal? ValorAvista { get; set; }
        public decimal? ValorFaturamento { get; set; }

        [Display(Name = "Forma de Pagamento")]
        public int? FormaPagamento { get; set; } // 0 avista, 1 faturamento

        [Display(Name = "Libera Faturamento")]
        public int? LiberaFaturamento { get; set; }
                

        public class ListaFormaPag
        {
            public int ID { get; set; }
            public string Descricao { get; set; }
            public List<ListaFormaPag> MetodoFormaPgto()
            {
                return new List<ListaFormaPag>
                {
                    new ListaFormaPag { ID = 1000, Descricao = "TODOS" },
                    new ListaFormaPag { ID = 0, Descricao = "Á VISTA" },
                    new ListaFormaPag { ID = 1, Descricao = "FATURAMENTO" },
                    new ListaFormaPag { ID = 2, Descricao = "NÃO COBRAR" },


                };
            }
        }

        [MaxLength(1000)]
        [Display(Name = "Observações")]
        public string Observacao { get; set; }

        public virtual string funcionarioCfp
        {
            get
            {
                DataContext db = new DataContext();
                var funcionario = db.Funcionarios.Find(FuncionarioId, EmpresaID);
                return funcionario.CPF;
            }
        }

        public virtual string funcionarionasc
        {
            get
            {
                
                DataContext db = new DataContext();
                var funcionario = db.Funcionarios.Find(FuncionarioId, EmpresaID);
                if (funcionario.DataNascimento != null)
                {
                    var dt = Convert.ToDateTime(funcionario.DataNascimento);
                    return dt.ToShortDateString();
                }
                else
                {
                    return "";
                }
                
            }
        }

        public virtual string funcionarioSexo
        {
            get
            {

                DataContext db = new DataContext();
                var funcionario = db.Funcionarios.Find(FuncionarioId, EmpresaID);
                if (funcionario != null)
                {                    
                    return funcionario.SexoFormatado;
                }
                else
                {
                    return "";
                }

            }
        }       

        public virtual string SetorDesc
        {
            get
            {            
                
                if (SetorId != null)
                {
                    DataContext db = new DataContext();
                    var setor = db.Setor.Find(SetorId, EmpresaID);
                    
                    return setor.Descricao;
                }
                else
                {
                    return "";
                }

            }
        }

        public virtual string FuncaoDesc
        {
            get
            {
                if (FuncaoId != null)
                {
                    DataContext db = new DataContext();
                    var funcao = db.Funcoes.Find(FuncaoId, EmpresaID);
                    if (funcao != null)
                    {
                        return funcao.Descricao;
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
        public virtual string CnpjEmpregador
        {
            get
            {
                DataContext db = new DataContext();
                var pessoa = db.Pessoas.Find(PessoaId, EmpresaID);
                if (pessoa != null)
                {
                    if (!string.IsNullOrEmpty(pessoa.CNPJ))
                    {
                        return pessoa.CNPJ;
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

        public virtual string EmpregadorEndereco
        {
            get
            {
                DataContext db = new DataContext();
                var pessoa = db.Pessoas.Find(PessoaId, EmpresaID);
                if (pessoa != null)
                {
                    if (!string.IsNullOrEmpty(pessoa.Endereco))
                    {
                        return pessoa.Endereco;
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

        public virtual string EmpregadorBairro
        {
            get
            {
                DataContext db = new DataContext();
                var pessoa = db.Pessoas.Find(PessoaId, EmpresaID);
                if (pessoa != null)
                {
                    if (!string.IsNullOrEmpty(pessoa.Bairro))
                    {
                        return pessoa.Bairro;
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

        public virtual string EmpregadorCidade
        {
            get
            {
                DataContext db = new DataContext();
                var pessoa = db.Pessoas.Find(PessoaId, EmpresaID);
                if (pessoa != null)
                {
                    if (!string.IsNullOrEmpty(pessoa.CidadeDesc))
                    {
                        return pessoa.CidadeDesc;
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

        public virtual string EmpregadorCep
        {
            get
            {
                DataContext db = new DataContext();
                var pessoa = db.Pessoas.Find(PessoaId, EmpresaID);
                if (pessoa != null)
                {
                    if (!string.IsNullOrEmpty(pessoa.CEP))
                    {
                        return pessoa.CEP;
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

        public virtual string CrmMedico
        {
            get
            {
                DataContext db = new DataContext();
                var pessoa = db.Pessoas.Find(MedicoExaminadorId, EmpresaID);
                if (pessoa != null)
                {
                    return pessoa.CRM;
                }
                else
                {
                    return "";
                }
            }
        }

        public virtual string funcionarioNis
        {
            get
            {
                DataContext db = new DataContext();
                var funcionario = db.Funcionarios.Find(FuncionarioId, EmpresaID);
                if (funcionario != null)
                {
                    return funcionario.NIT;
                }
                else
                {
                    return "";
                }
                
                
            }
        }

        public static void ExlcuirRelacionados(string id, string empresaid)
        {
            string deleteAgente = "delete from consultaagente where consultaid = " + id + " and empresaid = " + empresaid;
            string deleteExame  = "delete from consultaexame  where consultaid = " + id + " and empresaid = " + empresaid;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Properties.Settings.Default.Banco;           
            SqlCommand cmd = new SqlCommand(deleteAgente, con);
            SqlCommand cmd1 = new SqlCommand(deleteExame, con);
            cmd.CommandType = CommandType.Text;
            con.Open();
            try
            {
                int i = cmd.ExecuteNonQuery();
                int j = cmd1.ExecuteNonQuery();
                if (i > 0 || j > 0)
                {                    
                    
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                con.Close();
            }

        }

        public class ConsultaAgente
        {
            [Key, Column(Order = 0)]
            public int ConsultaID { get; set; }

            [Key, Column(Order = 1)]
            public int EmpresaID { get; set; }

            [Display(Name = "Agente")]
            [Key, Column(Order = 2)]
            public int AgenteId { get; set; }
                       

            public virtual string RiscoDesc
            {
                get
                {
                    DataContext db = new DataContext();
                    var risco = db.Agentes.Find(AgenteId, EmpresaID);
                    if (risco != null)
                    {
                        return risco.RiscoDesc;
                    }
                    else
                    {
                        return "";
                    }
                    
                }
             }

            public virtual string AgenteDesc
            {
                get
                {
                    DataContext db = new DataContext();
                    var agente = db.Agentes.Find(AgenteId,EmpresaID);
                    if (agente != null)
                    {
                        return agente.Descricao;
                    }
                    else
                    {
                        return "";
                    }
                }
            }

         
        }

        public class ConsultaExame
        {
            [Key, Column(Order = 0)]
            public int ConsultaID { get; set; }

            [Key, Column(Order = 1)]
            public int EmpresaID { get; set; }

            [Key, Column(Order = 2)]
            public int ExameId { get; set; }
            

            [Display(Name = "Encaminhamento")]
            public int? PessoaId { get; set; }

            [Display(Name = "Médico Analista")]
            public int? medicoId { get; set; }

            public virtual string medicoDesc
            {
                get
                {
                    if (medicoId != null)
                    {
                        DataContext db = new DataContext();
                        var medico = db.Pessoas.Find(medicoId, EmpresaID);
                        if (medico != null)
                        {
                            return medico.Razao;
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

            public virtual string EncaminhamentoDesc
            {
                get
                {
                    if (PessoaId != null)
                    {
                        DataContext db = new DataContext();
                        var pessoa = db.Pessoas.Find(PessoaId, EmpresaID);
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


            public decimal? ValorExame { get; set; }

            [DataType(DataType.Date, ErrorMessage = "Informe somente data válida.")]
            [Display(Name = "Coleta")]
            public DateTime? DataColeta { get; set; }

            [Display(Name = "Coleta")]
            public string DataColetaF
            {
                get
                {
                    DateTime dtc;
                    if (DataColeta != null)
                    {
                        dtc = Convert.ToDateTime(DataColeta);
                        return dtc.ToShortDateString();
                    }
                    else
                    {
                        return "";
                    }
                    
                    
                    
                }
            }

            [DataType(DataType.Date, ErrorMessage = "Informe somente data válida.")]
            [Display(Name = "Emissão")]
            public DateTime? DataEmissao { get; set; }

            [DataType(DataType.Date, ErrorMessage = "Informe somente data válida.")]
            [Display(Name = "Próxima Consulta")]
            public DateTime? ProximaConsulta { get; set; }

            [Display(Name = "Situação")]
            public int SituacaoExame { get; set; }

            [Display(Name = "Observações")]
            public string Observacao { get; set; }

            public virtual string ExameDesc {
                get
                {
                    DataContext db = new DataContext();
                    var exame = db.Exames.Where(x => x.ID == ExameId && x.EmpresaID == EmpresaID).FirstOrDefault();
                    return exame.Nome;
                }
            }


            [Display(Name = "Forma de Pagamento")]
            public int? FormaPgto { get; set; }
            public virtual string FormaPgtoFormatado
            {
                get
                {
                    if(FormaPgto != null)
                    {
                        if (FormaPgto == 0)
                        {
                            return "À vista";
                        }
                        else
                        {
                            return "Faturamento";
                        }
                    }
                    else
                    {
                        return "Não Informado";
                    }
                }
            }
            public class ListaFormaPagExame
            {
                public int ID { get; set; }
                public string Descricao { get; set; }
                public List<ListaFormaPagExame> MetodoFormaPgtoExame()
                {
                    return new List<ListaFormaPagExame>
                {
                    new ListaFormaPagExame { ID = 0, Descricao = "À VISTA" },
                    new ListaFormaPagExame { ID = 1, Descricao = "FATURAMENTO" },
                    new ListaFormaPagExame { ID = 2, Descricao = "NÃO COBRAR" },


                };
                }
            }

            public class ListaFormaPagF
            {
                public int ID { get; set; }
                public string Descricao { get; set; }
                public List<ListaFormaPagF> MetodoFormaPgto()
                {
                    return new List<ListaFormaPagF>
                {
                    new ListaFormaPagF { ID = 1000, Descricao = "TODOS" },
                    new ListaFormaPagF { ID = 0, Descricao = "Á VISTA" },
                    new ListaFormaPagF { ID = 1, Descricao = "FATURAMENTO" },
                    new ListaFormaPagF { ID = 2, Descricao = "NÃO COBRAR" },


                };
                }
            }

            public class ListaConclusaoExame
            {
                public int ID { get; set; }
                public string Descricao { get; set; }
                public List<ListaConclusaoExame> MetodoConclusao()
                {
                    return new List<ListaConclusaoExame>
                {
                    new ListaConclusaoExame { ID = 0, Descricao = "PENDENTE" },
                    new ListaConclusaoExame { ID = 1, Descricao = "NORMAL" },
                    new ListaConclusaoExame { ID = 2, Descricao = "ALTERADO" },
                    new ListaConclusaoExame { ID = 3, Descricao = "ESTÁVEL" },
                    new ListaConclusaoExame { ID = 4, Descricao = "AGRAVAMENTO" }


                };
                }
            }


            public class ListaConclusaoExameF
            {
                public int ID { get; set; }
                public string Descricao { get; set; }
                public List<ListaConclusaoExameF> MetodoConclusao()
                {
                    return new List<ListaConclusaoExameF>
                {
                    new ListaConclusaoExameF { ID = 1000, Descricao = "TODOS" },
                    new ListaConclusaoExameF { ID = 0, Descricao = "PENDENTE" },
                    new ListaConclusaoExameF { ID = 1, Descricao = "NORMAL" },
                    new ListaConclusaoExameF { ID = 2, Descricao = "ALTERADO" },
                    new ListaConclusaoExameF { ID = 3, Descricao = "ESTÁVEL" },
                    new ListaConclusaoExameF { ID = 4, Descricao = "AGRAVAMENTO" }


                };
                }
            }
            [Display(Name = "Resultado")]
            public virtual string ResultadoExames
            {
                get
                {
                    if (SituacaoExame == 1)
                    {
                        return "NORMAL";
                    }
                    if (SituacaoExame == 0)
                    {
                        return "PENDENTE";
                    }
                    else if (SituacaoExame == 2)
                    {
                        return "ALTERADO";
                    }
                    else if (SituacaoExame == 3)
                    {
                        return "ESTÁVEL";
                    }
                    else
                    {
                        return "AGRAVAMENTO";
                    }
                }
            }

            public int? FaturadoMedico { get; set; } //0 não 1// Sim
            public int? RepasseMedico { get; set; } //0 não 1// Sim

            public int? FaturaPara { get; set; } //1 cliente 2// convenio

            public int? FinanceiroID { get; set; }

        }

    }
}