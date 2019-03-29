using SaudeWeb.Models;
using SaudeWeb.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;


namespace SaudeWeb.Controllers
{
    public class FinanceiroController : Controller
    {
        private DataContext db = new DataContext();

        [HttpGet]
        public ActionResult FaturamentoMedico(DateTime? dataEmissao1, DateTime? dataEmissao2, int? Situacao, int? pessoaID)
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
                if (pessoaID != null)
                {

                    var consulta = db.ConsultaExames.Join(db.Consultas, consultaE => consultaE.ConsultaID, consultas => consultas.Id, (consultaE, consultas) => new { consultaE, consultas })
                                   .Where(
                                   x => x.consultaE.EmpresaID == usuariologado.empresaId &&
                                   x.consultaE.RepasseMedico == 1 &&
                                   x.consultaE.FaturadoMedico == 0 &&
                                   x.consultaE.medicoId == pessoaID);
                    if (dataEmissao1 == null)
                    {
                        dataEmissao1 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-01"));
                        consulta = consulta.Where(x => x.consultaE.DataEmissao >= dataEmissao1);
                    }
                    else
                    {
                        consulta = consulta.Where(x => x.consultaE.DataEmissao >= dataEmissao1);
                    }
                    if (dataEmissao2 == null)
                    {
                        dataEmissao2 = DateTime.Now;
                        consulta = consulta.Where(x => x.consultaE.DataEmissao <= dataEmissao2);
                    }
                    else
                    {
                        consulta = consulta.Where(x => x.consultaE.DataEmissao <= dataEmissao2);
                    }
                    List<FaturaViewModel> lista = new List<FaturaViewModel>();
                    decimal soma = 0;
                    foreach (var item in consulta)
                    {
                        
                        FaturaViewModel fat = new FaturaViewModel();
                        fat.ConsultaID = item.consultaE.ConsultaID;
                        fat.EmpresaID = item.consultaE.EmpresaID;
                        fat.PessoaID = Convert.ToInt32(item.consultas.PessoaId);
                        fat.MedicoID = Convert.ToInt32(item.consultaE.medicoId);
                        fat.FuncionarioID = Convert.ToInt32(item.consultas.FuncionarioId);
                        fat.ValorFaturar = FaturaViewModel.GetValorRepasse(item.consultaE.EmpresaID, Convert.ToInt32(pessoaID), item.consultaE.ExameId);
                        fat.DataConsulta = item.consultaE.DataEmissao;                        
                        soma = soma + Convert.ToDecimal(fat.ValorFaturar);
                        lista.Add(fat);
                    }
                    ViewBag.Valor = soma;
                    return View(lista);
                }
                else
                {
                    //var
                    ViewBag.erro = "Informe o Cliente e o período para faturamento";
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
        public ActionResult FaturamentoMedico(DateTime? dataEmissao1, DateTime? dataEmissao2, int? Situacao, int pessoaID, string Competencia, decimal ValorTotal, DateTime? Vencimento)
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
                if (pessoaID != null)
                {

                    var consulta = db.Consultas.Join(db.ConsultaExames, consultas => consultas.Id, y => y.ConsultaID, (consultas, y) => new { consultas, y })
                                                       .Where(x => x.consultas.PessoaId == pessoaID &&
                                                      x.consultas.PessoaId == usuariologado.empresaId &&
                                                      x.y.RepasseMedico == 1 &&
                                                      x.y.FaturadoMedico == 0);
                    if (dataEmissao1 == null)
                    {
                        dataEmissao1 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-01"));
                        consulta = consulta.Where(x => x.consultas.DataConsulta >= dataEmissao1);
                    }
                    else
                    {
                        consulta = consulta.Where(x => x.y.DataEmissao >= dataEmissao1);
                    }
                    if (dataEmissao2 == null)
                    {
                        dataEmissao2 = DateTime.Now;
                        consulta = consulta.Where(x => x.y.DataEmissao <= dataEmissao2);
                    }
                    else
                    {
                        consulta = consulta.Where(x => x.y.DataEmissao <= dataEmissao2);
                    }
                    List<FaturaViewModel> lista = new List<FaturaViewModel>();
                                        
                    decimal soma = 0;
                    ViewBag.Valor = soma;

                    foreach (var item in consulta)
                    {
                        soma = soma + FaturaViewModel.GetValorRepasse(item.consultas.EmpresaID, pessoaID, item.y.ExameId); ;
                    }

                    var empresa = db.Empresas.Find(usuariologado.empresaId);
                    var observacao = "";
                    var planoConta = Convert.ToInt32(empresa.PlanoContaConsulta);
                    var plconta = db.PlanoContas.Where(x => x.ID == planoConta && x.EmpresaID == usuariologado.empresaId).FirstOrDefault();
                    if (plconta != null)
                    {
                        observacao = plconta.Conta + " - " + plconta.Descricao;
                    }

                    Financeiro financeiro = new Financeiro();
                    financeiro.ID = LibProdusys.GetNewCode("financeiro", "id", " empresaid = " + usuariologado.empresaId.ToString());
                    financeiro.Juros = 0;
                    financeiro.Multa = 0;
                    financeiro.Desconto = 0;
                    financeiro.ParcelaID = 1;
                    financeiro.Tipo = 2;
                    financeiro.Valor = soma;
                    financeiro.ValorBaixado = 0;
                    DateTime dtcomp = Convert.ToDateTime("01/" + Competencia);
                    financeiro.Competencia = dtcomp.ToString("yyyy-MM");
                    financeiro.CategoriaID = empresa.PlanoContaConsulta;
                    financeiro.DataEmissao = DateTime.Now;
                    financeiro.DataVencimento = Convert.ToDateTime(Vencimento);
                    financeiro.EmpresaID = Convert.ToInt32(usuariologado.empresaId);
                    financeiro.Observacao = LibProdusys.FS(observacao);
                    financeiro.EmpresaID = Convert.ToInt32(usuariologado.empresaId);
                    financeiro.PessoaID = Convert.ToInt32(pessoaID);
                    db.RecPag.Add(financeiro);
                    db.SaveChanges();
                    TempData["financeiroID"] = financeiro.ID;
                    ArrayList listaUp = new ArrayList();
                    foreach (var item in consulta)
                    {
                        listaUp.Add(item.consultas.Id);
                    }

                    foreach (var item in listaUp)
                    {
                        int idd = Convert.ToInt32(item.ToString());
                        var update = "Update ConsultaExame set FaturadoMedico = 1 where consultaid = " + idd.ToString() + " empresaid = " + usuariologado.empresaId.ToString();
                        Exec exec = new Exec();
                        exec.ExecutarComandoSql(update);                        
                    }
                    return Redirect("~/Financeiro/Faturado/" + financeiro.ID);
                }
                else
                {
                    //var
                    ViewBag.erro = "Informe o Cliente e o período para faturamento";
                    return View();
                }
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        public ActionResult Faturamento()// não utilizado no momento. Não excluir
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
                DateTime dt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-01"));
                var consulta = db.Consultas.Where(x => x.FinanceiroID == null && x.EmpresaID == usuariologado.empresaId && x.ValorFaturamento > 0/* && x.DataConsulta >= dt*/);
                List<FaturaViewModel> lista = new List<FaturaViewModel>();
                FaturaViewModel f = new FaturaViewModel();
                decimal soma = 0;
                foreach (var item in consulta)
                {
                    FaturaViewModel fat = new FaturaViewModel();
                    fat.ConsultaID = item.Id;
                    fat.EmpresaID = item.EmpresaID;
                    fat.PessoaID = Convert.ToInt32(item.PessoaId);
                    fat.ValorFaturar = item.ValorFaturamento;
                    fat.DataConsulta = item.DataConsulta;
                    fat.FuncionarioID = Convert.ToInt32(item.FuncionarioId);
                    soma = soma + Convert.ToDecimal(item.ValorFaturamento);
                    lista.Add(fat);
                }
                ViewBag.Valor = soma;
                return View(lista);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }
        

