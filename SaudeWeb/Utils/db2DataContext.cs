using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.OleDb;

namespace SaudeWeb.Utils
{
    public class db2DataContext
    {
        [Key]
        public int cfo_codigo { get; set; }

        public string cfo_descricao { get; set; }

        public int? cfo_icms { get; set; }

        public int? cfo_ipi { get; set; }

        public string cfo_csosn { get; set; }


        public List<db2DataContext> testeConIbm()
        {

            
            var texto = ""; 
            OleDbConnection ibm = new OleDbConnection("Provider=IBMDADB2;Database=sample;Hostname=localhost;Protocol=TCPIP;Port = 50000; Uid = db2admin; Pwd = fw30264045");
            OleDbCommand cmd = new OleDbCommand("select * from cfop", ibm);
            List<db2DataContext> lista = new List<db2DataContext>();
            try
            {
                
                ibm.Open();
                OleDbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    db2DataContext cfop = new db2DataContext();
                    cfop.cfo_codigo = Convert.ToInt32(reader[0].ToString());
                    cfop.cfo_descricao = reader[1].ToString();
                    cfop.cfo_icms = Convert.ToInt32(reader[2].ToString());
                    if (!string.IsNullOrEmpty(reader[3].ToString()))
                    {
                        cfop.cfo_ipi = Convert.ToInt32(reader[3].ToString());
                    }
                    cfop.cfo_csosn = reader[4].ToString();
                    lista.Add(cfop);
                }
                reader.Close();

                
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                ibm.Close();
            }
            return lista;
        }
        
        
    }
}