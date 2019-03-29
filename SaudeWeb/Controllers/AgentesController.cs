using SaudeWeb.Models;
using SaudeWeb.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SaudeWeb.Controllers
{
    public class AgentesController : Controller
    {
        private DataContext db = new DataContext();

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
                List<Agente> lista = new List<Agente>();
                return View(lista);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        [HttpPost]
        public ActionResult Index(string txtagente, int? qtdeReg)
        {
            ViewBag.UsuarioLogado = Utils.User.GetCookieUsuarioLogado(Request, Session);
            if (!string.IsNullOrEmpty(ViewBag.UsuarioLogado))
            {
                if(qtdeReg == null)
                {
                    qtdeReg = 10;
                }
                Utils.User.UsuarioLogado usuariologado = Utils.User.GetDadosUsuarioLogado(ViewBag.UsuarioLogado);
                ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;
                ViewBag.RuleAdmin = usuariologado.admin;
                ViewBag.RuleFinanceiro = usuariologado.RuleFinanceiro;
                ViewBag.RuleMovimentacao = usuariologado.RuleMovimentacao;
                ViewBag.RuleCadastro = usuariologado.RuleCadastro;
                var agentes = db.Agentes.Where(x => x.EmpresaID == usuariologado.empresaId);
                if (!string.IsNullOrEmpty(txtagente))
                {
                    agentes = agentes.Where(x => x.Descricao.Contains(txtagente));
                }
                if (qtdeReg == 10)
                {
                    agentes = agentes.Take(10);
                }
                if (qtdeReg == 100)
                {
                    agentes = agentes.Take(100);
                }
                return View(agentes.OrderBy(x => x.Descricao).ToList());
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
                ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;
                ViewBag.RiscoId = new SelectList(db.Riscos.OrderBy(x => x.Descricao), "ID", "Descricao");
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
        public ActionResult Create([Bind(Include = "Id,EmpresaID,Descricao,RiscoId,DanosSaude,RecomendacaoMedica")] Agente agente)
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
                if (ModelState.IsValid)
                {
                    var usuario = "";
                    if (usuario != null)
                    {
                        
                        agente.Id = LibProdusys.GetNewCode("agente", "id", " empresaid = "+usuariologado.empresaId.ToString());                        
                        agente.RecomendacaoMedica = LibProdusys.FS(agente.RecomendacaoMedica);
                        agente.Descricao = LibProdusys.FS(agente.Descricao);
                        agente.DanosSaude =  LibProdusys.FS(agente.DanosSaude);
                        agente.EmpresaID = Convert.ToInt32(usuariologado.empresaId);
                        try
                        {
                            db.Agentes.Add(agente);
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        catch (Exception e)
                        {
                            e.ToString();
                            throw;
                        }
                    }
                }
                ViewBag.RiscoId = new SelectList(db.Riscos.OrderBy(x => x.Descricao), "ID", "Descricao", agente.RiscoId);
                ViewBag.EmpresaID = new SelectList(db.Empresas, "ID", "Razao", agente.EmpresaID);
                return View(agente);
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
                ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;
                ViewBag.RuleAdmin = usuariologado.admin;
                ViewBag.RuleFinanceiro = usuariologado.RuleFinanceiro;
                ViewBag.RuleMovimentacao = usuariologado.RuleMovimentacao;
                ViewBag.RuleCadastro = usuariologado.RuleCadastro;

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Agente agente = db.Agentes.Find(id, usuariologado.empresaId);
                if (agente == null)
                {
                    return HttpNotFound();
                }
               
                ViewBag.RiscoId = new SelectList(db.Riscos.OrderBy(x => x.Descricao), "ID", "Descricao", agente.RiscoId);
                ViewBag.EmpresaID = new SelectList(db.Empresas, "ID", "Razao", agente.EmpresaID);
                return View(agente);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EmpresaID,Descricao,RiscoId,DanosSaude,RecomendacaoMedica")] Agente agente)
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
                if (ModelState.IsValid)
                {
                    agente.RecomendacaoMedica = LibProdusys.FS(agente.RecomendacaoMedica);
                    agente.Descricao = LibProdusys.FS(agente.Descricao);
                    agente.DanosSaude = LibProdusys.FS(agente.DanosSaude);
                    db.Entry(agente).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.RiscoId = new SelectList(db.Riscos.OrderBy(x => x.Descricao), "ID", "Descricao", agente.RiscoId);
                ViewBag.EmpresaID = new SelectList(db.Empresas, "ID", "Razao", agente.EmpresaID);
                return View(agente);
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
                ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;
                ViewBag.RuleAdmin = usuariologado.admin;
                ViewBag.RuleFinanceiro = usuariologado.RuleFinanceiro;
                ViewBag.RuleMovimentacao = usuariologado.RuleMovimentacao;
                ViewBag.RuleCadastro = usuariologado.RuleCadastro;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Agente agente = db.Agentes.Find(id, usuariologado.empresaId);
                if (agente == null)
                {
                    return HttpNotFound();
                }
                if (LibProdusys.PodeExcluir(Convert.ToString(id), Convert.ToString(usuariologado.empresaId), "consultaagente", "agenteid"))
                {
                    ViewBag.PodeExcluir = true;
                }
                else
                {
                    ViewBag.PodeExcluir = false;
                    ViewBag.erro = "O registo não pode ser excluído, verifique consultas.";
                }
                return View(agente);
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
                ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;
                ViewBag.RuleAdmin = usuariologado.admin;
                ViewBag.RuleFinanceiro = usuariologado.RuleFinanceiro;
                ViewBag.RuleMovimentacao = usuariologado.RuleMovimentacao;
                ViewBag.RuleCadastro = usuariologado.RuleCadastro;
                Agente agente = db.Agentes.Find(id, usuariologado.empresaId);
                db.Agentes.Remove(agente);
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
