using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using static SaudeWeb.Utils.User;

namespace SaudeWeb.Models
{
    [DbConfigurationType(typeof(DataContextConfiguration))]
    public class DataContext : DbContext
    {

        public DataContext()
           : base(Properties.Settings.Default.Banco)
        { }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Properties().Configure(c =>
            {
                c.HasColumnName(c.ClrPropertyInfo.Name.ToUpper());
            });
        }
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Agente> Agentes { get; set; }
        public DbSet<Cidade> Cidades { get; set; }
        public DbSet<Consulta> Consultas { get; set; }
        public DbSet<Consulta.ConsultaAgente> ConsultaAgentes { get; set; }
        public DbSet<Consulta.ConsultaExame> ConsultaExames { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Exame> Exames { get; set; }
        public DbSet<Funcao> Funcoes { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Risco> Riscos { get; set; }
        public DbSet<Setor> Setor { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<TabelaPreco> TabPreco { get; set; }
        public DbSet<Financeiro> RecPag { get; set; }
        public DbSet<TabelaPreco.TabelaPrecoExame> TabExame { get; set; }
        public DbSet<Agenda> Agenda { get; set; }
        public DbSet<SaudeWeb.Utils.db2DataContext> db2DataContext { get; set; }
        public DbSet<PlanoConta> PlanoContas { get; set; }
        public DbSet<ContaCorrenteCaixa> ContaCorrenteCaixas { get; set; }
        public DbSet<Baixa> Baixas { get; set; }
        public DbSet<BancoCaixa> BancoCaixa { get; set; }

        public DbSet<Boleto> Boletos { get; set; }
    }
    class DataContextConfiguration : DbConfiguration
    {
        public DataContextConfiguration()
        {
            SetDatabaseInitializer<DataContext>(null);
        }
    }

    public class Exec
    {
        public bool ExecutarComandoSql(string comando)
        {
            bool retorno = true;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Properties.Settings.Default.Banco;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();                
            }
            catch (System.Exception)
            {
                retorno = false;
                con.Close();
                return false;
            }
            finally
            {
                con.Close();                
            }
            return retorno;
            
        }

    }

}