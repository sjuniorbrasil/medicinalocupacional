using SaudeWeb.Models;
using SaudeWeb.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using static SaudeWeb.Utils.LibProdusys;

namespace SaudeWeb.Controllers
{
    public class TabelaController : Controller
    {
        DataContext db = new DataContext();

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
                List<TabelaPreco> lista = new List<TabelaPreco>();
                return View(lista);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        [HttpPost]
        public ActionResult Index(string txttabela, int? qtdeReg)
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
                if (qtdeReg == null)
                {
                    qtdeReg = 10;
                }
                var tabela = db.TabPreco.Where(x => x.EmpresaID == usuariologado.empresaId);
                if (!string.IsNullOrEmpty(txttabela))
                {
                    tabela = tabela.Where(x => x.Descricao.Contains(txttabela));
                }
                if (qtdeReg == 10)
                {
                    tabela = tabela.Take(10);
                }
                if (qtdeReg == 100)
                {
                    tabela = tabela.Take(100);
                }
                return View(tabela.ToList());
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        public ActionResult Create(int? id)
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
                if (id != null)
                {
                    var tabela = db.TabPreco.Find(id, usuariologado.empresaId);
                    var tabExame = db.TabExame.Where(x => x.TabID == tabela.ID && x.EmpresaID == usuariologado.empresaId);
                    tabela.TabelaPrecos = tabExame.ToList();
                    ViewBag.Exame = true;
                    return View(tabela);
                }
                else
                {
                    var TabelaPrecos = new List<TabelaPreco.TabelaPrecoExame>();
                    ViewBag.Exame = false;
                    return View();
                }
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "ID,EmpresaID,Descricao")] TabelaPreco tabela)
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
                if (tabela.ID != 0)
                {
                    var tabExame = db.TabExame.Where(x => x.TabID == tabela.ID && x.EmpresaID == usuariologado.empresaId);
                    tabela.Descricao = LibProdusys.FS(tabela.Descricao);
                    tabela.TabelaPrecos = tabExame.ToList();
                    db.Entry(tabela).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.Exame = true;
                    return View(tabela);
                }
                else
                {                    
                    if (!string.IsNullOrEmpty(tabela.Descricao))
                    {
                        tabela.ID = GetNewCode("tabelapreco", "id", " empresaid = " + usuariologado.empresaId.ToString());
                        tabela.EmpresaID = Convert.ToInt32(usuariologado.empresaId);
                        tabela.Descricao = LibProdusys.FS(tabela.Descricao);
                        try
                        {
                            db.TabPreco.Add(tabela);
                            var tabExame = db.TabExame.Where(x => x.TabID == tabela.ID && x.EmpresaID == usuariologado.empresaId);
                            tabela.TabelaPrecos = tabExame.ToList();
                            db.SaveChanges();
                            ViewBag.sucesso = "Operação efetuada com sucesso !";
                            ViewBag.Exame = true;
                            return Redirect("~/Tabela/Create/" + tabela.ID);
                        }
                        catch (Exception e)
                        {
                            ViewBag.erro = "Erro ao gravar, entre em contato com o Suporte Técnico !";
                            e.ToString();
                            throw;
                        }
                    }
                    else
                    {
                        var TabelaPrecos = new List<TabelaPreco.TabelaPrecoExame>();
                        tabela.TabelaPrecos = TabelaPrecos;
                        ViewBag.Exame = false;
                        return View(tabela);
                    }
                }

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
                var tabela = db.TabPreco.Where(x => x.ID == id && x.EmpresaID == usuariologado.empresaId).FirstOrDefault();
                if (tabela == null)
                {
                    return HttpNotFound();
                }
                if (LibProdusys.PodeExcluir(Convert.ToString(id), Convert.ToString(usuariologado.empresaId), "Pessoa", "TabelaID"))
                {
                    ViewBag.PodeExcluir = true;
                }
                else
                {
                    ViewBag.PodeExcluir = false;
                    ViewBag.erro = "O registo não pode ser excluído, verifique clientes.";
                }
                return View(tabela);
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
                var tabela = db.TabPreco.Where(x => x.ID == id && x.EmpresaID == usuariologado.empresaId).FirstOrDefault();
                var tabelaExame = db.TabExame.Where(x => x.ExameID == id && x.EmpresaID == usuariologado.empresaId);
                db.TabPreco.Remove(tabela);
                db.TabExame.RemoveRange(tabelaExame);
                db.SaveChanges();
                return RedirectToAction("index");
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }

        }

        public ActionResult addExame(string TabelaExameDetal, string ExameIdDetal, string ExameValorDetal, string ExameValorRepasseDetal)
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
                var tabId = Convert.ToInt32(TabelaExameDetal);
                var exameid = Convert.ToInt32(ExameIdDetal);
                var examevalor = Convert.ToDecimal(ExameValorDetal);
                if(string.IsNullOrEmpty(ExameValorRepasseDetal))
                {
                    ExameValorRepasseDetal = "0";
                }
                var exameRepasse = Convert.ToDecimal(ExameValorRepasseDetal);
                var tabela = db.TabPreco.Find(tabId, usuariologado.empresaId);
                var temExame = db.TabExame.Where(x => x.EmpresaID == usuariologado.empresaId && x.ExameID == exameid && x.TabID == tabId).FirstOrDefault();
                if (temExame == null)
                {
                    TabelaPreco.TabelaPrecoExame exame = new TabelaPreco.TabelaPrecoExame();
                    exame.ExameID = exameid;
                    exame.TabID = tabId;
                    exame.ValorExame = examevalor;
                    exame.EmpresaID = Convert.ToInt32(usuariologado.empresaId);
                    exame.RepasseMedicoValor = exameRepasse;
                    try
                    {
                        db.TabExame.Add(exame);
                        db.SaveChanges();
                        ViewBag.Exame = true;
                    }
                    catch (Exception)
                    {
                        ViewBag.erro = "Erro ao Inlcuir Exame.";
                        throw;
                    }
                }
                else
                {
                    temExame.ExameID = exameid;
                    temExame.TabID = tabId;
                    temExame.ValorExame = examevalor;
                    temExame.EmpresaID = Convert.ToInt32(usuariologado.empresaId);
                    temExame.RepasseMedicoValor = exameRepasse;
                    try
                    {
                        db.Entry(temExame).State = EntityState.Modified;
                        db.SaveChanges();
                        ViewBag.Exame = true;
                    }
                    catch (Exception)
                    {
                        
                        ViewBag.erro = "Erro ao Inlcuir Exame.";
                        throw;
                    }
                }

                return RedirectToAction("/Create/" + tabela.ID);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        public ActionResult DelExame(string ExameID, string Id)
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
                var tabId = Convert.ToInt32(Id);
                var exameId = Convert.ToInt32(ExameID);
                var exame = db.TabExame.Where(x => x.EmpresaID == usuariologado.empresaId && x.TabID == tabId && x.ExameID == exameId).FirstOrDefault();
                db.TabExame.Remove(exame);
                db.SaveChanges();
                return RedirectToAction("/Create/" + tabId);
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