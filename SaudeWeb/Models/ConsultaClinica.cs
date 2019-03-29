using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static SaudeWeb.Models.Consulta;

namespace SaudeWeb.Models
{
    public class ConsultaClinica 
    {
        [Key, Column(Order = 0)]
        public int Id { get; set; }

        [Display(Name = "Filial")]
        [Key, Column(Order = 1)]
        public int EmpresaID { get; set; }

        public DateTime Data { get; set; }

        public DateTime DataRetorno { get; set; }

        public int? MedicoID { get; set; }

        public int PacienteID { get; set; }

        public List<ConsultaExame> Exames { get; set; }


    }
}