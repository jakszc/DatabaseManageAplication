using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FirmaTransportowa
{
    public partial class Logowanie : Form
    {
        public Logowanie()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            Rejestracja form = new Rejestracja();
            form.ShowDialog();
            form = null;
            Show();
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String conS = Properties.Settings.Default.projektCS;
            SqlConnection sqlC = new SqlConnection(conS);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader rd;

            cmd.Connection = sqlC;
            cmd.CommandText = "dbo.sprawdzDane";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@login", textBox1.Text));
            cmd.Parameters.Add(new SqlParameter("@password", textBox2.Text));

            sqlC.Open();
            rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                Kontener.login = textBox1.Text;
                String przywileje = rd[0].ToString();
                if (przywileje.Equals("user"))
                {
                    Kontener.przywilej = "user";
                    Properties.Settings.Default.id_klienta = Convert.ToInt32(rd[1].ToString());
                    Hide();
                    Klient form = new Klient();
                    form.ShowDialog();
                    form = null;
                    Application.Exit();
                }
                else if (przywileje.Equals("admin"))
                {
                    Kontener.przywilej = "admin";
                    Hide();
                    Admin form = new Admin();
                    form.ShowDialog();
                    form = null;
                    Show();
                }
                else if (przywileje.Equals("superadmin"))
                {
                    Kontener.przywilej = "superadmin";
                    Hide();
                    Admin form = new Admin();
                    form.ShowDialog();
                    form = null;
                    Show();
                }
            }
            else
            {
                MessageBox.Show("Podano zły login/hasło lub konto jest nieaktywne","Nieprawidłowe dane logowania!",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            textBox2.Clear();
            sqlC.Close();
        }
    }
    
}
