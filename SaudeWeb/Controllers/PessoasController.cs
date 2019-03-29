using SaudeWeb.Models;
using SaudeWeb.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using static SaudeWeb.Utils.LibProdusys;

namespace SaudeWeb.Controllers
{
    public class PessoasController : Controller
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
                List<Pessoa> lista = new List<Pessoa>();
                return View(lista);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        [HttpPost]
        public ActionResult Index(string txtnome , int? checkAtivo,  int? qtdeReg)
        {
            
            ViewBag.UsuarioLogado = Utils.User.GetCookieUsuarioLogado(Request, Session);
            if (!string.IsNullOrEmpty(ViewBag.UsuarioLogado))
            {   
                if(qtdeReg == null)
                {
                    qtdeReg = 10;
                    checkAtivo = 1;
                }
                Utils.User.UsuarioLogado usuariologado = Utils.User.GetDadosUsuarioLogado(ViewBag.UsuarioLogado);
                ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;
                ViewBag.RuleAdmin = usuariologado.admin;
                ViewBag.RuleFinanceiro = usuariologado.RuleFinanceiro;
                ViewBag.RuleMovimentacao = usuariologado.RuleMovimentacao;
                ViewBag.RuleCadastro = usuariologado.RuleCadastro;
                var pessoas = db.Pessoas.Include(p => p.cidade).Include(p => p.Empresa).Where(x => x.EmpresaID == usuariologado.empresaId && x.TipoCadastro == 1);
                if (!string.IsNullOrEmpty(txtnome))
                {
                    pessoas = pessoas.Where(x => x.Razao.Contains(txtnome));
                }
                if (checkAtivo == 1)
                {
                    pessoas = pessoas.Where(x => x.Situacao == 0);
                }
                if (qtdeReg == 10)
                {
                    pessoas = pessoas.Take(10);
                }
                if (qtdeReg == 10)
                {
                    pessoas = pessoas.Take(100);
                }
                return View(pessoas.OrderBy(x => x.Razao).ToList());
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        
        public ActionResult ObterCliente()
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
                var pessoas = db.Pessoas.Include(p => p.cidade).Include(p => p.Empresa).Where(x => x.EmpresaID == usuariologado.empresaId && x.TipoCadastro == 1);

                return View(pessoas.ToList());
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        // GET: Pessoas/Create
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
                ViewBag.CidadeID = new SelectList(db.Cidades.OrderBy(x => x.Descricao), "ID", "CidadeUF");
                ViewBag.Situacao = new SelectList(new Pessoa.ListaSituacao().MetodoListaSituacao(), "ID", "Descricao");
                ViewBag.TipoPessoa = new SelectList(new Pessoa.ListaTipoOPessoa().MetodoListaTipoPessoa(), "ID", "Descricao");
                ViewBag.Sexo = new SelectList(new Pessoa.ListaSexo().MetodoListaSexo(), "ID", "Descricao");
                ViewBag.FormaPgto = new SelectList(new Pessoa.ListaFormaPgto().MetodoListaFormaPgto(), "ID", "Descricao");
                ViewBag.TabelaID = new SelectList(db.TabPreco.Where(x =>  x.EmpresaID == usuariologado.empresaId).OrderBy(x => x.Descricao), "ID", "Descricao");
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
        public ActionResult Create([Bind(Include = "ID,EmpresaID,DataCadastro,Situacao,TipoPessoa,TipoCadastro,Razao,Fantasia,CNAE,CNPJ,IE,Endereco,Numero,Complemento,Bairro,CEP,Fone1,Fone2,Fone3,Contato,Email,Observacao,CidadeID,Sexo,CRM,FormaPgto,TabelaID")] Pessoa pessoa)
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
                    pessoa.ID = GetNewCode("pessoa", "id", " empresaid = " + usuariologado.empresaId.ToString());
                    pessoa.Bairro = FS(pessoa.Bairro);
                    pessoa.CEP = TrataCep(pessoa.CEP);
                    pessoa.CNAE = FS(pessoa.CNAE);
                    pessoa.CNPJ = TrataCNPJ(pessoa.CNPJ);
                    pessoa.Complemento = FS(pessoa.Complemento);
                    pessoa.Contato = FS(pessoa.Contato);
                    pessoa.CRM = FS(pessoa.CRM);
                    pessoa.Endereco = FS(pessoa.Endereco);
                    pessoa.Fantasia = FS(pessoa.Fantasia);
                    pessoa.Fone1 = TrataFone(pessoa.Fone1);
                    pessoa.Fone2 = TrataCelular(pessoa.Fone2);
                    pessoa.Fone3 = TrataCelular(pessoa.Fone3);
                    pessoa.IE = FS(pessoa.IE);
                    pessoa.Numero = FS(pessoa.Numero);
                    pessoa.Observacao = FS(pessoa.Observacao);
                    pessoa.Razao = FS(pessoa.Razao);
                    pessoa.TipoCadastro = 1;
                    ViewBag.TabelaID = new SelectList(db.TabPreco.Where(x => x.EmpresaID == usuariologado.empresaId).OrderBy(x => x.Descricao), "ID", "Descricao", pessoa.TabelaID);                    
                    ViewBag.TipoPessoa = new SelectList(new Pessoa.ListaTipoOPessoa().MetodoListaTipoPessoa(), "ID", "Descricao", pessoa.TipoPessoa);
                    ViewBag.Situacao = new SelectList(new Pessoa.ListaSituacao().MetodoListaSituacao(), "ID", "Descricao", pessoa.Situacao);
                    ViewBag.Sexo = new SelectList(new Pessoa.ListaSexo().MetodoListaSexo(), "ID", "Descricao", pessoa.Sexo);
                    ViewBag.FormaPgto = new SelectList(new Pessoa.ListaFormaPgto().MetodoListaFormaPgto(), "ID", "Descricao", pessoa.FormaPgto);
                    pessoa.DataCadastro = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    pessoa.EmpresaID = Convert.ToInt32(usuariologado.empresaId);
                    try
                    {
                        db.Pessoas.Add(pessoa);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch (Exception e)
                    {
                        e.ToString();
                        throw;
                    }

                }
                ViewBag.TabelaID = new SelectList(db.TabPreco.Where(x => x.EmpresaID == usuariologado.empresaId).OrderBy(x => x.Descricao), "ID", "Descricao", pessoa.TabelaID);
                ViewBag.TipoPessoa = new SelectList(new Pessoa.ListaTipoOPessoa().MetodoListaTipoPessoa(), "ID", "Descricao", pessoa.TipoPessoa);
                ViewBag.Situacao = new SelectList(new Pessoa.ListaSituacao().MetodoListaSituacao(), "ID", "Descricao", pessoa.Situacao);
                ViewBag.Sexo = new SelectList(new Pessoa.ListaSexo().MetodoListaSexo(), "ID", "Descricao", pessoa.Sexo);
                ViewBag.FormaPgto = new SelectList(new Pessoa.ListaFormaPgto().MetodoListaFormaPgto(), "ID", "Descricao", pessoa.FormaPgto);
                pessoa.DataCadastro = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                ViewBag.CidadeID = new SelectList(db.Cidades.OrderBy(x => x.Descricao), "ID", "CidadeUF", pessoa.CidadeID);
                return View(pessoa);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }

        }

