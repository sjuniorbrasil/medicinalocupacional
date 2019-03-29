using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;

namespace SaudeWeb.Models
{
    public class Pessoa
    {
        
        public Pessoa()
        {

        }
        [Key, Column(Order = 0)]
        public int ID { get; set; }

        [Key, Column(Order = 1)]
        public int EmpresaID { get; set; }
        public virtual Empresa Empresa { get; set; }

        [Display(Name = "Tabela de Preço")]
        public int? TabelaID { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Informe somente data válida.")]
        public DateTime DataCadastro { get; set; }
        public virtual string DataCadastroFormatado
        {
            get
            {
                DateTime data = DataCadastro;


                return data.ToShortDateString();
            }
        }

        [Display(Name = "Situação")]
        public int Situacao { get; set; } //0 - ativo 1 - inativo

        [Display(Name = "Pessoa")]
        public int TipoPessoa { get; set; } //1 - fisica, 2 - jurídica
        public virtual string TipoPessoaFormatado
        {
            get
            {
                if (TipoPessoa == 0)
                {
                    return "Física";
                }
                else if (TipoPessoa == 2)
                {
                    return "Jurídica";
                }
                else
                {
                    return "";
                }
            }
        }

        public virtual string CidadeDesc
        {
            get
            {
                if (CidadeID != null)
                {
                    DataContext db = new DataContext();
                    var cidade = db.Cidades.Find(CidadeID);
                    return cidade.CidadeUF;
                }
                else
                {
                    return "";
                }
            }
        }

        public int TipoCadastro { get; set; } //1 - cliente , 2 medico 
        public virtual string TipoCadastroFormatado
        {
            get
            {
                if (TipoCadastro == 1)
                {
                    return "Cliente";
                }
                else if (TipoCadastro == 2)
                {
                    return "Médico";
                }
                else
                {
                    return "";
                }
            }
        }

        [MaxLength(150)]
        [Required]
        [Display(Name = "Razão")]
        public string Razao { get; set; }

        [MaxLength(100)]
        public string Fantasia { get; set; }

        [MaxLength(15)]
        public string CNAE { get; set; }

        [MaxLength(20)]
        [Display(Name = "CNPJ/CPF")]        
        public string CNPJ { get; set; }

        [MaxLength(20)]
        [Display(Name = "Inscrição Estadual")]
        public string IE { get; set; }

        [MaxLength(100)]
        [Display(Name = "Endereço")]
        public string Endereco { get; set; }

        [MaxLength(15)]
        [Display(Name = "Número")]
        public string Numero { get; set; }

        [MaxLength(100)]
        public string Complemento { get; set; }

        [MaxLength(100)]
        public string Bairro { get; set; }

        [MaxLength(20)]
        public string CEP { get; set; }

        [MaxLength(20)]
        public string Fone1 { get; set; }

        [MaxLength(20)]
        public string Fone2 { get; set; }

        [MaxLength(20)]
        public string Fone3 { get; set; }

        [MaxLength(50)]
        public string Contato { get; set; }

        [MaxLength(150)]
        public string Email { get; set; }

        [MaxLength(1000)]
        [Display(Name = "Observações")]
        public string Observacao { get; set; }
        
        public Cidade cidade { get; set; }
        [Display(Name = "Cidade")]
        public int? CidadeID { get; set; }
        
        public int? Sexo { get; set; }
        public virtual string SexoFormatado
        {
            get
            {
               if(Sexo == 1)
                {
                    return "Masculino";
                }
                else
                {
                    return "Feminino";
                }
            }
        }

        public int? RepasseProporcional { get; set; }

        [MaxLength(20)]
        public string CRM { get; set; }

        [Display(Name = "Forma de Pagamento")]
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
                    else if (FormaPgto == 1)
                    {
                        return "Faturamento";
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "Não Informado";
                }
            }
        }

        public class PessoaPesquisa
        {
            public int ID { get; set; }
            public string Razao { get; set; }

        }

