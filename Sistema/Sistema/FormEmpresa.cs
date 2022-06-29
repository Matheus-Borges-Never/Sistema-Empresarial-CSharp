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
using System.Text.RegularExpressions;
using System.Net;
using System.Net.WebSockets;
using System.IO;

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

        private void mtbCep_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
        }

        private void btnCep_Click(object sender, EventArgs e)
        {
            //API ViaCep

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://viacep.com.br/ws/" + mtbCep.Text + "/json/");
            request.AllowAutoRedirect = false;
            HttpWebResponse ChecaServidor = (HttpWebResponse)request.GetResponse();

            if (ChecaServidor.StatusCode != HttpStatusCode.OK)
            {
                MessageBox.Show("Servidor indisponível");
                return; // Sai da rotina
            }

            using (Stream webStream = ChecaServidor.GetResponseStream())
            {
                if (webStream != null)
                {
                    using (StreamReader responseReader = new StreamReader(webStream))
                    {
                        string response = responseReader.ReadToEnd();
                        response = Regex.Replace(response, "[{},]", string.Empty);
                        response = response.Replace("\"", "");

                        String[] substrings = response.Split('\n');

                        int cont = 0;
                        foreach (var substring in substrings)
                        {
                            if (cont == 1)
                            {
                                string[] valor = substring.Split(":".ToCharArray());
                                if (valor[0] == "  erro")
                                {
                                    MessageBox.Show("CEP não encontrado");
                                    mtbCep.Focus();
                                    return;
                                }
                            }

                            //Rua
                            if (cont == 2)
                            {
                                string[] valor = substring.Split(":".ToCharArray());
                                txtRua.Text = valor[1];
                            }

                            //Complemento
                            if (cont == 3)
                            {
                                string[] valor = substring.Split(":".ToCharArray());
                                txtComplemento.Text = valor[1];
                            }

                            //Bairro
                            if (cont == 4)
                            {
                                string[] valor = substring.Split(":".ToCharArray());
                                txtBairro.Text = valor[1];
                            }

                            cont++;
                        }
                    }
                }
            }
        }
    }
}
