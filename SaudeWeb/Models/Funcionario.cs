using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaudeWeb.Models
{
    public class Funcionario
    {

        public Funcionario()
        {

        }
        [Key, Column(Order = 0)]
        public int ID { get; set; }

        [Key, Column(Order = 1)]
        public int EmpresaID { get; set; }
        public Empresa Empresa { get; set; }

        public int? ClienteID { get; set; }

        [Display(Name = "Nome")]
        [StringLength(100), Required(ErrorMessage = "Campo de preenchimento obrigatório")]
        public string Nome { get; set; }

        [Display(Name = "Naturalidade")]
        [StringLength(50)]
        public string Naturalidade { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Informe somente data válida.")]
        [Display(Name = "Data de Nascimento")]
        public DateTime? DataNascimento { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Informe somente data válida.")]
        public DateTime? DataCadastro { get; set; }

        [Display(Name = "Situação")]
        public int Situacao { get; set; }

        [Display(Name = "Sexo")]
        public int? Sexo { get; set; } // 1 - masculino - 2 feminino
        public virtual string SexoFormatado
        {
            get
            {
                if (Sexo != null)
                {
                    if (Sexo == 1)
                    {
                        return "Masculino";
                    }
                    else
                    {
                        return "Feminino";
                    }
                }
                else
                {
                    return "Não Informado";
                }
            }
        }

        [Display(Name = "Estado Cívil")]
        public int? EstadoCivil { get; set; } // 1 - solteiro, 2 - casado, 3 viuvo, 4 divorcio, 5 outros
        public virtual string EstadoCivilFormatado
        {
            get
            {
                if (EstadoCivil != null)
                {
                    if (EstadoCivil == 1)
                    {
                        return "Solteiro(a)";
                    }
                    else if (EstadoCivil == 2)
                    {
                        return "Casado(a)";
                    }
                    else if (EstadoCivil == 3)
                    {
                        return "Viúvo(a)";
                    }
                    else if (EstadoCivil == 4)
                    {
                        return "Divorciado(a)";
                    }
                    else
                    {
                        return "Outros";
                    }
                }
                else
                {
                    return "Não Informado";
                }
            }
        }
        public class ListaEstadoCivil
        {
            public int ID { get; set; }
            public string Descricao { get; set; }
            public List<ListaEstadoCivil> MetodoListaEstadoCivil()
            {
                return new List<ListaEstadoCivil>
                {
                    new ListaEstadoCivil { ID = 1, Descricao = "Solteiro(a)" },
                    new ListaEstadoCivil { ID = 2, Descricao = "Casado(a)" },
                    new ListaEstadoCivil { ID = 3, Descricao = "Viúvo(a)" },
                    new ListaEstadoCivil { ID = 4, Descricao = "Divorciado(a)" },
                    new ListaEstadoCivil { ID = 5, Descricao = "Outros" }
                };
            }
        }

        [Display(Name = "CPF")]
        [StringLength(20)]
        public string CPF { get; set; }

        [Display(Name = "RG")]
        [StringLength(20)]
        public string RG { get; set; }

        [StringLength(20)]
        public string NIT { get; set; }

        [StringLength(20)]
        public string CNH { get; set; }

        [Display(Name = "CTPS")]
        [StringLength(50)]
        public string CTPS { get; set; }

        [Display(Name = "Endereço")]
        [StringLength(100)]
        public string Endereco { get; set; }

        [Display(Name = "Número")]
        [StringLength(10)]
        public string Numero { get; set; }

        [Display(Name = "Complemento")]
        [StringLength(50)]
        public string Complemento { get; set; }

        [Display(Name = "Bairro")]
        [StringLength(100)]
        public string Bairro { get; set; }

        [Display(Name = "CEP")]
        [StringLength(50)]
        public string CEP { get; set; }

        [Display(Name = "Fone 1")]
        [StringLength(20)]
        public string Fone1 { get; set; }

        [Display(Name = "Fone 2")]
        [StringLength(20)]
        public string Fone2 { get; set; }

        [Display(Name = "Email")]
        [StringLength(150)]
        public string Email { get; set; }

        [Display(Name = "Observações")]
        [StringLength(1000)]
        public string Observacao { get; set; }

        public virtual string CidadeDesc
        {
            get
            {
                if (CidadeID != null)
                {
                    DataContext db = new DataContext();
                    var cidad = db.Cidades.Find(CidadeID);
                    return cidad.CidadeUF;
                }
                else
                {
                    return "";
                }
                
            }
        }

        public Cidade cidade { get; set; }
        public int? CidadeID { get; set; }
                       
        
    }
}