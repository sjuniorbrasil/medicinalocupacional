using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace SaudeWeb.Models
{
    public class HistoricoCliente
    {
        public int ClienteID { get; set; }

        public string ClienteRazao
        {
            get
            {
                DataContext db = new DataContext();

                var cliente = db.Pessoas.Where(x => x.EmpresaID == EmpresaID && x.ID == ClienteID).FirstOrDefault();
                if (cliente != null)
                {
                    return cliente.Razao;
                }
                else
                {
                    return "";
                }
            }
        }

        public int EmpresaID { get; set; }
        public HistoricoCliente(int cliente, int empresa)
        {
            this.ClienteID = cliente;
            this.EmpresaID = empresa;
        }

        public List<HistoricoFinanceiro> RecPags
        {
            get
            {
                List<HistoricoFinanceiro> lista = new List<HistoricoFinanceiro>();
                DataContext db = new DataContext();
                var financeiro = db.RecPag.Where(x => x.EmpresaID == EmpresaID && x.PessoaID == ClienteID);
                foreach (var item in financeiro)
                {
                    if(financeiro != null)
                    {
                        HistoricoFinanceiro historico = new HistoricoFinanceiro();
                        if(item.DataLiquidacao != null)
                        {
                            historico.DataLiquidacao = Convert.ToDateTime(item.DataLiquidacao).ToShortDateString();
                        }                        
                        historico.Emissao = Convert.ToDateTime(item.DataEmissao).ToShortDateString();
                        historico.DataVencimento = Convert.ToDateTime(item.DataVencimento).ToShortDateString();
                        historico.EmpresaID = item.EmpresaID;
                        historico.NumeroDocumento = item.NumeroDocumento;
                        historico.Valor = Convert.ToDecimal(item.Valor).ToString("C", CultureInfo.CurrentCulture);
                        if(item.ValorBaixado != null)
                        {
                            historico.ValorBaixa = Convert.ToDecimal(item.ValorBaixado).ToString("C", CultureInfo.CurrentCulture);
                        }
                        lista.Add(historico);
                    }
                }
                return lista;
            }
        }

        public List<HistoricoConsultas> Consultas
        {
            get
            {
                List<HistoricoConsultas> lista = new List<HistoricoConsultas>();
                DataContext db = new DataContext();
                var consultas = db.Consultas.Where(x => x.EmpresaID == EmpresaID && x.PessoaId == ClienteID);
                if(consultas != null)
                {
                    foreach (var item in consultas)
                    {
                        HistoricoConsultas historico = new HistoricoConsultas();
                        historico.ConsultaID = item.Id;
                        historico.DataConsulta = item.DataConclusaoF;
                        historico.EmpresaID = item.EmpresaID;
                        historico.Funcionario = item.funcionarioDesc;
                        historico.Situacao = item.SituacaoConsulta;                        
                        lista.Add(historico);
                    }
                }
                return lista;
            }
            
        }

        public class HistoricoFinanceiro
        {
            public string Emissao { get; set; }

            public string NumeroDocumento { get; set; }            

            public string Valor { get; set; }

            public string DataVencimento { get; set; }

            public string DataLiquidacao { get; set; }

            public int EmpresaID { get; set; }

            public string ValorBaixa { get; set; }
        }

        public class HistoricoConsultas
        {
            public int EmpresaID { get; set; }

            public int ConsultaID { get; set; }

            public string Funcionario { get; set; }

            public string DataConsulta { get; set; }

            public int Situacao { get; set; }

            public List<ExameConsulta> Exames
            {
                get
                {
                    List<ExameConsulta> lista = new List<ExameConsulta>();
                    DataContext db = new DataContext();
                    var exames = db.ConsultaExames.Where(x => x.EmpresaID == EmpresaID && x.ConsultaID == ConsultaID);
                    if(exames != null)
                    {
                        foreach (var item in exames)
                        {
                            ExameConsulta exame = new ExameConsulta();
                            exame.DataExame = Convert.ToDateTime(item.DataColeta).ToShortDateString();
                            exame.Exame = item.ExameDesc;
                            exame.SituacaoExame = item.ResultadoExames;
                            lista.Add(exame);
                        }
                    }
                    return lista;
                }
            }

            public virtual string SituacaoF
            {
                get
                {
                    if(Situacao == 0)
                    {
                        return "PENDENTE";
                    }
                    else if(Situacao == 1)
                    {
                        return "APTO";
                    }
                    else
                    {
                        return "INAPTO";
                    }
                }
            }

            public class ExameConsulta
            {
                public string Exame { get; set; }

                public string DataExame { get; set; }

                public string SituacaoExame { get; set; }

                //public string SituacaoDesc
                //{
                //    get
                //    {
                //        if(SituacaoExame == 0)
                //        {
                //            return "PENDENTE";
                //        }
                //        if (SituacaoExame == 1)
                //        {
                //            return "NORMAL";
                //        }
                //        if (SituacaoExame == 2)
                //        {
                //            return "ALTERADO";
                //        }
                //        if (SituacaoExame == 3)
                //        {
                //            return "ESTÁVEL";
                //        }
                //        return "AGRAVAMENTO";
                //    }
                //}
            }

        }
    }
}