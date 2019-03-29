using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SaudeWeb.Models;
using SaudeWeb.Utils;

namespace SaudeWeb.Controllers
{
    public class AgendaController : Controller
    {
        private DataContext db = new DataContext();
                
        public ActionResult Index( string dataF, int? situacao)
        {
            ViewBag.Situacao = new SelectList(new Agenda.ListaSituacaoAgenda().MetodoListaAgenda(), "ID", "Descricao");
            ViewBag.UsuarioLogado = Utils.User.GetCookieUsuarioLogado(Request, Session);
            if (!string.IsNullOrEmpty(ViewBag.UsuarioLogado))
            {
                Utils.User.UsuarioLogado usuariologado = Utils.User.GetDadosUsuarioLogado(ViewBag.UsuarioLogado);
                ViewBag.RuleFinanceiro = usuariologado.RuleFinanceiro;
                ViewBag.RuleMovimentacao = usuariologado.RuleMovimentacao;
                ViewBag.RuleCadastro = usuariologado.RuleCadastro;
                ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;
                ViewBag.RuleAdmin = usuariologado.admin;
                DateTime dataHoje;
                if (!string.IsNullOrEmpty(dataF))
                {
                    dataHoje = Convert.ToDateTime(dataF);
                }
                else
                {
                    dataHoje = DateTime.Now;
                }
                if(situacao == null)
                {
                    situacao = 0;
                }

                var dt = Convert.ToDateTime(dataHoje);
                var texto = dt.ToShortDateString();
                dt = Convert.ToDateTime(texto);
                var agenda = db.Agenda.Where(x => x.DataAgendado >= dt && x.DataAgendado <= dt && x.EmpresaID == usuariologado.empresaId).OrderBy(x => x.Hora).ToList();
                if (situacao != 1000)
                {
                    agenda = agenda.Where(x => x.Situacao == situacao).ToList();   
                }                
                return View(agenda);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }        
                
        public ActionResult Create()
        {
            ViewBag.UsuarioLogado = Utils.User.GetCookieUsuarioLogado(Request, Session);
            if (!string.IsNullOrEmpty(ViewBag.UsuarioLogado))
            {
                Utils.User.UsuarioLogado usuariologado = Utils.User.GetDadosUsuarioLogado(ViewBag.UsuarioLogado);
                ViewBag.RuleAdmin = usuariologado.admin;
                ViewBag.RuleFinanceiro = usuariologado.RuleFinanceiro;
                ViewBag.RuleMovimentacao = usuariologado.RuleMovimentacao;
                ViewBag.RuleCadastro = usuariologado.RuleCadastro;
                ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;                
                return View();
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,EmpresaID,ClienteID,FuncionarioID,DataAgendamento,DataAgendado,Hora,Observacoes,Situacao,NomeCliente,NomeFuncionario")] Agenda agenda)
        {
            ViewBag.UsuarioLogado = Utils.User.GetCookieUsuarioLogado(Request, Session);
            if (!string.IsNullOrEmpty(ViewBag.UsuarioLogado))
            {
                Utils.User.UsuarioLogado usuariologado = Utils.User.GetDadosUsuarioLogado(ViewBag.UsuarioLogado);                
                ViewBag.RuleFinanceiro = usuariologado.RuleFinanceiro;
                ViewBag.RuleMovimentacao = usuariologado.RuleMovimentacao;
                ViewBag.RuleCadastro = usuariologado.RuleCadastro;
                ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;
                ViewBag.RuleAdmin = usuariologado.admin;
                if (ModelState.IsValid)
                {
                    agenda.ID = LibProdusys.GetNewCode("agenda", "id", " empresaid = "+usuariologado.empresaId.ToString());                    
                    agenda.NomeCliente = LibProdusys.FS(agenda.NomeCliente);
                    agenda.NomeFuncionario = LibProdusys.FS(agenda.NomeFuncionario);
                    agenda.Situacao = 0;
                    agenda.DataAgendamento = DateTime.Now;
                    agenda.EmpresaID = Convert.ToInt32(usuariologado.empresaId);
                    try
                    {

                        db.Agenda.Add(agenda);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    
                }
                return View(agenda);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }
        
        public ActionResult Edit(int? id)
        {
            ViewBag.UsuarioLogado = Utils.User.GetCookieUsuarioLogado(Request, Session);
            
            if (!string.IsNullOrEmpty(ViewBag.UsuarioLogado))
            {
                Utils.User.UsuarioLogado usuariologado = Utils.User.GetDadosUsuarioLogado(ViewBag.UsuarioLogado);
                
                ViewBag.RuleFinanceiro = usuariologado.RuleFinanceiro;
                ViewBag.RuleMovimentacao = usuariologado.RuleMovimentacao;
                ViewBag.RuleCadastro = usuariologado.RuleCadastro;
                ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;
                ViewBag.RuleAdmin = usuariologado.admin;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Agenda agenda = db.Agenda.Find(id, usuariologado.empresaId);
                ViewBag.Situacao = new SelectList(new Agenda.ListaSituacaoAgenda().MetodoListaAgenda().Where(x => x.ID != 1000), "ID", "Descricao", agenda.Situacao);
                
                if (agenda == null)
                {
                    return HttpNotFound();
                }                
                return View(agenda);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,EmpresaID,ClienteID,FuncionarioID,DataAgendamento,DataAgendado,Hora,Observacoes,Situacao,DataConclusao,NomeCliente,NomeFuncionario")] Agenda agenda)
        {
            ViewBag.Situacao = new SelectList(new Agenda.ListaSituacaoAgenda().MetodoListaAgenda(), "ID", "Descricao", agenda.Situacao);
            ViewBag.UsuarioLogado = Utils.User.GetCookieUsuarioLogado(Request, Session);            
            if (!string.IsNullOrEmpty(ViewBag.UsuarioLogado))
            {
                Utils.User.UsuarioLogado usuariologado = Utils.User.GetDadosUsuarioLogado(ViewBag.UsuarioLogado);                
                ViewBag.RuleFinanceiro = usuariologado.RuleFinanceiro;
                ViewBag.RuleMovimentacao = usuariologado.RuleMovimentacao;
                ViewBag.RuleCadastro = usuariologado.RuleCadastro;
                ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;
                ViewBag.RuleAdmin = usuariologado.admin;
                if (ModelState.IsValid)
                {
                    if(agenda.Situacao == 0)
                    {
                        agenda.DataConclusao = null;
                    }
                    else
                    {
                        if(agenda.DataConclusao == null)
                        {
                            agenda.DataConclusao = DateTime.Now;
                        }
                        
                    }
                    agenda.NomeCliente = LibProdusys.FS(agenda.NomeCliente);
                    agenda.NomeFuncionario = LibProdusys.FS(agenda.NomeFuncionario);
                    ViewBag.Situacao = new SelectList(new Agenda.ListaSituacaoAgenda().MetodoListaAgenda(), "ID", "Descricao", agenda.Situacao);
                    db.Entry(agenda).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(agenda);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        
        public ActionResult Delete(int? id)
        {
            ViewBag.UsuarioLogado = Utils.User.GetCookieUsuarioLogado(Request, Session);
            if (!string.IsNullOrEmpty(ViewBag.UsuarioLogado))
            {
                Utils.User.UsuarioLogado usuariologado = Utils.User.GetDadosUsuarioLogado(ViewBag.UsuarioLogado);                
                ViewBag.RuleFinanceiro = usuariologado.RuleFinanceiro;
                ViewBag.RuleMovimentacao = usuariologado.RuleMovimentacao;
                ViewBag.RuleCadastro = usuariologado.RuleCadastro;
                ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;
                ViewBag.RuleAdmin = usuariologado.admin;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Agenda agenda = db.Agenda.Find(id, usuariologado.empresaId);
                if (agenda == null)
                {
                    return HttpNotFound();
                }
                return View(agenda);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }
                
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.UsuarioLogado = Utils.User.GetCookieUsuarioLogado(Request, Session);
            if (!string.IsNullOrEmpty(ViewBag.UsuarioLogado))
            {
                Utils.User.UsuarioLogado usuariologado = Utils.User.GetDadosUsuarioLogado(ViewBag.UsuarioLogado);                
                ViewBag.RuleFinanceiro = usuariologado.RuleFinanceiro;
                ViewBag.RuleMovimentacao = usuariologado.RuleMovimentacao;
                ViewBag.RuleCadastro = usuariologado.RuleCadastro;
                ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;
                ViewBag.RuleAdmin = usuariologado.admin;
                Agenda agenda = db.Agenda.Find(id, usuariologado.empresaId);
                db.Agenda.Remove(agenda);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
