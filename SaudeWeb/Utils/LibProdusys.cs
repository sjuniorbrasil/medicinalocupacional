using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.Mvc;
using System.Windows.Forms;
using System.Management;
using System.Management.Instrumentation;
using System.Net;
using System.Net.NetworkInformation;
using System.Linq;

namespace SaudeWeb.Utils
{
    public class LibProdusys
    {
        public static string Modulo10(string AValor)
        {
            int fpeso = 2;
            string faux = "";
            for (int i = AValor.Length - 1; i >= 0; i--)
            {
                var n = AValor.Substring(i, 1);
                faux = (Convert.ToInt32(n) * fpeso).ToString() + faux;
                if (fpeso == 1)
                {
                    fpeso = 2;
                }
                else
                {
                    fpeso = 1;
                }
            }
            int fdig = 0;
            for (int i = 0; i < faux.Length; i++)
            {
                var n = faux.Substring(i, 1);
                fdig = fdig + Convert.ToInt32(n);
            }
            fdig = 10 - (fdig % 10);
            if (fdig > 9)
            {
                fdig = 0;
            }
            return fdig.ToString();
        }

        public static string Modulo11(string AValor, int ABase = 9, bool AResto = false)
        {
            int fsoma = 0;
            int fpeso = 2;
            int fbase = ABase;
            for (int i = AValor.Length - 1; i >= 0; i--)
            {
                fsoma = fsoma + (Convert.ToInt16(AValor.Substring(i, 1)) * fpeso);
                if (fpeso < fbase)
                {
                    fpeso = fpeso + 1;
                }
                else
                {
                    fpeso = 2;
                }
            }
            if (AResto == true)
            {
                int fdig = fsoma % 11;
                return fdig.ToString();
            }
            else
            {
                int fdig = 11 - (fsoma % 11);
                if (fdig > 9)
                {
                    fdig = 0;
                }
                return fdig.ToString();
            }
        }

        public static int GetQtdeDias(DateTime data)
        {
            int retorno = 0;
            var dt = data.Month;
            if(dt == 2)
            {
                if (DateTime.IsLeapYear(data.Year))
                {
                    retorno = 28;
                }
                else
                {
                    retorno = 27;
                }
                
            } 
            else if((dt == 1) || (dt == 3) || (dt == 5) || (dt == 7) || (dt == 8) || (dt == 10) || (dt == 12))
            {
                retorno = 31;
            }
            else
            {
                retorno = 30;
            }

            return retorno;
        }

        public static int GetNewCode(string tabela, string chave1, string filtro)
        {
            int codigo = 1;
            string select = "select coalesce(max(" + chave1 + "),0) as cod " + " from " + tabela;
            if (!string.IsNullOrEmpty(filtro))
            {
                select += " where " + filtro;
            }
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Properties.Settings.Default.Banco;
            SqlCommand cmd = new SqlCommand(select,con);
            cmd.CommandType = CommandType.Text;
            try
            {
                con.Open();
                SqlDataReader dR = cmd.ExecuteReader();
                while (dR.Read())
                {
                    codigo = Convert.ToInt32(dR[0].ToString()) + 1;
                }
            }
            catch (Exception e)
            {
                e.ToString();
                throw;
            }
            finally
            {
                con.Close();
            }
            return codigo;
        }

        public void EnableCheck(Control controles)
        {
            foreach (Control ctrl in controles.Controls)
            {
                if (ctrl is CheckBox)
                {
                    ((CheckBox)ctrl).Enabled = true;
                }
                else if (ctrl.Controls.Count > 0)
                {
                    EnableCheck(ctrl);
                }
            }
        }

        public void CheckBoxUnchecked(Control controles)
        {
            foreach (Control ctrl in controles.Controls)
            {
                if (ctrl is CheckBox)
                {
                    ((CheckBox)ctrl).Checked = false;
                }
                else if (ctrl.Controls.Count > 0)
                {
                    CheckBoxUnchecked(ctrl);
                }
            }
        }

        public void DisableCheck(Control controles)
        {
            foreach (Control ctrl in controles.Controls)
            {
                if (ctrl is CheckBox)
                {
                    ((CheckBox)ctrl).Enabled = false;
                }
                else if (ctrl.Controls.Count > 0)
                {
                    DisableCheck(ctrl);
                }
            }
        }

        public void limparTextBoxes(Control controles)
        {
            foreach (Control ctrl in controles.Controls)
            {
                if (ctrl is TextBox)
                {
                    ((TextBox)ctrl).Text = String.Empty;
                }
                else if (ctrl.Controls.Count > 0)
                {
                    limparTextBoxes(ctrl);
                }
            }
        }

