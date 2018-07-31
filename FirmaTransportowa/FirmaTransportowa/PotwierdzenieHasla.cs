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
    public partial class PotwierdzenieHasla : Form
    {
        String conS = "";
        SqlConnection sqlC;
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rd;

        public PotwierdzenieHasla()
        {
            InitializeComponent();
            conS = Properties.Settings.Default.projektCS;
            sqlC = new SqlConnection(conS);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@id_klienta", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@haslo", SqlDbType.VarChar));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cmd.Connection = sqlC;
            cmd.CommandText = "select 1 from dbo.DaneLogowania where [id_klienta] = @id_klienta and [haslo] = @haslo";

            cmd.Parameters["@haslo"].Value = textBox1.Text;
            cmd.Parameters["@id_klienta"].Value = Properties.Settings.Default.id_klienta;
            sqlC.Open();
            rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                Properties.Settings.Default.poprawneHaslo = true;
                Close();
            }
            else
            {
                MessageBox.Show("Podano niepoprawne hasło.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Properties.Settings.Default.poprawneHaslo = false;
            }
            sqlC.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
