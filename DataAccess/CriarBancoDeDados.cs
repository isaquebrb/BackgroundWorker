using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using Entity;

namespace DataAccess
{
    public class CriarBancoDeDados
    {
        protected SqlConnection Conexao;

        public CriarBancoDeDados(string strDiretorioDb)
        {
            CriarArquivoBanco(strDiretorioDb);
            CriarTabelasBanco();
        }

        #region Métodos
        /// <summary>
        /// Método responsável por criar o banco de dados (arquivo.mdf)
        /// </summary>
        /// <param name="strDiretorioDb">Caminho da pasta onde será criado o banco</param>
        private void CriarArquivoBanco(string strDiretorioDb)
        {
            try
            {
                CriarConexao("conexaoTemp");
                AbrirConexao();

                string strCommand = $@"CREATE DATABASE BackgroundWorker
                                        ON
                                        ( NAME = BackgroundWorker_db,
                                            FILENAME = '{strDiretorioDb}\\BackgroundWorker_db.mdf',
                                            SIZE = 10,
                                            MAXSIZE = 50,
                                            FILEGROWTH = 5)
                                        LOG ON
                                        (NAME = BackgroundWorker_log,
                                            FILENAME = '{strDiretorioDb}\\BackgroundWorker_log.ldf',
                                            SIZE = 5MB,
                                            MAXSIZE = 25MB,
                                            FILEGROWTH = 5MB)";


                using (SqlCommand sqlCommand = new SqlCommand(strCommand, Conexao))
                {
                    sqlCommand.CommandTimeout = 600;
                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                FecharConexao();
            }
        }

        private void CriarTabelasBanco()
        {
            try
            {
                CriarConexao("conexaoBackgroundWorker");
                AbrirConexao();

                string strCommand = @"CREATE TABLE Categorias (
                                        Id INT NOT NULL,
                                        Nome VARCHAR(50) NOT NULL,
                                        Descricao VARCHAR(100) NULL,
                                        PRIMARY KEY(Id))

                                    CREATE TABLE Produtos (
                                        Id INT NOT NULL,
                                        Descricao VARCHAR(100) NULL,
                                        Nome VARCHAR(50) NOT NULL,
                                        Preco DECIMAL (18,2) NULL,
                                        Quantidade INT NULL,
                                        CategoriaId INT NOT NULL,
                                        PRIMARY KEY (Id),
                                        FOREIGN KEY (CategoriaId) REFERENCES Categorias(Id))";

                using(SqlCommand sqlCommand = new SqlCommand(strCommand, Conexao))
                {
                    sqlCommand.CommandTimeout = 600;
                    sqlCommand.ExecuteNonQuery();
                }

            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                FecharConexao();
            }
        }

        private void PreencherTabelasBanco()
        {
            try
            {
                AbrirConexao();

                List<Produtos> lista = new List<Produtos>();
                string strCommand = @"INSERT INTO Categorias()
                                      VALUES";

                using(SqlCommand sqlCommand = new SqlCommand(strCommand, Conexao))
                {
                    sqlCommand.CommandTimeout = 600;
                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                FecharConexao();
            }
        }

        /// <summary>
        /// Método responsável por criar uma conexão com o banco de dados descrito na string connection presente no app.config
        /// </summary>
        protected void CriarConexao(string nomeConexao)
        {
            string strConnection = ConfigurationManager.ConnectionStrings[nomeConexao].ConnectionString;
            Conexao = new SqlConnection(strConnection);
        }

        /// <summary>
        /// Método responsável por iniciar a conexão com o banco
        /// </summary>
        protected void AbrirConexao()
        {
            if (Conexao.State == ConnectionState.Closed)
            {
                Conexao.Open();
            }
        }

        /// <summary>
        /// Método responsável por finalizar a conexão com o banco se ela estiver 
        /// </summary>
        protected void FecharConexao()
        {
            if (Conexao.State == ConnectionState.Open)
            {
                Conexao.Close();
            }
        }
        #endregion
    }

}
