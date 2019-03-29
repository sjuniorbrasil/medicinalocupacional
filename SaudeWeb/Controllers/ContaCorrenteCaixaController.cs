using SaudeWeb.Models;
using SaudeWeb.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SaudeWeb.Controllers
{
    public class ContaCorrenteCaixaController : Controller
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
                List<ContaCorrenteCaixa> lista = new List<ContaCorrenteCaixa>(); 
                return View(lista);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }


        [HttpPost]
        public ActionResult Index(string txtConta, string txtDescricao)
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
                var conta = db.ContaCorrenteCaixas.Where(x => x.EmpresaID == usuariologado.empresaId);
                if (!string.IsNullOrEmpty(txtConta))
                {
                    conta = conta.Where(x => x.NumeroConta.Contains(txtConta));
                }
                if (!string.IsNullOrEmpty(txtDescricao))
                {
                    conta = conta.Where(x => x.Descricao.Contains(txtDescricao));
                }                
                return View(conta.ToList());
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
                ViewBag.RuleAdmin = usuariologado.admin;
                ViewBag.RuleFinanceiro = usuariologado.RuleFinanceiro;
                ViewBag.RuleMovimentacao = usuariologado.RuleMovimentacao;
                ViewBag.RuleCadastro = usuariologado.RuleCadastro;
                ViewBag.Banco = new SelectList(new ContaCorrenteCaixa.ListaBanco().MetodoListaBanco(), "ID", "Descricao");
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
        public ActionResult Create([Bind(Include = "ID,EmpresaID,Descricao,Banco,Agencia,NumeroConta,ContaPadrao")] ContaCorrenteCaixa contaCorrenteCaixa)
        {
            ViewBag.Banco = new SelectList(new ContaCorrenteCaixa.ListaBanco().MetodoListaBanco(), "ID", "Descricao", contaCorrenteCaixa.Banco);
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
                    contaCorrenteCaixa.ID = LibProdusys.GetNewCode("ContaCorrenteCaixa", "Id"," empresaid = "+usuariologado.empresaId.ToString());                   
                    if(contaCorrenteCaixa.ContaPadrao == 1)
                    {
                        string update = "Update ContaCorrenteCaixa set ContaPadrao = 0 where empresaid = " + Convert.ToString(usuariologado.empresaId);
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Properties.Settings.Default.Banco;
                        SqlCommand cmd = new SqlCommand(update,con);
                        cmd.CommandType = CommandType.Text;
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception)
                        {
                            ViewBag.erro = "Não Foi Possível processar a operação, tente novamente";
                            return View(contaCorrenteCaixa);
                            throw;
                        }
                    }
                    contaCorrenteCaixa.EmpresaID = Convert.ToInt32(usuariologado.empresaId);
                    contaCorrenteCaixa.Descricao = LibProdusys.FS(contaCorrenteCaixa.Descricao);
                    db.ContaCorrenteCaixas.Add(contaCorrenteCaixa);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(contaCorrenteCaixa);
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
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ContaCorrenteCaixa contaCorrenteCaixa = db.ContaCorrenteCaixas.Find(id, usuariologado.empresaId);
                if(contaCorrenteCaixa.Banco != null)
                {
                    ViewBag.Banco = new SelectList(new ContaCorrenteCaixa.ListaBanco().MetodoListaBanco(), "ID", "Descricao", contaCorrenteCaixa.Banco);
                }
                else
                {
                    ViewBag.Banco = new SelectList(new ContaCorrenteCaixa.ListaBanco().MetodoListaBanco(), "ID", "Descricao",-1);
                }
                
                if (contaCorrenteCaixa == null)
                {
                    return HttpNotFound();
                }
                return View(contaCorrenteCaixa);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }
                
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,EmpresaID,Descricao,Banco,Agencia,NumeroConta,ContaPadrao")] ContaCorrenteCaixa contaCorrenteCaixa)
        {
            ViewBag.Banco = new SelectList(new ContaCorrenteCaixa.ListaBanco().MetodoListaBanco(), "ID", "Descricao", contaCorrenteCaixa.Banco);
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
                    if (contaCorrenteCaixa.ContaPadrao == 1)
                    {
                        string update = "Update ContaCorrenteCaixa set ContaPadrao = 0 where empresaid = " + Convert.ToString(usuariologado.empresaId);
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Properties.Settings.Default.Banco;
                        SqlCommand cmd = new SqlCommand(update, con);
                        cmd.CommandType = CommandType.Text;
                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {

                            ViewBag.erro = "Não Foi Possível processar a operação, tente novamente"+e.ToString();
                            return View(contaCorrenteCaixa);
                            throw;
                        }
                    }
                    contaCorrenteCaixa.EmpresaID = Convert.ToInt32(usuariologado.empresaId);
                    contaCorrenteCaixa.Descricao = LibProdusys.FS(contaCorrenteCaixa.Descricao);
                    db.Entry(contaCorrenteCaixa).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(contaCorrenteCaixa);
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
                if (id != 1)
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
                    ContaCorrenteCaixa contaCorrenteCaixa = db.ContaCorrenteCaixas.Find(id, usuariologado.empresaId);
                    if (contaCorrenteCaixa == null)
                    {
                        return HttpNotFound();
                    }
                    return View(contaCorrenteCaixa);
                }
                else
                {
                    TempData["MensagemRetorno"] = "Conta Caixa Não pode ser Excluída.";
                    return RedirectToAction("Index");
                }
                
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
                ContaCorrenteCaixa contaCorrenteCaixa = db.ContaCorrenteCaixas.Find(id, usuariologado.empresaId);
                db.ContaCorrenteCaixas.Remove(contaCorrenteCaixa);
                db.SaveChanges();
                var conta = db.ContaCorrenteCaixas.Where(x => x.ID == 1 && x.EmpresaID == usuariologado.empresaId).FirstOrDefault();
                conta.ContaPadrao = 1;
                db.Entry(conta).State = EntityState.Modified;
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
