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
    public partial class PodgladZamowienia : Form
    {
        String conS;
        SqlConnection sqlC;
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rd;
        public PodgladZamowienia()
        {
            InitializeComponent();
            textBox3.Text = Kontener.sumKwota;
            textBox4.Text = Kontener.sumWaga;
            
            String conS = Properties.Settings.Default.projektCS;
            SqlConnection sqlC = new SqlConnection(conS);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlC;

            cmd.CommandText = "szczegolyZamowienia";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id_zamowienia", Kontener.idZamowienia);
            sqlC.Open();
            cmd.ExecuteNonQuery();

            using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                adap.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            sqlC.Close();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Kontener.dataTransportu <= DateTime.Now.Date)
            {
                MessageBox.Show("Nie można anulować zamówienia gdyż zostało już wysłane.", "Błąd anulacji.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DialogResult result = MessageBox.Show("Czy na pewno chcesz anulować to zamówienie?.", "Potwierdź anulacje.", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (DialogResult.Yes == result)
                {
                    String conS = Properties.Settings.Default.projektCS;
                    SqlConnection sqlC = new SqlConnection(conS);
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = sqlC;

                    cmd.CommandText = "anulujProdukty";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id_zamowienia", SqlDbType.Int));
                    cmd.Parameters["@id_zamowienia"].Value = Kontener.idZamowienia;
                    sqlC.Open();
                    cmd.ExecuteNonQuery();
                    sqlC.Close();


                    cmd.CommandText = "select * from dbo.policzProdukty()";
                    cmd.CommandType = CommandType.Text;
                    sqlC.Open();
                    rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        String conS2 = Properties.Settings.Default.projektCS;
                        SqlConnection sqlC2 = new SqlConnection(conS2);
                        SqlCommand cmd2 = new SqlCommand();
                        cmd2.Connection = sqlC2;

                        cmd2.CommandText = "update dbo.Katalog_produktow set [dostepna_ilosc] = @dostepna_ilosc where [id_katalogowe] = @id_katalogowe";
                        cmd2.CommandType = CommandType.Text;
                        cmd2.Parameters.Add(new SqlParameter("@dostepna_ilosc", SqlDbType.Int));
                        cmd2.Parameters.Add(new SqlParameter("@id_katalogowe", SqlDbType.Int));
                        cmd2.Parameters["@dostepna_ilosc"].Value = Convert.ToInt32(rd[1].ToString());
                        cmd2.Parameters["@id_katalogowe"].Value = Convert.ToInt32(rd[0].ToString());
                        sqlC2.Open();
                        cmd2.ExecuteNonQuery();
                        sqlC2.Close();

                    }
                    sqlC.Close();

                    cmd.CommandText = "anulujZamowienie";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters["@id_zamowienia"].Value = Kontener.idZamowienia;
                    sqlC.Open();
                    cmd.ExecuteNonQuery();
                    sqlC.Close();
                    Close();
                }
            }
        }
    }
}
