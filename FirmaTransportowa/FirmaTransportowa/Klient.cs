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
    public partial class Klient : Form
    {
        String conS;
        SqlConnection sqlC;
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rd;

        public Klient()
        {
            InitializeComponent();
            conS = Properties.Settings.Default.projektCS;
            sqlC = new SqlConnection(conS);
            cmd.Parameters.Add(new SqlParameter("@id_klienta", Properties.Settings.Default.id_klienta));
            powitanie();
        }

        private void powitanie()
        {
            cmd.Connection = sqlC;
            cmd.CommandText = "select [imie], [nazwisko], [nazwa_firmy], [NIP] from dbo.Klient where [id_klienta] = @id_klienta";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters["@id_klienta"].Value = Properties.Settings.Default.id_klienta;
            sqlC.Open();
            rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                if (rd[0].ToString().Length == 0)
                {
                    label1.Text = "Witamy pracownika firmy " + rd[2].ToString() + "!";
                }
                else
                {
                    label1.Text = "Witaj, " + rd[0].ToString() + " " + rd[1].ToString() + "!";
                }
            }
            sqlC.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("To spowoduje wylogowanie się. Czy chcesz kontynuować?", "Uwaga!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            NoweZamowienie form = new NoweZamowienie();
            form.ShowDialog();
            form = null;
            Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            ZamowieniaKlienta form = new ZamowieniaKlienta();
            form.ShowDialog();
            form = null;
            Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Hide();
            KontoKlienta form = new KontoKlienta();
            form.ShowDialog();
            form = null;
            powitanie();
            Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
