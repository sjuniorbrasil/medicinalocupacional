using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace SaudeWeb.Models
{
    public class GerencialPlanoContaViewModel
    {
        public int? tipo { get; set; }

        public string Conta { get; set; }

        [Display(Name = "ID")]
        public int? CategoriaID { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Valor")]
        public decimal? Valor { get; set; }

        [Display(Name = "Valor")]
        public decimal DebCred { get; set; }

        [Display(Name = "Valor Liquidado")]
        public decimal DebCredLiquidado { get; set; }

        public virtual string DebCredFormatado
        {
            get
            {
                if(DebCred != null)
                {
                    return DebCred.ToString("C", CultureInfo.CurrentCulture);
                }
                else
                {
                    return "";
                }
                
            }
        }

        public virtual string DebCredLiquidadoFormatado
        {
            get
            {
                if (DebCredLiquidado != null)
                {
                    return DebCredLiquidado.ToString("C", CultureInfo.CurrentCulture);
                }
                else
                {
                    return "";
                }

            }
        }

        public bool TemNivelSuperior { get; set; }
        public int NivelSuperior { get; set; }

        public List<GerencialPlanoContaViewModel> ListaMovimento(string empresaid, string codcategoria, string situacao, string periodo1, string periodo2, string apuracao)
        {
            List<GerencialPlanoContaViewModel> lista = new List<GerencialPlanoContaViewModel>();
            string select = " select a.Tipo, b.conta, b.descricao, a.categoriaid, \n" +
                            "       (select sum(z.Valor) from financeiro z \n" +
                            "        where z.EmpresaID = a.EmpresaID \n" +
                            "        and z.categoriaid = a.categoriaID) as Soma, \n" +
                            " CASE \n" +
                            "   WHEN a.tipo  = 1 THEN sum(a.valorbaixado)  \n" +
                            "   WHEN a.Tipo = 2 THEN sum(a.Valorbaixado*(-1))  \n" +
                            " END  as DebCredLiquidado, \n" +
                            " CASE \n" +
                            "   WHEN a.tipo  = 1 THEN sum(a.valor) \n" +
                            "   WHEN a.Tipo = 2 THEN sum(a.Valor*(-1)) \n" +
                            " END  as DebCred, a.empresaid \n" +
                            " from financeiro a \n" +
                            " join PlanoConta b on a.categoriaID = b.ID and a.empresaid = b.empresaid \n";
            if(apuracao == "3")
            {
                select += " join baixa c on a.id = c.financeiroid and a.parcelaid = c.parcelaid and a.empresaid = c.empresaid \n"+
                " join bancocaixa d on c.id = d.baixaid and c.empresaid = d.empresaid and d.DaTaCONCiliacao >= '"+periodo1+"'  and d.DaTaCONCiliacao <= '"+periodo2+"' \n";
            }
            select += " where a.empresaid = "+empresaid + "\n";
            if (!string.IsNullOrEmpty(codcategoria))
            {
                select += " and a.cod_categoria = " + codcategoria + "\n";
            }
            if (!string.IsNullOrEmpty(situacao))
            {
                if(situacao == "0")
                {
                    select += " and a.dataliquidacao is null \n";                    
                }
                if (situacao == "1")
                {
                    select += " and a.dataliquidacao is not null \n";
                }               
            }
            if(!string.IsNullOrEmpty(apuracao))//1 emissao 2 vencimento 3 baixa{
            {
                if(apuracao == "1")
                {
                    if (!string.IsNullOrEmpty(periodo1))
                    {
                        
                        select += " and a.dataemissao >= '"+ periodo1 +"'\n";
                    }
                    if (!string.IsNullOrEmpty(periodo2))
                    {
                        select += " and a.dataemissao <= '" + periodo2 + "'\n";
                    }
                }
                if(apuracao == "2")
                {
                    if (!string.IsNullOrEmpty(periodo1))
                    {
                        select += " and a.datavencimento >= '" + periodo1 + "'\n";
                    }
                    if (!string.IsNullOrEmpty(periodo2))
                    {
                        select += " and a.datavencimento <= '" + periodo2 + "'\n";
                    }
                }
            }
            select += " group by a.Tipo, b.conta, b.descricao, a.categoriaid, a.empresaid \n";
            select += " Order by b.conta ";
           
            SqlConnection con = new SqlConnection(Properties.Settings.Default.Banco);
            SqlCommand cmd = new SqlCommand(select, con);
            cmd.CommandType = System.Data.CommandType.Text;
            try
            {
                con.Open();
                SqlDataReader dR = cmd.ExecuteReader();
                while (dR.Read())
                {
                    GerencialPlanoContaViewModel movimento = new GerencialPlanoContaViewModel();
                    if (!string.IsNullOrEmpty(dR[0].ToString()))
                    {
                        movimento.tipo = Convert.ToInt32(dR[0].ToString());
                    }

                    if (!string.IsNullOrEmpty(dR[1].ToString()))
                    {
                        movimento.Conta = dR[1].ToString();
                    }

                    if (!string.IsNullOrEmpty(dR[2].ToString()))
                    {
                        movimento.Descricao =dR[2].ToString();
                    }
                    //if (!string.IsNullOrEmpty(dR[3].ToString()))
                    //{
                    //    movimento.CategoriaID = Convert.ToInt32(dR[3].ToString());
                    //}
                    if (!string.IsNullOrEmpty(dR[4].ToString()))
                    {
                        movimento.Valor = Convert.ToDecimal(dR[4].ToString());
                    }
                    if (!string.IsNullOrEmpty(dR[5].ToString()))
                    {
                        movimento.DebCredLiquidado = Convert.ToDecimal(dR[5].ToString());
                    }
                    if (!string.IsNullOrEmpty(dR[6].ToString()))
                    {
                        if(apuracao == "2")//baixado
                        {
                            movimento.DebCred = Convert.ToDecimal(dR[5].ToString());
                        }
                        else
                        {
                            movimento.DebCred = Convert.ToDecimal(dR[6].ToString());
                        }
                        
                    }
                    if (!string.IsNullOrEmpty(dR[7].ToString()))
                    {
                        movimento.CategoriaID = Convert.ToInt32(dR[3].ToString());                         
                        DataContext db = new DataContext();
                        var empresa = Convert.ToInt32(dR[7].ToString());
                        var itemPai = db.PlanoContas.Where(x => x.ID == movimento.CategoriaID && x.EmpresaID == empresa).FirstOrDefault();
                        movimento.NivelSuperior = Convert.ToInt32(itemPai.NivelSuperior);

                    }
                    lista.Add(movimento);
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }
            return lista;
        }

    }
}