        public List<PessoaPesquisa> ListaPessoas(string empresaid, string filtro)
        {
            List<PessoaPesquisa> lista = new List<PessoaPesquisa>();
            string select = " select a.id, a.razao from pessoa a "
                          + " join consulta b on a.id = b.pessoaid and a.empresaid = b.empresaid "
                          + " where coalesce(b.fatura,0) = 1 "
                          + " and a.empresaid = " + empresaid;
            if (!string.IsNullOrEmpty(filtro))
            {
                select += " a.razao like '%"+ filtro + "%' ";
            }
            select += " Group by a.id, a.razao "
                     +" Order By a.razao, a.id ";

            SqlConnection con = new SqlConnection(Properties.Settings.Default.Banco);
            SqlCommand cmd = new SqlCommand(select, con);
            cmd.CommandType = System.Data.CommandType.Text;
            try
            {
                con.Open();
                SqlDataReader dR = cmd.ExecuteReader();
                while (dR.Read())
                {
                    PessoaPesquisa pessoa = new PessoaPesquisa();
                    if (!string.IsNullOrEmpty(dR[0].ToString()))
                    {
                        pessoa.ID = Convert.ToInt32(dR[0].ToString());
                    }
                    if (!string.IsNullOrEmpty(dR[1].ToString()))
                    {
                        pessoa.Razao = dR[1].ToString();
                    }                    
                    lista.Add(pessoa);
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


        public class ListaSituacao
        {
            public int ID { get; set; }
            public string Descricao { get; set; }
            public List<ListaSituacao> MetodoListaSituacao()
            {
                return new List<ListaSituacao>
                {
                    new ListaSituacao { ID = 0, Descricao = "ATIVO" },
                    new ListaSituacao { ID = 1, Descricao = "INATIVO" }
                };
            }
        }

        public class ListaTipoOPessoa
        {
            public int ID { get; set; }
            public string Descricao { get; set; }
            public List<ListaTipoOPessoa> MetodoListaTipoPessoa()
            {
                return new List<ListaTipoOPessoa>
                {
                    new ListaTipoOPessoa { ID = 1, Descricao = "FÍSICA" },
                    new ListaTipoOPessoa { ID = 2, Descricao = "JURÍDICA" }
                };
            }
        }

        public class ListaTipoCadastro
        {
            public int ID { get; set; }
            public string Descricao { get; set; }
            public List<ListaTipoCadastro> MetodoListaCadastro()
            {
                return new List<ListaTipoCadastro>
                {
                    new ListaTipoCadastro { ID = 1, Descricao = "CLIENTE" },
                    new ListaTipoCadastro { ID = 2, Descricao = "MÉDICO" }
                };
            }
        }

        public class ListaSexo
        {
            public int ID { get; set; }
            public string Descricao { get; set; }
            public List<ListaSexo> MetodoListaSexo()
            {
                return new List<ListaSexo>
                {
                    new ListaSexo { ID = 1, Descricao = "MASCULINO" },
                    new ListaSexo { ID = 2, Descricao = "FEMININO" }
                };
            }
        }

        public class ListaFormaPgto
        {
            public int ID { get; set; }
            public string Descricao { get; set; }
            public List<ListaFormaPgto> MetodoListaFormaPgto()
            {
                return new List<ListaFormaPgto>
                {                    
                    new ListaFormaPgto { ID = 1, Descricao = "À VISTA" },
                    new ListaFormaPgto { ID = 2, Descricao = "FATURAMENTO" }
                };
            }
        }

        public class PessoaSetor
        {
            public int EmpresaId { get; set; }

            public int PessoaId { get; set; }

            public int SetorId { get; set; }

            public int FuncaoId { get; set; }

            public int AgenteId { get; set; }

            public int ExameId { get; set; }

            public int TipoConsulta { get; set; } // ver consulta

            


        }
    }
}