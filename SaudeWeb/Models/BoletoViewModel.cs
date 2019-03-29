using SaudeWeb.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BarcodeLib;
using System.IO;
using System.Drawing.Imaging;
using System.Web.Mvc;
using System.Drawing;

namespace SaudeWeb.Models
{
    public class BoletoViewModel
    {
       
        public int ID { get; set; }
               
        public int EmpresaID { get; set; }

        public int CedenteID { get; set; }

        public int SacadoID { get; set; }

        public DateTime DataEmissao { get; set; }

        public DateTime DataVencimento { get; set; }

        public int ContaID { get; set; }

        public string NossoNumero { get; set; }

        public decimal Valor { get; set; }

        public decimal? ValorBaixado { get; set; }

        public decimal Multa { get; set; }

        public decimal Juros { get; set; }

        public decimal Desconto { get; set; }

        public decimal ValorTotal { get; set; }

        public DateTime DataLiquidacao { get; set; }

        public int Situacao { get; set; }

        public string Observacoes { get; set; }

        public int NumeroRemessa { get; set; }

        public DateTime DataRemessa { get; set; }

        public DateTime DataCancelamento { get; set; }

        public string LocalPagmento { get; set; }

        public string NumeroDocumento { get; set; }

        public string Carteira { get; set; }
  

        public string Modalidade { get; set; }

        public string CodigoBeneficiario { get; set; }

        public string LinhaDigitavel { get; set; }

        public class Beneficiario
        {
            public string  Nome { get; set; }

            public string Agencia { get; set; }

            public string CodigoBeneficiario { get; set; }
            
        }
        public class Pagador
        {
            public string Nome { get; set; }

            public string CpfCnpj { get; set; }

            public string SacadorAvalista { get; set; }


        }

        public string CalculoLinhaDigitavel(string Banco, string Carteira, string Agencia, string Modalidade, string CodigoBeneficiario, string NossoNumero, DateTime vencimento, decimal valor, string campolivre, string Parcela)
        {
            var campo1 = "";
            var linha = "";
            campo1 = Banco+"9";
            campo1 += Carteira;
            campo1 += LibProdusys.StrZero(Agencia, 4);
            campo1 += LibProdusys.Modulo10(campo1);
            linha = campo1;
            var campo2 = Modalidade;
            campo2 += CodigoBeneficiario;
            campo2 += NossoNumero.Substring(0, 1);
            TimeSpan d = vencimento - Convert.ToDateTime("07/10/1997");
            string fatorvencto = LibProdusys.StrZero(d.Days.ToString(), 4);
            string vl = LibProdusys.StrZero(LibProdusys.TC(valor.ToString()), 10);
            string codbar = LibProdusys.StrZero(Banco, 3) + "9" + fatorvencto + vl + campolivre;
            string dv = LibProdusys.Modulo11(codbar);
            if (dv == "0")
            {
                dv = "1";
            }
            campo2 += codbar.Substring(0, 4) + dv + codbar.Substring(4);
            linha += campo2;
            var campo3 = "";
            campo3 = CodigoBeneficiario.Substring(1, CodigoBeneficiario.Length);
            campo3 += LibProdusys.StrZero(Parcela, 3);
            campo3 += LibProdusys.Modulo10(campo3);
            linha += campo3;
            var campo4 = "";
            campo4 = linha.Substring(4, 1);
            linha += campo4;
            var campo5 = "";
            campo5 += linha.Substring(5, 14);
            linha += campo5;            


            return linha;
        }

        public object GetCodBar(string id)
        {
            BarcodeLib.Barcode codbar = new BarcodeLib.Barcode();
            codbar.IncludeLabel = false;
            var img = codbar.Encode(BarcodeLib.TYPE.Interleaved2of5, id, Color.Black, Color.White, 320, 60);
            var simg = new MemoryStream();
            img.Save(simg, ImageFormat.Jpeg);
            simg.Position = 0;
            return new FileStreamResult(simg, "image/jpeg");
        }
    }
}