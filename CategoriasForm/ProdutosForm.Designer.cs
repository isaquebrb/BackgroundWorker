using DataAccess;
using System.IO;
using System.Security.AccessControl;

namespace CategoriasForm
{
    partial class ProdutosForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "Formulário de Produtos";
        }

        #endregion

        private void InitializeDataBase()
        {
            string strDiretorioSolution = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string strDiretorioDb = $"{strDiretorioSolution}\\DataAccess\\BancoDeDados";

            DirectoryInfo diretorioDb = new DirectoryInfo(strDiretorioDb);

            if (!Directory.Exists(strDiretorioDb))
            {
                diretorioDb = Directory.CreateDirectory(strDiretorioDb);
            }

            string[] arquivos = Directory.GetFiles(strDiretorioDb, "*.mdf");

            if (arquivos.Length == 0)
            {
                DirectorySecurity segurancaDiretorio = diretorioDb.GetAccessControl();

                FileSystemAccessRule permissaoUsuario1 = new FileSystemAccessRule("NT Service\\MSSQLSERVER", FileSystemRights.Modify, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow);
                segurancaDiretorio.AddAccessRule(permissaoUsuario1);
                diretorioDb.SetAccessControl(segurancaDiretorio);

                FileSystemAccessRule permissaoUsuario2 = new FileSystemAccessRule("NT Service\\MSSQL$SQLEXPRESS", FileSystemRights.Modify, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow);
                segurancaDiretorio.AddAccessRule(permissaoUsuario2);
                diretorioDb.SetAccessControl(segurancaDiretorio);

                CriarBancoDeDados createDb = new CriarBancoDeDados(strDiretorioDb);
            }
        }
    }
}

