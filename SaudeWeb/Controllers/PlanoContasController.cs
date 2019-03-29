using SaudeWeb.Models;
using SaudeWeb.Utils;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using static SaudeWeb.Utils.LibProdusys;

namespace SaudeWeb.Controllers
{
    public class PlanoContasController : Controller
    {
        private DataContext db = new DataContext();

        // GET: PlanoContas
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
                var plconta = db.PlanoContas.Where(x => x.EmpresaID == usuariologado.empresaId).OrderBy(x => x.Conta).ThenBy(x => x.Descricao).ToList();
                return View(plconta);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        public ActionResult EditSubCategoria(int? id)
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
                var plconta = db.PlanoContas.Where(x => x.ID == id && x.EmpresaID == usuariologado.empresaId).FirstOrDefault();
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }                
                if (plconta == null)
                {
                    return HttpNotFound();
                }
                return View(plconta);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        [HttpPost]
        public ActionResult EditSubCategoria([Bind(Include = "ID,EmpresaID,Descricao,Operacao,Nivel,NivelSuperior,Conta")] PlanoConta planoConta)
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
                    planoConta.Descricao = LibProdusys.FS(planoConta.Descricao);
                    db.Entry(planoConta).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(planoConta);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }


        public ActionResult NewSubCategoria(int? id)
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
                var plconta = db.PlanoContas.Where(x => x.ID == id && x.EmpresaID == usuariologado.empresaId).FirstOrDefault();
                plconta.Conta = "";
                var texto = plconta.Descricao;
                plconta.Descricao = "";
                plconta.NivelSuperior = plconta.ID;
                ViewBag.NivelSuperiordesc = Convert.ToString(plconta.Conta) + " - " + texto;
                return View(plconta);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }



        [HttpPost]
        public ActionResult NewSubCategoria([Bind(Include = "ID,EmpresaID,Descricao,Operacao,Nivel,NivelSuperior,Conta")] PlanoConta planoConta)
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
                var plconta = db.PlanoContas.Where(x => x.ID == planoConta.NivelSuperior && x.EmpresaID == usuariologado.empresaId).FirstOrDefault();
                planoConta.Operacao = plconta.Operacao;
                planoConta.EmpresaID = Convert.ToInt32(usuariologado.empresaId);
                planoConta.Nivel = 1;
                planoConta.Conta = plconta.Conta + planoConta.Conta;
                planoConta.Descricao = LibProdusys.FS(planoConta.Descricao);
                planoConta.ID = GetNewCode("planoconta", "id", " empresaid = "+usuariologado.empresaId.ToString());
                
                db.PlanoContas.Add(planoConta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        // GET: PlanoContas/Create
        public ActionResult Create()
        {
            ViewBag.UsuarioLogado = Utils.User.GetCookieUsuarioLogado(Request, Session);
            if (!string.IsNullOrEmpty(ViewBag.UsuarioLogado))
            {
                Utils.User.UsuarioLogado usuariologado = Utils.User.GetDadosUsuarioLogado(ViewBag.UsuarioLogado);
                ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;
                ViewBag.Operacao = new SelectList(new PlanoConta.ListaSOperacao().MetodoListaOperacao(), "ID", "Descricao");
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
        public ActionResult Create([Bind(Include = "ID,EmpresaID,Descricao,Operacao,Nivel,NivelSuperior,Conta")] PlanoConta planoConta)
        {
            ViewBag.Operacao = new SelectList(new PlanoConta.ListaSOperacao().MetodoListaOperacao(), "ID", "Descricao");
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
                    planoConta.ID = GetNewCode("planoconta", "id", " empresaid = " + usuariologado.empresaId.ToString());

                    planoConta.Conta = planoConta.Conta + ".";
                    planoConta.Descricao = LibProdusys.FS(planoConta.Descricao);
                    planoConta.EmpresaID = Convert.ToInt32(usuariologado.empresaId);
                    db.PlanoContas.Add(planoConta);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(planoConta);
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
                PlanoConta planoConta = db.PlanoContas.Find(id, usuariologado.empresaId);
                if (planoConta == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Operacao = new SelectList(new PlanoConta.ListaSOperacao().MetodoListaOperacao(), "ID", "Descricao", planoConta.Operacao);
                return View(planoConta);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,EmpresaID,Descricao,Operacao,Nivel,NivelSuperior,Conta")] PlanoConta planoConta)
        {
            ViewBag.Operacao = new SelectList(new PlanoConta.ListaSOperacao().MetodoListaOperacao(), "ID", "Descricao", planoConta.Operacao);
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
                    planoConta.Descricao = LibProdusys.FS(planoConta.Descricao);
                    db.Entry(planoConta).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(planoConta);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        // GET: PlanoContas/Delete/5
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
                PlanoConta planoConta = db.PlanoContas.Find(id, usuariologado.empresaId);
                if (planoConta == null)
                {
                    return HttpNotFound();
                }

                bool excluir = true;
                if (planoConta.NivelSuperior == null)
                {
                    var plcontaPai = db.PlanoContas.Count(x => x.NivelSuperior == planoConta.NivelSuperior && x.EmpresaID == usuariologado.empresaId);
                    if (plcontaPai > 0)
                    {
                        excluir = false;
                    }
                    else
                    {
                        excluir = true;
                    }
                }
                if (LibProdusys.PodeExcluir(Convert.ToString(id), Convert.ToString(usuariologado.empresaId), "financeiro", "categoriaid") && excluir )
                {
                    ViewBag.PodeExcluir = true;
                }
                else
                {
                    ViewBag.PodeExcluir = false;
                    if (planoConta.NivelSuperior == null)
                    {
                        ViewBag.erro = "O registo não pode ser excluído, verifique Subcategorias.";
                    }
                    else
                    {
                        ViewBag.erro = "O registo não pode ser excluído, verifique Financeiro.";
                    }
                }
                return View(planoConta);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        // POST: PlanoContas/Delete/5
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
                PlanoConta planoConta = db.PlanoContas.Find(id, usuariologado.empresaId);
                db.PlanoContas.Remove(planoConta);
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
