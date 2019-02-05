using DataAccess;
using System;
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
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnProdutos = new System.Windows.Forms.Button();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.lblProgressBar = new System.Windows.Forms.Label();
            this.btnCancelar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(50, 393);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(167, 23);
            this.progressBar.TabIndex = 0;
            // 
            // btnProdutos
            // 
            this.btnProdutos.Location = new System.Drawing.Point(235, 393);
            this.btnProdutos.Name = "btnProdutos";
            this.btnProdutos.Size = new System.Drawing.Size(105, 23);
            this.btnProdutos.TabIndex = 1;
            this.btnProdutos.Text = "Mostrar Produtos";
            this.btnProdutos.UseVisualStyleBackColor = true;
            this.btnProdutos.Click += new System.EventHandler(this.btnProdutos_Click);
            // 
            // dataGridView
            // 
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(39, 44);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(708, 289);
            this.dataGridView.TabIndex = 2;
            // 
            // lblProgressBar
            // 
            this.lblProgressBar.AutoSize = true;
            this.lblProgressBar.Location = new System.Drawing.Point(47, 419);
            this.lblProgressBar.Name = "lblProgressBar";
            this.lblProgressBar.Size = new System.Drawing.Size(54, 13);
            this.lblProgressBar.TabIndex = 3;
            this.lblProgressBar.Text = "Progresso";
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(357, 393);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 4;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // ProdutosForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.lblProgressBar);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.btnProdutos);
            this.Controls.Add(this.progressBar);
            this.Name = "ProdutosForm";
            this.Text = "Formulário de Produtos";
            this.Load += new System.EventHandler(this.ProdutosForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private void InitializeDataBase()
        {
            string strDiretorioSolution = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string strDiretorioDb = $"{strDiretorioSolution}\\DataAccess\\BancoDeDados";

            //Configura o DataDirectory para o diretório do projeto
            AppDomain.CurrentDomain.SetData("DataDirectory", strDiretorioSolution);

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

                Infra createDb = new Infra(strDiretorioDb);
            }
        }

        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnProdutos;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Label lblProgressBar;
        private System.Windows.Forms.Button btnCancelar;
    }
}

