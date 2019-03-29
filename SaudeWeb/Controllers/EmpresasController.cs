using SaudeWeb.Models;
using SaudeWeb.Utils;
using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SaudeWeb.Controllers
{
    public class EmpresasController : Controller
    {
        private DataContext db = new DataContext();      
       
        public ActionResult Edit()
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
                if (usuariologado.empresaId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ViewBag.CidadeID = new SelectList(db.Cidades.OrderBy(x => x.Descricao), "ID", "CidadeUF");
                Empresa empresa = db.Empresas.Find(usuariologado.empresaId);
                var nomeArquivo = "Logo" + Convert.ToString(usuariologado.empresaId) + ".bmp";
                ViewBag.Logo = nomeArquivo;
                if (empresa == null)
                {
                    return HttpNotFound();
                }
                return View(empresa);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }
                
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Razao,Fantasia,CNAE,CNPJ,IE,IM,Endereco,Bairro,Numero,CidadeID,Cep,Fone,Responsavel,Email,DataCalibracao,Audiometro,RepousoAcustico,PlanoContaConsulta")] Empresa empresa)
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
                    empresa.ModuloFinanceiro = usuariologado.moduloFinanceiro;
                    empresa.RepousoAcustico = LibProdusys.FS(empresa.RepousoAcustico);
                    empresa.Audiometro = LibProdusys.FS(empresa.Audiometro);
                    empresa.Bairro = LibProdusys.FS(empresa.Bairro);
                    empresa.Cep = LibProdusys.TrataCep(empresa.Cep);
                    empresa.CNAE = LibProdusys.FS(empresa.CNAE);
                    empresa.Endereco = LibProdusys.FS(empresa.Endereco);
                    empresa.Fantasia = LibProdusys.FS(empresa.Fantasia);
                    empresa.Fone = LibProdusys.TrataFone(empresa.Fone);
                    empresa.IE = LibProdusys.FS(empresa.IE);
                    empresa.IM = LibProdusys.FS(empresa.IM);
                    empresa.Numero = LibProdusys.FS(empresa.Numero);
                    empresa.Razao = LibProdusys.FS(empresa.Razao);
                    empresa.Responsavel = LibProdusys.FS(empresa.Responsavel);                    
                    ViewBag.CidadeID = new SelectList(db.Cidades.OrderBy(x => x.Descricao), "ID", "CidadeUF", empresa.CidadeID);
                    db.Entry(empresa).State = EntityState.Modified;
                    db.SaveChanges();
                    return Redirect("~/Home");
                }
                return View(empresa);
            }
            else
            {
                TempData["MensagemRetorno"] = "Faça Login para continuar.";
                return Redirect("~/Login");
            }
        }

        public ActionResult SalvaImg()
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
                return View("Edit");
            }
            else
            {                
                TempData["MensagemRetorno"] = "Selecione um arquivo para continuar";
                return View("Edit");
            }
        }

        [HttpPost]
        public ActionResult SalvaImg(HttpPostedFileBase file)
        {
            if (file != null)
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
                        if ((file.ContentLength > 0) && (file.ContentLength <= 1024000))
                        {
                            if (file.FileName.Contains(".bmp"))
                            {
                                var nomeArquivo = "Logo" + usuariologado.empresaId + ".bmp";
                                var caminho = Path.Combine(Server.MapPath("~/Imagens/Logo"), nomeArquivo);
                                file.SaveAs(caminho);
                                TempData["MensagemRetorno"] = "Arquivo enviado e salvo com sucesso no servidor.";
                                return RedirectToAction("Edit");
                            }
                            else
                            {
                                TempData["MensagemRetorno"] = "Erro ao enviar arquivo, o arquivo deve ter estar no formato Bitmap";
                                return RedirectToAction("Edit");
                            }
                        }
                        else
                        {
                            TempData["MensagemRetorno"] = "Erro ao enviar arquivo, o tamanho do arquivo não pode ser superior a 1 MegaByte";
                            return RedirectToAction("Edit");
                        }
                    }
                    else
                    {
                        TempData["MensagemRetorno"] = "Faça Login para continuar.";
                        return Redirect("~/Login");
                    }
                }
            }
            else
            {
                TempData["MensagemRetorno"] = "Selecione um arquivo para continuar";                
            }
            return RedirectToAction("Edit");
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
