using SaudeWeb.Utils;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Web.Mvc;

namespace SaudeWeb.Controllers
{
    public class ControllerDB2Controller : Controller
    {
        // GET: ControllerDB2
        public ActionResult Index()
        {
            db2DataContext cfop = new db2DataContext();
            List<db2DataContext> lista = cfop.testeConIbm();
            return View(lista);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string cfo_codigo, string cfo_descricao)
        {
            string insert = "Insert into cfop(cfo_codigo, cfo_descricao, cfo_icms, cfo_ipi, cfo_csosn) values(" + cfo_codigo + ", '" + cfo_descricao + "', 1, 1, '103')";
            OleDbConnection con = new OleDbConnection("Provider=IBMDADB2;Database=sample;Hostname=localhost;Protocol=TCPIP;Port = 50000; Uid = db2admin; Pwd = fw30264045");
            OleDbCommand cmd = new OleDbCommand(insert, con);
            try
            {
                con.Open();
                int i = cmd.ExecuteNonQuery();
                bool teste;
                if(i > 0)
                {
                    teste = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return RedirectToAction("Index");
        }
    }
}