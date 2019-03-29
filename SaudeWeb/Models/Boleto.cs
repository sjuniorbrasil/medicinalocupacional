using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;

namespace SaudeWeb.Models
{
    public class Boleto
    {
        [Key, Column(Order = 0)]
        public int ID { get; set; }

        [Key, Column(Order = 1)]
        public int EmpresaID { get; set; }

        public int CedenteID { get; set; }

        public int SacadoID { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Informe somente data válida.")]
        public DateTime DataEmissao { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Informe somente data válida.")]
        public DateTime DataVencimento { get; set; }

        public int ContaID { get; set; }

        public string NossoNumero { get; set; }

        public decimal Valor { get; set; }

        public decimal? ValorBaixado { get; set; }

        public decimal Multa { get; set; }

        public decimal Juros{ get; set; }

        public decimal Desconto { get; set; }

        public decimal ValorTotal { get; set; }

        public DateTime DataLiquidacao { get; set; }

        public int Situacao { get; set; }

        public string Observacoes { get; set; }

        public int NumeroRemessa { get; set; }

        public DateTime DataRemessa { get; set; }

        public DateTime DataCancelamento { get; set; }

        public string LinhaDigitavel { get; set; }

        public string NumeroDocumento { get; set; }

        public string Carteira { get; set; }

        public string Modalidade { get; set; }

        public string CodigoBeneficiario { get; set; }







    }
}