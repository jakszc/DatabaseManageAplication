namespace FirmaTransportowa
{
    partial class ZamowieniaKlienta
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.listaZamowienBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.zamowienie_listaZamowien = new FirmaTransportowa.Zamowienie_listaZamowien();
            this.listaZamowienTableAdapter = new FirmaTransportowa.Zamowienie_listaZamowienTableAdapters.listaZamowienTableAdapter();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.podglądZamówieniaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listaZamowienBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zamowienie_listaZamowien)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(4, 4);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(1128, 633);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseDown);
            // 
            // listaZamowienBindingSource
            // 
            this.listaZamowienBindingSource.DataMember = "listaZamowien";
            this.listaZamowienBindingSource.DataSource = this.zamowienie_listaZamowien;
            // 
            // zamowienie_listaZamowien
            // 
            this.zamowienie_listaZamowien.DataSetName = "Zamowienie_listaZamowien";
            this.zamowienie_listaZamowien.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // listaZamowienTableAdapter
            // 
            this.listaZamowienTableAdapter.ClearBeforeFill = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.podglądZamówieniaToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(218, 28);
            // 
            // podglądZamówieniaToolStripMenuItem
            // 
            this.podglądZamówieniaToolStripMenuItem.Name = "podglądZamówieniaToolStripMenuItem";
            this.podglądZamówieniaToolStripMenuItem.Size = new System.Drawing.Size(217, 24);
            this.podglądZamówieniaToolStripMenuItem.Text = "Podgląd zamówienia";
            this.podglądZamówieniaToolStripMenuItem.Click += new System.EventHandler(this.podglądZamówieniaToolStripMenuItem_Click);
            // 
            // ZamowieniaKlienta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1133, 639);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.dataGridView1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ZamowieniaKlienta";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listaZamowienBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zamowienie_listaZamowien)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource listaZamowienBindingSource;
        private Zamowienie_listaZamowien zamowienie_listaZamowien;
        private Zamowienie_listaZamowienTableAdapters.listaZamowienTableAdapter listaZamowienTableAdapter;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem podglądZamówieniaToolStripMenuItem;
    }
}