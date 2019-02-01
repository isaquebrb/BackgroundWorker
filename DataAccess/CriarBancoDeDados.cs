using Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    public class CriarBancoDeDados
    {
        protected SqlConnection Conexao;

        public CriarBancoDeDados(string strDiretorioDb)
        {
            CriarArquivoBanco(strDiretorioDb);
            CriarTabelaBanco();
            CriarEntidades();
        }

        private void CriarArquivoBanco(string strDiretorioDb)
        {
            try
            {
                CriarConexao("conexaoInicialDB");
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

        private void CriarTabelaBanco()
        {
            try
            {
                CriarConexao("conexaoDB");
                AbrirConexao();

                string strCommand = @"CREATE TABLE Categorias (
                                        Id int NOT NULL PRIMARY KEY,
                                        Nome char(50) NOT NULL,
                                        Descricao varchar(255)
                                        );
                                    CREATE TABLE Produtos (
                                        Id Int NOT NULL PRIMARY KEY,
                                        Nome char(50) NOT NULL,
                                        Descricao varchar(255),
                                        Preco Decimal(18,2) NOT NULL,
                                        Quantidade Int NOT NULL,
                                        CategoriaId Int FOREIGN KEY REFERENCES Categorias(Id)
                                    );";

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

            }
        }

        private void PreencherTabelas<T>(T entidade)
        {
            try
            {
                //if(entidade is Categorias)
                if (entidade.GetType() == typeof(Categorias))
                {
                    //Categorias categoria = entidade as Categorias;
                    Categorias categoria = (Categorias)(object)entidade;

                    string strCommmand = @"INSERT INTO Categorias (Id, Nome, Descricao)
                                            VALUES (@Id, @Nome, @Descricao)";

                    using (SqlCommand sqlCommand = new SqlCommand(strCommmand, Conexao))
                    {
                        sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = categoria.Id;
                        sqlCommand.Parameters.Add("@Nome", SqlDbType.Char, 50).Value = categoria.Nome;
                        sqlCommand.Parameters.Add("@Descricao", SqlDbType.VarChar, 255).Value = categoria.Descricao;
                        sqlCommand.CommandTimeout = 600;
                        sqlCommand.ExecuteNonQuery();
                    }
                }
                else if (entidade.GetType() == typeof(Produtos))
                {
                    Produtos produtos = (Produtos)(object)entidade;

                    string strCommmand = @"INSERT INTO Produtos (Id, Nome, Descricao, Preco, Quantidade, CategoriaId)
                                            VALUES(@Id, @Nome, @Descricao, @Preco, @Quantidade, @CategoriaId)";

                    using (SqlCommand sqlCommand = new SqlCommand(strCommmand, Conexao))
                    {
                        sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = produtos.Id;
                        sqlCommand.Parameters.Add("@Nome", SqlDbType.Char, 50).Value = produtos.Nome;
                        sqlCommand.Parameters.Add("@Descricao", SqlDbType.VarChar, 255).Value = produtos.Descricao;
                        sqlCommand.Parameters.Add("@Preco", SqlDbType.Decimal).Value = produtos.Preco;
                        sqlCommand.Parameters.Add("@Quantidade", SqlDbType.Int).Value = produtos.Quantidade;
                        sqlCommand.Parameters.Add("@CategoriaId", SqlDbType.Int).Value = produtos.CategoriaId;
                        sqlCommand.CommandTimeout = 600;
                        sqlCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void CriarEntidades()
        {
            try
            {
                IList<Categorias> listCategorias = new List<Categorias>();
                IList<Produtos> listProdutos = new List<Produtos>();

                listCategorias.Add(new Categorias { Id = 1, Nome = "Informática", Descricao = "Produtos relacionados a itens gamers e de computação." });
                listCategorias.Add(new Categorias { Id = 2, Nome = "Brinquedos", Descricao = "Produtos relacionados a itens destinados a divertir crianças." });
                listCategorias.Add(new Categorias { Id = 3, Nome = "Livros", Descricao = "Produtos relacionados a leitura." });
                listCategorias.Add(new Categorias { Id = 4, Nome = "Eletrodomésticos", Descricao = "Produtos relacionados a itens elétricos de uso doméstico." });

                listProdutos.Add(new Produtos { Id = 1, Nome = "Controle XBOX Wireless", Descricao = "Controle Microsoft XBOX 360 Conexão Wireless", Quantidade = 10, Preco = 100.99M, CategoriaId = 1 });
                listProdutos.Add(new Produtos { Id = 2, Nome = "Notebook Dell Latitude 5480", Descricao = "Notebook Dell i5 16GB RAM Latitude 5480", Quantidade = 3, Preco = 4400.99M, CategoriaId = 1 });
                listProdutos.Add(new Produtos { Id = 3, Nome = "Toy Jake Amarelo", Descricao = "Toy Jake Amarelo Plástico e Borracha", Quantidade = 150, Preco = 10.49M, CategoriaId = 2 });
                listProdutos.Add(new Produtos { Id = 4, Nome = "Livro O Silmarillion", Descricao = "Livro O Silmarillion J. R. R. Tolkien", Quantidade = 7, Preco = 60.20M, CategoriaId = 3 });
                listProdutos.Add(new Produtos { Id = 5, Nome = "TV LG 42 LM5600", Descricao = "TV LG 42 LM5600 FULLHD SMART 220V", Quantidade = 3, Preco = 2200.99M, CategoriaId = 4 });
                listProdutos.Add(new Produtos { Id = 6, Nome = "Geladeira Consul CM45K00", Descricao = "Geladeira Consul CM45K00 INOX 220V 475L", Quantidade = 6, Preco = 2300.29M, CategoriaId = 4 });

                AbrirConexao();

                foreach (Categorias categoria in listCategorias)
                {
                    PreencherTabelas(categoria);
                }

                foreach (Produtos produto in listProdutos)
                {
                    PreencherTabelas(produto);
                }

                FecharConexao();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Métodos Úteis
        protected void CriarConexao(string strConexao)
        {
            string strConnection = ConfigurationManager.ConnectionStrings[strConexao].ConnectionString;

            Conexao = new SqlConnection(strConnection);
        }

        protected void AbrirConexao()
        {
            if (Conexao.State == ConnectionState.Closed)
            {
                Conexao.Open();
            }
        }

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
