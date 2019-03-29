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
    public class FuncionariosController : Controller
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
                List<Funcionario> lista = new List<Funcionario>();
                return View(lista);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        [HttpPost]
        public ActionResult Index(string txtnome, int? checkAtivo, int? qtdeReg)
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
                var funcionarios = db.Funcionarios.Include(f => f.cidade).Include(f => f.Empresa).Where(x => x.EmpresaID == usuariologado.empresaId);
                if (!string.IsNullOrEmpty(txtnome))
                {
                    funcionarios = funcionarios.Where(x => x.Nome.Contains(txtnome));
                }
                if (checkAtivo == 1)
                {
                    funcionarios = funcionarios.Where(x => x.Situacao == 0);
                }
                if(qtdeReg == 10)
                {
                    funcionarios = funcionarios.Take(10);
                }
                if(qtdeReg == 100)
                {
                    funcionarios = funcionarios.Take(100);
                }
                
                return View(funcionarios.OrderBy(x => x.Nome).ToList());
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }
              

        // GET: Funcionarios/Create
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
                ViewBag.EmpresaID = new SelectList(db.Empresas, "ID", "Razao");
                ViewBag.Sexo = new SelectList(new Pessoa.ListaSexo().MetodoListaSexo(), "ID", "Descricao");
                ViewBag.EstadoCivil = new SelectList(new Funcionario.ListaEstadoCivil().MetodoListaEstadoCivil(), "ID", "Descricao");
                ViewBag.Situacao = new SelectList(new Pessoa.ListaSituacao().MetodoListaSituacao(), "ID", "Descricao");
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
        public ActionResult Create([Bind(Include = "ID,EmpresaID,Nome,Situacao,Naturalidade,DataNascimento,Sexo,EstadoCivil,CPF,RG,NIT,CNH,CTPS,Endereco,Numero,Complemento,Bairro,CEP,Fone1,Fone2,Email,Observacao,CidadeID")] Funcionario funcionario)
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
                    funcionario.ID = LibProdusys.GetNewCode("funcionario", "id", " empresaid = "+usuariologado.empresaId.ToString());                    
                    try
                    {
                        funcionario.DataNascimento = funcionario.DataNascimento;
                        funcionario.Bairro = LibProdusys.FS(funcionario.Bairro);
                        funcionario.CEP = LibProdusys.TrataCep(funcionario.CEP);
                        funcionario.CNH = LibProdusys.FS(funcionario.CNH);
                        funcionario.Complemento = LibProdusys.FS(funcionario.Complemento);
                        funcionario.CPF = LibProdusys.TrataCNPJ(funcionario.CPF);
                        funcionario.CTPS = LibProdusys.FS(funcionario.CTPS);
                        funcionario.Endereco = LibProdusys.FS(funcionario.Endereco);
                        funcionario.Fone1 = LibProdusys.TrataFone(funcionario.Fone1);
                        funcionario.Fone2 = LibProdusys.TrataCelular(funcionario.Fone2);
                        funcionario.Naturalidade = LibProdusys.FS(funcionario.Naturalidade);
                        funcionario.NIT = LibProdusys.FS(funcionario.NIT);
                        funcionario.Nome = LibProdusys.FS(funcionario.Nome);
                        funcionario.Numero = LibProdusys.FS(funcionario.Numero);
                        funcionario.Observacao = LibProdusys.FS(funcionario.Observacao);
                        funcionario.RG = LibProdusys.FS(funcionario.RG);
                        funcionario.EmpresaID = Convert.ToInt32(usuariologado.empresaId);
                        funcionario.DataCadastro = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                        db.Funcionarios.Add(funcionario);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                }
                ViewBag.EstadoCivil = new SelectList(new Funcionario.ListaEstadoCivil().MetodoListaEstadoCivil(), "ID", "Descricao", funcionario.EstadoCivil);
                ViewBag.Sexo = new SelectList(new Pessoa.ListaSexo().MetodoListaSexo(), "ID", "Descricao", funcionario.Sexo);
                ViewBag.CidadeID = new SelectList(db.Cidades.OrderBy(x => x.Descricao), "ID", "CidadeUF", funcionario.CidadeID);
                ViewBag.EmpresaID = new SelectList(db.Empresas, "ID", "Razao", funcionario.EmpresaID);
                ViewBag.Situacao = new SelectList(new Pessoa.ListaSituacao().MetodoListaSituacao(), "ID", "Descricao", funcionario.Situacao);
                return View(funcionario);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }

        }

        // GET: Funcionarios/Edit/5
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
                Funcionario funcionario = db.Funcionarios.Find(id, usuariologado.empresaId);
                if (funcionario == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Situacao = new SelectList(new Pessoa.ListaSituacao().MetodoListaSituacao(), "ID", "Descricao", funcionario.Situacao);
                ViewBag.EstadoCivil = new SelectList(new Funcionario.ListaEstadoCivil().MetodoListaEstadoCivil(), "ID", "Descricao", funcionario.EstadoCivil);
                ViewBag.Sexo = new SelectList(new Pessoa.ListaSexo().MetodoListaSexo(), "ID", "Descricao", funcionario.Sexo);
                ViewBag.CidadeID = new SelectList(db.Cidades.OrderBy(x => x.Descricao), "ID", "CidadeUF", funcionario.CidadeID);
                ViewBag.EmpresaID = new SelectList(db.Empresas, "ID", "Razao", funcionario.EmpresaID);
                return View(funcionario);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        // POST: Funcionarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,EmpresaID,Situacao,Nome,Naturalidade,DataNascimento,Sexo,EstadoCivil,CPF,RG,NIT,CNH,CTPS,Endereco,Numero,Complemento,Bairro,CEP,Fone1,Fone2,Email,Observacao,CidadeID")] Funcionario funcionario)
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
                    funcionario.Bairro = LibProdusys.FS(funcionario.Bairro);
                    funcionario.CEP = LibProdusys.TrataCep(funcionario.CEP);
                    funcionario.CNH = LibProdusys.FS(funcionario.CNH);
                    funcionario.Complemento = LibProdusys.FS(funcionario.Complemento);
                    funcionario.CPF = LibProdusys.TrataCNPJ(funcionario.CPF);
                    funcionario.CTPS = LibProdusys.FS(funcionario.CTPS);
                    funcionario.Endereco = LibProdusys.FS(funcionario.Endereco);
                    funcionario.Fone1 = LibProdusys.TrataFone(funcionario.Fone1);
                    funcionario.Fone2 = LibProdusys.TrataCelular(funcionario.Fone2);
                    funcionario.Naturalidade = LibProdusys.FS(funcionario.Naturalidade);
                    funcionario.NIT = LibProdusys.FS(funcionario.NIT);
                    funcionario.Nome = LibProdusys.FS(funcionario.Nome);
                    funcionario.Numero = LibProdusys.FS(funcionario.Numero);
                    funcionario.Observacao = LibProdusys.FS(funcionario.Observacao);
                    funcionario.RG = LibProdusys.FS(funcionario.RG);
                    db.Entry(funcionario).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.Situacao = new SelectList(new Pessoa.ListaSituacao().MetodoListaSituacao(), "ID", "Descricao", funcionario.Situacao);
                ViewBag.EstadoCivil = new SelectList(new Funcionario.ListaEstadoCivil().MetodoListaEstadoCivil(), "ID", "Descricao", funcionario.EstadoCivil);
                ViewBag.Sexo = new SelectList(new Pessoa.ListaSexo().MetodoListaSexo(), "ID", "Descricao", funcionario.Sexo);
                ViewBag.CidadeID = new SelectList(db.Cidades.OrderBy(x => x.Descricao), "ID", "CidadeUF", funcionario.CidadeID);
                ViewBag.EmpresaID = new SelectList(db.Empresas, "ID", "Razao", funcionario.EmpresaID);
                return View(funcionario);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        // GET: Funcionarios/Delete/5
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
                Funcionario funcionario = db.Funcionarios.Find(id, usuariologado.empresaId);
                if (funcionario == null)
                {
                    return HttpNotFound();
                }
                if (LibProdusys.PodeExcluir(Convert.ToString(id), Convert.ToString(usuariologado.empresaId), "consulta", "funcionarioid"))
                {
                    ViewBag.PodeExcluir = true;
                }
                else
                {
                    ViewBag.PodeExcluir = false;
                    ViewBag.erro = "O registo não pode ser excluído, verifique consultas.";
                }
                if (LibProdusys.PodeExcluir(Convert.ToString(id), Convert.ToString(usuariologado.empresaId), "agenda", "funcionarioid"))
                {
                    ViewBag.PodeExcluir = true;
                }
                else
                {
                    ViewBag.PodeExcluir = false;
                    ViewBag.erro = "O registo não pode ser excluído, verifique Agenda.";
                }
                return View(funcionario);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }

        }

        // POST: Funcionarios/Delete/5
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
                Funcionario funcionario = db.Funcionarios.Find(id, usuariologado.empresaId);
                db.Funcionarios.Remove(funcionario);
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
