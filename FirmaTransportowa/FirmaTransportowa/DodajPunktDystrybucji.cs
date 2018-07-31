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
    public partial class DodajPunktDystrybucji : Form
    {
        String conS = Properties.Settings.Default.projektCS;
        SqlConnection sqlC;
        SqlCommand cmd;
        SqlDataReader rd;
        public DodajPunktDystrybucji()
        {
            InitializeComponent();
            sqlC = new SqlConnection(conS);
            cmd = new SqlCommand();
            cmd.Connection = sqlC;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals("") || textBox2.Text.Equals(""))
            {
                MessageBox.Show("Formularz zawiera puste pola.", "Puste pola.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if(textBox1.Text.Length > 50 || textBox2.Text.Length > 100)
            {
                MessageBox.Show("Pola miasto i adres mogą mieć maksymalnie odpowiednio 50 i 100 znaków", "Za dużo znaków.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                cmd.CommandText = "insert into dbo.Punkt_dystrybucji([miasto],[adres],[status]) " +
                    "values(@miasto, @adres, 'aktywny')";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@miasto", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@adres", SqlDbType.VarChar));

                cmd.Parameters["@miasto"].Value = textBox1.Text;
                cmd.Parameters["@adres"].Value = textBox2.Text;
                sqlC.Open();
                cmd.ExecuteNonQuery();
                sqlC.Close();
                Close();
            }
        }
    }
}
