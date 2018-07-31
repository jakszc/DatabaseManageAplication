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
    public partial class ZamowieniaKlienta : Form
    {
        String conS;
        SqlConnection sqlC;
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rd;

        public ZamowieniaKlienta()
        {
            InitializeComponent();
            conS = Properties.Settings.Default.projektCS;
            sqlC = new SqlConnection(conS);
            cmd.Connection = sqlC;
            cmd.CommandText = "listaZamowien";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id_klienta", Properties.Settings.Default.id_klienta);

            sqlC.Open();
            cmd.ExecuteNonQuery();

            using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                adap.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            sqlC.Close();
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (dataGridView1.Rows[i].Cells[5].Value.ToString() == "")
                {
                    dataGridView1.Rows[i].Cells[7].Value = "";
                }
            }
            dataGridView1.Columns[0].Visible = false;
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

        private void podglądZamówieniaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Int32 rowToDelete = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (rowToDelete >= 0)
            {
                Kontener.idZamowienia = Convert.ToInt32(dataGridView1.Rows[rowToDelete].Cells[0].Value.ToString());
                Kontener.sumWaga = dataGridView1.Rows[rowToDelete].Cells[5].Value.ToString();
                Kontener.sumKwota = dataGridView1.Rows[rowToDelete].Cells[4].Value.ToString();
                Kontener.dataTransportu = DateTime.Parse(dataGridView1.Rows[rowToDelete].Cells[2].Value.ToString()).AddDays(-1);
                PodgladZamowienia form = new PodgladZamowienia();
                form.ShowDialog();
                form = null;
                sqlC.Open();
                cmd.ExecuteNonQuery();

                using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adap.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                sqlC.Close();
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (dataGridView1.Rows[i].Cells[5].Value.ToString() == "")
                    {
                        dataGridView1.Rows[i].Cells[7].Value = "";
                    }
                }
                dataGridView1.Columns[0].Visible = false;
            }
        }
    }
}
