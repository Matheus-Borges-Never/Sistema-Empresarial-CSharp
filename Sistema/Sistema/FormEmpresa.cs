using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Sistema
{
    public partial class FormEmpresa : Form
    {
        //Referencia para conexão
        SqlConnection conexao = new SqlConnection(@"Data Source = DESKTOP-SC2GFN8; Initial Catalog = sistemaEmpresarial; Integrated Security = True");

        public FormEmpresa()
        {
            InitializeComponent();
            txtEmpresa.Select();
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
                sql = "insert into empresa values('" + txtEmpresa.Text + "','" + txtSenha.Text + "','" + txtRua.Text + "','" + txtBairro.Text + "','" + txtComplemento.Text + "','" + mtbCep.Text + "','" + txtNumero.Text + "','" + mtbTelefone.Text + "')";
                cm = new SqlCommand(sql, conexao);

                int ret = cm.ExecuteNonQuery();
                if (ret > 0)
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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Form1 index = new Form1(); //Chama  tela index
            this.Hide();
            index.Show();
        }
    }
}
