namespace FirmaTransportowa
{
    partial class ModyfikujKierowcaa
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
            this.buttonZatwierdz = new System.Windows.Forms.Button();
            this.buttonAnuluj = new System.Windows.Forms.Button();
            this.labelTytul = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelStawka = new System.Windows.Forms.Label();
            this.tbPESEL = new System.Windows.Forms.TextBox();
            this.tbImie = new System.Windows.Forms.TextBox();
            this.tbNazwisko = new System.Windows.Forms.TextBox();
            this.tbStawka = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // buttonZatwierdz
            // 
            this.buttonZatwierdz.AutoSize = true;
            this.buttonZatwierdz.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonZatwierdz.Location = new System.Drawing.Point(349, 409);
            this.buttonZatwierdz.Margin = new System.Windows.Forms.Padding(4);
            this.buttonZatwierdz.Name = "buttonZatwierdz";
            this.buttonZatwierdz.Size = new System.Drawing.Size(129, 37);
            this.buttonZatwierdz.TabIndex = 4;
            this.buttonZatwierdz.Text = "Zatwierdź";
            this.buttonZatwierdz.UseVisualStyleBackColor = true;
            this.buttonZatwierdz.Click += new System.EventHandler(this.buttonZatwierdz_Click);
            // 
            // buttonAnuluj
            // 
            this.buttonAnuluj.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonAnuluj.Location = new System.Drawing.Point(16, 409);
            this.buttonAnuluj.Margin = new System.Windows.Forms.Padding(4);
            this.buttonAnuluj.Name = "buttonAnuluj";
            this.buttonAnuluj.Size = new System.Drawing.Size(119, 37);
            this.buttonAnuluj.TabIndex = 5;
            this.buttonAnuluj.Text = "Anuluj";
            this.buttonAnuluj.UseVisualStyleBackColor = true;
            this.buttonAnuluj.Click += new System.EventHandler(this.buttonAnuluj_Click);
            // 
            // labelTytul
            // 
            this.labelTytul.AutoSize = true;
            this.labelTytul.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelTytul.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelTytul.Location = new System.Drawing.Point(16, 11);
            this.labelTytul.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTytul.Name = "labelTytul";
            this.labelTytul.Size = new System.Drawing.Size(352, 31);
            this.labelTytul.TabIndex = 6;
            this.labelTytul.Text = "Dane kierowcy: imieNazwisko";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(152, 106);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "PESEL:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(172, 150);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "Imię:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(136, 199);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 17);
            this.label3.TabIndex = 9;
            this.label3.Text = "Nazwisko:";
            // 
            // labelStawka
            // 
            this.labelStawka.AutoSize = true;
            this.labelStawka.Location = new System.Drawing.Point(76, 250);
            this.labelStawka.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelStawka.Name = "labelStawka";
            this.labelStawka.Size = new System.Drawing.Size(131, 17);
            this.labelStawka.TabIndex = 10;
            this.labelStawka.Text = "Stawka miesięczna:";
            // 
            // tbPESEL
            // 
            this.tbPESEL.Location = new System.Drawing.Point(239, 102);
            this.tbPESEL.Margin = new System.Windows.Forms.Padding(4);
            this.tbPESEL.Name = "tbPESEL";
            this.tbPESEL.Size = new System.Drawing.Size(132, 22);
            this.tbPESEL.TabIndex = 11;
            // 
            // tbImie
            // 
            this.tbImie.Location = new System.Drawing.Point(239, 146);
            this.tbImie.Margin = new System.Windows.Forms.Padding(4);
            this.tbImie.Name = "tbImie";
            this.tbImie.Size = new System.Drawing.Size(132, 22);
            this.tbImie.TabIndex = 12;
            // 
            // tbNazwisko
            // 
            this.tbNazwisko.Location = new System.Drawing.Point(239, 196);
            this.tbNazwisko.Margin = new System.Windows.Forms.Padding(4);
            this.tbNazwisko.Name = "tbNazwisko";
            this.tbNazwisko.Size = new System.Drawing.Size(132, 22);
            this.tbNazwisko.TabIndex = 13;
            // 
            // tbStawka
            // 
            this.tbStawka.Location = new System.Drawing.Point(239, 246);
            this.tbStawka.Margin = new System.Windows.Forms.Padding(4);
            this.tbStawka.Name = "tbStawka";
            this.tbStawka.Size = new System.Drawing.Size(132, 22);
            this.tbStawka.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(95, 294);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 17);
            this.label4.TabIndex = 15;
            this.label4.Text = "Data Zwolnienia:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(157, 336);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 17);
            this.label5.TabIndex = 16;
            this.label5.Text = "Status:";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "aktywny",
            "chorobowe (L4)",
            "zwolniony"});
            this.comboBox1.Location = new System.Drawing.Point(239, 333);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(132, 24);
            this.comboBox1.TabIndex = 18;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(239, 294);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.ShowCheckBox = true;
            this.dateTimePicker1.Size = new System.Drawing.Size(132, 22);
            this.dateTimePicker1.TabIndex = 19;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // ModyfikujKierowcaa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 459);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbStawka);
            this.Controls.Add(this.tbNazwisko);
            this.Controls.Add(this.tbImie);
            this.Controls.Add(this.tbPESEL);
            this.Controls.Add(this.labelStawka);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelTytul);
            this.Controls.Add(this.buttonAnuluj);
            this.Controls.Add(this.buttonZatwierdz);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ModyfikujKierowcaa";
            this.Text = "Modyfikuj Kierowce";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonZatwierdz;
        private System.Windows.Forms.Button buttonAnuluj;
        private System.Windows.Forms.Label labelTytul;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelStawka;
        private System.Windows.Forms.TextBox tbPESEL;
        private System.Windows.Forms.TextBox tbImie;
        private System.Windows.Forms.TextBox tbNazwisko;
        private System.Windows.Forms.TextBox tbStawka;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
    }
}