using DataAccess;
using Entity;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading;
using System.Windows.Forms;

namespace CategoriasForm
{
    public partial class ProdutosForm : Form
    {
        bool _bancoExiste;
        private int _totalRegistros;
        string _nomeStrConexao = string.Empty;

        public ProdutosForm()
        {
            InitializeComponent();
            InitializeDataBase();
            backgroundWorker.WorkerReportsProgress = true;
        }

        private void ProdutosForm_Load(object sender, EventArgs e)
        {
            btnCancelar.Visible = false;
            btnCancelar.Enabled = false;
            progressBar.Visible = false;
            lblProgressBar.Visible = false;

            dataGridView.ColumnCount = 5;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView.Columns[0].Name = "Nome";
            dataGridView.Columns[1].Name = "Descrição";
            dataGridView.Columns[2].Name = "Preço";
            dataGridView.Columns[3].Name = "Quantidade";
            dataGridView.Columns[4].Name = "Categoria";
            dataGridView.RowHeadersWidth = 21;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridView.ColumnHeadersHeight = 23;

            _bancoExiste = CriarBancoDeDados.verificaSeExisteBanco();

            if (_bancoExiste)
                _nomeStrConexao = "conexaoDB";
            else
                _nomeStrConexao = "newConexaoDB";
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Produtos produto = (Produtos)e.Argument;
            Categorias categorias = new Categorias();
            produto.Categorias = categorias;
            
            string strConnection = ConfigurationManager.ConnectionStrings[_nomeStrConexao].ConnectionString;
            string strCommand = @"SELECT P.Nome, P.Descricao, P.Preco, P.Quantidade, C.Nome as 'Categoria'
                                    FROM Produtos P
                                    LEFT JOIN Categorias C ON P.CategoriaId = C.Id";
            SqlDataReader reader;
            int i = 1;

            try
            {
                using (SqlConnection Conexao = new SqlConnection(strConnection))
                {
                    SqlCommand sqlCommand = new SqlCommand(strCommand, Conexao);

                    Conexao.Open();
                    reader = sqlCommand.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            produto.Nome = reader["Nome"].ToString().Trim();
                            produto.Descricao = reader["Descricao"].ToString().Trim();
                            produto.Preco = decimal.TryParse(reader["Preco"].ToString(), out decimal decimalResult) ? decimalResult : 0;
                            produto.Quantidade = int.TryParse(reader["Quantidade"].ToString(), out int intResult) ? intResult : 0;
                            produto.Categorias.Nome = reader["Categoria"].ToString().Trim();
                            //Adicionando uma pausa de 2 segundos no processamento
                            Thread.Sleep(2000);
                            //Relatório do progresso
                            backgroundWorker.ReportProgress(i,produto);

                            if (backgroundWorker.CancellationPending)
                            {
                                e.Cancel = true;
                                backgroundWorker.ReportProgress(0);
                                return;
                            }
                            i += 1;
                        }
                    }
                    Conexao.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                lblProgressBar.Text = "Cancelado pelo usuário.";
                progressBar.Value = 0;
            }
            else if(e.Error != null)
            {
                lblProgressBar.Text = e.Error.Message;
            }
            else
            {
                lblProgressBar.Text = "Todos os registros foram carregados.";
            }
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (!backgroundWorker.CancellationPending)
            {
                // Obtem o status do usuário que foi enviado como parte do método ReportProgress() a partir do evento DoWork
                Produtos produto = (Produtos)e.UserState;
                //Incluindo os dados no gridview
                dataGridView.Rows.Add(produto.Nome, produto.Descricao, produto.Preco, produto.Quantidade, produto.Categorias.Nome);
                progressBar.Value = e.ProgressPercentage;
                lblProgressBar.Text = $"Processando linha...{e.ProgressPercentage.ToString()} de {_totalRegistros}";
            }
        }

        private void btnProdutos_Click(object sender, EventArgs e)
        {
            progressBar.Visible = true;
            lblProgressBar.Visible = true;
            progressBar.Maximum = TotalRegistros();

            if (!backgroundWorker.IsBusy)
            {
                Produtos produto = new Produtos();
                dataGridView.Rows.Clear();
                //Iniciando a Thread em background
                backgroundWorker.RunWorkerAsync(produto);
                btnProdutos.Enabled = false;
                btnCancelar.Enabled = true;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
                btnCancelar.Enabled = false;
                btnProdutos.Enabled = true;
            }
        }

        private int TotalRegistros()
        {
            string strConnection = ConfigurationManager.ConnectionStrings[_nomeStrConexao].ConnectionString;
            SqlConnection Conexao = new SqlConnection(strConnection);

            try
            {
                Conexao.Open();
                using (SqlCommand sqlComannd = new SqlCommand("SELECT COUNT(*) FROM Produtos", Conexao))
                {
                    _totalRegistros = int.TryParse(sqlComannd.ExecuteScalar().ToString(), out int result) ? result : 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Conexao.Close();
            }
            return _totalRegistros;
        }
    }
}
