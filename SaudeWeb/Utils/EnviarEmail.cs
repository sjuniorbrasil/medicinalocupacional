using SaudeWeb.Models;
using System;
using System.Net;
using System.Net.Mail;

namespace SaudeWeb.Utils
{
    public static class Utils
    {

        public static string Encrypt(string ATexto, bool ADecrypt)
        {
            string AKey = "19fbd782f663283aef8c478441e00168";
            string S = "";
            int J = 0;
            int Var4 = 0;
            for (var i = 0; i < AKey.Length; i++)
            {
                Var4 = Var4 + Convert.ToInt32(AKey[i]);
            }
            string cV4 = Var4.ToString();

            if (ADecrypt == true)
            {
                int I = 0;
                while (I < ATexto.Length -1)
                {
                    int Var1 = Convert.ToInt32(cV4[J]);
                    J = J + 1;
                    if (J > cV4.Length - 1)
                    {
                        J = 0;
                    }
                    string B = ATexto.Substring(I, 2);
                    int Var3 = Convert.ToByte(B,16);
                    int Var2 = Var1 ^ Var3;
                    S = S + Convert.ToChar(Var2);
                    I = I + 2;
                    
                }
            }
            else
            {
                for (var i = 0; i < ATexto.Length; i++)
                {
                    int Var1 = Convert.ToInt32(cV4[J]);
                    J = J + 1;
                    if (J > cV4.Length - 1)
                    {
                        J = 0;
                    }
                    int Var2 = Convert.ToInt32(ATexto[i]);
                    int Var3 = Var1 ^ Var2;
                    string B = Var3.ToString("X");
                    if (B.Length < 2)
                    {
                        B = "0" + B;
                    }
                    S = S + B;
                }
            }
            return S;
        }
    }

    public class EnviarEmail
    {
        public EnviarEmail()
        {

        }
        public string inicio(string arq_from, string arq_to, string arq_cc, string arq_bcc, string arq_subjct, string arq_body, string arq_path, string arq_atch, string srv_smtp, string srv_user, string srv_pass)
        {
            int x = 0;
            int tam = 0;
            int inicio = 0;
            int fim = 0;
            string pedaco;

            if ((arq_from.Length + arq_to.Length + arq_bcc.Length) == 0)
            {
                if (arq_from.Length == 0)
                    return "Informe o remetente";
                if (arq_to.Length == 0)
                    return "Informe destinatario";
                if (arq_bcc.Length == 0)
                    return "Informe destinatario Copia Oculta";
            }

            if (arq_subjct.Length == 0)
                return "Informe o assunto";
            if (arq_path.Length == 0)
                arq_path = "c:\\temp\\arqmail.txt";
            if (srv_smtp.Length == 0)
                return "Informe o Servidor Smtp";
            if (srv_user.Length == 0)
                return "Informe o Usuario";
            if (srv_pass.Length == 0)
                return "Informe a Senha";

            SmtpClient client = new SmtpClient(srv_smtp);
            client.Credentials = new NetworkCredential(srv_user, srv_pass);
            MailMessage message = new MailMessage(new MailAddress(arq_from.ToString(), arq_from), new MailAddress(arq_to, arq_to));
            if (arq_cc.Length > 0) { message.CC.Add(new MailAddress(arq_cc)); }
            //********************************AQUI CARREGA COPIAS QDO MAIS DE UMA ********************************        
            x = arq_bcc.IndexOf(",");
            while (arq_bcc.IndexOf(",") > 0 || arq_bcc.IndexOf(";") > 0)
            {
                tam = arq_bcc.Length;
                fim = arq_bcc.IndexOf(",");
                if (fim < 0) { fim = arq_bcc.IndexOf(";"); }
                pedaco = arq_bcc.Substring(inicio, fim);
                message.Bcc.Add(new MailAddress(pedaco));
                inicio = fim + 1;
                arq_bcc = arq_bcc.Substring(inicio, tam - inicio);
                tam = arq_bcc.Length;
                inicio = 0;
            }
            if (arq_bcc.Length > 0) { message.Bcc.Add(new MailAddress(arq_bcc)); }
            //******************************************************************************************************
            message.Subject = arq_subjct;
            message.Body = arq_body;
            //if (arq_atch.Length > 0)
            //{
            //    Attachment att = new Attachment(@arq_atch);
            //    message.Attachments.Add(att);
            //}
            client.Send(message);

            //StreamWriter arqmail = new StreamWriter(arq_path, true, Encoding.ASCII);
            //arq_from = "De: " + message.From.ToString();
            //arq_to = "Para: " + message.To.ToString();
            //arq_cc = "CC: " + message.CC.ToString(); ;
            //arq_bcc = "Bcc: " + message.Bcc.ToString();
            //arq_subjct = "Assunto: " + message.Subject.ToString();
            //arq_body = "Menssagem: " + message.Body.ToString();
            //arqmail.WriteLine(arq_from);
            //arqmail.WriteLine(arq_to);
            //arqmail.WriteLine(arq_cc);
            //arqmail.WriteLine(arq_bcc);
            //arqmail.WriteLine(arq_subjct);
            //arqmail.WriteLine(arq_body);
            //arqmail.WriteLine("Data/Hora: " + DateTime.Now);
            //arqmail.WriteLine("================================================================");
            //arqmail.Close();
            return "Enviado com Sucesso";
        }
        public void EnviaEmail(string destinatario, string assunto, string corpo, string anexo)
        {
                DataContext db = new DataContext();              

                EnviarEmail enviarEmail = new EnviarEmail();

               

                enviarEmail.inicio(
                    "suporte@produsys.com.br",
                    destinatario,
                    "",
                    "",
                    assunto,
                    corpo,
                    anexo,
                    "",
                    "mail.prosige.com.br",
                    "teste@prosige.com.br",
                    "");
            }
        


    }
}