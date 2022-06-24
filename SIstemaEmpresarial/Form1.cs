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

namespace SIstemaEmpresarial
{
    public partial class Form1 : Form
    {
        //Referencia para conexão
        SqlConnection conexao = new SqlConnection(@"Data Source=DESKTOP-SC2GFN8;
            Initial Catalog=sistema;Integrated Security=True");

        public Form1()
        {
            InitializeComponent();
            txtEmpresa.Select();
        }

        private void btnAcessar_Click(object sender, EventArgs e)
        {
            if (conexao.State == ConnectionState.Closed)
            {
                conexao.Open();     //Abri conexão com o BD
            }
            if (conexao.State == ConnectionState.Open)
            {
                SqlCommand cm;
                string query = "select * from empresa where nome = '" + txtEmpresa.Text + "' and senha = '" + txtSenha.Text + "' ";
                cm = new SqlCommand(query, conexao);
                SqlDataAdapter da = new SqlDataAdapter();
                cm.ExecuteNonQuery();
                da.SelectCommand = cm;
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("Acesso efetuado!! ");
                    mostrar();
                    conexao.Close(); //Fecha conexao com o BD
                }
                else
                {
                    MessageBox.Show("Usuario incorreto ou não existe!!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtEmpresa.Text = "";   //Limpa a textbox apos o erro
                    txtSenha.PasswordChar = '*';    //Limpa a textbox apos o erro
                    txtEmpresa.Select();    //Seleciona a textbox
                }
            }
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            FormEmpresa empresa = new FormEmpresa(); //Chama  tela de cadastro de empresa
            this.Hide();
            empresa.Show();
        }

        //Inserir Funcionario
        private void btnInserir_Click(object sender, EventArgs e)
        {
            if (conexao.State == ConnectionState.Closed)
            {
                conexao.Open();     //Abri conexão com o BD
            }
            if (conexao.State == ConnectionState.Open)
            {
                SqlCommand cm;
                string query = "insert into funcionario values('" + txtNome.Text + "','" + cmbCargo.Text + "'," + txtSalario.Text + "," + lblEmpresa.Text + ")";
                cm = new SqlCommand(query, conexao);
                SqlDataAdapter da = new SqlDataAdapter();
                cm.ExecuteNonQuery();
                da.SelectCommand = cm;
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (cm.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Funcionario Inserido com Sucesso");
                    conexao.Close(); //Fecha conexao com o BD
                }
                else
                {
                    MessageBox.Show("Erro");
                }
            }

        }
        private void mostrar()
        {
            SqlCommand cm;
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable tb = new DataTable();
            string sql = "select * from empresa where nome = '" + txtEmpresa.Text + "' and senha = '" + txtSenha.Text + "' ";
            cm = new SqlCommand(sql, conexao);
            da.SelectCommand = cm;
            da.Fill(tb);
            dtgBuscar.DataSource = null;
            dtgBuscar.DataSource = tb;

            if (dtgBuscar.Rows.Count > 0)
            {
                lblEmpresa.Text = dtgBuscar.CurrentRow.Cells[0].Value.ToString();
            }
        }

        private void mostrarFun()
        {
            SqlCommand cm;
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable tb = new DataTable();
            string sql = "select f.id_funcionario, f.nome, f.cargo, f.salario from empresa e inner join funcionario f on e.id_empresa = f.id_empresa where e.id_empresa = " + lblEmpresa.Text + "";
            cm = new SqlCommand(sql, conexao);
            da.SelectCommand = cm;
            da.Fill(tb);
            dtgBuscar.DataSource = null;
            dtgBuscar.DataSource = tb;

        }

        private void dtgBuscar_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void lblEmpresa_Click(object sender, EventArgs e)
        {
        }

        private void dtgBuscar_Click(object sender, EventArgs e)
        {
            if (dtgBuscar.Rows.Count > 0)
            {
                lblId.Text = dtgBuscar.CurrentRow.Cells[0].Value.ToString();
                txtAtNome.Text = dtgBuscar.CurrentRow.Cells[1].Value.ToString();
                cbbAtCargo.Text = dtgBuscar.CurrentRow.Cells[2].Value.ToString();
                txtAtSalario.Text = dtgBuscar.CurrentRow.Cells[3].Value.ToString();
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            mostrarFun();
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            if (conexao.State == ConnectionState.Closed)
            {
                conexao.Open();     //Abri conexão com o BD
            }
            if (conexao.State == ConnectionState.Open)
            {
                SqlCommand cm;
                string sql = "";
                sql = "update funcionario set nome=@NOME, cargo=@CARGO, salario=@SALARIO where id_funcionario=@ID";
                cm = new SqlCommand(sql, conexao);

                cm.Parameters.Add("@NOME", SqlDbType.VarChar).Value = txtAtNome.Text;
                cm.Parameters.Add("@CARGO", SqlDbType.VarChar).Value = cbbAtCargo.Text;
                cm.Parameters.Add("@SALARIO", SqlDbType.Decimal).Value = txtAtSalario.Text;
                cm.Parameters.Add("@ID", SqlDbType.BigInt).Value = lblId.Text;

                int ret = cm.ExecuteNonQuery();
                if (ret > 0)
                {
                    MessageBox.Show("O Funcionario foi alterado com sucesso!");
                    mostrarFun();

                }
                else
                {
                    MessageBox.Show("Erro");
                }
            }

        }
    }
}
