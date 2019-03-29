using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaudeDesktop
{
    public partial class Form1 : Form
    {
        

        public Form1()
        {
            InitializeComponent();
        }
        private static readonly HttpClient client = new HttpClient();

        private async void button1_ClickAsync(object sender, EventArgs e)
        {
             
            var values = new Dictionary<string, string> { { "Descricao", textBox1.Text } };
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("http://localhost:60481/Setores/Cadastro", content);
            var responseString = await response.Content.ReadAsStringAsync();
            
        }
    }
}