        [HttpGet]
        public ActionResult Faturamento(DateTime? dataEmissao1, DateTime? dataEmissao2, int? Situacao, int? pessoaID)
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
                if (pessoaID != null)
                {

                    var consulta = db.Consultas.Where(x => x.PessoaId == pessoaID &&
                                                      x.EmpresaID == usuariologado.empresaId &&
                                                      x.ValorFaturamento > 0 &&
                                                      (x.FinanceiroID == null || x.Fatura == 1));
                    if (dataEmissao1 == null)
                    {
                        dataEmissao1 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-01"));
                        consulta = consulta.Where(x => x.DataConsulta >= dataEmissao1);
                    }
                    else
                    {
                        consulta = consulta.Where(x => x.DataConsulta >= dataEmissao1);
                    }
                    if (dataEmissao2 == null)
                    {
                        dataEmissao2 = DateTime.Now;
                        consulta = consulta.Where(x => x.DataConsulta <= dataEmissao2);
                    }
                    else
                    {
                        consulta = consulta.Where(x => x.DataConsulta <= dataEmissao2);
                    }
                    List<FaturaViewModel> lista = new List<FaturaViewModel>();
                    decimal soma = 0;
                    foreach (var item in consulta)
                    {
                        FaturaViewModel fat = new FaturaViewModel();
                        fat.ConsultaID = item.Id;
                        fat.EmpresaID = item.EmpresaID;
                        fat.PessoaID = Convert.ToInt32(item.PessoaId);
                        fat.ValorFaturar = item.ValorFaturamento;
                        fat.DataConsulta = item.DataConsulta;
                        fat.FuncionarioID = Convert.ToInt32(item.FuncionarioId);
                        soma = soma + Convert.ToDecimal(item.ValorFaturamento);
                        lista.Add(fat);
                    }
                    ViewBag.Valor = soma;
                    return View(lista);
                }
                else
                {
                    //var
                    ViewBag.erro = "Informe o Cliente e o período para faturamento";
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
        public ActionResult Faturamento(DateTime? Vencimento, DateTime? dataEmissao1, DateTime? dataEmissao2, int? pessoaID, string Competencia, decimal ValorTotal)
        {
            ViewBag.UsuarioLogado = Utils.User.GetCookieUsuarioLogado(Request, Session);
            if (!string.IsNullOrEmpty(ViewBag.UsuarioLogado))
            {
                if ((pessoaID != null) && (ValorTotal > 0))
                {
                    Utils.User.UsuarioLogado usuariologado = Utils.User.GetDadosUsuarioLogado(ViewBag.UsuarioLogado);
                    ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;
                    ViewBag.RuleAdmin = usuariologado.admin;
                    ViewBag.RuleFinanceiro = usuariologado.RuleFinanceiro;
                    ViewBag.RuleMovimentacao = usuariologado.RuleMovimentacao;
                    ViewBag.RuleCadastro = usuariologado.RuleCadastro;

                    var consulta = db.Consultas.Where(x => x.PessoaId == pessoaID &&
                                                      x.EmpresaID == usuariologado.empresaId &&
                                                      x.ValorFaturamento > 0 &&
                                                      (x.FinanceiroID == null || x.Fatura == 1));
                    if (dataEmissao1 == null)
                    {
                        dataEmissao1 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-01"));
                        consulta = consulta.Where(x => x.DataConsulta >= dataEmissao1);
                    }
                    else
                    {
                        consulta = consulta.Where(x => x.DataConsulta >= dataEmissao1);
                    }
                    if (dataEmissao2 == null)
                    {
                        dataEmissao2 = DateTime.Now;
                        consulta = consulta.Where(x => x.DataConsulta <= dataEmissao2);
                    }
                    else
                    {
                        consulta = consulta.Where(x => x.DataConsulta <= dataEmissao2);
                    }

                    decimal soma = 0;

                    foreach (var item in consulta)
                    {
                        soma = soma + Convert.ToDecimal(item.ValorFaturamento);
                    }

                    var empresa = db.Empresas.Find(usuariologado.empresaId);
                    var observacao = "";
                    var planoConta = Convert.ToInt32(empresa.PlanoContaConsulta);
                    var plconta = db.PlanoContas.Where(x => x.ID == planoConta && x.EmpresaID == usuariologado.empresaId).FirstOrDefault();
                    if (plconta != null)
                    {
                        observacao = plconta.Conta + " - " + plconta.Descricao;
                    }

                    Financeiro financeiro = new Financeiro();
                    financeiro.ID = LibProdusys.GetNewCode("financeiro", "id", " empresaid = " + usuariologado.empresaId.ToString());
                    financeiro.Juros = 0;
                    financeiro.Multa = 0;
                    financeiro.Desconto = 0;
                    financeiro.ParcelaID = 1;
                    financeiro.Tipo = 1;
                    financeiro.Valor = soma;
                    financeiro.ValorBaixado = 0;
                    DateTime dtcomp = Convert.ToDateTime("01/" + Competencia);
                    financeiro.Competencia = dtcomp.ToString("yyyy-MM");
                    financeiro.CategoriaID = empresa.PlanoContaConsulta;
                    financeiro.DataEmissao = DateTime.Now;
                    financeiro.DataVencimento = Convert.ToDateTime(Vencimento);
                    financeiro.EmpresaID = Convert.ToInt32(usuariologado.empresaId);
                    financeiro.Observacao = LibProdusys.FS(observacao);
                    financeiro.EmpresaID = Convert.ToInt32(usuariologado.empresaId);
                    financeiro.PessoaID = Convert.ToInt32(pessoaID);
                    db.RecPag.Add(financeiro);
                    db.SaveChanges();
                    TempData["financeiroID"] = financeiro.ID;
                    ArrayList listaUp = new ArrayList();
                    foreach (var item in consulta)
                    {
                        listaUp.Add(item.Id);
                    }

                    foreach (var item in listaUp)
                    {
                        int idd = Convert.ToInt32(item.ToString());
                        var consultaUp = db.Consultas.Where(x => x.EmpresaID == usuariologado.empresaId && x.Id == idd).FirstOrDefault();
                        consultaUp.FinanceiroID = financeiro.ID;
                        consultaUp.Fatura = 2;
                        db.Entry(consultaUp).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return Redirect("~/Financeiro/Faturado/" + financeiro.ID);
                }
                else
                {
                    ViewBag.erro = "Informe o Cliente e o período para faturamento";
                    return View();
                }
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        public ActionResult Faturado(int? id)
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

                var fatura = db.Consultas.Where(x => x.EmpresaID == usuariologado.empresaId && x.FinanceiroID == id);
                var financeiro = db.RecPag.Where(x => x.ID == id && x.EmpresaID == usuariologado.empresaId).FirstOrDefault();
                decimal valor = 0;
                List<FaturaViewModel> lista = new List<FaturaViewModel>();
                foreach (var item in fatura)
                {
                    FaturaViewModel fat = new FaturaViewModel();
                    fat.ConsultaID = item.Id;
                    fat.EmpresaID = item.EmpresaID;
                    fat.PessoaID = Convert.ToInt32(item.PessoaId);
                    fat.ValorFaturar = item.ValorFaturamento;
                    fat.DataConsulta = item.DataConsulta;
                    fat.FuncionarioID = Convert.ToInt32(item.FuncionarioId);
                    valor = valor + Convert.ToDecimal(item.ValorFaturamento);
                    lista.Add(fat);
                }
                ViewBag.Cliente = financeiro.ClienteDesc;
                ViewBag.Vencimento = financeiro.VencimentoFormatado;
                ViewBag.Valor = valor;
                return View(lista);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        [HttpPost]
        public ActionResult Index(string pessoaID, string dataEmissao1, string dataEmissao2, string dataVencimento1, string dataVencimento2, string dataLiquidacao1, string dataLiquidacao2, int? situacao, int? tipo, int? qtdeReg)
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
                ViewBag.Situacao = new SelectList(new Financeiro.Situacao().MetodoListaSituacao(), "ID", "Descricao");
                ViewBag.tipo = new SelectList(new Financeiro.TipoF().MetodoListaTipo(), "ID", "Descricao");
                var recPag = db.RecPag.Where(x => x.EmpresaID == usuariologado.empresaId);
                if (!string.IsNullOrEmpty(pessoaID))
                {
                    int pessoa = Convert.ToInt32(pessoaID);
                    recPag = recPag.Where(x => x.PessoaID == pessoa);
                }
                if (!string.IsNullOrEmpty(dataEmissao1))
                {
                    DateTime dt = Convert.ToDateTime(dataEmissao1);
                    recPag = recPag.Where(x => x.DataEmissao >= dt);
                }
                if (!string.IsNullOrEmpty(dataEmissao2))
                {
                    DateTime dt = Convert.ToDateTime(dataEmissao2);
                    recPag = recPag.Where(x => x.DataEmissao <= dt);
                }
                if (!string.IsNullOrEmpty(dataVencimento1))
                {
                    DateTime dt = Convert.ToDateTime(dataVencimento1);
                    recPag = recPag.Where(x => x.DataVencimento >= dt);
                }

                if (!string.IsNullOrEmpty(dataVencimento2))
                {
                    DateTime dt = Convert.ToDateTime(dataVencimento2);
                    recPag = recPag.Where(x => x.DataVencimento <= dt);
                }

                if (!string.IsNullOrEmpty(dataLiquidacao1))
                {
                    DateTime dt = Convert.ToDateTime(dataLiquidacao1);
                    recPag = recPag.Where(x => x.DataLiquidacao >= dt);
                }
                if (!string.IsNullOrEmpty(dataLiquidacao2))
                {
                    DateTime dt = Convert.ToDateTime(dataLiquidacao2);
                    recPag = recPag.Where(x => x.DataLiquidacao <= dt);
                }
                if (situacao == null)
                {
                    recPag = recPag.Where(x => ((x.ValorBaixado + x.Desconto) - (x.Juros + x.Multa)) < x.Valor);
                }
                else
                {
                    if (situacao == 0)
                    {
                        recPag = recPag.Where(x => ((x.ValorBaixado + x.Desconto) - (x.Juros + x.Multa)) < x.Valor);
                    }
                    if (situacao == 1)
                    {
                        recPag = recPag.Where(x => ((x.ValorBaixado + x.Desconto) - (x.Juros + x.Multa)) >= x.Valor);
                    }
                }
                if (tipo == 1)
                {
                    recPag = recPag.Where(x => x.Tipo == 1);
                }
                if (tipo == 2)
                {
                    recPag = recPag.Where(x => x.Tipo == 2);
                }
                if (qtdeReg != 1000)
                {
                    recPag = recPag.Take(Convert.ToInt32(qtdeReg));
                }
                return View(recPag.ToList());
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        // GET: Financeiro
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

                ViewBag.Situacao = new SelectList(new Financeiro.Situacao().MetodoListaSituacao(), "ID", "Descricao");
                ViewBag.tipo = new SelectList(new Financeiro.TipoF().MetodoListaTipo(), "ID", "Descricao");
                List<Financeiro> lista = new List<Financeiro>();
                return View(lista.ToList());
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        public ActionResult EmitirBoleto(int id = 47, int parcelaId = 1, int Conta = 1)
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

                var financeiro = db.RecPag.Where(x => x.ID == id && x.EmpresaID == usuariologado.empresaId && x.ParcelaID == parcelaId).FirstOrDefault();
                var conta = db.ContaCorrenteCaixas.Where(x => x.ID == Conta && x.EmpresaID == usuariologado.empresaId).FirstOrDefault();
                if(financeiro != null)
                {
                    Boleto boleto = new Boleto();
                    boleto.ID = LibProdusys.GetNewCode("boleto", "id", " empresaid = " + usuariologado.empresaId.ToString());
                    boleto.Carteira = conta.Carteira;
                    boleto.CodigoBeneficiario = conta.CodigoBeneficiario;
                    boleto.Modalidade = conta.Modalidade;
                    boleto.EmpresaID = Convert.ToInt32(usuariologado.empresaId);
                    boleto.CedenteID = Convert.ToInt32(usuariologado.empresaId);
                    boleto.ContaID = conta.ID;
                    boleto.Juros = 0;
                    boleto.Multa = 0;
                    boleto.NossoNumero = "";
                    boleto.NumeroDocumento = financeiro.NumeroDocumento;
                    boleto.NumeroRemessa = 1;
                    boleto.Observacoes = financeiro.Observacao;
                    boleto.SacadoID = financeiro.PessoaID;
                    boleto.Situacao = 0;
                    boleto.Valor = financeiro.Valor;
                    boleto.ValorBaixado = 0;
                    boleto.ValorTotal = boleto.Valor;
                    boleto.DataEmissao = Convert.ToDateTime(financeiro.DataEmissao);
                    boleto.DataVencimento = Convert.ToDateTime(financeiro.DataVencimento);
                    db.Boletos.Add(boleto);
                    db.SaveChanges();

                    BoletoViewModel bv = new BoletoViewModel();
                    bv.Modalidade = boleto.Modalidade;
                    bv.CodigoBeneficiario = boleto.CodigoBeneficiario;
                    bv.Carteira = boleto.Carteira;
                    bv.CedenteID = boleto.CedenteID;
                    bv.ContaID = boleto.ContaID;
                    bv.DataEmissao = boleto.DataEmissao;
                    bv.DataVencimento = boleto.DataVencimento;
                    bv.Desconto = boleto.Desconto;
                    bv.EmpresaID = boleto.EmpresaID;
                    bv.ID = boleto.ID;
                    bv.Juros = boleto.Juros;
                    bv.LocalPagmento = "Pagável em qualquer Banco";
                    bv.Multa = boleto.Multa;
                    bv.NossoNumero = boleto.NossoNumero;
                    bv.NumeroDocumento = boleto.NumeroDocumento;
                    bv.NumeroRemessa = boleto.NumeroRemessa;
                    bv.Observacoes = boleto.Observacoes;
                    bv.SacadoID = boleto.SacadoID;
                    bv.Situacao = boleto.Situacao;
                    bv.Valor = boleto.Valor;
                    bv.ValorBaixado = boleto.ValorBaixado;
                    bv.ValorTotal = boleto.ValorTotal;                    
                    bv.LinhaDigitavel = bv.CalculoLinhaDigitavel(conta.Banco.ToString(), bv.Carteira, conta.Agencia, bv.Modalidade, bv.CodigoBeneficiario, "1", bv.DataVencimento, bv.Valor, "", LibProdusys.StrZero(financeiro.ParcelaID.ToString(), 3));
                    var codbar = bv.GetCodBar(bv.LinhaDigitavel);
                    var empresa = db.Empresas.Find(usuariologado.empresaId);
                    var financeiroPagador = db.Pessoas.Where(x => x.EmpresaID == usuariologado.empresaId && x.ID == financeiro.PessoaID).FirstOrDefault();
                    BoletoViewModel.Beneficiario beneficiario = new BoletoViewModel.Beneficiario();
                    beneficiario.Agencia = conta.Agencia;
                    beneficiario.CodigoBeneficiario = "";
                    beneficiario.Nome = empresa.Razao;
                    
                    BoletoViewModel.Pagador pagador = new BoletoViewModel.Pagador();
                    pagador.CpfCnpj = financeiroPagador.CNPJ;
                    pagador.Nome = financeiroPagador.Razao;
                    pagador.SacadorAvalista = empresa.Razao;                    

                    ViewBag.ImgCodbar = codbar;


                    return View(bv);
                }
                
                return View();
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        // GET: Financeiro/Create
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
                //ViewBag.Situacao = new SelectList(new Financeiro.Situacao().MetodoListaSituacao().Where(x => x.ID != 1000), "ID", "Descricao");
                ViewBag.TipoCompetencia = new SelectList(new Financeiro.TipoCompetencia().MetodoLista(), "ID", "Descricao");
                ViewBag.Tipo = new SelectList(new Financeiro.TipoF().MetodoListaTipo().Where(x => x.ID != 1000), "ID", "Descricao");

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
        public ActionResult Create([Bind(Include = "ID,EmpresaID,ParcelaID,Tipo,DataEmissao,DataVencimento,DataLiquidacao,Valor,Juros,Multa,Desconto,ValorBaixado,PessoaID,NumeroDocumento,ConsultaID,Observacao,ParcelaIni,ParcelaFim,CategoriaID,Competencia")] Financeiro financeiro, int? ParcelaIni, int? ParcelaFim, int? TipoCompetencia)
        {
            ViewBag.EmpresaID = new SelectList(db.Empresas, "ID", "Razao", financeiro.EmpresaID);
            ViewBag.UsuarioLogado = Utils.User.GetCookieUsuarioLogado(Request, Session);
            if (!string.IsNullOrEmpty(ViewBag.UsuarioLogado))
            {
                Utils.User.UsuarioLogado usuariologado = Utils.User.GetDadosUsuarioLogado(ViewBag.UsuarioLogado);
                ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;
                ViewBag.RuleAdmin = usuariologado.admin;
                ViewBag.RuleFinanceiro = usuariologado.RuleFinanceiro;
                ViewBag.RuleMovimentacao = usuariologado.RuleMovimentacao;
                ViewBag.RuleCadastro = usuariologado.RuleCadastro;
                ViewBag.Tipo = new SelectList(new Financeiro.TipoF().MetodoListaTipo().Where(x => x.ID != 1000), "ID", "Descricao", financeiro.Tipo);
                ViewBag.TipoCompetencia = new SelectList(new Financeiro.TipoCompetencia().MetodoLista(), "ID", "Descricao");
                if ((financeiro.CategoriaID != null) &&
                    (financeiro.PessoaID != null) &&
                    (financeiro.Valor != null) && financeiro.DataVencimento > DateTime.Now.AddYears(-50))

                {
                    if ((ParcelaIni == 0) || (ParcelaIni == null))
                    {
                        ParcelaIni = 1;
                    }
                    if ((ParcelaFim == 0) || (ParcelaFim == null))
                    {
                        ParcelaFim = 1;
                    }
                    int inicio = Convert.ToInt32(ParcelaIni);
                    int Fim = Convert.ToInt32(ParcelaFim);

                    var id = 0;
                    id = LibProdusys.GetNewCode("financeiro", "id", " empresaid = " + usuariologado.empresaId.ToString());
                    var interacao = 0;
                    DateTime ultimaParcela = DateTime.Now;
                    for (int i = inicio; i < Fim + 1; i++)
                    {
                        interacao = interacao + 1;
                        financeiro.ID = id;
                        financeiro.ParcelaID = i;
                        financeiro.EmpresaID = Convert.ToInt32(usuariologado.empresaId);
                        financeiro.Observacao = LibProdusys.FS(financeiro.Observacao);
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Properties.Settings.Default.Banco;
                        string insert = "Insert into Financeiro(id, empresaid, parcelaid, tipo, dataemissao, datavencimento, dataliquidacao, valor, juros, multa, desconto, pessoaid, numerodocumento, observacao, consultaid, valorbaixado, categoriaid, competencia)"
                                      + "values(@id, @empresaid, @parcelaid, @tipo, @dataemissao, @datavencimento, @dataliquidacao, @valor, @juros, @multa, @desconto, @pessoaid, @numerodocumento, @observacao, @consultaid, @valorbaixado,@categoriaid, @competencia)";
                        SqlCommand cmd = new SqlCommand(insert, con);
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = financeiro.ID;
                        cmd.Parameters.Add("@empresaid", SqlDbType.Int).Value = financeiro.EmpresaID;
                        cmd.Parameters.Add("@parcelaid", SqlDbType.Int).Value = financeiro.ParcelaID;
                        cmd.Parameters.Add("@tipo", SqlDbType.Int).Value = financeiro.Tipo;
                        cmd.Parameters.Add("@categoriaid", SqlDbType.Int).Value = financeiro.CategoriaID;

                        if (TipoCompetencia == 0)
                        {
                            cmd.Parameters.Add("@competencia", SqlDbType.VarChar).Value = financeiro.Competencia;
                        }
                        else
                        {
                            cmd.Parameters.Add("@competencia", SqlDbType.VarChar).Value = financeiro.DataVencimento.ToString("yyyy-MM");
                        }

                        if (financeiro.DataEmissao == null)
                        {
                            cmd.Parameters.Add("@dataemissao", SqlDbType.DateTime).Value = DateTime.Now;
                        }
                        else
                        {
                            cmd.Parameters.Add("@dataemissao", SqlDbType.DateTime).Value = financeiro.DataEmissao;
                        }

                        if (interacao == 1)
                        {
                            cmd.Parameters.Add("@datavencimento", SqlDbType.DateTime).Value = financeiro.DataVencimento;
                            ultimaParcela = financeiro.DataVencimento;
                        }
                        else
                        {
                            if (interacao == 2)
                            {
                                //DateTime dataV = financeiro.DataVencimento.AddDays(LibProdusys.GetQtdeDias(ultimaParcela.AddMonths(1)));
                                DateTime dataV = financeiro.DataVencimento.AddMonths(1);
                                ultimaParcela = dataV;
                                cmd.Parameters.Add("@datavencimento", SqlDbType.DateTime).Value = dataV;
                            }
                            else
                            {
                                //cmd.Parameters.Add("@datavencimento", SqlDbType.DateTime).Value = ultimaParcela.AddDays(LibProdusys.GetQtdeDias(ultimaParcela.AddMonths(1)));
                                cmd.Parameters.Add("@datavencimento", SqlDbType.DateTime).Value = ultimaParcela.AddMonths(1);
                                //ultimaParcela = ultimaParcela.AddDays(LibProdusys.GetQtdeDias(ultimaParcela.AddMonths(1)));
                                ultimaParcela = ultimaParcela.AddMonths(1);
                            }
                        }

                        if (financeiro.DataLiquidacao == null)
                        {
                            cmd.Parameters.Add("@dataliquidacao", SqlDbType.DateTime).Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters.Add("@dataliquidacao", SqlDbType.DateTime).Value = financeiro.DataLiquidacao;
                        }

                        cmd.Parameters.Add("@valor", SqlDbType.Decimal).Value = financeiro.Valor;
                        if (financeiro.Juros == null)
                        {
                            cmd.Parameters.Add("@juros", SqlDbType.Decimal).Value = 0;
                        }
                        else
                        {
                            cmd.Parameters.Add("@juros", SqlDbType.Decimal).Value = financeiro.Juros;
                        }
                        if (financeiro.Multa == null)
                        {
                            cmd.Parameters.Add("@multa", SqlDbType.Decimal).Value = 0;
                        }
                        else
                        {
                            cmd.Parameters.Add("@multa", SqlDbType.Decimal).Value = financeiro.Multa;
                        }
                        if (financeiro.Desconto == null)
                        {
                            cmd.Parameters.Add("@desconto", SqlDbType.Decimal).Value = 0;
                        }
                        else
                        {
                            cmd.Parameters.Add("@desconto", SqlDbType.Decimal).Value = financeiro.Desconto;
                        }


                        cmd.Parameters.Add("@pessoaid", SqlDbType.Int).Value = financeiro.PessoaID;
                        if (!string.IsNullOrEmpty(financeiro.NumeroDocumento))
                        {
                            cmd.Parameters.Add("@numerodocumento", SqlDbType.VarChar).Value = financeiro.NumeroDocumento;
                        }
                        else
                        {
                            cmd.Parameters.Add("@numerodocumento", SqlDbType.VarChar).Value = "";
                        }
                        cmd.Parameters.Add("@observacao", SqlDbType.VarChar).Value = financeiro.Observacao;
                        if (financeiro.ConsultaID == null)
                        {
                            cmd.Parameters.Add("@consultaid", SqlDbType.VarChar).Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters.Add("@consultaid", SqlDbType.VarChar).Value = financeiro.ConsultaID;
                        }
                        if (financeiro.ValorBaixado == null)
                        {
                            cmd.Parameters.Add("@valorbaixado", SqlDbType.Decimal).Value = 0;
                        }
                        else
                        {
                            cmd.Parameters.Add("@valorbaixado", SqlDbType.Decimal).Value = financeiro.ValorBaixado;
                        }

                        con.Open();
                        try
                        {
                            int ix = cmd.ExecuteNonQuery();
                            if (ix > 0)
                            {

                            }
                        }
                        catch (Exception)
                        {
                            ViewBag.erro = "Erro ao gravar no banco de dados entre em contato com o Suporte Técnico";
                            throw;
                        }
                        finally
                        {
                            con.Close();
                        }
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.erro = "Há campos obrigatórios sem preenchimento";
                    return View(financeiro);
                }

            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        // GET: Financeiro/Edit/5
        public ActionResult Edit(int? id, int? parcelaID)
        {
            ViewBag.UsuarioLogado = Utils.User.GetCookieUsuarioLogado(Request, Session);
            if (!string.IsNullOrEmpty(ViewBag.UsuarioLogado))
            {
                bool baixado = false;
                Utils.User.UsuarioLogado usuariologado = Utils.User.GetDadosUsuarioLogado(ViewBag.UsuarioLogado);
                ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;
                ViewBag.RuleAdmin = usuariologado.admin;
                ViewBag.RuleFinanceiro = usuariologado.RuleFinanceiro;
                ViewBag.RuleMovimentacao = usuariologado.RuleMovimentacao;
                ViewBag.RuleCadastro = usuariologado.RuleCadastro;
                if (id == null)
                {
                    LibProdusys.DadosNavegador dados = new LibProdusys.DadosNavegador();
                    var resumo = dados.Resumo();
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Financeiro financeiro = db.RecPag.Find(id, usuariologado.empresaId, parcelaID);
                if (financeiro == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Tipo = new SelectList(new Financeiro.TipoF().MetodoListaTipo().Where(x => x.ID != 1000), "ID", "Descricao", financeiro.Tipo);
                ViewBag.baixado = false;
                if (financeiro.DataLiquidacao != null)
                {
                    ViewBag.baixado = true;
                    baixado = true;
                }
                if (baixado)
                {
                    var baixa = db.Baixas.Where(x => x.EmpresaID == usuariologado.empresaId && x.ParcelaID == parcelaID && x.FinanceiroID == x.FinanceiroID).FirstOrDefault();
                    var bancoCaixa = db.BancoCaixa.Where(x => x.BaixaID == baixa.ID && x.EmpresaID == usuariologado.empresaId).FirstOrDefault();
                    ViewBag.contaID = new SelectList(db.ContaCorrenteCaixas.Where(x => x.EmpresaID == usuariologado.empresaId), "ID", "Descricao", bancoCaixa.ContaID);
                    ViewBag.TpDocto = new SelectList(new BancoCaixa.ListaTipoDocumento().MetodoListaOpcao(), "ID", "Descricao", bancoCaixa.TpDocto);
                }
                else
                {
                    ViewBag.contaID = new SelectList(db.ContaCorrenteCaixas.Where(x => x.EmpresaID == usuariologado.empresaId), "ID", "Descricao");
                    ViewBag.TpDocto = new SelectList(new BancoCaixa.ListaTipoDocumento().MetodoListaOpcao(), "ID", "Descricao");
                }
                Financeiro.SituacaoRegistro(financeiro.EmpresaID, financeiro.ID, financeiro.ParcelaID);
                dynamic dina = Financeiro.SituacaoRegistro(financeiro.EmpresaID, financeiro.ID, financeiro.ParcelaID);
                
                ViewBag.SituacaoDoc = dina;
                return View(financeiro);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,EmpresaID,ParcelaID,Tipo,DataEmissao,DataVencimento,DataLiquidacao,Valor,Juros,Multa,Desconto,ValorBaixado,PessoaID,NumeroDocumento,ConsultaID,Observacao,CategoriaID,Competencia")]
        Financeiro financeiro, int? baixar, int? contaID, int? TpDocto, string chBanco, string chAgencia, string chNumero, DateTime? chVencimento)
        {
            ViewBag.UsuarioLogado = Utils.User.GetCookieUsuarioLogado(Request, Session);
            if (!string.IsNullOrEmpty(ViewBag.UsuarioLogado))
            {
                Utils.User.UsuarioLogado usuariologado = Utils.User.GetDadosUsuarioLogado(ViewBag.UsuarioLogado);
                ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;
                ViewBag.Tipo = new SelectList(new Financeiro.TipoF().MetodoListaTipo().Where(x => x.ID != 1000), "ID", "Descricao", financeiro.Tipo);
                ViewBag.contaID = new SelectList(db.ContaCorrenteCaixas.Where(x => x.EmpresaID == usuariologado.empresaId), "ID", "Descricao", contaID);
                ViewBag.TpDocto = new SelectList(new BancoCaixa.ListaTipoDocumento().MetodoListaOpcao(), "ID", "Descricao", TpDocto);
                if (ModelState.IsValid)
                {
                   
                    if (baixar == 1)
                    {
                       
                        if (financeiro.DataLiquidacao == null)
                        {
                            financeiro.DataLiquidacao = DateTime.Now;
                        }
                        else
                        {
                            financeiro.DataLiquidacao = financeiro.DataLiquidacao;
                        }
                    }

                    financeiro.Observacao = LibProdusys.FS(financeiro.Observacao);
                    db.Entry(financeiro).State = EntityState.Modified;
                    db.SaveChanges();
                    if (baixar == 1)
                    {
                        Baixa baixa = new Baixa();
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

                        baixa.Obs = LibProdusys.FS(baixa.Obs);
                        baixa.EmpresaID = Convert.ToInt32(usuariologado.empresaId);
                        db.Baixas.Add(baixa);
                        db.SaveChanges();

                        var alteraRecpag = db.RecPag.Where(x => x.EmpresaID == usuariologado.empresaId && x.ID == financeiro.ID && x.ParcelaID == financeiro.ParcelaID).FirstOrDefault();
                        alteraRecpag.ValorBaixado = Convert.ToDecimal(baixa.Total);
                        db.Entry(alteraRecpag).State = EntityState.Modified;
                        db.SaveChanges();

                        var conta = db.ContaCorrenteCaixas.Where(x => x.EmpresaID == usuariologado.empresaId && x.ID == contaID).FirstOrDefault();
                        BancoCaixa bancoCaixa = new BancoCaixa();
                        bancoCaixa.ID = LibProdusys.GetNewCode("bancocaixa", "id", " empresaid = " + usuariologado.empresaId.ToString()); ;

                        bancoCaixa.PlanoContaID = Convert.ToInt32(financeiro.CategoriaID);
                        bancoCaixa.Tipo = financeiro.Tipo;
                        bancoCaixa.EmpresaID = Convert.ToInt32(usuariologado.empresaId);
                        bancoCaixa.DataEmissao = DateTime.Now;
                        if (chVencimento != null)
                        {
                            bancoCaixa.DataVencimento = Convert.ToDateTime(chVencimento);
                        }
                        else
                        {
                            bancoCaixa.DataVencimento = baixa.DataBaixa;
                        }

                        if (chNumero == "")
                        {
                            bancoCaixa.NumeroDocumento = financeiro.NumeroDocumento;
                        }
                        else
                        {
                            bancoCaixa.NumeroDocumento = chNumero;
                        }
                        bancoCaixa.Obs = "Liquidação do lançamento ID/Parcela: " + financeiro.IDParcela + " na data de " + Convert.ToString(financeiro.DataLiquidacao);
                        if (TpDocto == null)
                        {
                            bancoCaixa.TpDocto = 1;
                        }
                        else
                        {
                            bancoCaixa.TpDocto = Convert.ToInt32(TpDocto);
                        }
                        bancoCaixa.valor = Convert.ToDecimal(baixa.Total);
                        bancoCaixa.Agencia = chAgencia;
                        bancoCaixa.Banco = chBanco;
                        bancoCaixa.ContaID = conta.ID;
                        bancoCaixa.PessoaId = financeiro.PessoaID;
                        bancoCaixa.BaixaID = baixa.ID;
                        bancoCaixa.Conta = conta.NumeroConta;
                        db.BancoCaixa.Add(bancoCaixa);
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(financeiro);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        // GET: Financeiro/Delete/5
        public ActionResult Delete(int? id, int? parcelaID)
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
                Financeiro financeiro = db.RecPag.Find(id, usuariologado.empresaId, parcelaID);
                if (financeiro == null)
                {
                    return HttpNotFound();
                }
                ViewBag.PodeExcluir = true;
                var baixa = db.Baixas.Count(x => x.ParcelaID == parcelaID && x.EmpresaID == usuariologado.empresaId && x.FinanceiroID == id);
                if (baixa > 0)
                {
                    ViewBag.PodeExcluir = false;
                    ViewBag.erro = "O registro não pode ser excluído, verifique baixas.";
                }
                return View(financeiro);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        // POST: Financeiro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int parcelaID)
        {
            ViewBag.UsuarioLogado = Utils.User.GetCookieUsuarioLogado(Request, Session);
            if (!string.IsNullOrEmpty(ViewBag.UsuarioLogado))
            {
                Utils.User.UsuarioLogado usuariologado = Utils.User.GetDadosUsuarioLogado(ViewBag.UsuarioLogado);
                ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;
                Financeiro financeiro = db.RecPag.Find(id, usuariologado.empresaId, parcelaID);
                db.RecPag.Remove(financeiro);
                db.SaveChanges();
                db.Dispose();
                db = new DataContext();

                var consultaUp = db.Consultas.Where(x => x.EmpresaID == usuariologado.empresaId && x.FinanceiroID == financeiro.ID).ToList();
                foreach (var item in consultaUp)
                {
                    var consulta = db.Consultas.Where(x => x.EmpresaID == usuariologado.empresaId && x.Id == item.Id).FirstOrDefault();
                    consulta.FinanceiroID = null;
                    if (item.Fatura == 0)
                    {
                        consulta.Fatura = 0;
                    }
                    else
                    {
                        consulta.Fatura = 1;
                    }
                    db.Entry(consulta).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }


        public ActionResult Estorno(int? id, int? parcelaID)
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
                Financeiro financeiro = db.RecPag.Find(id, usuariologado.empresaId, parcelaID);
                if (financeiro == null)
                {
                    return HttpNotFound();
                }
                ViewBag.PodeExcluir = true;
                var baixa = db.Baixas.Count(x => x.ParcelaID == parcelaID && x.EmpresaID == usuariologado.empresaId && x.FinanceiroID == id);
                if (baixa > 0)
                {
                    ViewBag.PodeExcluir = false;
                    ViewBag.erro = "O registro não pode ser excluído, verifique baixas.";
                }
                return View(financeiro);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        // POST: Financeiro/Delete/5
        [HttpPost, ActionName("Estorno")]
        [ValidateAntiForgeryToken]
        public ActionResult EstornoConfirmed(int id, int parcelaID)
        {
            ViewBag.UsuarioLogado = Utils.User.GetCookieUsuarioLogado(Request, Session);
            if (!string.IsNullOrEmpty(ViewBag.UsuarioLogado))
            {
                Utils.User.UsuarioLogado usuariologado = Utils.User.GetDadosUsuarioLogado(ViewBag.UsuarioLogado);
                ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;
                Financeiro financeiro = db.RecPag.Find(id, usuariologado.empresaId, parcelaID);
                financeiro.DataLiquidacao = null;
                financeiro.Desconto = 0;
                financeiro.Juros = 0;
                financeiro.Multa = 0;
                financeiro.ValorBaixado = 0;
                db.Entry(financeiro).State = EntityState.Modified;
                db.SaveChanges();
                var baixa = db.Baixas.Where(x => x.EmpresaID == usuariologado.empresaId && x.FinanceiroID == financeiro.ID && x.ParcelaID == financeiro.ParcelaID);
                ArrayList listaUp = new ArrayList();
                foreach (var item in baixa)
                {
                    listaUp.Add(item.ID);
                }
                db.Baixas.RemoveRange(baixa);
                db.SaveChanges();
                foreach (var item in listaUp)
                {
                    int idd = Convert.ToInt32(item.ToString());
                    var bancoCaixa = db.BancoCaixa.Where(x => x.EmpresaID == usuariologado.empresaId && x.BaixaID == idd).FirstOrDefault();
                    db.BancoCaixa.Remove(bancoCaixa);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        // GET: Financeiro/Edit/5
        public ActionResult Baixa(int? id, int? parcelaID)
        {
            ViewBag.UsuarioLogado = Utils.User.GetCookieUsuarioLogado(Request, Session);
            if (!string.IsNullOrEmpty(ViewBag.UsuarioLogado))
            {
                bool baixado = false;
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
                Financeiro financeiro = db.RecPag.Find(id, usuariologado.empresaId, parcelaID);
                if (financeiro == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Tipo = new SelectList(new Financeiro.TipoF().MetodoListaTipo().Where(x => x.ID != 1000), "ID", "Descricao", financeiro.Tipo);
                ViewBag.baixado = false;
                if (financeiro.DataLiquidacao != null)
                {
                    ViewBag.baixado = true;
                    baixado = true;
                }
                if (baixado)
                {
                    var baixa = db.Baixas.Where(x => x.EmpresaID == usuariologado.empresaId && x.ParcelaID == parcelaID && x.FinanceiroID == x.FinanceiroID).FirstOrDefault();
                    var bancoCaixa = db.BancoCaixa.Where(x => x.BaixaID == baixa.ID && x.EmpresaID == usuariologado.empresaId).FirstOrDefault();
                    ViewBag.contaID = new SelectList(db.ContaCorrenteCaixas.Where(x => x.EmpresaID == usuariologado.empresaId), "ID", "Descricao", bancoCaixa.ContaID);
                    ViewBag.TpDocto = new SelectList(new BancoCaixa.ListaTipoDocumento().MetodoListaOpcao(), "ID", "Descricao", bancoCaixa.TpDocto);
                }
                else
                {
                    ViewBag.contaID = new SelectList(db.ContaCorrenteCaixas.Where(x => x.EmpresaID == usuariologado.empresaId), "ID", "Descricao");
                    ViewBag.TpDocto = new SelectList(new BancoCaixa.ListaTipoDocumento().MetodoListaOpcao(), "ID", "Descricao");
                }
                return View(financeiro);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        [HttpPost]
        public ActionResult Baixa([Bind(Include = "ID,EmpresaID,ParcelaID,Tipo,DataEmissao,DataVencimento,DataLiquidacao,Valor,Juros,Multa,Desconto,ValorBaixado,PessoaID,NumeroDocumento,ConsultaID,Observacao,CategoriaID,Competencia")]
        Financeiro financeiro,  int? contaID, int? TpDocto, string chBanco, string chAgencia, string chNumero, DateTime? chVencimento)
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
                ViewBag.Tipo = new SelectList(new Financeiro.TipoF().MetodoListaTipo().Where(x => x.ID != 1000), "ID", "Descricao", financeiro.Tipo);
                ViewBag.contaID = new SelectList(db.ContaCorrenteCaixas.Where(x => x.EmpresaID == usuariologado.empresaId), "ID", "Descricao", contaID);
                ViewBag.TpDocto = new SelectList(new BancoCaixa.ListaTipoDocumento().MetodoListaOpcao(), "ID", "Descricao", TpDocto);
                if (ModelState.IsValid)
                {
                    if(financeiro.DataLiquidacao == null)
                    {
                        financeiro.DataLiquidacao = DateTime.Now;
                    }                    
                    DataContext db1 = new DataContext();
                    var financeiroAntigo = db1.RecPag.Where(x => x.ID == financeiro.ID && x.EmpresaID == usuariologado.empresaId && x.ParcelaID == financeiro.ParcelaID).FirstOrDefault();
                    var j = financeiroAntigo.Juros;
                    var m = financeiroAntigo.Multa;
                    var d = financeiroAntigo.Desconto;
                    var b = financeiroAntigo.ValorBaixado;
                    financeiro.Juros = j + financeiro.Juros;
                    financeiro.Multa = m + financeiro.Multa;
                    financeiro.Desconto = d + financeiro.Desconto;
                    financeiro.ValorBaixado = b + financeiro.ValorBaixado;
                    financeiro.Observacao = LibProdusys.FS(financeiro.Observacao);
                    db.Entry(financeiro).State = EntityState.Modified;
                    db.SaveChanges();

                    Baixa baixa = new Baixa();
                    baixa.ID = LibProdusys.GetNewCode("baixa", "id", " empresaid = " + usuariologado.empresaId.ToString()); 


                    baixa.FinanceiroID = financeiro.ID;
                    baixa.ParcelaID = financeiro.ParcelaID;
                    baixa.Valor = financeiro.ValorBaixado - b;
                    baixa.Juros = financeiro.Juros - j;
                    baixa.Multa = financeiro.Multa - m;
                    baixa.Desconto = financeiro.Desconto - d;
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

                    baixa.Obs = LibProdusys.FS(baixa.Obs);
                    baixa.EmpresaID = Convert.ToInt32(usuariologado.empresaId);
                    db.Baixas.Add(baixa);
                    db.SaveChanges();

                    //var alteraRecpag = db.RecPag.Where(x => x.EmpresaID == usuariologado.empresaId && x.ID == financeiro.ID && x.ParcelaID == financeiro.ParcelaID).FirstOrDefault();
                    //alteraRecpag.ValorBaixado = Convert.ToDecimal(baixa.Total);
                    //db.Entry(alteraRecpag).State = EntityState.Modified;
                    //db.SaveChanges();

                    var conta = db.ContaCorrenteCaixas.Where(x => x.EmpresaID == usuariologado.empresaId && x.ID == contaID).FirstOrDefault();
                    BancoCaixa bancoCaixa = new BancoCaixa();
                    bancoCaixa.ID = LibProdusys.GetNewCode("bancocaixa", "id", " empresaid = " + usuariologado.empresaId.ToString()); ;

                    bancoCaixa.PlanoContaID = Convert.ToInt32(financeiro.CategoriaID);
                    bancoCaixa.Tipo = financeiro.Tipo;
                    bancoCaixa.EmpresaID = Convert.ToInt32(usuariologado.empresaId);
                    bancoCaixa.DataEmissao = DateTime.Now;
                    if (chVencimento != null)
                    {
                        bancoCaixa.DataVencimento = Convert.ToDateTime(chVencimento);
                    }
                    else
                    {
                        bancoCaixa.DataVencimento = baixa.DataBaixa;
                    }

                    if (chNumero == "")
                    {
                        bancoCaixa.NumeroDocumento = financeiro.NumeroDocumento;
                    }
                    else
                    {
                        bancoCaixa.NumeroDocumento = chNumero;
                    }
                    bancoCaixa.Obs = "Liquidação do lançamento ID/Parcela: " + financeiro.IDParcela + " na data de " + Convert.ToString(financeiro.DataLiquidacao);
                    if (TpDocto == null)
                    {
                        bancoCaixa.TpDocto = 1;
                    }
                    else
                    {
                        bancoCaixa.TpDocto = Convert.ToInt32(TpDocto);
                    }
                    bancoCaixa.valor = Convert.ToDecimal(baixa.Total);
                    bancoCaixa.Agencia = chAgencia;
                    bancoCaixa.Banco = chBanco;
                    bancoCaixa.ContaID = conta.ID;
                    bancoCaixa.PessoaId = financeiro.PessoaID;
                    bancoCaixa.BaixaID = baixa.ID;
                    bancoCaixa.Conta = conta.NumeroConta;
                    db.BancoCaixa.Add(bancoCaixa);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                return View(financeiro);
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
