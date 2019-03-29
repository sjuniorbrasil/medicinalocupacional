using SaudeWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaudeWeb.Controllers
{
    public class HistoricoClienteController : Controller
    {
        DataContext db = new DataContext();
        // GET: HistoricoCliente
        public ActionResult Index(int id)
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
                HistoricoCliente historicoCliente = new HistoricoCliente(id, Convert.ToInt32(usuariologado.empresaId));
                List<HistoricoCliente> lista = new List<HistoricoCliente>();
                lista.Add(historicoCliente);
                return View(lista);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }
    }
}