        public void limparCombo(Control controles)
        {
            foreach (Control ctrl in controles.Controls)
            {
                if (ctrl is ComboBox)
                {
                    ((ComboBox)ctrl).Text = String.Empty;
                }
                else if (ctrl.Controls.Count > 0)
                {
                    limparCombo(ctrl);
                }
            }
        }

        public void EnableTxt(Control controles)
        {
            foreach (Control ctrl in controles.Controls)
            {
                if (ctrl is TextBox)
                {
                    ((TextBox)ctrl).Enabled = true;
                }
                else if (ctrl.Controls.Count > 0)
                {
                    EnableTxt(ctrl);
                }
            }
        }

        public void DisableTxt(Control controles)
        {
            foreach (Control ctrl in controles.Controls)
            {
                if (ctrl is TextBox)
                {
                    ((TextBox)ctrl).Enabled = false;
                }
                else if (ctrl.Controls.Count > 0)
                {
                    DisableTxt(ctrl);
                }
            }
        }

        public static bool ValidaCpf(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;

            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)
                return false;

            tempCpf = cpf.Substring(0, 9);
            soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();

            tempCpf = tempCpf + digito;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);
        }
        public static bool ValidaCnpj(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;

            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

            if (cnpj.Length != 14)
                return false;

            tempCnpj = cnpj.Substring(0, 12);

            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();

            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cnpj.EndsWith(digito);
        }

        public static string StrZero(string s, int qtde = 6)
        {
            var retorno = "";
            for (int i = 1; i <= Math.Abs(qtde) - s.Length; i++)
            {
                retorno = retorno + "0";
            }
            if (qtde > 0)
            {
                retorno = retorno + s;
            }
            else
            {
                retorno = s + retorno;
            }
            return retorno;
        }           

        public static int CalculaIdade(DateTime nascimento)
        {
            var birthdate = nascimento;
            var today = DateTime.Now;
            var age = today.Year - birthdate.Year;
            if (birthdate > today.AddYears(-age)) age--;
            return age;
        }

        public static Boolean PodeExcluir(string id, string empresaid, string tabela, string campo)
        {
            string select = "Select count(*)" + " From " + tabela + " where " + campo + " = " + id;
            if (!string.IsNullOrEmpty(empresaid))
            {
                select += " and empresaid = " + empresaid;
            }
            SqlConnection con = new SqlConnection();           
            con.ConnectionString = Properties.Settings.Default.Banco;            
            SqlCommand cmd = new SqlCommand(select, con);
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                try
                {
                    int contador = Convert.ToInt32(dr[0].ToString());
                    if (contador > 0)
                    {
                        return false;
                    }
                }
                catch (Exception erro)
                {
                    erro.ToString();
                }
                
            }
            catch (Exception erro)
            {
                erro.ToString();
                throw;
            }
            finally
            {
                con.Close();
            }            
            return true;
        }
        
        public static string TrataSoNumero(string texto)
        {
            string retorno = "";
            if (!string.IsNullOrEmpty(texto))
            {
                string numeros = "0123456789";
                for (int i = 0; i < texto.Length; i++)
                {
                    if (numeros.IndexOf(texto[i]) > -1)
                    {
                        retorno = retorno + texto[i];
                    }
                }
            }
            else
            {
                texto = "";
            }

            return retorno;
        }

        public static string FS(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                s = "";
            }
            return s.ToUpper();
        }


        public static string TrataCelular(string s)
        {
            var retorno = TrataSoNumero(s);
            if (retorno.Length == 8)
            {
                s = "(  ) 9" + retorno.Substring(0, 4) + "-" + retorno.Substring(4, 4);
            }
            else if (retorno.Length == 9)
            {
                s = "(  )" + retorno.Substring(0, 5) + "-" + retorno.Substring(5, 4);
            }
            else if (retorno.Length == 10)
            {
                s = "(" + retorno.Substring(0, 2) + ") 9" + retorno.Substring(2, 4) + "-" + retorno.Substring(6, 4);
            }
            else if (retorno.Length == 11)
            {
                s = "(" + retorno.Substring(0, 2) + ") " + retorno.Substring(2, 5) + "-" + retorno.Substring(6, 4);
            }
            else
            {
                s = "";
            }
            return s;
        }
        public static string TrataFone(string s)
        {
            var retorno = TrataSoNumero(s);
            if (retorno.Length == 8)
            {
                s = "(  ) " + retorno.Substring(0, 4) + "-" + retorno.Substring(4, 4);
            }
            else if (retorno.Length == 10)
            {
                s = "(" + retorno.Substring(0, 2) + ") " + retorno.Substring(2, 4) + "-" + retorno.Substring(6, 4);
            }
            else
            {
                s = "";
            }
            return s;
        }

        public static string TrataCNPJ(string s)
        {
            var retorno = TrataSoNumero(s);
            if (retorno.Length == 11)
            {
                s = retorno.Substring(0, 3) + "." + retorno.Substring(3, 3) + "." + retorno.Substring(6, 3) + "-" + retorno.Substring(9, 2);
            }
            else if (retorno.Length == 14)
            {
                s = retorno.Substring(0, 2) + "." + retorno.Substring(2, 3) + "." + retorno.Substring(5, 3) + "/" + retorno.Substring(8, 4) + "-" + retorno.Substring(12, 2);
            }
            return s;

        }

        public static string TrataCep(string s)
        {
            var retorno = TrataSoNumero(s);
            if (retorno.Length >= 8)
            {
                s = retorno.Substring(0, 5) + "-" + retorno.Substring(5, 3);
            }
            else
            {
                s = "";
            }

            return s;
        }

        public static string FN(decimal AValor)
        {
            var s = AValor.ToString(CultureInfo.CreateSpecificCulture("de-DE"));
            s = s.Replace(".", "");
            s = s.Replace(",", ".");
            return s;
        }

        public static string TC(string ATexto)
        {
            string Remover = ".,/-()<>:&%_ ";
            if (string.IsNullOrEmpty(ATexto))
            {
                return "";
            }
            else
            {
                string result = ATexto.Trim();
                for (int i = 0; i < Remover.Length; i++)
                {
                    result = result.Replace(Remover[i].ToString(), "");
                }
                return result;
            }
        }

        public static string RA(string S)
        {
            if (string.IsNullOrEmpty(S))
                return "";
            else
            {
                byte[] bytes = System.Text.Encoding.GetEncoding("iso-8859-8").GetBytes(S);
                var retorno = System.Text.Encoding.UTF8.GetString(bytes);
                retorno = retorno.Replace("<", "&lt;");
                retorno = retorno.Replace(">", "&gt;");
                retorno = retorno.Replace("&", "&amp;");
                retorno = retorno.Replace("'", "");
                retorno = retorno.Replace("º", "");
                return retorno;
            }
        }

        

        public static string FormatarZeros(string ATexto, int ALength)
        {
            string Texto = "0";
            if (!string.IsNullOrEmpty(ATexto))
            {
                Texto = ATexto.Trim();
            }
            if (Texto.Length > ALength)
            {
                Texto = Texto.Substring(0, ALength);
            }

            string Formato = "";
            for (int i = 1; i <= ALength; i++)
            {
                Formato = Formato + "0";
            }
            Formato = "{0:" + Formato + "}";
            return string.Format(Formato, Convert.ToInt64(Texto));
        }

        public void DisableCombo(Control controles)
        {
            foreach (Control ctrl in controles.Controls)
            {
                if (ctrl is ComboBox)
                {
                    ((ComboBox)ctrl).Enabled = false;
                }
                else if (ctrl.Controls.Count > 0)
                {
                    DisableCombo(ctrl);
                }
            }
        }

        public void EnableCombo(System.Windows.Forms.Control controles)
        {
            foreach (Control ctrl in controles.Controls)
            {
                if (ctrl is ComboBox)
                {
                    ((ComboBox)ctrl).Enabled = true;

                }
                else if (ctrl.Controls.Count > 0)
                {
                    EnableCombo(ctrl);
                }
            }
        }
        //ControlesComboBox

        //ControlesMaskedTextBox
        public void limparMTextBoxes(Control controles)
        {

            foreach (Control ctrl in controles.Controls)
            {

                if (ctrl is MaskedTextBox)
                {
                    ((MaskedTextBox)ctrl).Text = String.Empty;

                }
                else if (ctrl.Controls.Count > 0)
                {
                    limparMTextBoxes(ctrl);
                }
            }
        }

        public void EnableMasked(Control controles)
        {

            foreach (Control ctrl in controles.Controls)
            {

                if (ctrl is MaskedTextBox)
                {
                    ((MaskedTextBox)ctrl).Enabled = true;

                }
                else if (ctrl.Controls.Count > 0)
                {
                    EnableMasked(ctrl);
                }
            }
        }

        public void DisableMasked(Control controles)
        {

            foreach (Control ctrl in controles.Controls)
            {

                if (ctrl is MaskedTextBox)
                {
                    ((MaskedTextBox)ctrl).Enabled = false;

                }
                else if (ctrl.Controls.Count > 0)
                {
                    DisableMasked(ctrl);
                }
            }
        }
        //ControlesMaskedTextBox

        //ControlesDataGridView
        public void EnableDataGrid(Control controles)
        {

            foreach (Control ctrl in controles.Controls)
            {

                if (ctrl is DataGrid)
                {
                    ((DataGrid)ctrl).Enabled = true;

                }
                else if (ctrl.Controls.Count > 0)
                {
                    EnableDataGrid(ctrl);
                }
            }
        }

        public void DisableDataGrid(Control controles)
        {

            foreach (Control ctrl in controles.Controls)
            {

                if (ctrl is DataGrid)
                {
                    ((DataGrid)ctrl).Enabled = false;

                }
                else if (ctrl.Controls.Count > 0)
                {
                    DisableDataGrid(ctrl);
                }
            }
        }

        public void messageboxCamposObrigatorio()
        {
            MessageBox.Show("Os campos em negrito são obrigatórios !", "Mensagem o Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void messageboxSucesso()
        {
            MessageBox.Show("Operação efetuada com sucesso !", "Mensagem do Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void messageboxErro(string erro)
        {
            MessageBox.Show("Erro: Erro Ao Gravar no banco de dados " + erro.ToString(), "Mensagem do Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void messageboxDataInv()
        {
            MessageBox.Show("Data inválida", "Mensagem do Sistema", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public void PeguntaExcluir()
        {
            MessageBox.Show("Deseja realmente excluir esse registro", "Mensagem do Sistema", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
        }

        public void ClienteInadimplente()
        {
            MessageBox.Show("Cliente Inadimplente !", "Mensagem do Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public void MessageCnpjInvalido()
        {
            MessageBox.Show("CNPJ Inválido !", "Mensagem do Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public void MessageCpfInvalido()
        {
            MessageBox.Show("CNPJ Inválido !", "Mensagem do Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public void MessageCnpjCadastrado()
        {
            MessageBox.Show("CNPJ Já Cadastrado !", "Mensagem do Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public void UserSenhaInvalida()
        {
            MessageBox.Show("Usuário ou Senha Inválidos", "Mensagem do Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static bool ValidarEmail(string email)
        {
            bool validEmail = false;
            int indexArr = email.IndexOf('@');
            if (indexArr > 0)
            {
                int indexDot = email.IndexOf('.', indexArr);
                if (indexDot - 1 > indexArr)
                {
                    if (indexDot + 1 < email.Length)
                    {
                        string indexDot2 = email.Substring(indexDot + 1, 1);
                        if (indexDot2 != ".")
                        {
                            validEmail = true;
                        }
                    }
                }
            }
            return validEmail;
        }

        public class Conversor
        {
            // O método EscreverExtenso recebe um valor do tipo decimal
            public static string EscreverExtenso(decimal valor)
            {
                if (valor <= 0 | valor >= 1000000000000000)
                    return "Valor não suportado pelo sistema.";
                else
                {
                    string strValor = valor.ToString("000000000000000.00");
                    string valor_por_extenso = string.Empty;

                    for (int i = 0; i <= 15; i += 3)
                    {
                        valor_por_extenso += Escrever_Valor_Extenso(Convert.ToDecimal(strValor.Substring(i, 3)));

                        if (i == 0 & valor_por_extenso != string.Empty)
                        {
                            if (Convert.ToInt32(strValor.Substring(0, 3)) == 1)
                                valor_por_extenso += " TRILHÃO" + ((Convert.ToDecimal(strValor.Substring(3, 12)) > 0) ? " E " : string.Empty);
                            else if (Convert.ToInt32(strValor.Substring(0, 3)) > 1)
                                valor_por_extenso += " TRILHÕES" + ((Convert.ToDecimal(strValor.Substring(3, 12)) > 0) ? " E " : string.Empty);
                        }
                        else if (i == 3 & valor_por_extenso != string.Empty)
                        {
                            if (Convert.ToInt32(strValor.Substring(3, 3)) == 1)
                                valor_por_extenso += " BILHÃO" + ((Convert.ToDecimal(strValor.Substring(6, 9)) > 0) ? " E " : string.Empty);
                            else if (Convert.ToInt32(strValor.Substring(3, 3)) > 1)
                                valor_por_extenso += " BILHÕES" + ((Convert.ToDecimal(strValor.Substring(6, 9)) > 0) ? " E " : string.Empty);
                        }
                        else if (i == 6 & valor_por_extenso != string.Empty)
                        {
                            if (Convert.ToInt32(strValor.Substring(6, 3)) == 1)
                                valor_por_extenso += " MILHÃO" + ((Convert.ToDecimal(strValor.Substring(9, 6)) > 0) ? " E " : string.Empty);
                            else if (Convert.ToInt32(strValor.Substring(6, 3)) > 1)
                                valor_por_extenso += " MILHÕES" + ((Convert.ToDecimal(strValor.Substring(9, 6)) > 0) ? " E " : string.Empty);
                        }
                        else if (i == 9 & valor_por_extenso != string.Empty)
                            if (Convert.ToInt32(strValor.Substring(9, 3)) > 0)
                                valor_por_extenso += " MIL" + ((Convert.ToDecimal(strValor.Substring(12, 3)) > 0) ? " E " : string.Empty);

                        if (i == 12)
                        {
                            if (valor_por_extenso.Length > 8)
                                if (valor_por_extenso.Substring(valor_por_extenso.Length - 6, 6) == "BILHÃO" | valor_por_extenso.Substring(valor_por_extenso.Length - 6, 6) == "MILHÃO")
                                    valor_por_extenso += " DE";
                                else
                                    if (valor_por_extenso.Substring(valor_por_extenso.Length - 7, 7) == "BILHÕES" | valor_por_extenso.Substring(valor_por_extenso.Length - 7, 7) == "MILHÕES"
    | valor_por_extenso.Substring(valor_por_extenso.Length - 8, 7) == "TRILHÕES")
                                    valor_por_extenso += " DE";
                                else
                                        if (valor_por_extenso.Substring(valor_por_extenso.Length - 8, 8) == "TRILHÕES")
                                    valor_por_extenso += " DE";

                            if (Convert.ToInt64(strValor.Substring(0, 15)) == 1)
                                valor_por_extenso += " REAL";
                            else if (Convert.ToInt64(strValor.Substring(0, 15)) > 1)
                                valor_por_extenso += " REAIS";

                            if (Convert.ToInt32(strValor.Substring(16, 2)) > 0 && valor_por_extenso != string.Empty)
                                valor_por_extenso += " E ";
                        }

                        if (i == 15)
                            if (Convert.ToInt32(strValor.Substring(16, 2)) == 1)
                                valor_por_extenso += " CENTAVO";
                            else if (Convert.ToInt32(strValor.Substring(16, 2)) > 1)
                                valor_por_extenso += " CENTAVOS";
                    }
                    return valor_por_extenso;
                }
            }

           

            public static string Escrever_Valor_Extenso(decimal valor)
            {
                if (valor <= 0)
                    return string.Empty;
                else
                {
                    string montagem = string.Empty;
                    if (valor > 0 & valor < 1)
                    {
                        valor *= 100;
                    }
                    string strValor = valor.ToString("000");
                    int a = Convert.ToInt32(strValor.Substring(0, 1));
                    int b = Convert.ToInt32(strValor.Substring(1, 1));
                    int c = Convert.ToInt32(strValor.Substring(2, 1));

                    if (a == 1) montagem += (b + c == 0) ? "CEM" : "CENTO";
                    else if (a == 2) montagem += "DUZENTOS";
                    else if (a == 3) montagem += "TREZENTOS";
                    else if (a == 4) montagem += "QUATROCENTOS";
                    else if (a == 5) montagem += "QUINHENTOS";
                    else if (a == 6) montagem += "SEISCENTOS";
                    else if (a == 7) montagem += "SETECENTOS";
                    else if (a == 8) montagem += "OITOCENTOS";
                    else if (a == 9) montagem += "NOVECENTOS";

                    if (b == 1)
                    {
                        if (c == 0) montagem += ((a > 0) ? " E " : string.Empty) + "DEZ";
                        else if (c == 1) montagem += ((a > 0) ? " E " : string.Empty) + "ONZE";
                        else if (c == 2) montagem += ((a > 0) ? " E " : string.Empty) + "DOZE";
                        else if (c == 3) montagem += ((a > 0) ? " E " : string.Empty) + "TREZE";
                        else if (c == 4) montagem += ((a > 0) ? " E " : string.Empty) + "QUATORZE";
                        else if (c == 5) montagem += ((a > 0) ? " E " : string.Empty) + "QUINZE";
                        else if (c == 6) montagem += ((a > 0) ? " E " : string.Empty) + "DEZESSEIS";
                        else if (c == 7) montagem += ((a > 0) ? " E " : string.Empty) + "DEZESSETE";
                        else if (c == 8) montagem += ((a > 0) ? " E " : string.Empty) + "DEZOITO";
                        else if (c == 9) montagem += ((a > 0) ? " E " : string.Empty) + "DEZENOVE";
                    }
                    else if (b == 2) montagem += ((a > 0) ? " E " : string.Empty) + "VINTE";
                    else if (b == 3) montagem += ((a > 0) ? " E " : string.Empty) + "TRINTA";
                    else if (b == 4) montagem += ((a > 0) ? " E " : string.Empty) + "QUARENTA";
                    else if (b == 5) montagem += ((a > 0) ? " E " : string.Empty) + "CINQUENTA";
                    else if (b == 6) montagem += ((a > 0) ? " E " : string.Empty) + "SESSENTA";
                    else if (b == 7) montagem += ((a > 0) ? " E " : string.Empty) + "SETENTA";
                    else if (b == 8) montagem += ((a > 0) ? " E " : string.Empty) + "OITENTA";
                    else if (b == 9) montagem += ((a > 0) ? " E " : string.Empty) + "NOVENTA";

                    if (strValor.Substring(1, 1) != "1" & c != 0 & montagem != string.Empty) montagem += " E ";

                    if (strValor.Substring(1, 1) != "1")
                        if (c == 1) montagem += "UM";
                        else if (c == 2) montagem += "DOIS";
                        else if (c == 3) montagem += "TRÊS";
                        else if (c == 4) montagem += "QUATRO";
                        else if (c == 5) montagem += "CINCO";
                        else if (c == 6) montagem += "SEIS";
                        else if (c == 7) montagem += "SETE";
                        else if (c == 8) montagem += "OITO";
                        else if (c == 9) montagem += "NOVE";

                    return montagem;
                }
            }

           
        }



        public static bool ValidaPis(string pis)
        {
            int[] multiplicador = new int[10] { 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;

            if (pis.Trim().Length == 0)
                return false;

            pis = pis.Trim();
            pis = pis.Replace("-", "").Replace(".", "").PadLeft(11, '0');

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(pis[i].ToString()) * multiplicador[i];

            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            return pis.EndsWith(resto.ToString());
        }

        public class DadosNavegador
        {
            
            public _Navegador Navegador { get; set; }

            public class _Navegador
            {
                public string Nome { get; set; }
                public string Versao { get; set; }
                public Boolean isBeta { get; set; }
                public Boolean isAOL { get; set; }
                public Boolean RastreadorWeb { get; set; }
            }

            public _Sistema Sistema { get; set; }

            public class _Sistema
            {
                public string Plataforma { get; set; }
                public string Win { get; set; }
            }

            public _Suporta Suporta { get; set; }

            public class _Suporta
            {
                public string VersaoJavaScript { get; set; }
                public Boolean Cookies { get; set; }
                public Boolean Frames { get; set; }
                public Boolean Tabelas { get; set; }
                public Boolean VBScript { get; set; }
                public Boolean JavaApplets { get; set; }
                public Boolean ActiveX { get; set; }
            }

            public _Resolucao Resolucao { get; set; }

            public class _Resolucao
            {
                public Int32 Altura { get; set; }
                public Int32 Largura { get; set; }
                public Int32 BitDepth { get; set; }
            }

            public _Mobile Mobile { get; set; }

            public class _Mobile
            {
                public Boolean isMobile { get; set; }
                public string Fabricante { get; set; }
                public string Modelo { get; set; }
            }

            /// <summary>
            /// Instancia a Classe e coleta os dados do navegador
            /// </summary>
            public DadosNavegador()
            {
                System.Web.HttpBrowserCapabilities browser = System.Web.HttpContext.Current.Request.Browser;

                this.Navegador = new _Navegador();
                this.Sistema = new _Sistema();
                this.Mobile = new _Mobile();
                this.Resolucao = new _Resolucao();

                this.Suporta = new _Suporta();

                this.Navegador.Nome = browser.Browser;
                this.Navegador.Versao = browser.Version;
                this.Navegador.isBeta = browser.Beta;
                this.Navegador.isAOL = browser.AOL;
                this.Navegador.RastreadorWeb = browser.Crawler;

                this.Sistema.Plataforma = browser.Platform;

                if (browser.Win16) { this.Sistema.Win = "16"; } else { this.Sistema.Win = "32"; }

                this.Mobile.isMobile = browser.IsMobileDevice;

                if (Mobile.isMobile)
                {
                    this.Mobile.Fabricante = browser.MobileDeviceManufacturer.Replace("Unknown", "Não Reconhecido");
                    this.Mobile.Modelo = browser.MobileDeviceModel.Replace("Unknown", "Não Reconhecido");
                }
                else
                {
                    this.Mobile.Fabricante = "";
                    this.Mobile.Modelo = "";
                }

                this.Resolucao.Altura = browser.ScreenPixelsWidth;
                this.Resolucao.Largura = browser.ScreenPixelsHeight;
                this.Resolucao.BitDepth = browser.ScreenBitDepth;

                this.Suporta.VersaoJavaScript = browser.EcmaScriptVersion.ToString();
                this.Suporta.VBScript = browser.VBScript;
                this.Suporta.JavaApplets = browser.JavaApplets;
                this.Suporta.Tabelas = browser.Tables;
                this.Suporta.Frames = browser.Frames;
                this.Suporta.Cookies = browser.Cookies;
                this.Suporta.ActiveX = browser.ActiveXControls;
            }

            /// <summary>
            /// Retorna uma string com dados resumidos do navegador e sistema 
            /// </summary>
            public string Resumo()
            {
                return string.Format("Navegador: {0}v{1}{2}, {3}Plataforma: {4} {5},Versão JS: {6}, Cookies: {7}, Resolução: {8}x{9}",
                        this.Navegador.Nome,
                        this.Navegador.Versao,                        
                        isBeta(),
                        isMobile(),                        
                        this.Sistema.Plataforma,
                        this.Sistema.Win,                        
                        this.Suporta.VersaoJavaScript,
                        this.Suporta.Cookies.ToString().ToLower().Replace("true", "Sim").Replace("false", "Não"),                        
                        this.Resolucao.Largura,
                        this.Resolucao.Altura);
            }

            /// <summary>
            /// Retorna string se versão do Navegador for Beta
            /// </summary>
            private string isBeta()
            {
                if (this.Navegador.isBeta)
                {
                    return " Beta";
                }

                return string.Empty;
            }

            /// <summary>
            /// Retorna string com dados do dispositivo movel se este for o caso
            /// </summary>
            private string isMobile()
            {
                if (this.Mobile.isMobile)
                {
                    return string.Format("Mobile Fab: {0}, Mobile Mod: {1}, ", this.Mobile.Fabricante, this.Mobile.Modelo);
                }

                return string.Empty;
            }
        }

        public class ModeloDocumentoFiscal
        {
            public string ID { get; set; }
            public string Descricao { get; set; }
            public List<ModeloDocumentoFiscal> MetodoModeloDoc()
            {
                return new List<ModeloDocumentoFiscal>
                {
                    new ModeloDocumentoFiscal { ID = "0", Descricao = " " },
                    new ModeloDocumentoFiscal { ID = "01", Descricao = "01 Nota Fiscal 1/1A" },
                    new ModeloDocumentoFiscal { ID = "1B", Descricao = "1B Nota Fiscal Avulsa" },
                    new ModeloDocumentoFiscal { ID = "02", Descricao = "02 Nota Fiscal de Venda a Consumidor" },
                    new ModeloDocumentoFiscal { ID = "2D", Descricao = "2D Cupom Fiscal" },
                    new ModeloDocumentoFiscal { ID = "2E", Descricao = "2E Cupom Fiscal Bilhete de Passagem" },
                    new ModeloDocumentoFiscal { ID = "04", Descricao = "04 Nota Fiscal de Produtor" },
                    new ModeloDocumentoFiscal { ID = "06", Descricao = "06 Nota Fiscal/Conta de Energia Elétrica" },
                    new ModeloDocumentoFiscal { ID = "07", Descricao = "07 Nota Fiscal de Serviço de Transporte" },
                    new ModeloDocumentoFiscal { ID = "08", Descricao = "08 Conhecimento de Transporte Rodoviário de Cargas" },
                    new ModeloDocumentoFiscal { ID = "8B", Descricao = "8B Conhecimento de Transporte de Cargas Avulso" },
                    new ModeloDocumentoFiscal { ID = "09", Descricao = "09 Conhecimento de Transporte Aquaviário de Cargas" },
                    new ModeloDocumentoFiscal { ID = "10", Descricao = "10 Conhecimento Aéreo" },
                    new ModeloDocumentoFiscal { ID = "11", Descricao = "11 Conhecimento de Transporte Ferroviário de Cargas" },
                    new ModeloDocumentoFiscal { ID = "13", Descricao = "13 Bilhete de Passagem Rodoviário" },
                    new ModeloDocumentoFiscal { ID = "14", Descricao = "14 Bilhete de Passagem Aquaviário" },
                    new ModeloDocumentoFiscal { ID = "15", Descricao = "15 Bilhete de Passagem e Nota de Bagagem" },
                    new ModeloDocumentoFiscal { ID = "16", Descricao = "16 Bilhete de Passagem Ferroviário" },
                    new ModeloDocumentoFiscal { ID = "17", Descricao = "17 Despacho de Transporte" },
                    new ModeloDocumentoFiscal { ID = "18", Descricao = "18 Resumo de Movimento Diário" },
                    new ModeloDocumentoFiscal { ID = "20", Descricao = "20 Ordem de Coleta de Cargas" },
                    new ModeloDocumentoFiscal { ID = "21", Descricao = "21 Nota Fiscal de Serviço de Comunicação" },
                    new ModeloDocumentoFiscal { ID = "22", Descricao = "22 Nota Fiscal de Serviço de Telecomunicação" },
                    new ModeloDocumentoFiscal { ID = "23", Descricao = "23 GNRE" },
                    new ModeloDocumentoFiscal { ID = "24", Descricao = "24 Autorização de Carregamento e Transporte" },
                    new ModeloDocumentoFiscal { ID = "25", Descricao = "25 Manifesto de Carga" },
                    new ModeloDocumentoFiscal { ID = "26", Descricao = "26 Conhecimento de Transporte Multimodal de Cargas" },
                    new ModeloDocumentoFiscal { ID = "27", Descricao = "27 Nota Fiscal De Transporte Ferroviário De Carga" },
                    new ModeloDocumentoFiscal { ID = "28", Descricao = "28 Nota Fiscal/Conta de Fornecimento de Gás Canalizado" },
                    new ModeloDocumentoFiscal { ID = "29", Descricao = "29 Nota Fiscal/Conta De Fornecimento de água Canalizada" },
                    new ModeloDocumentoFiscal { ID = "55", Descricao = "55 Nota Fiscal Eletrônica" },
                    new ModeloDocumentoFiscal { ID = "57", Descricao = "57 Conhecimento de Transporte Eletrônico - CT-e" },
                    new ModeloDocumentoFiscal { ID = "59", Descricao = "59 Cupom Fiscal Eletrônico - CF-e" }
                };
            }
        }


        public static string GetWorkGroup()
        {
            ManagementObject computer_system = new ManagementObject(string.Format("Win32_ComputerSystem.Name='{0}'", Environment.MachineName));
            object result = computer_system["Workgroup"];
            return result.ToString();
        }

        public static string GetWinUser()
        {
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            return userName;
        }

        public static string SD(string tabela, string keyField, string ResultField, string Where, string and)
        {
            string retorno = "";
            string select = "select "+ ResultField +"   from " + tabela + " where "+ keyField + " = " + Where;
            if (!string.IsNullOrEmpty(and))
            {
                select += " and " + and;
            }
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Properties.Settings.Default.Banco;
            SqlCommand cmd = new SqlCommand(select, con);
            cmd.CommandType = CommandType.Text;
            try
            {
                con.Open();
                SqlDataReader dR = cmd.ExecuteReader();
                while (dR.Read())
                {
                    retorno = dR[0].ToString();
                }
            }
            catch (Exception e)
            {
                e.ToString();
                throw;
            }
            finally
            {
                con.Close();
            }
            return retorno;
        }

        public static string UltimoDia(DateTime data)
        {
            string retorno = "";
            if(data != null)
            {
                var dataF = data.ToString("MM/yyyy");
                data = Convert.ToDateTime(dataF);
                retorno = data.AddMonths(1).AddDays(-1).ToShortDateString();
            }
            return retorno;
        }

        public static string FStr(string texto)// 
        {
            return "'" + texto + "'";
        }

        public static string GetMyIP()
        {
            string retorno = ". . . .";
            IPAddress[] ip = Dns.GetHostAddresses(LocalHostName());
            retorno = ip[1].ToString();
            return retorno;
        }

        public static string LocalHostName()
        {
            string retorno = "";
            retorno = Dns.GetHostName();
            return retorno;
        }

        public static DateTime GetServerDate()
        {
            DateTime retorno = DateTime.UtcNow;
            string select = "select GETDATE() as data";            
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Properties.Settings.Default.Banco;
            SqlCommand cmd = new SqlCommand(select, con);
            cmd.CommandType = CommandType.Text;
            try
            {
                con.Open();
                SqlDataReader dR = cmd.ExecuteReader();
                while (dR.Read())
                {
                    retorno = Convert.ToDateTime(dR[0].ToString());
                }
            }
            catch (Exception e)
            {
                e.ToString();
                throw;
            }
            finally
            {
                con.Close();
            }
            return retorno;
        }

        public static string GetMacAdress()
        {
            var firstMacAddress = NetworkInterface.GetAllNetworkInterfaces()
                .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .Select(nic => nic.GetPhysicalAddress().ToString()).FirstOrDefault();

            return firstMacAddress;
        }

        public static bool IsNCM(string ncm)
        {
            var retorno = true;
            if((ncm.Length != 8) && (ncm.Length != 2))
            {
                retorno = false;
            }            
            return retorno;

        }

        public static string SqlIn(string texto)
        {
            var textoFormat = "";
            if (!string.IsNullOrEmpty(texto))
            {
                while (texto.Contains(",,"))
                {
                    texto = texto.Replace(",,", ",");
                }
                textoFormat = texto.Substring(0, 1);
                if(textoFormat == ",")
                {
                    texto = texto.Substring(1, (texto.Length-1));
                }
                textoFormat = texto.Substring((texto.Length - 1), 1);
                if (textoFormat == ",")
                {
                    texto = texto.Substring(0, texto.Length - 2);
                }
                
            }
            return texto;
        }


    }
}
