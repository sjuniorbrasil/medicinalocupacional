using SaudeWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SaudeWeb.Controllers
{
    public class PesquisaController : Controller
    {
        DataContext db = new DataContext();
        public JsonResult PesquisaCidade(string filtro = "")
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
                var cidades = new object();                
                if (!string.IsNullOrEmpty(filtro))
                {
                    cidades = db.Cidades.Where(x => x.Descricao.Contains(filtro)).ToList();
                }
                else
                {
                    cidades = db.Cidades.ToList();
                }

                return Json(cidades, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Json("~/Login");
            }
        }

        public JsonResult PesquisaExame(string filtro = "")
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
                var exames = new object();                
                if(!string.IsNullOrEmpty(filtro))
                {
                    exames = db.Exames.Where(x => x.EmpresaID == usuariologado.empresaId 
                             && x.Nome.Contains(filtro)).
                             Select(s => new { s.ID, s.Nome }).ToList();
                }
                else
                {
                    exames = db.Exames.Where(x => x.EmpresaID == usuariologado.empresaId).
                             Select(s => new { s.ID, s.Nome }).ToList();
                }

                return Json(exames, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Json("~/Login");
            }
        }

        public JsonResult PesquisaCategoria(string filtro = "")
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
                var plConta = new object();
                if (string.IsNullOrEmpty(filtro))
                {
                    plConta = db.PlanoContas.Where(x => x.EmpresaID == usuariologado.empresaId 
                              && x.NivelSuperior != null)
                              .ToList();
                }
                else
                {
                    plConta = db.PlanoContas.Where(x => x.EmpresaID == usuariologado.empresaId 
                              && x.Descricao.Contains(filtro) 
                              && x.NivelSuperior != null)
                              .ToList();
                }

                return Json(plConta, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Json("~/Login");
            }
        }

        public JsonResult PesquisaFuncionario(string filtro = "")
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
                var funcionarios = new object(); ;
                if (string.IsNullOrEmpty(filtro))
                {
                    funcionarios = db.Funcionarios.Where(x => x.EmpresaID == usuariologado.empresaId)
                                   .Select(s => new { s.ID, s.Nome})
                                   .ToList();
                }
                else
                {
                    funcionarios = db.Funcionarios.Where(x => x.EmpresaID == usuariologado.empresaId
                                   && x.Nome.Contains(filtro))
                                   .Select(s => new { s.ID, s.Nome})
                                   .ToList();
                }

                return Json(funcionarios, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Json("~/Login");
            }
        }

        public JsonResult PesquisaAgente(string filtro = "")
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
                var agentes = new object();
                if (string.IsNullOrEmpty(filtro))
                {
                    agentes = db.Agentes.Where(x => x.EmpresaID == usuariologado.empresaId)
                        .Select(s => new { s.Id, s.Descricao })      
                        .ToList();
                }
                else
                {
                    agentes = db.Agentes.Where(x => x.EmpresaID == usuariologado.empresaId && x.Descricao.Contains(filtro))
                              .Select(s => new { s.Id, s.Descricao})
                              .ToList();
                }

                return Json(agentes, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Json("~/Login");
            }
        }

        public JsonResult PesquisaMedico(string filtro = "")
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
                var medicos = new object();
                if (string.IsNullOrEmpty(filtro))
                {
                    medicos = db.Pessoas.Where(x => x.EmpresaID == usuariologado.empresaId)
                        .Select(s => new { s.ID, s.Razao})
                        .ToList();
                }
                else
                {
                    medicos = db.Pessoas.Where(x => x.EmpresaID == usuariologado.empresaId && x.Razao.Contains(filtro))
                        .Select(s => new { s.ID, s.Razao })
                        .ToList();
                }

                return Json(medicos, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Json("~/Login");
            }
        }

        public JsonResult PesquisaCliente(string filtro = "")
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
                var clientes = new object();
                if (string.IsNullOrEmpty(filtro))
                {
                    clientes = db.Pessoas.Where(x => x.EmpresaID == usuariologado.empresaId)
                        .Select(s => new { s.ID, s.Razao })
                        .ToList();
                }
                else
                {
                    clientes = db.Pessoas.Where(x => x.EmpresaID == usuariologado.empresaId && x.Razao.Contains(filtro))
                        .Select(s => new { s.ID, s.Razao })
                        .ToList();
                }
                return Json(clientes, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Json("~/Login");
            }
        }

        public JsonResult PesquisaFaturamentoPendente(string filtro = "")
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
                var clientes = new List<Pessoa.PessoaPesquisa>();
                Pessoa pessoa = new Pessoa();
                if (string.IsNullOrEmpty(filtro))
                {
                    clientes = pessoa.ListaPessoas(Convert.ToString(usuariologado.empresaId), "");
                }
                else
                {
                    clientes = pessoa.ListaPessoas(Convert.ToString(usuariologado.empresaId), filtro);
                }
                return Json(clientes, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Json("~/Login");
            }
        }


        public JsonResult PesquisaFaturamentoPendenteMedico(string filtro = "")
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
                var clientes = new object();
                    
                if (!string.IsNullOrEmpty(filtro))
                {
                    clientes = db.Pessoas.Join(db.ConsultaExames,
                               pessoas => pessoas.ID,
                               consultaExame => consultaExame.medicoId,
                               (pessoas, consultaExame) => new { pessoas, consultaExame })
                               .Where(x => x.pessoas.EmpresaID == usuariologado.empresaId
                               && x.consultaExame.RepasseMedico == 1
                               && x.consultaExame.FaturadoMedico == 0
                               && x.pessoas.Razao.Contains(filtro))
                               .Select(s => new { s.pessoas.ID, s.pessoas.Razao });
                    
                }   
                else
                {
                    clientes = db.Pessoas.Join(db.ConsultaExames,
                               pessoas => pessoas.ID,
                               consultaExame => consultaExame.medicoId,
                               (pessoas, consultaExame) => new { pessoas, consultaExame })
                               .Where(x => x.pessoas.EmpresaID == usuariologado.empresaId
                               && x.consultaExame.RepasseMedico == 1
                               && x.consultaExame.FaturadoMedico == 0).Select(s => new { s.pessoas.ID, s.pessoas.Razao });
                }
                

                return Json(clientes, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Json("~/Login");
            }
        }
    }
}