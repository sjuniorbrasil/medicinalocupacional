using SaudeWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace SaudeWeb.Controllers
{
    public class ReportController : Controller
    {
        DataContext db = new DataContext();
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RepCadastroFiltro()
        {
            ViewBag.UsuarioLogado = Utils.User.GetCookieUsuarioLogado(Request, Session);
            Utils.User.UsuarioLogado usuariologado = Utils.User.GetDadosUsuarioLogado(ViewBag.UsuarioLogado);
            ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;
            ViewBag.RuleAdmin = usuariologado.admin;
            ViewBag.RuleFinanceiro = usuariologado.RuleFinanceiro;
            ViewBag.RuleMovimentacao = usuariologado.RuleMovimentacao;
            ViewBag.RuleCadastro = usuariologado.RuleCadastro;
            ViewBag.TipoCadastro = new SelectList(new Consulta.ListaTipoCadastro().MetodoLista(), "ID", "Descricao");
            return View();
        }

        [HttpPost]
        public ActionResult RepCadastroFiltro(string txtCodcliente, string TipoCadastro)
        {
            ViewBag.TipoCadastro = new SelectList(new Consulta.ListaTipoCadastro().MetodoLista(), "ID", "Descricao");
            ViewBag.UsuarioLogado = Utils.User.GetCookieUsuarioLogado(Request, Session);
            if (!string.IsNullOrEmpty(ViewBag.UsuarioLogado))
            {
                Utils.User.UsuarioLogado usuariologado = Utils.User.GetDadosUsuarioLogado(ViewBag.UsuarioLogado);
                ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;
                ViewBag.RuleAdmin = usuariologado.admin;
                ViewBag.RuleFinanceiro = usuariologado.RuleFinanceiro;
                ViewBag.RuleMovimentacao = usuariologado.RuleMovimentacao;
                ViewBag.RuleCadastro = usuariologado.RuleCadastro;
                var pessoas = db.Pessoas.Include(p => p.cidade).Include(p => p.Empresa).Where(x => x.EmpresaID == usuariologado.empresaId);
                if (!string.IsNullOrEmpty(txtCodcliente))
                {
                    var cod = Convert.ToInt32(txtCodcliente);
                    pessoas = pessoas.Where(x => x.ID == cod);
                }
                if (!string.IsNullOrEmpty(TipoCadastro))
                {
                    if (TipoCadastro != "0")
                    {
                        var tp = Convert.ToInt32(TipoCadastro);
                        pessoas = pessoas.Where(x => x.TipoCadastro == tp);
                    }
                }
                return View("RepCadastro", pessoas.OrderBy(x => x.Razao).ToList());
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }

        }

        public ActionResult RepFuncionarioFiltro()
        {
            ViewBag.UsuarioLogado = Utils.User.GetCookieUsuarioLogado(Request, Session);
            Utils.User.UsuarioLogado usuariologado = Utils.User.GetDadosUsuarioLogado(ViewBag.UsuarioLogado);
            ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;
            ViewBag.RuleAdmin = usuariologado.admin;
            ViewBag.RuleFinanceiro = usuariologado.RuleFinanceiro;
            ViewBag.RuleMovimentacao = usuariologado.RuleMovimentacao;
            ViewBag.RuleCadastro = usuariologado.RuleCadastro;
            return View();
        }

        [HttpPost]
        public ActionResult RepFuncionarioFiltro(string txtCod, string DataNascimento, string txtClienteID)
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
                var funcionarios = db.Funcionarios.Include(p => p.cidade).Include(p => p.Empresa).Where(x => x.EmpresaID == usuariologado.empresaId);
                if (!string.IsNullOrEmpty(txtCod))
                {
                    var cod = Convert.ToInt32(txtCod);
                    funcionarios = funcionarios.Where(x => x.ID == cod);
                }
                if (!string.IsNullOrEmpty(DataNascimento))
                {

                    var dt = Convert.ToDateTime(DataNascimento);
                    funcionarios = funcionarios.Where(x => x.DataNascimento == dt);

                }
                if (!string.IsNullOrEmpty(txtClienteID))
                {
                    var cliente = Convert.ToInt32(txtClienteID);
                    funcionarios = funcionarios.Where(x => x.ClienteID == cliente);
                }
                return View("RepFuncionario", funcionarios.OrderBy(x => x.Nome).ToList());
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }

        }

        public ActionResult RepConsultaFiltro()
        {
            ViewBag.UsuarioLogado = Utils.User.GetCookieUsuarioLogado(Request, Session);
            Utils.User.UsuarioLogado usuariologado = Utils.User.GetDadosUsuarioLogado(ViewBag.UsuarioLogado);
            ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;
            ViewBag.RuleAdmin = usuariologado.admin;
            ViewBag.RuleFinanceiro = usuariologado.RuleFinanceiro;
            ViewBag.RuleMovimentacao = usuariologado.RuleMovimentacao;
            ViewBag.RuleCadastro = usuariologado.RuleCadastro;
            ViewBag.FormaPagamentoF = new SelectList(new Consulta.ListaFormaPag().MetodoFormaPgto(), "ID", "Descricao");
            ViewBag.SituacaoConsultaF = new SelectList(new Consulta.ListaSituacaoConsultaF().MetodoListaConsulta(), "ID", "Descricao");
            return View();
        }

        [HttpPost]
        public ActionResult RepConsultaFiltro(string txtCodcliente, string txtCodfuncioanrio, string txtdata, string txtdata1, int? SituacaoConsultaF, string txtCodmedico, int? FormaPagamentoF)
        {
            ViewBag.UsuarioLogado = Utils.User.GetCookieUsuarioLogado(Request, Session);
            if (!string.IsNullOrEmpty(ViewBag.UsuarioLogado))
            {
                ViewBag.FormaPagamentoF = new SelectList(new Consulta.ListaFormaPag().MetodoFormaPgto(), "ID", "Descricao");
                ViewBag.SituacaoConsultaF = new SelectList(new Consulta.ListaSituacaoConsultaF().MetodoListaConsulta(), "ID", "Descricao");
                Utils.User.UsuarioLogado usuariologado = Utils.User.GetDadosUsuarioLogado(ViewBag.UsuarioLogado);
                ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;
                ViewBag.RuleAdmin = usuariologado.admin;
                ViewBag.RuleFinanceiro = usuariologado.RuleFinanceiro;
                ViewBag.RuleMovimentacao = usuariologado.RuleMovimentacao;
                ViewBag.RuleCadastro = usuariologado.RuleCadastro;
                var consultas = new List<Consulta>();
                if ((!string.IsNullOrEmpty(txtCodcliente)) && (!string.IsNullOrEmpty(txtCodfuncioanrio)) && (!string.IsNullOrEmpty(txtdata)))
                {
                    if (SituacaoConsultaF == null)
                    {
                        consultas = db.Consultas.Include(c => c.Empresa).Where(x => x.EmpresaID == usuariologado.empresaId).ToList();
                    }
                    else
                    {
                        consultas = db.Consultas.Include(c => c.Empresa).Where(x => x.EmpresaID == usuariologado.empresaId && x.SituacaoConsulta == SituacaoConsultaF).ToList();
                    }

                }
                else
                {
                    consultas = db.Consultas.Include(c => c.Empresa).Where(x => x.EmpresaID == usuariologado.empresaId && x.PessoaId != null && x.FuncionarioId != null).ToList();
                    if (!string.IsNullOrEmpty(txtCodcliente))
                    {
                        var cliid = Convert.ToInt32(txtCodcliente);
                        consultas = consultas.Where(x => x.PessoaId == cliid).ToList();
                    }
                    if (!string.IsNullOrEmpty(txtCodfuncioanrio))
                    {
                        var funid = Convert.ToInt32(txtCodfuncioanrio);
                        consultas = consultas.Where(x => x.FuncionarioId == funid).ToList();
                    }
                    if (!string.IsNullOrEmpty(txtdata))
                    {
                        var data = Convert.ToDateTime(txtdata);
                        consultas = consultas.Where(x => x.DataConsulta >= data).ToList();
                    }
                    if (!string.IsNullOrEmpty(txtdata1))
                    {
                        var data = Convert.ToDateTime(txtdata1);
                        consultas = consultas.Where(x => x.DataConsulta <= data).ToList();
                    }
                    if (SituacaoConsultaF == null)
                    {
                        consultas = consultas.Where(x => x.SituacaoConsulta == 0).ToList();
                    }
                    if (!string.IsNullOrEmpty(txtCodmedico))
                    {
                        var cod = Convert.ToInt32(txtCodmedico);
                        consultas = consultas.Where(x => x.MedicoExaminadorId == cod).ToList();
                    }
                    if (SituacaoConsultaF != 10)
                    {
                        consultas = consultas.Where(x => x.SituacaoConsulta == SituacaoConsultaF).ToList();
                    }
                    if (FormaPagamentoF != 1000)
                    {
                        consultas = consultas.Where(x => x.FormaPagamento == FormaPagamentoF).ToList();
                    }


                }

                return View("RepConsulta", consultas);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        public ActionResult RepExamesComplementaresFiltro()
        {
            ViewBag.UsuarioLogado = Utils.User.GetCookieUsuarioLogado(Request, Session);
            Utils.User.UsuarioLogado usuariologado = Utils.User.GetDadosUsuarioLogado(ViewBag.UsuarioLogado);
            ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;
            ViewBag.RuleAdmin = usuariologado.admin;
            ViewBag.RuleFinanceiro = usuariologado.RuleFinanceiro;
            ViewBag.RuleMovimentacao = usuariologado.RuleMovimentacao;
            ViewBag.RuleCadastro = usuariologado.RuleCadastro;
            ViewBag.TipoConsulta = new SelectList(new Consulta.ListaTipoConsultaF().MetodoListaConsulta(), "ID", "Descricao");
            ViewBag.SituacaoConsultaF = new SelectList(new Consulta.ListaSituacaoConsultaF().MetodoListaConsulta(), "ID", "Descricao");
            ViewBag.FormaPagamentoF = new SelectList(new Consulta.ConsultaExame.ListaFormaPagF().MetodoFormaPgto(), "ID", "Descricao");
            ViewBag.SituacaoExame = new SelectList(new Consulta.ConsultaExame.ListaConclusaoExameF().MetodoConclusao(), "ID", "Descricao");
            return View();
        }

        [HttpPost]
        public ActionResult RepExamesComplementaresFiltro(string txtCodexame, string txtCodcliente, string txtCodfuncioanrio, string txtdata, string txtdata1, int? SituacaoConsultaF, int? FormaPagamentoF, string txtCodmedico, string txtCodEncaminhamento, int? TipoConsulta, int? SituacaoExame)
        {
            decimal somaExame = 0;
            ViewBag.FormaPagamentoF = new SelectList(new Consulta.ConsultaExame.ListaFormaPagF().MetodoFormaPgto(), "ID", "Descricao");
            ViewBag.SituacaoExame = new SelectList(new Consulta.ConsultaExame.ListaConclusaoExameF().MetodoConclusao(), "ID", "Descricao");
            ViewBag.SituacaoConsultaF = new SelectList(new Consulta.ListaSituacaoConsultaF().MetodoListaConsulta().OrderByDescending(x => x.ID), "ID", "Descricao");
            ViewBag.TipoConsulta = new SelectList(new Consulta.ListaTipoConsultaF().MetodoListaConsulta(), "ID", "Descricao");
            ViewBag.UsuarioLogado = Utils.User.GetCookieUsuarioLogado(Request, Session);
            if (!string.IsNullOrEmpty(ViewBag.UsuarioLogado))
            {
                ExamesCompViewModel exame = new ExamesCompViewModel();
                ViewBag.SituacaoConsultaF = new SelectList(new Consulta.ListaSituacaoConsultaF().MetodoListaConsulta(), "ID", "Descricao");
                Utils.User.UsuarioLogado usuariologado = Utils.User.GetDadosUsuarioLogado(ViewBag.UsuarioLogado);
                ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;
                ViewBag.RuleAdmin = usuariologado.admin;
                ViewBag.RuleFinanceiro = usuariologado.RuleFinanceiro;
                ViewBag.RuleMovimentacao = usuariologado.RuleMovimentacao;
                ViewBag.RuleCadastro = usuariologado.RuleCadastro;
                string situacaoExame = Convert.ToString(SituacaoExame);
                string situacaoConsulta = Convert.ToString(SituacaoConsultaF);
                string empresaCod = Convert.ToString(usuariologado.empresaId);
                string tipoConsulta = Convert.ToString(TipoConsulta);
                string formaPagamento = Convert.ToString(FormaPagamentoF);
                List<ExamesCompViewModel> exames = exame.ListaExames(empresaCod, txtCodcliente, txtCodmedico, txtCodEncaminhamento, txtdata, txtdata1, situacaoConsulta, tipoConsulta, formaPagamento, situacaoExame, "", txtCodfuncioanrio, txtCodexame);
                if (!string.IsNullOrEmpty(txtdata))
                {
                    var dt1 = Convert.ToDateTime(txtdata);
                    txtdata = dt1.ToShortDateString();
                }
                else
                {
                    txtdata = "  /  /    ";
                }
                if (!string.IsNullOrEmpty(txtdata1))
                {
                    var dt2 = Convert.ToDateTime(txtdata1);
                    txtdata1 = dt2.ToShortDateString();
                }
                else
                {
                    txtdata = "  /  /    ";
                }
                ViewBag.Periodo = "Período " + txtdata + " a " + txtdata1;

                foreach (var item in exames)
                {
                    somaExame = somaExame + Convert.ToDecimal(item.ValorExame);
                }
                ViewBag.SomaValor = somaExame;
                var pesquisa = new Consulta.ConsultaExame.ListaFormaPagF().MetodoFormaPgto().Find(x => x.ID == FormaPagamentoF);
                var situacao = new Consulta.ListaSituacaoConsultaF().MetodoListaConsulta().Find(x => x.ID == SituacaoConsultaF);
                var sitExame = new Consulta.ConsultaExame.ListaConclusaoExameF().MetodoConclusao().Find(x => x.ID == SituacaoExame);
                ViewBag.Pagamento = "Forma de Pagamento: " + pesquisa.Descricao;
                ViewBag.Situacao = "Situação da Consulta: " + situacao.Descricao;
                ViewBag.SitExame = "Conclusão do Exame: " + sitExame.Descricao;
                return View("RepExamesComplementares", exames);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        public ActionResult RepRecPagFiltro()
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
                ViewBag.Tipo = new SelectList(new Financeiro.TipoF().MetodoListaTipo(), "ID", "Descricao");
                return View();
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }
        [HttpPost]
        public ActionResult RepRecPagFiltro(string pessoaID, string dataEmissao1, string dataEmissao2, string dataVencimento1, string dataVencimento2, string dataLiquidacao1, string dataLiquidacao2, int? situacao, int? tipo, int? qtdeReg)
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
                ViewBag.Tipo = new SelectList(new Financeiro.TipoF().MetodoListaTipo(), "ID", "Descricao");
                var recPag = db.RecPag.Where(x => x.EmpresaID == usuariologado.empresaId);
                if (!string.IsNullOrEmpty(pessoaID))
                {
                    int pessoa = Convert.ToInt32(pessoaID);
                    recPag = recPag.Where(x => x.PessoaID == pessoa);
                    ViewBag.Cadastro = db.Pessoas.Where(x => x.ID == pessoa && x.EmpresaID == usuariologado.empresaId).FirstOrDefault();
                }
                if (!string.IsNullOrEmpty(dataEmissao1))
                {
                    DateTime dt = Convert.ToDateTime(dataEmissao1);
                    recPag = recPag.Where(x => x.DataEmissao >= dt);
                    dataEmissao1 = dt.ToShortDateString();

                }
                if (!string.IsNullOrEmpty(dataEmissao2))
                {
                    DateTime dt = Convert.ToDateTime(dataEmissao2);
                    recPag = recPag.Where(x => x.DataEmissao <= dt);
                    dataEmissao2 = dt.ToShortDateString();
                }
                if (!string.IsNullOrEmpty(dataVencimento1))
                {
                    DateTime dt = Convert.ToDateTime(dataVencimento1);
                    recPag = recPag.Where(x => x.DataVencimento >= dt);
                    dataVencimento1 = dt.ToShortDateString();
                }
                if (!string.IsNullOrEmpty(dataVencimento2))
                {

                    DateTime dt = Convert.ToDateTime(dataVencimento2);
                    recPag = recPag.Where(x => x.DataVencimento <= dt);
                    dataVencimento2 = dt.ToShortDateString();
                }
                if (!string.IsNullOrEmpty(dataLiquidacao1))
                {
                    DateTime dt = Convert.ToDateTime(dataLiquidacao1);
                    recPag = recPag.Where(x => x.DataLiquidacao >= dt);
                    dataLiquidacao1 = dt.ToShortDateString();
                }
                if (!string.IsNullOrEmpty(dataLiquidacao2))
                {
                    DateTime dt = Convert.ToDateTime(dataLiquidacao2);
                    recPag = recPag.Where(x => x.DataLiquidacao <= dt);
                    dataLiquidacao2 = dt.ToShortDateString();
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
                    ViewBag.Tipo = "RECEBIMENTOS";
                }
                if (tipo == 2)
                {
                    recPag = recPag.Where(x => x.Tipo == 2);
                    ViewBag.Tipo = "PAGAMENTOS";
                }
                if (tipo == 1000)
                {
                    ViewBag.Tipo = "TODOS";
                }


                if (situacao == 1000)
                {
                    ViewBag.Situacao = "TODOS";
                }
                else if (situacao == 0)
                {
                    ViewBag.Situacao = "PENDENTES";
                }
                else
                {
                    ViewBag.Situacao = "LIQUIDADOS";
                }
                
                ViewBag.Emissao = dataEmissao1 + " a " + dataEmissao2;
                ViewBag.Vencimento = dataVencimento1 + " a " + dataVencimento2;
                ViewBag.Liquidacao = dataLiquidacao1 + " a " + dataLiquidacao2;
                decimal receberPendente = 0;
                decimal pagarPendente = 0;
                decimal pago = 0;
                decimal recebido = 0;
                foreach (var item in recPag)
                {
                    if ((item.Tipo == 1) && (((item.ValorBaixado + item.Desconto) - (item.Juros + item.Multa)) < item.Valor) && (item.DataLiquidacao != null))
                    {
                        receberPendente = receberPendente + item.ValorPendente;
                    }
                    if ((item.Tipo == 2) && (((item.ValorBaixado + item.Desconto) - (item.Juros + item.Multa)) < item.Valor) && (item.DataLiquidacao != null))
                    {
                        pagarPendente = pagarPendente + item.ValorPendente;
                    }
                    if ((item.Tipo == 2) && (item.DataLiquidacao == null))
                    {
                        pagarPendente = pagarPendente + item.ValorPendente;
                    }
                    if ((item.Tipo == 1) && (item.DataLiquidacao == null))
                    {
                        receberPendente = receberPendente + item.ValorPendente;
                    }
                    if ((item.Tipo == 2) && (item.DataLiquidacao == null))
                    {
                        pagarPendente = pagarPendente + item.ValorPendente;
                    }
                    if ((item.Tipo == 2) && (item.DataLiquidacao != null))
                    {
                        pago = pago + item.ValorBaixado;
                    }
                    if ((item.Tipo == 1) && (item.DataLiquidacao != null))
                    {
                        recebido = recebido + item.ValorBaixado;
                    }
                }
                ViewBag.receberPendente = receberPendente.ToString("C", CultureInfo.CurrentCulture);
                ViewBag.pagarPendente = pagarPendente.ToString("C", CultureInfo.CurrentCulture);
                ViewBag.pago = pago.ToString("C", CultureInfo.CurrentCulture);
                ViewBag.recebido = recebido.ToString("C", CultureInfo.CurrentCulture);
                ViewBag.SaldoPendente = (receberPendente - pagarPendente).ToString("C", CultureInfo.CurrentCulture);
                ViewBag.SaldoLiquidado = (recebido - pago).ToString("C", CultureInfo.CurrentCulture);
                return View("RepRecPag", recPag.ToList());
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        public ActionResult RepGerencialPlanoContaFiltro()
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
                ViewBag.Tipo = new SelectList(new Financeiro.TipoF().MetodoListaTipo(), "ID", "Descricao");
                ViewBag.Apuracao = new SelectList(new BancoCaixa.ListaOpcao().MetodoListaOpcao(), "ID", "Descricao");


                return View();
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }


        [HttpPost]
        public ActionResult RepGerencialPlanoContaFiltro(int? categoriaid, int? Situacao, DateTime? Periodo1, DateTime? Periodo2, int? Apuracao)
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
                GerencialPlanoContaViewModel movimento = new GerencialPlanoContaViewModel();
                DateTime dt1;
                DateTime dt2;
                string periodo1 = "";
                string periodo2 = "";
                if (Periodo1 == null)
                {
                    dt1 = DateTime.Now.AddYears(-100);
                    periodo1 = "  /  /    ";
                }
                else
                {
                    periodo1 =  (Convert.ToDateTime(Periodo1).ToShortDateString());
                    dt1 = Convert.ToDateTime(Periodo1);
                }
                if (Periodo2 == null)
                {
                    dt2 = DateTime.Now.AddYears(+100);
                    periodo2 = "  /  /    ";
                }
                else
                {
                    dt2 = Convert.ToDateTime(Periodo2);
                    periodo2 = (Convert.ToDateTime(Periodo2).ToShortDateString());
                }
                ViewBag.Periodo = periodo1 + " a " + periodo2;
                string apuracao;
                if(Apuracao == 1)
                {
                    apuracao = "EMISSÃO";
                }
                else if(Apuracao == 2)
                {
                    apuracao = "VENCIMENTO";
                }
                else
                {
                    apuracao = "CONCILIAÇÃO";
                }
                ViewBag.Apuracao = apuracao;
                string situacao = "";
                if(Situacao == 1000)
                {
                    situacao = "TODOS";
                }
                else if(Situacao == 0)
                {
                    situacao = "PENDENTES";
                }
                else
                {
                    situacao = "LIQUIDADOS";
                }
                ViewBag.Situacao = situacao;


                List<GerencialPlanoContaViewModel> lista = movimento.ListaMovimento(usuariologado.empresaId.ToString(),
                                                                                    categoriaid.ToString(),
                                                                                    Situacao.ToString(),
                                                                                    dt1.ToString("yyyy-MM-dd"),
                                                                                    dt2.ToString("yyyy-MM-dd"),
                                                                                    Apuracao.ToString());

                
                List<GerencialPlanoContaViewModel> listaCompleta = new List<GerencialPlanoContaViewModel>();
                foreach (var item in lista)
                {
                    GerencialPlanoContaViewModel planoConta = new GerencialPlanoContaViewModel();
                    var plconta = db.PlanoContas.Where(x => x.EmpresaID == usuariologado.empresaId && x.ID == item.CategoriaID).FirstOrDefault();

                    
                    if (plconta.NivelSuperior != null)
                    {
                        if (listaCompleta.Where(x => x.CategoriaID == plconta.NivelSuperior).Count() == 0)
                        {
                            var plPai = db.PlanoContas.Where(x => x.EmpresaID == usuariologado.empresaId && x.ID == plconta.NivelSuperior).FirstOrDefault();
                            planoConta.CategoriaID = plPai.ID;
                            planoConta.Conta = plPai.Conta;
                            planoConta.Descricao = plPai.Descricao;
                            planoConta.TemNivelSuperior = false;
                            foreach (var item1 in lista)
                            {
                                if(item1.NivelSuperior == plPai.ID)
                                {                                    
                                    planoConta.DebCred = planoConta.DebCred + item1.DebCred;
                                }
                                
                            }
                            listaCompleta.Add(planoConta);
                            planoConta = new GerencialPlanoContaViewModel();
                        }
                    }
                    planoConta.CategoriaID = item.CategoriaID;
                    planoConta.Conta = item.Conta;
                    planoConta.Descricao = item.Descricao;
                    planoConta.TemNivelSuperior = true;
                    planoConta.DebCred = item.DebCred; 
                    listaCompleta.Add(planoConta);

                }


                return View("RepGerencialPlanoConta", listaCompleta);
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