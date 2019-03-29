using SaudeWeb.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SaudeWeb.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            ViewBag.UsuarioLogado = Utils.User.GetCookieUsuarioLogado(Request, Session);
            if (!string.IsNullOrEmpty(ViewBag.UsuarioLogado))
            {
                Utils.User.UsuarioLogado usuariologado = Utils.User.GetDadosUsuarioLogado(ViewBag.UsuarioLogado);
                ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;
                ViewBag.RuleAdmin = usuariologado.admin;
                ViewBag.RuleFinanceiro = usuariologado.RuleFinanceiro;
                ViewBag.RuleMovimentacao = usuariologado.RuleMovimentacao;
                ViewBag.RuleCadastro = usuariologado.RuleCadastro;
                DataContext db = new DataContext();
                var plconta = db.PlanoContas.Count(x => x.EmpresaID == usuariologado.empresaId);
                if (plconta == 0)
                {
                    PlanoConta plContaC = new PlanoConta();
                    plContaC.ID = 1;
                    plContaC.EmpresaID = Convert.ToInt32(usuariologado.empresaId);
                    plContaC.Descricao = "RECEITAS";
                    plContaC.Conta = "01.";
                    plContaC.Operacao = 1;
                    db.PlanoContas.Add(plContaC);
                    db.SaveChanges();
                    
                    PlanoConta plContaD = new PlanoConta();
                    plContaD.ID = 2;
                    plContaD.Operacao = 2;
                    plContaD.EmpresaID = Convert.ToInt32(usuariologado.empresaId);
                    plContaD.Descricao = "DESPESAS";
                    plContaD.Conta = "02.";
                    db.PlanoContas.Add(plContaD);
                    db.SaveChanges();
                    
                }
                var conta = db.ContaCorrenteCaixas.Where(x => x.EmpresaID == usuariologado.empresaId && x.ID == 1).FirstOrDefault();
                if(conta == null)
                {
                    ContaCorrenteCaixa c = new ContaCorrenteCaixa();
                    c.ID = 1;
                    c.NumeroConta = "CAIXA";
                    c.ContaPadrao = 1;
                    c.Banco = 999;
                    c.Descricao = "CONTA CAIXA";
                    c.EmpresaID = Convert.ToInt32(usuariologado.empresaId);
                    db.ContaCorrenteCaixas.Add(c);
                    db.SaveChanges();
                }
                var teste = Utils.LibProdusys.SqlIn(",,12,,1,55,,,");
                return View();
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }
       
    }
}