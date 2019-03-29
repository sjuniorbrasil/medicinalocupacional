using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SaudeWeb.Models
{
    public class Empresa
    {
        public Empresa()
        {

        }
        [Key]
        public int ID { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "Campo de Prenchimento Obrigatório")]
        [Display(Name = "Razão Social")]
        public string Razao { get; set; }

        [MaxLength(100)]
        public string Fantasia { get; set; }

        [MaxLength(20)]
        public string CNAE { get; set; }

        [MaxLength(20)]
        public string CNPJ { get; set; }

        [MaxLength(20)]
        public string IE { get; set; }

        [MaxLength(20)]
        public string IM { get; set; }

        [MaxLength(100)]
        [Display(Name = "Endereço")]
        public string Endereco { get; set; }

        [MaxLength(50)]
        public string Bairro { get; set; }

        [MaxLength(10)]
        [Display(Name = "Número")]
        public string Numero { get; set; }

        [Display(Name = "Cidade")]
        public int CidadeID { get; set; }

        [MaxLength(15)]
        public string Cep { get; set; }

        [MaxLength(30)]
        public string Fone { get; set; }

        [MaxLength(100)]
        [Display(Name = "Responsável")]
        public string Responsavel { get; set; }

        [MaxLength(150)]
        public string Email { get; set; }

        [MaxLength(100)]
        [Display(Name ="Audiômetro")]
        public string Audiometro { get; set; }

        public int? ModuloFinanceiro { get; set; }

        [MaxLength(50)]
        [Display(Name = "Repouso Acústico")]
        public string RepousoAcustico { get; set; }

        [Display(Name = "Data Calibração")]
        public DateTime? DataCalibracao { get; set; }

        [Display(Name = "Plano de Contas Consultas")]
        public int? PlanoContaConsulta { get; set; }

        public virtual string PlanoContaConsultaDesc
        {
            get
            {
                DataContext db = new DataContext();
                var categoria = db.PlanoContas.Where(x => x.ID == PlanoContaConsulta && x.EmpresaID == ID).FirstOrDefault();
                if (categoria != null)
                {
                    return categoria.Conta + " - " + categoria.Descricao;
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

        public static string CalculaHash(string Senha)
        {
            try
            {
                System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(Senha);
                byte[] hash = md5.ComputeHash(inputBytes);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }
                return sb.ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }

        

    }
}