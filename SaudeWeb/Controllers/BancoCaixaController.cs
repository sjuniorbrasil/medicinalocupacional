using SaudeWeb.Models;
using SaudeWeb.Utils;
using System;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SaudeWeb.Controllers
{
    public class BancoCaixaController : Controller
    {
        private DataContext db = new DataContext();
                
        [HttpPost]
        public ActionResult Index(int? contaID, DateTime? data1, DateTime? data2, int? opcao, int? situacao)
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
                ViewBag.opcao = new SelectList(new BancoCaixa.ListaOpcao().MetodoListaOpcao(), "ID", "Descricao");
                ViewBag.situacao = new SelectList(new BancoCaixa.ListaSituacao().MetodoListaSituacao(), "ID", "Descricao");
                ViewBag.contaID = new SelectList(db.ContaCorrenteCaixas.Where(x => x.EmpresaID == usuariologado.empresaId), "ID", "Descricao"); ;
                var movimento = db.BancoCaixa.Where(x => x.EmpresaID == usuariologado.empresaId);
                decimal saldoAtual = 0;
                if (movimento.Count() > 0)
                {
                    ViewBag.Saldo = movimento.Sum(x => x.DebCred);
                    saldoAtual = ViewBag.Saldo;
                    ViewBag.SaldoF = movimento.Sum(x => x.DebCred).ToString("C", CultureInfo.CurrentCulture);
                }                
                if (contaID != null)
                {
                    movimento = movimento.Where(x => x.ContaID == contaID);
                    if (movimento.Count() > 0)
                    {
                        ViewBag.Saldo = movimento.Sum(x => x.DebCred);
                        saldoAtual = ViewBag.Saldo;
                        ViewBag.SaldoF = movimento.Sum(x => x.DebCred).ToString("C", CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        ViewBag.SaldoF = 0.ToString("C", CultureInfo.CurrentCulture);
                    }
                }
                if (situacao != 1000)
                {
                    if (situacao == 1)
                    {
                        movimento = movimento.Where(x => x.DataConciliacao == null);
                    }
                    else
                    {
                        movimento = movimento.Where(x => x.DataConciliacao != null);
                    }
                }
                if (opcao == 1)
                {
                    if (data1 != null)
                    {
                        movimento = movimento.Where(x => x.DataEmissao >= data1);
                    }
                    if (data2 != null)
                    {
                        movimento = movimento.Where(x => x.DataEmissao <= data2);
                    }
                }
                else if (opcao == 2)
                {
                    if (data1 != null)
                    {
                        movimento = movimento.Where(x => x.DataVencimento >= data1);
                    }
                    if (data2 != null)
                    {
                        movimento = movimento.Where(x => x.DataVencimento <= data2);
                    }
                }
                else 
                {
                    if (data1 != null)
                    {
                        movimento = movimento.Where(x => x.DataConciliacao >= data1);
                    }
                    if (data2 != null)
                    {
                        movimento = movimento.Where(x => x.DataConciliacao <= data2);
                    }
                }
                decimal SaldoCredito = 0;
                decimal SaldoDebito = 0;
                decimal SaldoCreditoDebito = 0;
                foreach (var item in movimento)
                {
                    if(item.Tipo == 1)
                    {
                        SaldoCredito = SaldoCredito + item.DebCred;
                    }
                    else
                    {
                        SaldoDebito = SaldoDebito + item.DebCred;
                    }

                }
                SaldoCreditoDebito = SaldoCredito - System.Math.Abs(SaldoDebito);
                ViewBag.SaldoCredito = SaldoCredito.ToString("C", CultureInfo.CurrentCulture);
                ViewBag.SaldoDebito = SaldoDebito.ToString("C", CultureInfo.CurrentCulture);
                ViewBag.SaldoCreditoDebito = SaldoCreditoDebito.ToString("C", CultureInfo.CurrentCulture);

                return View(movimento.ToList());
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }
        
        public ActionResult Index( DateTime? data1, DateTime? data2, int? opcao)
        {
            ViewBag.UsuarioLogado = Utils.User.GetCookieUsuarioLogado(Request, Session);
            data1 = DateTime.Today.AddDays(-1) ;
            data2 = DateTime.Today;
            opcao = 1;
            if (!string.IsNullOrEmpty(ViewBag.UsuarioLogado))
            {
                Utils.User.UsuarioLogado usuariologado = Utils.User.GetDadosUsuarioLogado(ViewBag.UsuarioLogado);
                ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;
                ViewBag.RuleAdmin = usuariologado.admin;
                ViewBag.RuleFinanceiro = usuariologado.RuleFinanceiro;
                ViewBag.RuleMovimentacao = usuariologado.RuleMovimentacao;
                ViewBag.RuleCadastro = usuariologado.RuleCadastro;
                ViewBag.opcao = new SelectList(new BancoCaixa.ListaOpcao().MetodoListaOpcao(), "ID", "Descricao");
                ViewBag.situacao = new SelectList(new BancoCaixa.ListaSituacao().MetodoListaSituacao(), "ID", "Descricao");
                ViewBag.contaID = new SelectList(db.ContaCorrenteCaixas.Where(x => x.EmpresaID == usuariologado.empresaId), "ID", "Descricao"); ;
                var movimento = db.BancoCaixa.Where(x => x.EmpresaID == usuariologado.empresaId);
                decimal saldoAtual = 0;
                if (movimento.Count() > 0)
                {
                    ViewBag.Saldo = movimento.Sum(x => x.DebCred);
                    saldoAtual = ViewBag.Saldo;
                    ViewBag.SaldoF = movimento.Sum(x => x.DebCred).ToString("C", CultureInfo.CurrentCulture);
                }
                if (opcao == 1)
                {
                    if (data1 != null)
                    {
                        movimento = movimento.Where(x => x.DataEmissao >= data1);
                    }
                    if (data2 != null)
                    {
                        movimento = movimento.Where(x => x.DataEmissao <= data2);
                    }
                }
                else if (opcao == 2)
                {
                    if (data1 != null)
                    {
                        movimento = movimento.Where(x => x.DataVencimento >= data1);
                    }
                    if (data2 != null)
                    {
                        movimento = movimento.Where(x => x.DataVencimento <= data2);
                    }
                }
                else
                {
                    if (data1 != null)
                    {
                        movimento = movimento.Where(x => x.DataConciliacao >= data1);
                    }
                    if (data2 != null)
                    {
                        movimento = movimento.Where(x => x.DataConciliacao <= data2);
                    }
                }
                decimal SaldoCredito = 0;
                decimal SaldoDebito = 0;
                decimal SaldoCreditoDebito = 0;
                foreach (var item in movimento)
                {
                    if (item.Tipo == 1)
                    {
                        SaldoCredito = SaldoCredito + item.DebCred;
                    }
                    else
                    {
                        SaldoDebito = SaldoDebito + item.DebCred;
                    }

                }
                SaldoCreditoDebito = SaldoCredito - System.Math.Abs(SaldoDebito);
                ViewBag.SaldoCredito = SaldoCredito.ToString("C", CultureInfo.CurrentCulture);
                ViewBag.SaldoDebito = SaldoDebito.ToString("C", CultureInfo.CurrentCulture);
                ViewBag.SaldoCreditoDebito = SaldoCreditoDebito.ToString("C", CultureInfo.CurrentCulture);

                return View(movimento.ToList());
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
                ViewBag.ContaID = new SelectList(db.ContaCorrenteCaixas.Where(x => x.EmpresaID == usuariologado.empresaId), "ID", "Descricao");
                ViewBag.TpDocto = new SelectList(new BancoCaixa.ListaTipoDocumento().MetodoListaOpcao(), "ID", "Descricao");
                ViewBag.Tipo = new SelectList(new BancoCaixa.ListaTipo().MetodoListaOpcao(), "ID", "Descricao");

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
        public ActionResult Create([Bind(Include = "ID,BaixaID,EmpresaID,Tipo,ContaID,TpDocto,PessoaId,DataEmissao,DataVencimento,DataConciliacao,NumeroDocumento,Banco,Agencia,Obs,valor,Cheque,Transferencia,Emitente,PlanoContaID")] BancoCaixa bancoCaixa)
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
                ViewBag.ContaID = new SelectList(db.ContaCorrenteCaixas.Where(x => x.EmpresaID == usuariologado.empresaId), "ID", "Descricao", bancoCaixa.ContaID);
                ViewBag.TpDocto = new SelectList(new BancoCaixa.ListaTipoDocumento().MetodoListaOpcao(), "ID", "Descricao", bancoCaixa.TpDocto);
                ViewBag.Tipo = new SelectList(new BancoCaixa.ListaTipo().MetodoListaOpcao() , "ID", "Descricao", bancoCaixa.Tipo);
                if (ModelState.IsValid)
                {
                    var observacao = "";
                    if (string.IsNullOrEmpty(bancoCaixa.Obs))
                    {
                        
                        var planoConta = Convert.ToInt32(bancoCaixa.PlanoContaID);
                        var plconta = db.PlanoContas.Where(x => x.ID == planoConta && x.EmpresaID == usuariologado.empresaId).FirstOrDefault();
                        if (plconta != null)
                        {
                            observacao = plconta.Conta + " - " + plconta.Descricao;
                        }
                    }
                    else
                    {
                        observacao = bancoCaixa.Obs;
                    }
                    //finaneiro
                    Financeiro financeiro = new Financeiro();
                    var contador2 = db.RecPag.Count(x => x.EmpresaID == usuariologado.empresaId);                    
                    financeiro.ID = LibProdusys.GetNewCode("financeiro", "id", " empresaid = "+usuariologado.empresaId.ToString());                    
                    financeiro.ParcelaID = 1;
                    financeiro.Competencia = Convert.ToDateTime(bancoCaixa.DataEmissao).ToString("yyyy-MM");
                    financeiro.PessoaID = Convert.ToInt32(bancoCaixa.PessoaId);
                    financeiro.Tipo = bancoCaixa.Tipo;
                    financeiro.Valor = bancoCaixa.valor;
                    financeiro.ValorBaixado = bancoCaixa.valor;
                    financeiro.CategoriaID = Convert.ToInt32(bancoCaixa.PlanoContaID);
                    financeiro.DataEmissao = Convert.ToDateTime(bancoCaixa.DataEmissao);
                    financeiro.DataLiquidacao = bancoCaixa.DataEmissao;
                    financeiro.DataVencimento = Convert.ToDateTime(bancoCaixa.DataVencimento);
                    financeiro.EmpresaID = Convert.ToInt32(usuariologado.empresaId);
                    financeiro.NumeroDocumento = bancoCaixa.NumeroDocumento;
                    financeiro.Observacao = LibProdusys.FS(observacao);
                    db.RecPag.Add(financeiro);
                    db.SaveChanges();

                    //baixa
                    Baixa baixa = new Baixa();
                    var contador = db.Baixas.Count(x => x.EmpresaID == usuariologado.empresaId);
                    baixa.ID = LibProdusys.GetNewCode("baixa", "id", " empresaid = " + usuariologado.empresaId.ToString());

                    baixa.FinanceiroID = financeiro.ID;
                    baixa.ParcelaID = financeiro.ParcelaID;
                    baixa.Valor = financeiro.Valor;
                    baixa.Juros = financeiro.Juros;
                    baixa.Multa = financeiro.Multa;
                    baixa.Desconto = financeiro.Desconto;
                    baixa.JMD = baixa.Juros + baixa.Multa - baixa.Desconto;
                    baixa.Total = baixa.Valor + baixa.JMD;

                    if (financeiro.DataLiquidacao == null)
                    {
                        baixa.DataBaixa = DateTime.Now;
                    }
                    else
                    {
                        baixa.DataBaixa = financeiro.DataLiquidacao;
                    }

                    baixa.Obs = LibProdusys.FS(observacao);
                    baixa.EmpresaID = Convert.ToInt32(usuariologado.empresaId);
                    db.Baixas.Add(baixa);
                    db.SaveChanges();

                    var alteraRecpag = db.RecPag.Where(x => x.EmpresaID == usuariologado.empresaId && x.ID == financeiro.ID && x.ParcelaID == financeiro.ParcelaID).FirstOrDefault();
                    alteraRecpag.ValorBaixado = Convert.ToDecimal(baixa.Total);
                    db.Entry(alteraRecpag).State = EntityState.Modified;
                    db.SaveChanges();

                    //controle conta corrente/caixa
                    var conta = db.ContaCorrenteCaixas.Where(x => x.EmpresaID == usuariologado.empresaId && x.ID == bancoCaixa.ContaID).FirstOrDefault();

                    bancoCaixa.ID = LibProdusys.GetNewCode("bancocaixa", "id", " empresaid = " + usuariologado.empresaId.ToString());                    
                    bancoCaixa.EmpresaID = Convert.ToInt32(usuariologado.empresaId);

                    bancoCaixa.Obs = LibProdusys.FS(observacao);
                    bancoCaixa.Agencia = conta.Agencia;
                    bancoCaixa.Banco = Convert.ToString(conta.Banco);
                    bancoCaixa.ContaID = conta.ID;                    
                    bancoCaixa.BaixaID = baixa.ID;
                    bancoCaixa.Conta = conta.NumeroConta;
                    db.BancoCaixa.Add(bancoCaixa);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(bancoCaixa);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }
                
        public ActionResult Edit(int? id, int? baixaid)
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
                BancoCaixa bancoCaixa = db.BancoCaixa.Find(id, baixaid, usuariologado.empresaId);
                if (bancoCaixa == null)
                {
                    return HttpNotFound();
                }
                ViewBag.ContaID = new SelectList(db.ContaCorrenteCaixas.Where(x => x.EmpresaID == usuariologado.empresaId), "ID", "Descricao", bancoCaixa.ContaID);
                ViewBag.TpDocto = new SelectList(new BancoCaixa.ListaTipoDocumento().MetodoListaOpcao(), "ID", "Descricao", bancoCaixa.TpDocto);
                ViewBag.Tipo = new SelectList(new BancoCaixa.ListaTipo().MetodoListaOpcao(), "ID", "Descricao", bancoCaixa.Tipo);
                return View(bancoCaixa);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,BaixaID,EmpresaID,Tipo,ContaID,TpDocto,PessoaId,DataEmissao,DataVencimento,DataConciliacao,NumeroDocumento,Banco,Agencia,Obs,valor,Cheque,Transferencia,Emitente,PlanoContaID")] BancoCaixa bancoCaixa)
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
                    var bcx = db.BancoCaixa.Where(x => x.ID == bancoCaixa.ID && x.EmpresaID == usuariologado.empresaId && x.BaixaID == bancoCaixa.BaixaID).FirstOrDefault();
                    bcx.Obs = bancoCaixa.Obs;
                    bcx.DataConciliacao = bancoCaixa.DataConciliacao;
                    db.Entry(bcx).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.ContaID = new SelectList(db.ContaCorrenteCaixas.Where(x => x.EmpresaID == usuariologado.empresaId), "ID", "Descricao", bancoCaixa.ContaID);
                ViewBag.TpDocto = new SelectList(new BancoCaixa.ListaTipoDocumento().MetodoListaOpcao(), "ID", "Descricao", bancoCaixa.TpDocto);
                ViewBag.Tipo = new SelectList(new BancoCaixa.ListaTipo().MetodoListaOpcao(), "ID", "Descricao", bancoCaixa.Tipo);
                return View(bancoCaixa);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }
                
        public ActionResult Delete(int? id, int? baixaid)
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
                BancoCaixa bancoCaixa = db.BancoCaixa.Find(id, baixaid,usuariologado.empresaId);   
                
                if (bancoCaixa == null)
                {
                    return HttpNotFound();
                }
                ViewBag.PodeExcluir = true;
                var baixa = db.Baixas.Where(x => x.ID == bancoCaixa.BaixaID && x.EmpresaID == usuariologado.empresaId).FirstOrDefault();
                if(baixa != null)
                {
                    var financeiro = db.RecPag.Where(x => x.EmpresaID == usuariologado.empresaId && x.ID == baixa.FinanceiroID && x.ParcelaID == baixa.ParcelaID).FirstOrDefault();
                    if(financeiro != null)
                    {
                        if(financeiro.ConsultaID != null)
                        {
                            ViewBag.erro = "O registro não pode ser excluído, utilize a tela 'Estorno de Baixa'";
                            ViewBag.PodeExcluir = false;
                        }
                    }
                }
                
                return View(bancoCaixa);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }
                
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int baixaid)
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
                BancoCaixa bancoCaixa = db.BancoCaixa.Find(id, baixaid, usuariologado.empresaId);
                Baixa baixa = db.Baixas.Where(x => x.ID == baixaid && x.EmpresaID == usuariologado.empresaId).FirstOrDefault();
                Financeiro financeiro = db.RecPag.Where(x => x.ID == baixa.FinanceiroID && x.ParcelaID == baixa.ParcelaID && x.EmpresaID == usuariologado.empresaId).FirstOrDefault();
                db.BancoCaixa.Remove(bancoCaixa);
                db.Baixas.Remove(baixa);
                db.RecPag.Remove(financeiro);
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