        // GET: Pessoas/Edit/5
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
                Pessoa pessoa = db.Pessoas.Find(id, usuariologado.empresaId);
                if (pessoa == null)
                {
                    return HttpNotFound();
                }
                ViewBag.TabelaID = new SelectList(db.TabPreco.Where(x => x.EmpresaID == usuariologado.empresaId).OrderBy(x => x.Descricao), "ID", "Descricao", pessoa.TabelaID);
                ViewBag.TipoPessoa = new SelectList(new Pessoa.ListaTipoOPessoa().MetodoListaTipoPessoa(), "ID", "Descricao", pessoa.TipoPessoa);
                ViewBag.Situacao = new SelectList(new Pessoa.ListaSituacao().MetodoListaSituacao(), "ID", "Descricao", pessoa.Situacao);
                ViewBag.Sexo = new SelectList(new Pessoa.ListaSexo().MetodoListaSexo(), "ID", "Descricao", pessoa.Sexo);
                ViewBag.FormaPgto = new SelectList(new Pessoa.ListaFormaPgto().MetodoListaFormaPgto(), "ID", "Descricao", pessoa.FormaPgto);
                ViewBag.CidadeID = new SelectList(db.Cidades.OrderBy(x => x.Descricao), "ID", "CidadeUF", pessoa.CidadeID);

