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
using static SaudeWeb.Utils.LibProdusys;

namespace SaudeWeb.Controllers
{
    public class ConsultasController : Controller
    {
        private DataContext db = new DataContext();

        public ActionResult ImpAudiometria(int? id)
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
                var consulta = db.Consultas.Where(x => x.Id == id && x.EmpresaID == usuariologado.empresaId).FirstOrDefault();
                if (consulta != null)
                {
                    var empresa = db.Empresas.Find(usuariologado.empresaId);
                    DateTime dt = Convert.ToDateTime(consulta.DataConsulta);
                    ViewBag.Aparelho = empresa.Audiometro;
                    if(empresa.DataCalibracao != null)
                    {
                        DateTime dtC = Convert.ToDateTime(empresa.DataCalibracao);

                        ViewBag.DataCalibracao = dtC.ToShortDateString();
                    }
                    ViewBag.Repouso = empresa.RepousoAcustico;
                    ViewBag.DataExtenso = dt.ToLongDateString();
                    ViewBag.Data = dt.ToShortDateString();
                    ViewBag.Cliente = consulta.ClienteDesc;
                    ViewBag.Funcionario = consulta.funcionarioDesc;
                    ViewBag.Cidade = empresa.CidadeDesc;
                    ViewBag.Razao = empresa.Razao;
                    ViewBag.filialEndereco = empresa.Endereco + ", Nº " + empresa.Numero + ", " + empresa.Bairro;
                    ViewBag.filialCidade = "CEP: " + empresa.Cep + ", " + empresa.CidadeDesc;
                    var nomeArquivo = "Logo" + Convert.ToString(usuariologado.empresaId) + ".bmp";
                    ViewBag.Logo = nomeArquivo;
                    if (!string.IsNullOrEmpty(consulta.funcionarionasc))
                    {
                        ViewBag.Idade = Convert.ToString(LibProdusys.CalculaIdade(Convert.ToDateTime(consulta.funcionarionasc))) + " anos";
                    }
                    
                }
                return View(consulta);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        public ActionResult ImpFichaMedica(int? id)
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
                var consulta = db.Consultas.Where(x => x.Id == id && x.EmpresaID == usuariologado.empresaId).FirstOrDefault();
                if (consulta != null)
                {
                    var empresa = db.Empresas.Find(usuariologado.empresaId);
                    DateTime dt = Convert.ToDateTime(consulta.DataConsulta);
                    ViewBag.DataExtenso = dt.ToLongDateString();
                    ViewBag.Data = dt.ToShortDateString();
                    ViewBag.Cliente = consulta.ClienteDesc;
                    ViewBag.Funcionario = consulta.funcionarioDesc;
                    ViewBag.Cidade = empresa.CidadeDesc;
                    ViewBag.Razao = empresa.Razao;
                    ViewBag.filialEndereco = empresa.Endereco + ", Nº " + empresa.Numero + ", " + empresa.Bairro;
                    ViewBag.filialCidade = "CEP: " + empresa.Cep + ", " + empresa.CidadeDesc;
                    var nomeArquivo = "Logo" + Convert.ToString(usuariologado.empresaId) + ".bmp";
                    ViewBag.Logo = nomeArquivo;
                }
                return View(consulta);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }
        
