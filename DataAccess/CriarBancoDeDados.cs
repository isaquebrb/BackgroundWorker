using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace DataAccess
{
    public class CriarBancoDeDados
    {
        protected SqlConnection Conexao;

        public CriarBancoDeDados(string strDiretorioDb)
        {
            CriarConexao();
            CriarArquivoBanco(strDiretorioDb);
        }

        private void CriarArquivoBanco(string strDiretorioDb)
        {
            try
            {
                AbrirConexao();

                string strCommand = $@"CREATE DATABASE BackgroundWorker  
                                        ON   
                                        ( NAME = BackgroundWorker_db,  
                                            FILENAME = '{strDiretorioDb}\\BackgroundWorker_db.mdf',  
                                            SIZE = 10,  
                                            MAXSIZE = 50,  
                                            FILEGROWTH = 5 )  
                                        LOG ON  
                                        (NAME = BackgroundWorker_log,  
                                            FILENAME = '{strDiretorioDb}\\BackgroundWorker_log.ldf',  
                                            SIZE = 5MB,  
                                            MAXSIZE = 25MB,  
                                            FILEGROWTH = 5MB)";

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

        #region Métodos
        protected void CriarConexao()
        {
            string strConnection = ConfigurationManager.ConnectionStrings["connectionToDB"].ConnectionString;

            Conexao = new SqlConnection(strConnection);
        }

        protected void AbrirConexao()
        {
            if(Conexao.State == ConnectionState.Closed)
            {
                Conexao.Open();
            }
        }

        protected void FecharConexao()
        {
            if(Conexao.State == ConnectionState.Open)
            {
                Conexao.Close();
            }

        }
        #endregion
    }

}
