using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SaudeWeb.Models
{
    public class FaturaViewModel
    {
        public int PessoaID { get; set; }

        public int? ExameID { get; set; }

        public int? MedicoID { get; set; }

        public int EmpresaID { get; set; }
        [Display(Name ="Cliente")]
        public virtual string PessoaDesc
        {
            get
            {
                DataContext db = new DataContext();
                var pessoa = db.Pessoas.Where(x => x.EmpresaID == EmpresaID 
                             && x.ID == PessoaID)
                             .Select(s => new { s.Razao}).FirstOrDefault();
                if(pessoa != null)
                {
                    return pessoa.Razao;
                }
                else
                {
                    return "";
                }                
            }
        }

        public int FuncionarioID { get; set; }

        [Display(Name = "Funcionário")]
        public virtual string FuncionarioDesc
        {
            get
            {
                DataContext db = new DataContext();
                var pessoa = db.Funcionarios.Where(x => x.EmpresaID == EmpresaID && x.ID == FuncionarioID).Select(s => new { s.Nome}).FirstOrDefault();
                if (pessoa != null)
                {
                    return pessoa.Nome;
                }
                else
                {
                    return "";
                }

            }
        }

        public int? ConsultaID { get; set; }

        [Display(Name = "Valor")]
        public decimal? ValorFaturar { get; set; }

        [Display(Name = "Data da Consulta")]
        public DateTime? DataConsulta { get; set; }

        public static decimal GetValorRepasse(int EmpresaID, int PessoaID, int ExameID)
        {
            DataContext db = new DataContext();
            decimal retorno = 0;
            var medico = db.Pessoas.Where(x => x.EmpresaID == EmpresaID && x.ID == PessoaID).FirstOrDefault();
            if (medico != null)
            {
                if (medico.TabelaID != null)
                {
                    var tabela = db.TabExame.Where(x => x.EmpresaID == EmpresaID 
                                 && x.TabID == medico.TabelaID 
                                 && x.ExameID == ExameID)
                                 .Select(s => new { s.RepasseMedicoValor }).FirstOrDefault();
                    if (tabela != null)
                    {
                        retorno = Convert.ToDecimal(tabela.RepasseMedicoValor);
                    }
                    else
                    {
                        var exame = db.Exames.Where(x => x.EmpresaID == EmpresaID && x.ID == ExameID).Select(s => new { s.ValorRepasse }).FirstOrDefault();
                        if(exame != null)
                        {
                            retorno = Convert.ToDecimal(exame.ValorRepasse);
                        }
                        else
                        {
                            retorno = 0;
                        }                        
                    }
                }
                else
                {
                    var exame = db.Exames.Where(x => x.EmpresaID == EmpresaID && x.ID == ExameID).FirstOrDefault();
                    if (exame != null)
                    {
                        retorno = Convert.ToDecimal(exame.ValorRepasse);
                    }
                    else
                    {
                        retorno = 0;
                    }
                }
            }
            return retorno;

        }

        public virtual string Data
        {
            get
            {
                return Convert.ToDateTime(DataConsulta).ToShortDateString();
            }
        }

        public List<ExamesFatura> Exames
        {
            get
            {
                List<ExamesFatura> lista = new List<ExamesFatura>();
                DataContext db = new DataContext();
                var consultaExame = db.ConsultaExames.Where(x => x.EmpresaID == EmpresaID && x.ConsultaID == ConsultaID && x.FormaPgto == 1);
                    //.    Select(s => new { s.DataColeta, s.ExameId, s.ValorExame, s.ExameDesc});
                foreach (var item in consultaExame)
                {
                    ExamesFatura exames = new ExamesFatura();
                    exames.DataExame = Convert.ToDateTime(item.DataColeta).ToShortDateString();
                    exames.ExameID = item.ExameId;
                    exames.Valor = item.ValorExame;
                    exames.Descricao = item.ExameDesc;
                    lista.Add(exames);
                }
                return lista;
            }
        }

        public class ExamesFatura
        {
            public int? ExameID { get; set; }

            public string Descricao { get; set; }

            public decimal? Valor { get; set; }

            public string DataExame { get; set; }
            
        }

        public static decimal GetTotalConsulta(int Consultaid, int EmpresaId)
        {
            DataContext db = new DataContext();
            decimal retorno = 0;
            var exames = db.ConsultaExames.Where(x => x.EmpresaID == EmpresaId && x.ConsultaID == Consultaid);
            foreach (var item in exames)
            {
                if(item != null)
                {
                    retorno = Convert.ToInt32(item.ValorExame) + retorno;
                }                
            }
            return retorno;
        }
    }
}