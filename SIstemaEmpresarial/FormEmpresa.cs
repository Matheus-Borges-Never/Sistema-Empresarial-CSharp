using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using RestSharp;
using RestSharp.Deserializers;

namespace SIstemaEmpresarial
{
    public partial class FormEmpresa : Form
    {

        //Referencia para conexão
        SqlConnection conexao = new SqlConnection(@"Data Source=DESKTOP-SC2GFN8;
            Initial Catalog=sistema;Integrated Security=True");
        public FormEmpresa()
        {
            InitializeComponent();
            txtEmpresa.Select();
        }

        private void Sistem_Click(object sender, EventArgs e)
        {

        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            if (conexao.State == ConnectionState.Closed)
            {
                conexao.Open();
            }
            if (conexao.State == ConnectionState.Open)
            {
                SqlCommand cm;
                string sql = "";
                sql = "insert into empresa values ('" + txtEmpresa.Text + "','" + txtSenha.Text + "','" + txtRua.Text + "','" + txtBairro.Text + "','" + txtComplemento.Text + "'," + mtbCep.Text + ",'" + txtNumero.Text + "'," + mtbTelefone.Text + ")";
                cm = new SqlCommand(sql, conexao);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cm;
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (cm.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Empresa inserida com sucesso!");
                    txtEmpresa.Text = "";
                    txtSenha.Text = "";
                    txtRua.Text = "";
                    txtBairro.Text = "";
                    txtComplemento.Text = "";
                    mtbCep.Text = "";
                    txtNumero.Text = "";
                    mtbTelefone.Text = "";

                }
                else
                {
                    MessageBox.Show("Usuario já existe!!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 index = new Form1(); //Chama  tela index
            this.Hide();
            index.Show();
        }

        private void txtCep_TextChanged(object sender, EventArgs e)
        {
        }

        //Daqui para baixo é a API que ainda falta ser finalizada

        private void mtbCep_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            if (mtbCep.TextLength == 8)
            {
                RestClient restClient = new RestClient(string.Format("https://viacep.com.br/ws/(0)/json/", mtbCep.Text));

                RestRequest restRequest = new RestRequest(Method.GET);

                IRestResponse restResponse = restClient.Execute(restRequest);

                if(restResponse.ContentLength <= -1 || restResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    MessageBox.Show("Houve um problema com sua requisiçao: " + restResponse.Content, "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }else
                {

                }
            }
        }

        private void txtEndereco_TextChanged(object sender, EventArgs e)
        {

        }
    }

    class DadosRetorno
    {
        public string ceo { get; set; }
        public string logradouro { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string localidade { get; set; }
        public string uf { get; set; }
        public string unidade { get; set; }
        public string ibge { get; set; }
        public string gia { get; set; }
    }
}
