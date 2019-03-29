using SaudeWeb.Models;
using SaudeWeb.Utils;
using System;
using System.Linq;
using System.Web.Mvc;
using static SaudeWeb.Utils.User;

namespace SaudeWeb.Controllers
{
    public class LoginController : Controller
    {
        Usuario userSite = new Usuario();
        Empresa empresa = new Empresa();
        
        DataContext db = new DataContext();
        EnviarEmail envio = new EnviarEmail();
        Empresa filial = new Empresa();
        // GET: Login
        public ActionResult Index()
        {
            ModelState.Clear();           
            return View();
        }

        [HttpGet]
        public ActionResult Sair()
        {
            ViewBag.UsuarioLogado = null;
            Utils.User.SetCookieUsuarioLogado(Response, Session, "", -1);
            return View("Index");
        }

        [HttpPost]
        public ActionResult Sair(string teste)
        {
            ViewBag.UsuarioLogado = null;
            Utils.User.SetCookieUsuarioLogado(Response, Session, "", -1);
            return View("Index");
        }

        [HttpPost]
        public ActionResult Index(string email, string senha)
        {  
            string decrip, encrip;
            
            var temSenha = db.Usuarios.Where(x => x.email == email).FirstOrDefault();

            encrip = Empresa.CalculaHash(senha);
            if (temSenha != null)
            {
                if (temSenha.senha != "")
                {
                    decrip = temSenha.senha;
                    if (decrip == encrip)
                    {
                        userSite.email = temSenha.email;
                        var select = db.Usuarios.Where(x => x.email == email).FirstOrDefault();
                        var codFilial = select.empresaId;                        
                        ViewBag.UsuarioLogado = select.email;
                        Utils.User.SetCookieUsuarioLogado(Response, Session, select.email, 10);
                        ViewBag.UsuarioLogado = select.email;
                        return Redirect("~/Home");
                    }
                    else
                    {
                        ViewBag.erro = "Senha Incorreta";
                        return View();
                    }
                }
                else
                {
                    return View();
                }
            }
            else
            {
                ViewBag.erro = "Email Não Cadastrado";
                return View();
            }
        }

