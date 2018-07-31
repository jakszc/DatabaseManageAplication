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
    public partial class SzukajLoginu : Form
    {
        public SzukajLoginu()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            String conS = Properties.Settings.Default.projektCS;
            SqlConnection sqlC = new SqlConnection(conS);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader rd;

            cmd.Connection = sqlC;
            cmd.CommandText = "select [login], [imie], [nazwisko], [nazwa_firmy] from dbo.Klient k join dbo.DaneLogowania d on k.id_klienta = d.id_klienta";
            cmd.CommandType = CommandType.Text;

            sqlC.Open();
            rd = cmd.ExecuteReader();
            int i = 0;
            while (rd.Read())
            {
                var index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells["Login"].Value = rd[0].ToString();
                dataGridView1.Rows[index].Cells["imie"].Value = rd[1].ToString();
                dataGridView1.Rows[index].Cells["Nazwisko"].Value = rd[2].ToString();
                dataGridView1.Rows[index].Cells["Firma"].Value = rd[3].ToString();
            }
        }

        private void SzukajLoginu_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                var hti = dataGridView1.HitTest(e.X, e.Y);
                if (hti.RowY >= 0 && hti.RowIndex >= 0)
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[hti.RowIndex].Selected = true;
                }

            }
        }

        private void wybierzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Int32 rowToDelete = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (rowToDelete >= 0)
            {
                Kontener.wyszukanyLogin = dataGridView1.Rows[rowToDelete].Cells["Login"].Value.ToString();
                Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (!dataGridView1.Rows[i].Cells[comboBox1.SelectedIndex].Value.ToString().ToLower().Contains(textBox1.Text.ToLower()))
                {
                    dataGridView1.Rows.RemoveAt(i);
                    i--;
                }
            }                          
        }

        private void odświeżListęToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String conS = Properties.Settings.Default.projektCS;
            SqlConnection sqlC = new SqlConnection(conS);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader rd;

            cmd.Connection = sqlC;

            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            cmd.CommandText = "select [login], [imie], [nazwisko], [nazwa_firmy] from dbo.Klient k join dbo.DaneLogowania d on k.id_klienta = d.id_klienta";
            cmd.CommandType = CommandType.Text;

            sqlC.Open();
            rd = cmd.ExecuteReader();
            int i = 0;
            while (rd.Read())
            {
                var index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells["Login"].Value = rd[0].ToString();
                dataGridView1.Rows[index].Cells["imie"].Value = rd[1].ToString();
                dataGridView1.Rows[index].Cells["Nazwisko"].Value = rd[2].ToString();
                dataGridView1.Rows[index].Cells["Firma"].Value = rd[3].ToString();
            }
        }
    }
}