        public ActionResult ImpDeclaracaoComp(int? id)
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
                var consulta = db.Consultas.Where(x => x.Id == id && x.EmpresaID == usuariologado.empresaId).FirstOrDefault();
                if (consulta != null)
                {
                    var empresa = db.Empresas.Find(usuariologado.empresaId);
                    DateTime dt = Convert.ToDateTime(consulta.DataConsulta);
                    ViewBag.DataExtenso = dt.ToLongDateString();
                    ViewBag.Cliente = consulta.ClienteDesc;
                    ViewBag.Funcionario = consulta.funcionarioDesc;
                    ViewBag.Cidade = empresa.CidadeDesc;
                    ViewBag.Razao = empresa.Razao;
                    ViewBag.filialEndereco = empresa.Endereco + ", Nº " + empresa.Numero + ", " + empresa.Bairro;
                    ViewBag.filialCidade = "CEP: " + empresa.Cep + ", " + empresa.CidadeDesc;
                    var nomeArquivo = "Logo" + Convert.ToString(usuariologado.empresaId) + ".bmp";
                    ViewBag.Logo = nomeArquivo;
                }    
                return View();
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        public ActionResult ImpReciboPag(int id)
        {
            ViewBag.UsuarioLogado = Utils.User.GetCookieUsuarioLogado(Request, Session);
            if (!string.IsNullOrEmpty(ViewBag.UsuarioLogado))
            {
                ViewBag.SituacaoConsultaF = new SelectList(new Consulta.ListaSituacaoConsultaF().MetodoListaConsulta(), "ID", "Descricao");
                Utils.User.UsuarioLogado usuariologado = Utils.User.GetDadosUsuarioLogado(ViewBag.UsuarioLogado);
                ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;
                ViewBag.RuleAdmin = usuariologado.admin;
                ViewBag.RuleFinanceiro = usuariologado.RuleFinanceiro;
                ViewBag.RuleMovimentacao = usuariologado.RuleMovimentacao;
                ViewBag.RuleCadastro = usuariologado.RuleCadastro;
                var exames = db.ConsultaExames.Where(x => x.ConsultaID == id && x.EmpresaID == usuariologado.empresaId).ToList();
                if (exames.Count > 0)
                {
                    var vlexamesAvista = db.ConsultaExames.Where(x => x.EmpresaID == usuariologado.empresaId
                                                                && x.ConsultaID == id
                                                                && x.FormaPgto == 0).Sum(x => x.ValorExame);
                    if (vlexamesAvista != null)
                    {
                        ViewBag.valor = vlexamesAvista.Value.ToString("N2");
                        ViewBag.vlExtenso = Conversor.EscreverExtenso(vlexamesAvista.Value);
                    }
                    else
                    {
                        ViewBag.vlExtenso = "";

                    }
                    ViewBag.DataExtenso = DateTime.Now.ToLongDateString();

                    string listaExames = "";
                    foreach (var item in exames)
                    {
                        if (listaExames == "")
                        {
                            listaExames = item.ExameDesc;
                        }
                        else
                        {
                            listaExames = listaExames + ", " + item.ExameDesc;
                        }

                    }
                    var consulta = db.Consultas.Find(id, usuariologado.empresaId);
                    var cliente = db.Pessoas.Where(x => x.ID == consulta.PessoaId && x.EmpresaID == usuariologado.empresaId).FirstOrDefault();
                    ViewBag.Cliente = cliente.Razao;
                    ViewBag.ClienteEndereco = cliente.Endereco + ", " + cliente.Numero + ", " + cliente.Bairro + ", " + cliente.CidadeDesc;
                    ViewBag.descExames = listaExames;
                    var filial = db.Empresas.Find(usuariologado.empresaId);
                    ViewBag.Controle = Convert.ToString(consulta.Id) + Convert.ToString(consulta.EmpresaID);
                    ViewBag.filialRazao = filial.Razao;
                    ViewBag.filialEndereco = filial.Endereco + ", Nº " + filial.Numero + ", " + filial.Bairro;
                    ViewBag.filialCidade = "CEP: " + filial.Cep + ", " + filial.CidadeDesc;
                    var nomeArquivo = "Logo" + Convert.ToString(usuariologado.empresaId) + ".bmp";
                    ViewBag.Logo = nomeArquivo;
                    return View(consulta);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        

        public ActionResult Index()
        {
            ViewBag.UsuarioLogado = Utils.User.GetCookieUsuarioLogado(Request, Session);
            if (!string.IsNullOrEmpty(ViewBag.UsuarioLogado))
            {               
                ViewBag.SituacaoConsultaF = new SelectList(new Consulta.ListaSituacaoConsultaF().MetodoListaConsulta(), "ID", "Descricao");
                Utils.User.UsuarioLogado usuariologado = Utils.User.GetDadosUsuarioLogado(ViewBag.UsuarioLogado);
                ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;
                ViewBag.RuleAdmin = usuariologado.admin;
                ViewBag.RuleFinanceiro = usuariologado.RuleFinanceiro;
                ViewBag.RuleMovimentacao = usuariologado.RuleMovimentacao;
                ViewBag.RuleCadastro = usuariologado.RuleCadastro;
                DateTime dtHora = new DateTime();
                dtHora = DateTime.Now.AddMinutes(-5);
                string deleteGarbage = "delete from consulta where garbage = 1 and empresaid ="+ usuariologado.empresaId.ToString() +"and dataconsulta <= '" + dtHora.ToString("yyyy-MM-dd hh:mm:ss")+"'";
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Properties.Settings.Default.Banco;
                SqlCommand cmd = new SqlCommand(deleteGarbage, con);
                cmd.CommandType = CommandType.Text;
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    con.Close();
                }
                List<Consulta> lista = new List<Consulta>();
                return View(lista);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        [HttpPost]
        public ActionResult Index(string txtCodcliente, string txtCodfuncioanrio, string txtdata, int? SituacaoConsultaF, int? qtdeReg)
        {
            ViewBag.UsuarioLogado = Utils.User.GetCookieUsuarioLogado(Request, Session);
            if (!string.IsNullOrEmpty(ViewBag.UsuarioLogado))
            {
                if (qtdeReg == null)
                {
                    qtdeReg = 10;
                }
                ViewBag.SituacaoConsultaF = new SelectList(new Consulta.ListaSituacaoConsultaF().MetodoListaConsulta(), "ID", "Descricao");
                Utils.User.UsuarioLogado usuariologado = Utils.User.GetDadosUsuarioLogado(ViewBag.UsuarioLogado);
                ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;
                ViewBag.RuleAdmin = usuariologado.admin;
                ViewBag.RuleFinanceiro = usuariologado.RuleFinanceiro;
                ViewBag.RuleMovimentacao = usuariologado.RuleMovimentacao;
                ViewBag.RuleCadastro = usuariologado.RuleCadastro;
                DateTime dtHora = new DateTime();
                dtHora = DateTime.Now.AddMinutes(-5);
                string deleteGarbage = "delete from consulta where garbage = 1 and empresaid =" + usuariologado.empresaId.ToString() + "and dataconsulta <= '" + dtHora.ToString("yyyy-MM-dd hh:mm:ss") + "'";
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Properties.Settings.Default.Banco;
                SqlCommand cmd = new SqlCommand(deleteGarbage, con);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {

                    throw;
                }

                cmd.CommandType = CommandType.Text;

                var consultas = new List<Consulta>();
                if ((!string.IsNullOrEmpty(txtCodcliente)) && (!string.IsNullOrEmpty(txtCodfuncioanrio)) && (!string.IsNullOrEmpty(txtdata)))
                {
                    if (SituacaoConsultaF == null)
                    {
                        consultas = db.Consultas.Include(c => c.Empresa).Where(x => x.EmpresaID == usuariologado.empresaId && x.SituacaoConsulta == 0).ToList();
                    }
                    else
                    {
                        consultas = db.Consultas.Include(c => c.Empresa).Where(x => x.EmpresaID == usuariologado.empresaId && x.SituacaoConsulta == SituacaoConsultaF).ToList();
                    }

                }
                else
                {
                    consultas = db.Consultas.Include(c => c.Empresa).Where(x => x.EmpresaID == usuariologado.empresaId).ToList();
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
                        consultas = consultas.Where(x => x.DataConsulta == data).ToList();
                    }
                    if (SituacaoConsultaF == null)
                    {
                        consultas = consultas.Where(x => x.SituacaoConsulta == 0).ToList();
                    }
                    else
                    {
                        if (SituacaoConsultaF != 10)
                        {
                            consultas = consultas.Where(x => x.SituacaoConsulta == SituacaoConsultaF).ToList();
                        }
                    }
                    if (qtdeReg == 10)
                    {
                        consultas = consultas.Take(10).ToList();
                    }
                    if (qtdeReg == 100)
                    {
                        consultas = consultas.Take(100).ToList();
                    }
                }
                return View(consultas);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        public ActionResult Create(int? Id)
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
                ViewBag.TemFinanceiro = false;
                if (Id != null)
                {
                    var consulta = db.Consultas.Find(Id, usuariologado.empresaId);                
                    if (consulta.FinanceiroID != null)
                    {
                        ViewBag.TemFinanceiro = true;
                    }
                    var funcionario = db.Funcionarios.Find(consulta.FuncionarioId, usuariologado.empresaId);
                    if (funcionario != null)
                    {
                        ViewBag.Sexo = new SelectList(new Pessoa.ListaSexo().MetodoListaSexo(), "ID", "Descricao", funcionario.Sexo);
                        ViewBag.EstadoCivil = new SelectList(new Funcionario.ListaEstadoCivil().MetodoListaEstadoCivil(), "ID", "Descricao", funcionario.EstadoCivil);
                    }
                    else
                    {
                        ViewBag.Sexo = new SelectList(new Pessoa.ListaSexo().MetodoListaSexo(), "ID", "Descricao");
                        ViewBag.EstadoCivil = new SelectList(new Funcionario.ListaEstadoCivil().MetodoListaEstadoCivil(), "ID", "Descricao");
                    }
                    var medico = db.Pessoas.Find(consulta.MedicoExaminadorId, usuariologado.empresaId);
                    if (medico != null)
                    {
                        ViewBag.MedicoExaminadorId = new SelectList(db.Pessoas.Where(x => x.EmpresaID == usuariologado.empresaId && x.TipoCadastro == 2 && x.Situacao == 0), "ID", "Razao", consulta.MedicoExaminadorId);
                    }
                    else
                    {
                        ViewBag.MedicoExaminadorId = new SelectList(db.Pessoas.Where(x => x.EmpresaID == usuariologado.empresaId && x.TipoCadastro == 2 && x.Situacao == 0), "ID", "Razao");
                    }
                    ViewBag.SituacaoConsulta = new SelectList(new Consulta.ListaSituacaoConsulta().MetodoListaConsulta(), "ID", "Descricao", consulta.SituacaoConsulta);

                    if (consulta.FormaPagamento != null)
                    {
                        ViewBag.FormaPagamento = new SelectList(new Pessoa.ListaFormaPgto().MetodoListaFormaPgto(), "ID", "Descricao", consulta.FormaPagamento);
                    }
                    else
                    {
                        ViewBag.FormaPagamento = new SelectList(new Pessoa.ListaFormaPgto().MetodoListaFormaPgto(), "ID", "Descricao");
                    }
                    if (consulta.FuncionarioId != null)
                    {
                        ViewBag.FuncionarioId = new SelectList(db.Funcionarios.Where(x => x.EmpresaID == usuariologado.empresaId && x.Situacao == 0), "ID", "Nome", consulta.FuncionarioId);
                    }
                    else
                    {
                        ViewBag.FuncionarioId = new SelectList(db.Funcionarios.Where(x => x.EmpresaID == usuariologado.empresaId && x.Situacao == 0), "ID", "Nome");
                    }
                    if (consulta.SetorId != null)
                    {
                        ViewBag.SetorId = new SelectList(db.Setor.Where(x => x.EmpresaID == usuariologado.empresaId), "Id", "Descricao", consulta.SetorId);
                    }
                    else
                    {
                        ViewBag.SetorId = new SelectList(db.Setor.Where(x => x.EmpresaID == usuariologado.empresaId), "Id", "Descricao");
                    }
                    if (consulta.FuncaoId != null)
                    {
                        ViewBag.FuncaoId = new SelectList(db.Funcoes.Where(x => x.EmpresaID == usuariologado.empresaId), "Id", "Descricao", consulta.FuncaoId);
                    }
                    else
                    {
                        ViewBag.FuncaoId = new SelectList(db.Funcoes.Where(x => x.EmpresaID == usuariologado.empresaId), "Id", "Descricao");
                    }
                    var agentes = db.ConsultaAgentes.Where(x => x.EmpresaID == usuariologado.empresaId && x.ConsultaID == consulta.Id).ToList();
                    if (agentes != null)
                    {
                        consulta.AgentesC = agentes;
                    }
                    else
                    {
                        consulta.AgentesC = new List<Consulta.ConsultaAgente>();
                    }                    
                    var examesComplementares = db.ConsultaExames.Where(x => x.EmpresaID == usuariologado.empresaId && x.ConsultaID == consulta.Id).Distinct().ToList();
                    if (examesComplementares != null)
                    {
                        consulta.ExamesC = examesComplementares;                        
                    }
                    else
                    {
                        consulta.ExamesC = new List<Consulta.ConsultaExame>();
                    }
                    
                    
                    ViewBag.TipoConsulta = new SelectList(new Consulta.ListaTipoConsulta().MetodoListaConsulta(), "ID", "Descricao", consulta.TipoConsulta);
                    ViewBag.ConclusaoDetal = new SelectList(new Consulta.ConsultaExame.ListaConclusaoExame().MetodoConclusao(), "ID", "Descricao");
                    ViewBag.PagamentoDetal = new SelectList(new Consulta.ConsultaExame.ListaFormaPagExame().MetodoFormaPgtoExame(), "ID", "Descricao");
                    ViewBag.EmpresaID = new SelectList(db.Empresas, "ID", "Razao", consulta.EmpresaID);

                    return View(consulta);
                }
                else
                {
                    var contador = db.Consultas.Count(x => x.EmpresaID == usuariologado.empresaId);
                    var consulta = 0;
                    if (contador > 0)
                    {
                        consulta = db.Consultas.Where(x => x.EmpresaID == usuariologado.empresaId).Max(x => x.Id) + 1;
                        ViewBag.Id = consulta;
                    }
                    else
                    {
                        consulta = 1;
                        ViewBag.Id = 1;
                    }
                    Consulta aso = new Consulta();                    
                    aso.AgentesC = new List<Consulta.ConsultaAgente>();
                    aso.ExamesC = new List<Consulta.ConsultaExame>();
                    aso.EmpresaID = Convert.ToInt32(usuariologado.empresaId);
                    aso.Id = consulta;
                    aso.DataConsulta = DateTime.Now;
                    aso.Garbage = 1;
                    try
                    {
                        db.Consultas.Add(aso);
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    ViewBag.TipoConsulta = new SelectList(new Consulta.ListaTipoConsulta().MetodoListaConsulta(), "ID", "Descricao");
                    ViewBag.ConclusaoDetal = new SelectList(new Consulta.ConsultaExame.ListaConclusaoExame().MetodoConclusao(), "ID", "Descricao");
                    ViewBag.PagamentoDetal = new SelectList(new Consulta.ConsultaExame.ListaFormaPagExame().MetodoFormaPgtoExame(), "ID", "Descricao");
                    ViewBag.SituacaoConsulta = new SelectList(new Consulta.ListaSituacaoConsulta().MetodoListaConsulta(), "ID", "Descricao");
                    ViewBag.FormaPagamento = new SelectList(new Pessoa.ListaFormaPgto().MetodoListaFormaPgto(), "ID", "Descricao");
                    ViewBag.Sexo = new SelectList(new Pessoa.ListaSexo().MetodoListaSexo(), "ID", "Descricao");
                    ViewBag.EstadoCivil = new SelectList(new Funcionario.ListaEstadoCivil().MetodoListaEstadoCivil(), "ID", "Descricao");
                    ViewBag.PessoaId = new SelectList(db.Pessoas.Where(x => x.EmpresaID == usuariologado.empresaId && x.TipoCadastro == 1 && x.Situacao == 0), "ID", "Razao");
                    ViewBag.MedicoExaminadorId = new SelectList(db.Pessoas.Where(x => x.EmpresaID == usuariologado.empresaId && x.TipoCadastro == 2 && x.Situacao == 0), "ID", "Razao");
                    ViewBag.FuncionarioId = new SelectList(db.Funcionarios.Where(x => x.EmpresaID == usuariologado.empresaId && x.Situacao == 0), "ID", "Nome");
                    ViewBag.SetorId = new SelectList(db.Setor.Where(x => x.EmpresaID == usuariologado.empresaId), "Id", "Descricao");
                    ViewBag.FuncaoId = new SelectList(db.Funcoes.Where(x => x.EmpresaID == usuariologado.empresaId), "Id", "Descricao");
                    return View(aso);
                }
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }
                
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EmpresaID,PessoaId,FuncionarioId,SetorId,FuncaoId,DataConsulta,DataConclusao,ProximaConsulta,UltimoEmprego,UltimaFuncaoExercida,TempoPermanenciaUltiMoEmprego,Peso,Altura,Temperatura,PressaoArterial,TempoAposUltimoEmprego,MedicoExaminadorId,MedicoLiberacaoId,EncaMinhamentoEspecialista,DataEncaminhado,SituacaoConsulta,ValorConsulta,FormaPagamento,Observacao,TipoConsulta,FinanceiroID")] Consulta consulta)
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
                Funcionario funcionario = new Funcionario();
                if (ModelState.IsValid)
                {
                    if (consulta.PessoaId != null && consulta.FuncionarioId != null)
                    {
                        consulta.EmpresaID = Convert.ToInt32(usuariologado.empresaId);
                        try
                        {
                            
                            var novaConsulta = db.Consultas.Count(x => x.Id == consulta.Id && x.EmpresaID == usuariologado.empresaId);
                            if(novaConsulta > 0)
                            {

                            }
                            else
                            {
                                var contadorId = db.Consultas.Count(x => x.EmpresaID == usuariologado.empresaId);
                                if (contadorId > 0)
                                {
                                    contadorId = db.Consultas.Where(x => x.EmpresaID == usuariologado.empresaId).Max(x => x.Id) + 1;
                                }
                                else
                                {
                                    contadorId = 1;
                                }
                                consulta.Id = contadorId;
                            }
                            if(consulta.DataConsulta == null)
                            {
                                consulta.DataConsulta = DateTime.Now;
                            }
                            consulta.Garbage = 0;
                            consulta.Altura = LibProdusys.FN(Convert.ToDecimal(consulta.Altura));
                            consulta.Observacao = LibProdusys.FS(consulta.Observacao);
                            consulta.Peso = LibProdusys.FN(Convert.ToDecimal(consulta.Peso));
                            consulta.PressaoArterial = LibProdusys.FS(consulta.PressaoArterial);
                            consulta.Temperatura = LibProdusys.FS(consulta.Temperatura);
                            consulta.TempoAposUltimoEmprego = LibProdusys.FS(consulta.TempoAposUltimoEmprego);
                            consulta.TempoPermanenciaUltiMoEmprego = LibProdusys.FS(consulta.TempoPermanenciaUltiMoEmprego);
                            consulta.UltimaFuncaoExercida = LibProdusys.FS(consulta.UltimaFuncaoExercida);
                            consulta.UltimoEmprego = LibProdusys.FS(consulta.UltimoEmprego);                            
                            ViewBag.TipoConsulta = new SelectList(new Consulta.ListaTipoConsulta().MetodoListaConsulta(), "ID", "Descricao", consulta.TipoConsulta);
                            ViewBag.SituacaoConsulta = new SelectList(new Consulta.ListaSituacaoConsulta().MetodoListaConsulta(), "ID", "Descricao", consulta.SituacaoConsulta);
                            ViewBag.FormaPagamento = new SelectList(new Pessoa.ListaFormaPgto().MetodoListaFormaPgto(), "ID", "Descricao", consulta.FormaPagamento);
                            ViewBag.Sexo = new SelectList(new Pessoa.ListaSexo().MetodoListaSexo(), "ID", "Descricao", funcionario.Sexo);
                            ViewBag.EstadoCivil = new SelectList(new Funcionario.ListaEstadoCivil().MetodoListaEstadoCivil(), "ID", "Descricao", funcionario.EstadoCivil);
                            ViewBag.PessoaId = new SelectList(db.Pessoas.Where(x => x.EmpresaID == usuariologado.empresaId && x.TipoCadastro == 1 && x.Situacao == 0), "ID", "Razao", consulta.PessoaId);
                            ViewBag.MedicoExaminadorId = new SelectList(db.Pessoas.Where(x => x.EmpresaID == usuariologado.empresaId && x.TipoCadastro == 2 && x.Situacao == 0), "ID", "Razao", consulta.MedicoExaminadorId);
                            ViewBag.FuncionarioId = new SelectList(db.Funcionarios.Where(x => x.EmpresaID == usuariologado.empresaId && x.Situacao == 0), "ID", "Nome", consulta.FuncionarioId);
                            ViewBag.SetorId = new SelectList(db.Setor.Where(x => x.EmpresaID == usuariologado.empresaId), "Id", "Descricao", consulta.SetorId);
                            ViewBag.FuncaoId = new SelectList(db.Funcoes.Where(x => x.EmpresaID == usuariologado.empresaId), "Id", "Descricao", consulta.FuncaoId);
                            ViewBag.EmpresaID = new SelectList(db.Empresas, "ID", "Razao", consulta.EmpresaID);
                            //db.Consultas.Add(consulta);
                            //db.SaveChanges();
                            //return RedirectToAction("Index");
                            if (consulta.SituacaoConsulta != 0)
                            {
                                if (consulta.DataConclusao == null)
                                {
                                    consulta.DataConclusao = DateTime.Now;
                                }
                            }
                            var vlexamesA = db.ConsultaExames.Where(x => x.EmpresaID == usuariologado.empresaId && x.ConsultaID == consulta.Id && x.FormaPgto == 0).Sum(x => x.ValorExame); // a vista
                            var vlexamesF = db.ConsultaExames.Where(x => x.EmpresaID == usuariologado.empresaId && x.ConsultaID == consulta.Id && x.FormaPgto == 1).Sum(x => x.ValorExame);// faturamento
                            var vinculo = db.Funcionarios.Find(consulta.FuncionarioId, usuariologado.empresaId);
                            if (consulta.TipoConsulta != 8)
                            {
                                if(vinculo != null)
                                {
                                    vinculo.ClienteID = Convert.ToInt32(consulta.PessoaId);
                                    db.Entry(vinculo).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                            else
                            {
                                if (vinculo != null)
                                {
                                    vinculo.ClienteID = null;
                                    db.Entry(vinculo).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                            if(vlexamesA == null)
                            {
                                vlexamesA = 0;
                            }
                            if(vlexamesF == null)
                            {
                                vlexamesF = 0;
                            }
                            consulta.ValorAvista = vlexamesA;
                            consulta.ValorFaturamento = vlexamesF;
                            consulta.ValorConsulta = vlexamesA + vlexamesF;
                            if(vlexamesF > 0)
                            {
                                consulta.Fatura = 1;
                            }
                            else
                            {
                                consulta.Fatura = 0;
                            }
                            if (novaConsulta > 0)
                            {
                                db.Entry(consulta).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            else
                            {
                                db.Consultas.Add(consulta);
                                db.SaveChanges();
                            }
                            var fin = db.RecPag.Where(x => x.EmpresaID == usuariologado.empresaId && x.ConsultaID == consulta.Id).FirstOrDefault();
                            if (fin == null)
                            {
                                if (consulta.ValorAvista > 0)
                                {
                                    var empresa = db.Empresas.Find(usuariologado.empresaId);
                                    var observacao = "";
                                    var planoConta = Convert.ToInt32(empresa.PlanoContaConsulta);
                                    var plconta = db.PlanoContas.Where(x => x.ID == planoConta && x.EmpresaID == usuariologado.empresaId).FirstOrDefault();
                                    if (plconta != null)
                                    {
                                        observacao = plconta.Conta + " - " + plconta.Descricao;
                                    }

                                    Financeiro financeiro = new Financeiro();
                                    var contador2 = db.RecPag.Count(x => x.EmpresaID == usuariologado.empresaId);
                                    if (contador2 > 0)
                                    {
                                        var ultimo = db.RecPag.Where(x => x.EmpresaID == usuariologado.empresaId).Max(x => x.ID);
                                        financeiro.ID = ultimo + 1;
                                    }
                                    else
                                    {
                                        financeiro.ID = 1;
                                    }
                                    financeiro.ConsultaID = consulta.Id;
                                    financeiro.Competencia = Convert.ToDateTime(consulta.DataConsulta).ToString("yyyy-MM");
                                    financeiro.Juros = 0;
                                    financeiro.Multa = 0;
                                    financeiro.Desconto = 0;
                                    financeiro.ParcelaID = 1;
                                    financeiro.PessoaID = Convert.ToInt32(consulta.PessoaId);
                                    financeiro.Tipo = 1;
                                    financeiro.Valor = Convert.ToDecimal(consulta.ValorAvista);
                                    financeiro.ValorBaixado = Convert.ToDecimal(consulta.ValorAvista);
                                    financeiro.CategoriaID = empresa.PlanoContaConsulta;
                                    financeiro.DataEmissao = Convert.ToDateTime(consulta.DataConsulta);
                                    financeiro.DataLiquidacao = DateTime.Now;
                                    financeiro.DataVencimento = DateTime.Now;
                                    financeiro.EmpresaID = Convert.ToInt32(usuariologado.empresaId);
                                    financeiro.NumeroDocumento = "ConsultaID-" + Convert.ToString(consulta.Id);
                                    financeiro.Observacao = LibProdusys.FS(observacao);
                                    db.RecPag.Add(financeiro);
                                    db.SaveChanges();

                                    var consultaUp = db.Consultas.Where(x => x.EmpresaID == usuariologado.empresaId && x.Id == consulta.Id).FirstOrDefault();
                                    if(consultaUp != null)
                                    {
                                        consultaUp.FinanceiroID = financeiro.ID;
                                        db.Entry(consultaUp).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }

                                    //baixa
                                    Baixa baixa = new Baixa();
                                    var contador = db.Baixas.Count(x => x.EmpresaID == usuariologado.empresaId);
                                    if (contador > 0)
                                    {
                                        var ultimo = db.Baixas.Where(x => x.EmpresaID == usuariologado.empresaId).Max(x => x.ID);
                                        baixa.ID = ultimo + 1;
                                    }
                                    else
                                    {
                                        baixa.ID = 1;
                                    }

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
                                    var conta = db.ContaCorrenteCaixas.Where(x => x.EmpresaID == usuariologado.empresaId && x.ContaPadrao == 1).FirstOrDefault();
                                    BancoCaixa bancoCaixa = new BancoCaixa();
                                    var contadorB = db.BancoCaixa.Count(x => x.EmpresaID == usuariologado.empresaId);
                                    if (contadorB > 0)
                                    {
                                        var ultimo = db.BancoCaixa.Where(x => x.EmpresaID == usuariologado.empresaId).Max(x => x.ID);
                                        bancoCaixa.ID = ultimo + 1;
                                    }
                                    else
                                    {
                                        bancoCaixa.ID = 1;
                                    }
                                    bancoCaixa.Tipo = 1;
                                    bancoCaixa.TpDocto = 1;
                                    bancoCaixa.valor = Convert.ToDecimal(baixa.Valor);
                                    bancoCaixa.EmpresaID = Convert.ToInt32(usuariologado.empresaId);
                                    bancoCaixa.PessoaId = consulta.PessoaId;
                                    bancoCaixa.DataEmissao = DateTime.Now;
                                    bancoCaixa.DataVencimento = DateTime.Now;
                                    bancoCaixa.Obs = LibProdusys.FS(observacao);
                                    bancoCaixa.Agencia = conta.Agencia;
                                    bancoCaixa.Banco = Convert.ToString(conta.Banco);
                                    bancoCaixa.ContaID = conta.ID;
                                    bancoCaixa.BaixaID = baixa.ID;
                                    bancoCaixa.Conta = conta.NumeroConta;
                                    db.BancoCaixa.Add(bancoCaixa);
                                    db.SaveChanges();

                                }
                            }                            
                            return RedirectToAction("Index");
                        }
                        catch (Exception e)
                        {
                            ViewBag.erro = e.ToString();
                            throw;
                        }
                    }
                    else
                    {
                        var agentes = db.ConsultaAgentes.Where(x => x.EmpresaID == usuariologado.empresaId && x.ConsultaID == consulta.Id).ToList();
                        if (agentes != null)
                        {
                            consulta.AgentesC = agentes;
                        }
                        else
                        {
                            consulta.AgentesC = new List<Consulta.ConsultaAgente>();
                        }

                        var examesComplementares = db.ConsultaExames.Where(x => x.ConsultaID == consulta.Id && x.EmpresaID == consulta.EmpresaID).ToList();
                        if (examesComplementares != null)
                        {
                            consulta.ExamesC = examesComplementares;
                        }
                        else
                        {
                            consulta.ExamesC = new List<Consulta.ConsultaExame>();
                        }
                        ViewBag.ConclusaoDetal = new SelectList(new Consulta.ConsultaExame.ListaConclusaoExame().MetodoConclusao(), "ID", "Descricao");
                        ViewBag.TipoConsulta = new SelectList(new Consulta.ListaTipoConsulta().MetodoListaConsulta(), "ID", "Descricao", consulta.TipoConsulta);
                        ViewBag.SituacaoConsulta = new SelectList(new Consulta.ListaSituacaoConsulta().MetodoListaConsulta(), "ID", "Descricao", consulta.SituacaoConsulta);
                        ViewBag.FormaPagamento = new SelectList(new Pessoa.ListaFormaPgto().MetodoListaFormaPgto(), "ID", "Descricao", consulta.FormaPagamento);
                        ViewBag.Sexo = new SelectList(new Pessoa.ListaSexo().MetodoListaSexo(), "ID", "Descricao", funcionario.Sexo);
                        ViewBag.EstadoCivil = new SelectList(new Funcionario.ListaEstadoCivil().MetodoListaEstadoCivil(), "ID", "Descricao", funcionario.EstadoCivil);
                        ViewBag.PessoaId = new SelectList(db.Pessoas.Where(x => x.EmpresaID == usuariologado.empresaId && x.TipoCadastro == 1 && x.Situacao == 0), "ID", "Razao", consulta.PessoaId);
                        ViewBag.MedicoExaminadorId = new SelectList(db.Pessoas.Where(x => x.EmpresaID == usuariologado.empresaId && x.TipoCadastro == 2 && x.Situacao == 0), "ID", "Razao", consulta.MedicoExaminadorId);
                        ViewBag.FuncionarioId = new SelectList(db.Funcionarios.Where(x => x.EmpresaID == usuariologado.empresaId && x.Situacao == 0), "ID", "Nome", consulta.FuncionarioId);
                        ViewBag.SetorId = new SelectList(db.Setor.Where(x => x.EmpresaID == usuariologado.empresaId), "Id", "Descricao", consulta.SetorId);
                        ViewBag.FuncaoId = new SelectList(db.Funcoes.Where(x => x.EmpresaID == usuariologado.empresaId), "Id", "Descricao", consulta.FuncaoId);
                        ViewBag.PagamentoDetal = new SelectList(new Consulta.ConsultaExame.ListaFormaPagExame().MetodoFormaPgtoExame(), "ID", "Descricao");
                        ViewBag.erro = "Há campos obrigatórios sem preenchimento! ";
                        return View(consulta);                        
                    }
                }
                ViewBag.PagamentoDetal = new SelectList(new Consulta.ConsultaExame.ListaFormaPagExame().MetodoFormaPgtoExame(), "ID", "Descricao");
                ViewBag.TipoConsulta = new SelectList(new Consulta.ListaTipoConsulta().MetodoListaConsulta(), "ID", "Descricao", consulta.TipoConsulta);
                ViewBag.SituacaoConsulta = new SelectList(new Consulta.ListaSituacaoConsulta().MetodoListaConsulta(), "ID", "Descricao", consulta.SituacaoConsulta);
                ViewBag.FormaPagamento = new SelectList(new Pessoa.ListaFormaPgto().MetodoListaFormaPgto(), "ID", "Descricao", consulta.FormaPagamento);
                ViewBag.Sexo = new SelectList(new Pessoa.ListaSexo().MetodoListaSexo(), "ID", "Descricao", funcionario.Sexo);
                ViewBag.EstadoCivil = new SelectList(new Funcionario.ListaEstadoCivil().MetodoListaEstadoCivil(), "ID", "Descricao", funcionario.EstadoCivil);
                ViewBag.PessoaId = new SelectList(db.Pessoas.Where(x => x.EmpresaID == usuariologado.empresaId && x.TipoCadastro == 1 && x.Situacao == 0), "ID", "Razao", consulta.PessoaId);
                ViewBag.MedicoExaminadorId = new SelectList(db.Pessoas.Where(x => x.EmpresaID == usuariologado.empresaId && x.TipoCadastro == 2 && x.Situacao == 0), "ID", "Razao", consulta.MedicoExaminadorId);
                ViewBag.FuncionarioId = new SelectList(db.Funcionarios.Where(x => x.EmpresaID == usuariologado.empresaId && x.Situacao == 0), "ID", "Nome", consulta.FuncionarioId);
                ViewBag.SetorId = new SelectList(db.Setor.Where(x => x.EmpresaID == usuariologado.empresaId), "Id", "Descricao", consulta.SetorId);
                ViewBag.FuncaoId = new SelectList(db.Funcoes.Where(x => x.EmpresaID == usuariologado.empresaId), "Id", "Descricao", consulta.FuncaoId);
                ViewBag.EmpresaID = new SelectList(db.Empresas, "ID", "Razao", consulta.EmpresaID);
                return View(consulta);
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
                Consulta consulta = db.Consultas.Find(id, usuariologado.empresaId);
                if (consulta == null)
                {
                    return HttpNotFound();
                }
                return View(consulta);
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
                Consulta consulta = db.Consultas.Find(id, usuariologado.empresaId);                
                db.Consultas.Remove(consulta);
                db.SaveChanges();
                Consulta.ExlcuirRelacionados(Convert.ToString(id), Convert.ToString(usuariologado.empresaId));
                return RedirectToAction("Index");
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        [HttpPost]
        public ActionResult addAgente(string IDdaConsulta, string ConsultaIdAjax, string PessoaId1, string FuncionarioId1, string SetorId1, string FuncaoId1,
                                      string DataConsulta1, string DataConclusao1, string ProximaConsulta1, string UltimoEmprego1, string UltimaFuncaoExercida1,
                                      string TempoPermanenciaUltiMoEmprego1, string Peso1, string Altura1, string Temperatura1, string PressaoArterial1,
                                      string TempoAposUltimoEmprego1, string MedicoExaminadorId1, string EncaMinhamentoEspecialista1, string DataEncaminhado1,
                                      string SituacaoConsulta1, string FormaPagamento1, string Observacao1, string tpConsultaAgente)
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
                int consultaId = Convert.ToInt32(IDdaConsulta);
                int agenteId = Convert.ToInt32(ConsultaIdAjax);
                SqlConnection con = new SqlConnection(Properties.Settings.Default.Banco);
                var insert = " Delete from consultaagente where consultaid = " + IDdaConsulta + "and Empresaid = " + usuariologado.empresaId + " and agenteid = " + agenteId + "; "
                           + " Insert into consultaagente(consultaid, agenteid, empresaid) values(@consultaid, @agenteid, @empresaid)";
                SqlCommand cmd = new SqlCommand(insert, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@consultaid", SqlDbType.Int).Value = Convert.ToInt32(consultaId);
                cmd.Parameters.Add("@agenteid", SqlDbType.Int).Value = Convert.ToInt32(ConsultaIdAjax);
                cmd.Parameters.Add("@empresaid", SqlDbType.Int).Value = usuariologado.empresaId;
                con.Open();
                try
                {
                    int i = cmd.ExecuteNonQuery();
                    if (i > 0)
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
                var novaConsulta = db.Consultas.Count(x => x.Id == consultaId && x.EmpresaID == usuariologado.empresaId);
                var consulta = db.Consultas.Where(x => x.EmpresaID == usuariologado.empresaId && x.Id == consultaId).FirstOrDefault();
                if (novaConsulta > 0)
                {
                    
                }
                else
                {

                    var contadorId = db.Consultas.Count(x => x.EmpresaID == usuariologado.empresaId);
                    if (contadorId > 0)
                    {
                        contadorId = db.Consultas.Where(x => x.EmpresaID == usuariologado.empresaId).Max(x => x.Id) + 1;
                    }
                    else
                    {
                        contadorId = 1;
                    }       
                    if(consulta == null)
                    {
                        consulta = new Consulta();
                    }
                    consulta.Id = contadorId;
                    consulta.EmpresaID = Convert.ToInt32(usuariologado.empresaId);
                }
                
                if (!string.IsNullOrEmpty(PessoaId1))
                {
                    consulta.PessoaId = Convert.ToInt32(PessoaId1);
                }
                if (!string.IsNullOrEmpty(FuncionarioId1))
                {
                    consulta.FuncionarioId = Convert.ToInt32(FuncionarioId1);
                }
                if (!string.IsNullOrEmpty(SetorId1))
                {
                    consulta.SetorId = Convert.ToInt32(SetorId1);
                }
                if (!string.IsNullOrEmpty(FuncaoId1))
                {
                    consulta.FuncaoId = Convert.ToInt32(FuncaoId1);
                }
                if (!string.IsNullOrEmpty(DataConsulta1))
                {
                    consulta.DataConsulta = Convert.ToDateTime(DataConsulta1);
                }
                else
                {
                    consulta.DataConsulta = DateTime.Now;
                }                
                if (!string.IsNullOrEmpty(DataConclusao1))
                {
                    consulta.DataConclusao = Convert.ToDateTime(DataConclusao1);
                }
                if (!string.IsNullOrEmpty(ProximaConsulta1))
                {
                    consulta.ProximaConsulta = Convert.ToDateTime(ProximaConsulta1);
                }
                consulta.Garbage = 0;
                consulta.TipoConsulta = Convert.ToInt32(tpConsultaAgente);
                consulta.UltimoEmprego = UltimoEmprego1;
                consulta.UltimaFuncaoExercida = UltimaFuncaoExercida1;
                consulta.TempoPermanenciaUltiMoEmprego = TempoPermanenciaUltiMoEmprego1;
                consulta.Peso = Peso1;
                consulta.Altura = Altura1;
                consulta.Temperatura = Temperatura1;
                consulta.PressaoArterial = PressaoArterial1;
                consulta.TempoAposUltimoEmprego = TempoAposUltimoEmprego1;
                if (!string.IsNullOrEmpty(MedicoExaminadorId1))
                {
                    consulta.MedicoExaminadorId = Convert.ToInt32(MedicoExaminadorId1);
                }
                consulta.EncaMinhamentoEspecialista = EncaMinhamentoEspecialista1;
                if (!string.IsNullOrEmpty(DataEncaminhado1))
                {
                    consulta.DataEncaminhado = Convert.ToDateTime(DataEncaminhado1);
                }
                if (!string.IsNullOrEmpty(SituacaoConsulta1))
                {
                    consulta.SituacaoConsulta = Convert.ToInt32(SituacaoConsulta1);
                    if(consulta.SituacaoConsulta == 1)
                    {
                        if(consulta.DataConclusao == null)
                        {
                            consulta.DataConclusao = DateTime.Now;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(FormaPagamento1))
                {
                    consulta.FormaPagamento = Convert.ToInt32(FormaPagamento1);
                }
                consulta.Observacao = Observacao1;
                if(novaConsulta > 0)
                {
                    db.Entry(consulta).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    db.Consultas.Add(consulta);
                    db.SaveChanges();
                }
                
                //var teste = db.ConsultaAgentes.Where(x => x.EmpresaID == usuariologado.empresaId && x.ConsultaID == consultaId && x.AgenteId == agenteId).ToList();
                //consulta.AgentesC = teste;
                return RedirectToAction("/Create/" + consulta.Id);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        [HttpPost]
        public ActionResult addExame(string IDdaConsulta2, string PessoaId12, string FuncionarioId12, string SetorId12, string FuncaoId12,
                                      string DataConsulta12, string DataConclusao12, string ProximaConsulta12, string UltimoEmprego12, string UltimaFuncaoExercida12,
                                      string TempoPermanenciaUltiMoEmprego12, string Peso12, string Altura12, string Temperatura12, string PressaoArterial12,
                                      string TempoAposUltimoEmprego12, string MedicoExaminadorId12, string EncaMinhamentoEspecialista12, string DataEncaminhado12,
                                      string SituacaoConsulta12, string FormaPagamento12, string Observacao12, string ExameIdDetal, string MedicoExameId,
                                      string EncExameID, string DataColetaDetal, string DataEmissaoDetal, string PagamentoDetal, string ConclusaoDetal,
                                      string ObservacaoConsultaDetal, string ProximaConsultaDetal, string tpConsultaExame)
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
                int consultaId = Convert.ToInt32(IDdaConsulta2);
                int exameId = Convert.ToInt32(ExameIdDetal);
                SqlConnection con = new SqlConnection(Properties.Settings.Default.Banco);
                var insert = "Delete from consultaexame where consultaid = "+ IDdaConsulta2 + "and Empresaid = "+ usuariologado.empresaId + " and exameid = "+ exameId + "; "
                            + " Insert into consultaexame(consultaid, EmpresaID, ExameID, pessoaId, medicoId, ValorExame, Formapgto, datacoleta, dataemissao, proximaconsulta, situacaoexame, observacao, FaturaPara, FaturadoMedico, RepasseMedico)"
                            + "                 values(@consultaid, @EmpresaID, @ExameID, @pessoaId, @medicoId, @ValorExame, @Formapgto, @datacoleta, @dataemissao, @proximaconsulta, @situacaoexame, @observacao, @FaturaPara, @FaturadoMedico, @RepasseMedico)";
                SqlCommand cmd = new SqlCommand(insert, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@consultaid", SqlDbType.Int).Value = Convert.ToInt32(consultaId);
                cmd.Parameters.Add("@exameid", SqlDbType.Int).Value = Convert.ToInt32(exameId);
                cmd.Parameters.Add("@empresaid", SqlDbType.Int).Value = usuariologado.empresaId;
                if (!string.IsNullOrEmpty(EncExameID))
                {
                    cmd.Parameters.Add("@pessoaid", SqlDbType.Int).Value = Convert.ToInt32(EncExameID); //encaminhamento
                    
                }
                else
                {
                    cmd.Parameters.Add("@pessoaid", SqlDbType.Int).Value = -1; //encaminhamento
                }
                if (!string.IsNullOrEmpty(MedicoExameId))
                {
                    cmd.Parameters.Add("@medicoid", SqlDbType.Int).Value = Convert.ToInt32(MedicoExameId);
                    var medico = Convert.ToInt32(MedicoExameId);
                    var medicoDetal = db.Pessoas.Where(x => x.EmpresaID == usuariologado.empresaId && x.ID == medico).FirstOrDefault(); 
                    if(medicoDetal != null)
                    {
                        if(medicoDetal.RepasseProporcional == 1)
                        {
                            cmd.Parameters.Add("@RepasseMedico", SqlDbType.Int).Value = 1;
                            cmd.Parameters.Add("@FaturadoMedico", SqlDbType.Int).Value = 0;
                        }
                        else
                        {
                            cmd.Parameters.Add("@RepasseMedico", SqlDbType.Int).Value = 0;
                            cmd.Parameters.Add("@FaturadoMedico", SqlDbType.Int).Value = 0;
                        }
                    }
                    else
                    {
                        cmd.Parameters.Add("@RepasseMedico", SqlDbType.Int).Value = 0;
                        cmd.Parameters.Add("@FaturadoMedico", SqlDbType.Int).Value = 0;
                    }                    
                }
                else
                {                    
                    cmd.Parameters.Add("@medicoid", SqlDbType.Int).Value = -1;
                    cmd.Parameters.Add("@RepasseMedico", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@FaturadoMedico", SqlDbType.Int).Value = 0;
                }
                if (Convert.ToInt32(PagamentoDetal) != 2)//  2 = Não cobrar
                {
                    var empresaid = Convert.ToString(usuariologado.empresaId);
                    
                    cmd.Parameters.Add("@valorExame", SqlDbType.Decimal).Value = TabelaPreco.GetValorExame(empresaid, ExameIdDetal, PessoaId12);                      
                }
                else
                {
                    cmd.Parameters.Add("@valorExame", SqlDbType.Decimal).Value = 0;
                }
                cmd.Parameters.Add("@formapgto", SqlDbType.Int).Value = Convert.ToInt32(PagamentoDetal);
                if (!string.IsNullOrEmpty(DataColetaDetal))
                {
                    cmd.Parameters.Add("@datacoleta", SqlDbType.DateTime).Value = Convert.ToDateTime(DataColetaDetal);
                }
                else
                {                    
                    cmd.Parameters.Add("@datacoleta", SqlDbType.DateTime).Value = DateTime.Now;
                }

                if (!string.IsNullOrEmpty(DataEmissaoDetal))
                {
                    cmd.Parameters.Add("@dataemissao", SqlDbType.DateTime).Value = Convert.ToDateTime(DataEmissaoDetal);
                }
                else
                {                    
                    cmd.Parameters.Add("@dataemissao", SqlDbType.DateTime).Value = DateTime.Now;
                }

                if (!string.IsNullOrEmpty(ProximaConsultaDetal))
                {
                    cmd.Parameters.Add("@proximaconsulta", SqlDbType.DateTime).Value = Convert.ToDateTime(ProximaConsultaDetal);
                }
                else
                {                   
                    cmd.Parameters.Add("@proximaconsulta", SqlDbType.DateTime).Value = DBNull.Value;
                }
                cmd.Parameters.Add("@situacaoexame", SqlDbType.Int).Value = Convert.ToInt32(ConclusaoDetal);
                cmd.Parameters.Add("@observacao", SqlDbType.VarChar).Value = ObservacaoConsultaDetal;
                cmd.Parameters.Add("@FaturaPara", SqlDbType.Int).Value = 1;// 1 - cliente 2 - convenio
                con.Open();
                try
                {
                    int i = cmd.ExecuteNonQuery();
                    if (i > 0)
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
                var novaConsulta = db.Consultas.Count(x => x.Id == consultaId && x.EmpresaID == usuariologado.empresaId);
                var consulta = db.Consultas.Where(x => x.EmpresaID == usuariologado.empresaId && x.Id == consultaId).FirstOrDefault();
                if (novaConsulta > 0)
                {

                }
                else
                {
                    var contadorId = db.Consultas.Count(x => x.EmpresaID == usuariologado.empresaId);
                    if (contadorId > 0)
                    {
                        contadorId = db.Consultas.Where(x => x.EmpresaID == usuariologado.empresaId).Max(x => x.Id) + 1;
                    }
                    else
                    {
                        contadorId = 1;
                    }
                    if(consulta == null)
                    {
                        consulta = new Consulta();
                    }
                    consulta.Id = contadorId;
                    consulta.EmpresaID = Convert.ToInt32(usuariologado.empresaId);
                }
                if (!string.IsNullOrEmpty(PessoaId12))
                {
                    consulta.PessoaId = Convert.ToInt32(PessoaId12);
                }
                if (!string.IsNullOrEmpty(FuncionarioId12))
                {
                    consulta.FuncionarioId = Convert.ToInt32(FuncionarioId12);
                }
                if (!string.IsNullOrEmpty(SetorId12))
                {
                    consulta.SetorId = Convert.ToInt32(SetorId12);
                }
                if (!string.IsNullOrEmpty(FuncaoId12))
                {
                    consulta.FuncaoId = Convert.ToInt32(FuncaoId12);
                }
                if (!string.IsNullOrEmpty(DataConsulta12))
                {
                    consulta.DataConsulta = Convert.ToDateTime(DataConsulta12);
                }
                else
                {
                    consulta.DataConsulta = DateTime.Now;
                }
                if (!string.IsNullOrEmpty(DataConclusao12))
                {
                    consulta.DataConclusao = Convert.ToDateTime(DataConclusao12);
                }
                if (!string.IsNullOrEmpty(ProximaConsulta12))
                {
                    consulta.ProximaConsulta = Convert.ToDateTime(ProximaConsulta12);
                }
                consulta.Garbage = 0;
                consulta.TipoConsulta = Convert.ToInt32(tpConsultaExame);
                consulta.UltimoEmprego = UltimoEmprego12;
                consulta.UltimaFuncaoExercida = UltimaFuncaoExercida12;
                consulta.TempoPermanenciaUltiMoEmprego = TempoPermanenciaUltiMoEmprego12;
                consulta.Peso = Peso12;
                consulta.Altura = Altura12;
                consulta.Temperatura = Temperatura12;
                consulta.PressaoArterial = PressaoArterial12;
                consulta.TempoAposUltimoEmprego = TempoAposUltimoEmprego12;
                if (!string.IsNullOrEmpty(MedicoExaminadorId12))
                {
                    consulta.MedicoExaminadorId = Convert.ToInt32(MedicoExaminadorId12);
                }
                consulta.EncaMinhamentoEspecialista = EncaMinhamentoEspecialista12;
                if (!string.IsNullOrEmpty(DataEncaminhado12))
                {
                    consulta.DataEncaminhado = Convert.ToDateTime(DataEncaminhado12);
                }
                if (!string.IsNullOrEmpty(SituacaoConsulta12))
                {
                    consulta.SituacaoConsulta = Convert.ToInt32(SituacaoConsulta12);
                    if(consulta.SituacaoConsulta == 1)
                    {
                        if(consulta.DataConclusao == null)
                        {
                            consulta.DataConclusao = DateTime.Now;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(FormaPagamento12))
                {
                    consulta.FormaPagamento = Convert.ToInt32(FormaPagamento12);
                }
                consulta.Observacao = Observacao12;
                if(novaConsulta > 0)
                {
                    db.Entry(consulta).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    db.Consultas.Add(consulta);
                    db.SaveChanges();
                }
                //var teste = db.ConsultaAgentes.Where(x => x.EmpresaID == 1 && x.ConsultaID == consultaId && x.AgenteId == agenteId).ToList();
                //consulta.AgentesC = teste;
                return RedirectToAction("/Create/" + consulta.Id);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }

        }


        public ActionResult DelAgente(string AgenteId, string Id)
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
                var consultaId = Convert.ToInt32(Id);
                var agenteId = Convert.ToInt32(AgenteId);
                var consultaAgente = db.ConsultaAgentes.Where(x => x.EmpresaID == usuariologado.empresaId && x.ConsultaID == consultaId && x.AgenteId == agenteId).FirstOrDefault();
                db.ConsultaAgentes.Remove(consultaAgente);
                db.SaveChanges();
                return RedirectToAction("/Create/"+consultaId);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }

        }

        public ActionResult DelExame(string ExameId, string Id)
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
                var consultaId = Convert.ToInt32(Id);
                var exameId = Convert.ToInt32(ExameId);
                var consultaExame = db.ConsultaExames.Where(x => x.EmpresaID == usuariologado.empresaId && x.ConsultaID == consultaId && x.ExameId == exameId).FirstOrDefault();
                db.ConsultaExames.Remove(consultaExame);
                db.SaveChanges();
                return RedirectToAction("/Create/" + consultaId);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }


        }

        public ActionResult ImpAso(string id)
        {
            if (!string.IsNullOrEmpty(id))
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
                    var consultaId = Convert.ToInt32(id);
                    Consulta aso = db.Consultas.Where(x => x.Id == consultaId && x.EmpresaID == usuariologado.empresaId).FirstOrDefault();
                    if(aso != null)
                    {
                        aso.ExamesC = db.ConsultaExames.Where(x => x.ConsultaID == aso.Id).ToList();
                        aso.AgentesC = db.ConsultaAgentes.Where(x => x.ConsultaID == aso.Id).ToList();
                        var empresa = db.Empresas.Find(usuariologado.empresaId);
                        ViewBag.Razao = empresa.Razao;
                        ViewBag.Endereco = empresa.Endereco + ", nº" + empresa.Numero + ", " + empresa.Bairro;
                        ViewBag.Email = empresa.Fone + " - " + empresa.Email;
                        var nomeArquivo = "Logo" + Convert.ToString(usuariologado.empresaId) + ".bmp";                        
                        ViewBag.Logo = nomeArquivo;
                        return View(aso);
                    }
                    else
                    {
                        return View();
                    }
                    
                }
                else
                {
                    TempData["MensagemRetorno"] = "Faça Login para continuar.";
                    return Redirect("~/Login");
                }
            }
            return View();

        }


        public ActionResult EditExame(int? ID, int? exameId)
        {

            ViewBag.UsuarioLogado = Utils.User.GetCookieUsuarioLogado(Request, Session);
            if (!string.IsNullOrEmpty(ViewBag.UsuarioLogado))
            {
                ViewBag.SituacaoConsultaF = new SelectList(new Consulta.ListaSituacaoConsultaF().MetodoListaConsulta(), "ID", "Descricao");
                Utils.User.UsuarioLogado usuariologado = Utils.User.GetDadosUsuarioLogado(ViewBag.UsuarioLogado);
                ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;
                ViewBag.RuleAdmin = usuariologado.admin;
                ViewBag.RuleFinanceiro = usuariologado.RuleFinanceiro;
                ViewBag.RuleMovimentacao = usuariologado.RuleMovimentacao;
                ViewBag.RuleCadastro = usuariologado.RuleCadastro;
                var consultaExame = db.ConsultaExames.Where(x => x.EmpresaID == usuariologado.empresaId && x.ConsultaID == ID && x.ExameId == exameId).FirstOrDefault();
                if(consultaExame != null)
                {
                    ViewBag.SituacaoExame = new SelectList(new Consulta.ConsultaExame.ListaConclusaoExame().MetodoConclusao(), "ID", "Descricao", consultaExame.SituacaoExame);
                    ViewBag.FormaPgto = new SelectList(new Consulta.ConsultaExame.ListaFormaPagExame().MetodoFormaPgtoExame(), "ID", "Descricao", consultaExame.FormaPgto);
                    return PartialView(consultaExame);
                }
                else
                {
                    TempData["MensagemRetorno"] = "Ocorreu um erro, tente novamente, caso persistir entrar em contato com o supote técnico";
                    return RedirectToAction("/Create/" + consultaExame.ConsultaID);
                }
                
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        [HttpPost]
        public ActionResult EditExame(Consulta.ConsultaExame consultaExame)
        {
            ViewBag.UsuarioLogado = Utils.User.GetCookieUsuarioLogado(Request, Session);
            if (!string.IsNullOrEmpty(ViewBag.UsuarioLogado))
            {
                ViewBag.SituacaoConsultaF = new SelectList(new Consulta.ListaSituacaoConsultaF().MetodoListaConsulta(), "ID", "Descricao");
                Utils.User.UsuarioLogado usuariologado = Utils.User.GetDadosUsuarioLogado(ViewBag.UsuarioLogado);
                ViewBag.ModuloFinanceiro = usuariologado.moduloFinanceiro;
                ViewBag.RuleAdmin = usuariologado.admin;
                ViewBag.RuleFinanceiro = usuariologado.RuleFinanceiro;
                ViewBag.RuleMovimentacao = usuariologado.RuleMovimentacao;
                ViewBag.RuleCadastro = usuariologado.RuleCadastro;
                ViewBag.SituacaoExame = new SelectList(new Consulta.ConsultaExame.ListaConclusaoExame().MetodoConclusao(), "ID", "Descricao", consultaExame.SituacaoExame);
                ViewBag.FormaPgto = new SelectList(new Consulta.ConsultaExame.ListaFormaPagExame().MetodoFormaPgtoExame(), "ID", "Descricao", consultaExame.FormaPgto);
                db.Entry(consultaExame).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("/Create/" + consultaExame.ConsultaID);
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
