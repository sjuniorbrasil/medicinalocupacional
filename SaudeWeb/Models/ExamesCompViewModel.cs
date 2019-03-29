using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SaudeWeb.Models
{
    public class ExamesCompViewModel
    {        
        public int ConsultaID { get; set; }
        
        public int EmpresaID { get; set; }

        public int ExameId { get; set; }

        public int FuncionarioID { get; set; }

        public int? PessoaId { get; set; }

        
        public int? medicoId { get; set; }


        public decimal? ValorExame { get; set; }

        
        public DateTime? DataColeta { get; set; }

        public int TipoConsulta { get; set; }


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
        
        public DateTime? DataEmissao { get; set; }
        
        public DateTime? ProximaConsulta { get; set; }

        public DateTime? DataConsulta { get; set; }
        public string DataConsultaF {
            get
            {
                if(DataConsulta != null)
                {
                    DateTime dt =  Convert.ToDateTime(DataConsulta);
                    return dt.ToShortDateString();
                }
                else
                {
                    return "";
                }
            }
        }

        public DateTime? DataNascimento { get; set; }

        public int SituacaoExame { get; set; }

        public int SituacaoConsulta { get; set; }

        public string Observacao { get; set; }

        public string Cliente { get; set; }
        public string Encaminhado { get; set; }

        public string Medico { get; set; }

        public string Funcionario { get; set; }

        public virtual string ExameDesc
        {
            get
            {
                DataContext db = new DataContext();
                var exame = db.Exames.Where(x => x.ID == ExameId && x.EmpresaID == EmpresaID).FirstOrDefault();
                return exame.Nome;
            }
        }
        
        public int? FormaPgto { get; set; }
        public virtual string FormaPgtoFormatado
        {
            get
            {
                if (FormaPgto != null)
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
        public string ResultadoExames
        {
            get
            {
                if (SituacaoExame == 1)
                {
                    return "NORMAL";
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
        public List<ExamesCompViewModel> ListaExames(string empresaid, string codcliente, string codmedico, string codencaminhamento, string dataconsulta,
                      string dataconsulta1, string situacaoconsulta, string tipoconsulta, string formapagamento, string situacaoexame, string consultaid, string codfuncionario, string codexame)
        {
            List<ExamesCompViewModel> lista = new List<ExamesCompViewModel>();
            string select = " select a.ConsultaID, a.DataColeta, a.DataEmissao, a.EmpresaID, a.ExameID, b.tipoconsulta, "
                          + " a.FormaPgto, a.medicoId, a.pessoaID, a.SituacaoExame, Coalesce(a.ValorExame,0), b.dataconsulta, b.situacaoconsulta, "
                          + " f.Descricao, d.razao as Encaminhado, e.razao as Medico, c.nome as Funcionario, c.datanascimento, g.razao"
                          + " from consultaexame a " 
                          + " join consulta b on a.consultaid = b.ID and a.EmpresaID = b.EmpresaID "
                          + " left join funcionario c on b.FuncionarioID = c.id and b.EmpresaID = c.empresaid "
                          + " left join pessoa d on a.pessoaID = d.id and a.EmpresaID = d.empresaid "
                          + " left join pessoa e on a.medicoId = e.id and a.EmpresaID = e.empresaid "
                          + " left join exame f on a.ExameID = f.ID and a.EmpresaID = f.EmpresaID "
                          + " left join pessoa g on b.PessoaID = g.id and b.EmpresaID = g.empresaid"
                          + " where b.id is not null and a.empresaid = "+ empresaid
                          + " and b.pessoaid is not null "
                          + " and b.funcionarioid is not null ";

            if (!string.IsNullOrEmpty(codexame))
            {
                select += " and a.exameid = " + codexame;
            }

            if (!string.IsNullOrEmpty(codfuncionario))
            {
                select += " and b.funcionarioid = " + codfuncionario;
            }
            if (!string.IsNullOrEmpty(consultaid))
            {
                select += " and a.consultaid = " + consultaid;
            }
            if (!string.IsNullOrEmpty(codcliente))
            {
                select += " and b.pessoaid = " + codcliente;
            }
            if (!string.IsNullOrEmpty(codmedico))
            {
                select += " and a.medicoid = " + codmedico;
            }
            if (!string.IsNullOrEmpty(codencaminhamento))
            {
                select += " and a.pessoaid = " + codencaminhamento;
            }
            if (!string.IsNullOrEmpty(dataconsulta))
            {
                select += " and b.dataconsulta >= '" + dataconsulta + "'";
            }
            if (!string.IsNullOrEmpty(dataconsulta1))
            {
                select += " and b.dataconsulta <= '" + dataconsulta1 + "'";
            }
            if (!string.IsNullOrEmpty(situacaoconsulta))
            {
                if(situacaoconsulta != "10")
                {
                    select += " and b.situacaoconsulta = " + situacaoconsulta;
                }                
            }
            if (!string.IsNullOrEmpty(tipoconsulta))
            {
                if (tipoconsulta != "1000")
                {
                    select += " and b.tipoconsulta = " + tipoconsulta;
                }
            }

            if (!string.IsNullOrEmpty(formapagamento))
            {
                if (formapagamento != "1000")
                {
                    select += " and a.FormaPgto = " + formapagamento;
                }
            }

            if (!string.IsNullOrEmpty(situacaoexame))
            {
                if (situacaoexame != "1000")
                {
                    select += " and a.SituacaoExame = " + situacaoexame;
                }
            }


            SqlConnection con = new SqlConnection(Properties.Settings.Default.Banco);
            SqlCommand cmd = new SqlCommand(select, con);
            cmd.CommandType = System.Data.CommandType.Text;
            try
            {
                con.Open();
                SqlDataReader dR = cmd.ExecuteReader();
                while (dR.Read())
                {
                    ExamesCompViewModel exames = new ExamesCompViewModel();
                    if (!string.IsNullOrEmpty(dR[0].ToString()))
                    {
                        exames.ConsultaID = Convert.ToInt32(dR[0].ToString());
                    }

                    if (!string.IsNullOrEmpty(dR[1].ToString()))
                    {
                        exames.DataColeta = Convert.ToDateTime(dR[1].ToString());
                    }

                    if (!string.IsNullOrEmpty(dR[2].ToString()))
                    {
                        exames.DataEmissao = Convert.ToDateTime(dR[2].ToString());
                    }

                    if (!string.IsNullOrEmpty(dR[3].ToString()))
                    {
                        exames.EmpresaID = Convert.ToInt32(dR[3].ToString());
                    }

                    if (!string.IsNullOrEmpty(dR[4].ToString()))
                    {
                        exames.ExameId = Convert.ToInt32(dR[4].ToString());
                    }
                    if (!string.IsNullOrEmpty(dR[5].ToString()))
                    {
                        exames.TipoConsulta = Convert.ToInt32(dR[5].ToString());
                    }
                    if (!string.IsNullOrEmpty(dR[6].ToString()))
                    {
                        exames.FormaPgto = Convert.ToInt32(dR[6].ToString());
                    }

                    if (!string.IsNullOrEmpty(dR[7].ToString()))
                    {
                        exames.medicoId = Convert.ToInt32(dR[7].ToString());
                    }
                    if (!string.IsNullOrEmpty(dR[8].ToString()))
                    {
                        exames.PessoaId = Convert.ToInt32(dR[8].ToString());
                    }
                    if (!string.IsNullOrEmpty(dR[9].ToString()))
                    {
                        exames.SituacaoExame = Convert.ToInt32(dR[9].ToString());
                    }

                    if (!string.IsNullOrEmpty(dR[10].ToString()))
                    {
                        exames.ValorExame = Convert.ToDecimal(dR[10].ToString());
                    }
                    if (!string.IsNullOrEmpty(dR[11].ToString()))
                    {
                        exames.DataConsulta = Convert.ToDateTime(dR[11].ToString());
                    }

                    if (!string.IsNullOrEmpty(dR[12].ToString()))
                    {
                        exames.SituacaoConsulta = Convert.ToInt32(dR[12].ToString());
                    }

                    exames.Encaminhado = dR[15].ToString();
                    exames.Medico = dR[14].ToString();
                    exames.Funcionario = dR[16].ToString();

                    if (!string.IsNullOrEmpty(dR[17].ToString()))
                    {
                        exames.DataNascimento = Convert.ToDateTime(dR[17].ToString());
                    }
                    exames.Cliente = dR[18].ToString();


                    lista.Add(exames);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
            return lista;
        }
    }
}