                return View(pessoa);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }

        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,EmpresaID,DataCadastro,Situacao,TipoPessoa,TipoCadastro,Razao,Fantasia,CNAE,CNPJ,IE,Endereco,Numero,Complemento,Bairro,CEP,Fone1,Fone2,Fone3,Contato,Email,Observacao,CidadeID,Sexo,CRM,FormaPgto,TabelaID")] Pessoa pessoa)
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
                    pessoa.Bairro = FS(pessoa.Bairro);
                    pessoa.CEP = TrataCep(pessoa.CEP);
                    pessoa.CNAE = FS(pessoa.CNAE);
                    pessoa.CNPJ = TrataCNPJ(pessoa.CNPJ);
                    pessoa.Complemento = FS(pessoa.Complemento);
                    pessoa.Contato = FS(pessoa.Contato);
                    pessoa.CRM = FS(pessoa.CRM);
                    pessoa.Endereco = FS(pessoa.Endereco);
                    pessoa.Fantasia = FS(pessoa.Fantasia);
                    pessoa.Fone1 = TrataFone(pessoa.Fone1);
                    pessoa.Fone2 = TrataCelular(pessoa.Fone2);
                    pessoa.Fone3 = TrataCelular(pessoa.Fone3);
                    pessoa.IE = FS(pessoa.IE);
                    pessoa.Numero = FS(pessoa.Numero);
                    pessoa.Observacao = FS(pessoa.Observacao);
                    pessoa.Razao = FS(pessoa.Razao);
                    pessoa.TipoCadastro = 1;
                    ViewBag.TabelaID = new SelectList(db.TabPreco.Where(x => x.EmpresaID == usuariologado.empresaId).OrderBy(x => x.Descricao), "ID", "Descricao", pessoa.TabelaID);
                    ViewBag.TipoPessoa = new SelectList(new Pessoa.ListaTipoOPessoa().MetodoListaTipoPessoa(), "ID", "Descricao", pessoa.TipoPessoa);
                    ViewBag.Situacao = new SelectList(new Pessoa.ListaSituacao().MetodoListaSituacao(), "ID", "Descricao", pessoa.Situacao);
                    ViewBag.Sexo = new SelectList(new Pessoa.ListaSexo().MetodoListaSexo(), "ID", "Descricao", pessoa.Sexo);
                    ViewBag.FormaPgto = new SelectList(new Pessoa.ListaFormaPgto().MetodoListaFormaPgto(), "ID", "Descricao", pessoa.FormaPgto);
                    db.Entry(pessoa).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.CidadeID = new SelectList(db.Cidades.OrderBy(x => x.Descricao), "ID", "CidadeUF", pessoa.CidadeID);

                return View(pessoa);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        // GET: Pessoas/Delete/5
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
                Pessoa pessoa = db.Pessoas.Find(id, usuariologado.empresaId);
                if (pessoa == null)
                {
                    return HttpNotFound();
                }
                bool excluir = true;                
                if (LibProdusys.PodeExcluir(Convert.ToString(id), Convert.ToString(usuariologado.empresaId), "consulta", "medicoexaminadorid"))
                {
                    excluir = false;
                }
                else
                {
                    excluir = true;
                }

                if (LibProdusys.PodeExcluir(Convert.ToString(id), Convert.ToString(usuariologado.empresaId), "consulta", "medicoliberacaoid") && !excluir)
                {
                    excluir = false;
                }
                else
                {
                    excluir = true;
                }
                if (LibProdusys.PodeExcluir(Convert.ToString(id), Convert.ToString(usuariologado.empresaId), "consultaexame", "medicoid") && !excluir)
                {
                    excluir = false;
                }
                else
                {
                    excluir = true;
                }                
                if (LibProdusys.PodeExcluir(Convert.ToString(id), Convert.ToString(usuariologado.empresaId), "agenda", "clienteid") && !excluir)
                {
                    excluir = false;
                }
                else
                {
                    excluir = true;
                }
                if (LibProdusys.PodeExcluir(Convert.ToString(id), Convert.ToString(usuariologado.empresaId), "consulta", "Pessoaid") && !excluir)
                {
                    excluir = false;
                }
                else
                {
                    excluir = true;
                }
                if (excluir == false)
                {
                    ViewBag.PodeExcluir = true;
                }
                else
                {
                    ViewBag.PodeExcluir = false;
                    ViewBag.erro = "O registo não pode ser excluído, verifique Consultas ou Agenda.";
                }
                return View(pessoa);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        // POST: Pessoas/Delete/5
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
                Pessoa pessoa = db.Pessoas.Find(id, usuariologado.empresaId);
                db.Pessoas.Remove(pessoa);
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
