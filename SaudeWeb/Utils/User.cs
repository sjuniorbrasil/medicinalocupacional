using SaudeWeb.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace SaudeWeb.Utils
{
    public static class User
    {
        public class UsuarioLogado
        {
            public string Email { get; set; }

            public string PasswordHash { get; set; }

            public int? admin { get; set; }

            public int? empresaId { get; set; }

            public int? moduloFinanceiro { get; set; }

            public int? RuleCadastro { get; set; }

            public int? RuleMovimentacao { get; set; }

            public int? RuleFinanceiro { get; set; }
        }
        public static string MD5(string S)
        {
            try
            {
                System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(S);
                byte[] hash = md5.ComputeHash(inputBytes);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("fw30264045"));
                }
                return sb.ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }



        public class Usuario
        {
            public int Id { get; set; }

            [Display(Name = "Email")]
            [Required]
            public string email { get; set; }
            [Display(Name = "Senha")]
            public string senha { get; set; }

            public int? admin { get; set; }

            public int? empresaId { get; set; }

            public int? RuleCadastro { get; set; }

            public int? RuleMovimentacao { get; set; }

            public int? RuleFinanceiro { get; set; }
        }

        public static UsuarioLogado GetDadosUsuarioLogado(string UsuarioLogado)
        {
            DataContext db = new DataContext();
            Usuario usuario = db.Usuarios.Where(a => a.email == UsuarioLogado).FirstOrDefault();
            if (usuario == null)
            {
                return null;
            }
            else
            {

                UsuarioLogado retorno = new UsuarioLogado();
                retorno.Email = usuario.email;
                retorno.admin = usuario.admin;
                retorno.empresaId = usuario.empresaId;
                retorno.admin = usuario.admin;
                retorno.RuleCadastro = usuario.RuleCadastro;
                retorno.RuleFinanceiro = usuario.RuleFinanceiro;
                retorno.RuleMovimentacao = usuario.RuleMovimentacao;
                var modulos = db.Empresas.Where(x => x.ID == usuario.empresaId).FirstOrDefault();
                if (modulos != null)
                {
                    retorno.moduloFinanceiro = modulos.ModuloFinanceiro;
                }
                else
                {
                    retorno.moduloFinanceiro = 0;
                }

                return retorno;
            }

        }


        public static void SetCookieUsuarioLogado(HttpResponseBase AResponse, HttpSessionStateBase ASession, string Email, int Validade)
        {
            if (string.IsNullOrEmpty(Email))
            {
                ASession["Email"] = null;
            }
            else
            {
                ASession["Email"] = Email;
            }
            AResponse.Cookies["UsuarioLogado"]["Email"] = Email;
            AResponse.Cookies["UsuarioLogado"].Expires = DateTime.Now.Date.AddDays(Validade);
        }


        public static string GetCookieUsuarioLogado(HttpRequestBase ARequest, HttpSessionStateBase ASession)
        {
            if (ASession["Email"] == null)
            {
                var c = ARequest.Cookies["UsuarioLogado"];
                if (c == null)
                {
                    return "";
                }
                else
                {
                    return c["Email"];
                }
            }
            else
            {
                return ASession["Email"].ToString();
            }
        }


    }
}