        [HttpGet]
        public ActionResult RecuperarSenha()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RecuperarSenha(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ViewBag.erro = "Informe o Email";
                return View("RecuperarSenha");
            }
            else
            {
                var temSenha = db.Usuarios.Where(x => x.email == email).FirstOrDefault();
                string cptemail = DateTime.Now.Millisecond.ToString() + DateTime.Now.Minute.ToString() + email.Substring(0, 3) + DateTime.Now.Day;
                ViewBag.email = temSenha.email;
                string retorno = "";
                retorno = Empresa.CalculaHash(cptemail);
                var recSenha = db.Usuarios.Where(x => x.email == email).FirstOrDefault();
                if (recSenha != null)
                {
                    recSenha.senha = retorno;
                    try
                    {
                        db.Entry(recSenha).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        var filial1 = db.Empresas.Find(temSenha.empresaId);
                        string textoEmail = "Olá, voce solicitou uma nova senha para acesso ao sistema Medicina Ocupacional. \n"
                            + "Segue sua nova senha de acesso: " + cptemail + ". \n"
                            + "- Para sua segurança orientamos alterar sua senha no próximo acesso. \n\n\n\n"
                            + filial1.Razao + ". \n"
                            + filial1.Endereco + ", Nº " + filial1.Numero + ", " + filial1.Bairro + ". \n"
                            + filial1.Email + ". \n"
                            + "Fone: " + filial1.Fone + ".\n";

                        envio.EnviaEmail(recSenha.email, "Senha de Acesso", textoEmail, "");
                        ViewBag.sucesso = "Sua Nova Senha foi Enviada para o Email Informadao";
                        return View("Index");

                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else
                {
                    ViewBag.erro = "Email Não Cadastrado, entre em Contato com o Administrador do Sistema";
                    //return Redirect("~/Login");
                    return View();
                }
            }
        }

        [HttpGet]
        public ActionResult CriarSenha()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CriarSenha(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ViewBag.erro = "Informe o Email";
                return View("CriarSenha");
            }
            else
            {
                string cptemail = DateTime.Now.Millisecond.ToString() + DateTime.Now.Minute.ToString() + email.Substring(0, 3) + DateTime.Now.Day;
                string retorno = "";
                retorno = Empresa.CalculaHash(cptemail);
                var recSenha = db.Usuarios.Where(x => x.email == email).FirstOrDefault();                
                if (recSenha != null)
                {
                    recSenha.senha = retorno;
                    try
                    {
                        var filial1 = db.Empresas.Find(recSenha.empresaId);
                        string textoEmail = "Olá, voce solicitou uma nova senha para acesso ao sistema Medicina Ocupacional. \n"
                            + "Segue sua nova senha de acesso: " + cptemail + ". \n"
                            + "- Para sua segurança orientamos alterar sua senha no próximo acesso. \n\n\n\n"
                            + filial1.Razao + ". \n"
                            + filial1.Endereco + ", Nº " + filial1.Numero + ", " + filial1.Bairro + ". \n"
                            + filial1.Email + ". \n"
                            + "Fone: " + filial1.Fone + ".\n";

                        db.Entry(recSenha).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        envio.EnviaEmail(recSenha.email, "Senha de Acesso", textoEmail, "");
                        ViewBag.sucesso = "Sua Nova Senha foi Enviada para o Email Informadao";
                        return View("Index");
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else
                {
                    ViewBag.erro = "Email Não Cadastrado, entre em Contato com o Administrador do Sistema";                    
                    return View();
                }
            }
        }

        public ActionResult AlterarSenha()
        {
            ViewBag.UsuarioLogado = Utils.User.GetCookieUsuarioLogado(Request, Session);
            if (!string.IsNullOrEmpty(ViewBag.UsuarioLogado))
            {
                Utils.User.UsuarioLogado usuariologado = Utils.User.GetDadosUsuarioLogado(ViewBag.UsuarioLogado);
                ViewBag.RuleAdmin = usuariologado.admin;
                ViewBag.RuleFinanceiro = usuariologado.RuleFinanceiro;
                ViewBag.RuleMovimentacao = usuariologado.RuleMovimentacao;
                ViewBag.RuleCadastro = usuariologado.RuleCadastro;
                ViewBag.User = usuariologado.Email;
                return View();
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        [HttpPost]
        public ActionResult AlterarSenha(string email, string senha1, string senha2)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha1) || string.IsNullOrEmpty(senha2))
            {

                ViewBag.erro = "Confirme os dados para alterar a senha";
                ViewBag.User = userSite.email;
                return View("AlterarSenha");
            }
            else
            {
                if (senha1 != senha2)
                {
                    ViewBag.erro = "As senhas digitadas são diferentes";
                    ViewBag.User = userSite.email;
                    return View("AlterarSenha");
                }
                else
                {
                    string retorno = "";
                    retorno = Empresa.CalculaHash(senha1);
                    var recSenha = db.Usuarios.Where(x => x.email == email).FirstOrDefault();
                    if (recSenha != null)
                    {
                        recSenha.senha = retorno;
                        try
                        {
                            db.Entry(recSenha).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            ViewBag.sucesso = "Senha Alterada Com Sucesso";

                        }
                        catch (Exception e)
                        {
                            ViewBag.erro = "Erro ao Alterar a Senha " + e.ToString();
                        }
                    }
                    else
                    {
                        ViewBag.erro = "Email Não Cadastrado, entre em Contato com o Administrador do Sistema";
                        return Redirect("~/Login");
                    }
                    ViewBag.sucesso = "Senha Alterada Com Sucesso";
                    return Redirect("~/Login");

                }
            }

        }
    }
}



