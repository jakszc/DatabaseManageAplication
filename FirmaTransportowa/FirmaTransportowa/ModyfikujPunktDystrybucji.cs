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
    public partial class ModyfikujPunktDystrybucji : Form
    {
        String idObiektu;
        String conS;
        SqlConnection sqlC;
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rd;
        Boolean edytowanoBoxy;
        Boolean zapisanoZmiany;
        //tutaj zmiana nazw zmiennych do backupow
        String bMiasto, bAdres;
        int bStatus;

        public ModyfikujPunktDystrybucji(String id)
        {
            InitializeComponent();
            idObiektu = id;
            edytowanoBoxy = false;
            zapisanoZmiany = true;

            conS = Properties.Settings.Default.projektCS;
            sqlC = new SqlConnection(conS);
            cmd.Connection = sqlC;
            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));

            //tutaj aktualizacja całego zapytania
            cmd.CommandText = "select [miasto], [adres], [status] from dbo.Punkt_dystrybucji where [id_punktu] = @id";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters["@id"].Value = Convert.ToInt32(idObiektu);

            sqlC.Open();
            rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                labelTytul.Text = "Dane punktu";
                bMiasto = tbMiasto.Text = rd[0].ToString();
                bAdres = tbAdres.Text = rd[1].ToString();
                if (rd[2].ToString().Equals("aktywny")) 
                    bStatus = comboBox1.SelectedIndex = 0;
                else
                    bStatus = comboBox1.SelectedIndex = 1;

            }
            sqlC.Close();
        }

        private void buttonZatwierdz_Click(object sender, EventArgs e)
        {
            String varSprawdzDane = "";
            // tutaj zmiana sprawdzanych textBoxow
            if (!tbMiasto.Text.Equals(bMiasto) || !tbAdres.Text.Equals(bAdres) || comboBox1.SelectedIndex != bStatus)
            {
                edytowanoBoxy = true;
                zapisanoZmiany = false;
            }

            if (edytowanoBoxy)
            {
                varSprawdzDane = sprawdzDane();
                if (varSprawdzDane.Length == 0)
                {
                    //tutaj zmiana całego zapytania, i parametrow
                    cmd.CommandText = "update dbo.Punkt_dystrybucji set [miasto] = @miasto, [adres] = @adres, [status] = @status where [id_punktu] = @id";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("@miasto", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@adres", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@status", SqlDbType.VarChar));
                    cmd.Parameters["@miasto"].Value = tbMiasto.Text;
                    cmd.Parameters["@adres"].Value = tbAdres.Text;
                    cmd.Parameters["@status"].Value = comboBox1.Text;

                    sqlC.Open();
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        zapisanoZmiany = true;
                        edytowanoBoxy = false;
                    }
                    else
                    {
                        MessageBox.Show("Wystąpił nieoczekiwany błąd, sprawdź bazę danych.", "UWAGA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    sqlC.Close();

                    if (zapisanoZmiany)
                    {
                        Close();
                    }
                }
                else
                {
                    DialogResult result = MessageBox.Show(varSprawdzDane + "\n\n -OK- aby wrócić do edycji\n-Anuluj- aby anulować edycję danych", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                    if (result != DialogResult.OK)
                    {
                        //tutaj zmiana textBoxow i backupow
                        tbMiasto.Text = bMiasto;
                        tbAdres.Text = bAdres;
                        comboBox1.SelectedIndex = bStatus;
                        //
                        edytowanoBoxy = false;
                        zapisanoZmiany = true;
                    }
                }
            }
            else Close();
        }

        private void buttonAnuluj_Click(object sender, EventArgs e)
        {
            Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (edytowanoBoxy || !zapisanoZmiany)
            {
                DialogResult result = MessageBox.Show("Niezatwierdzone zmiany nie zostaną zapisane. Czy na pewno kontynuować?", "Ostrzeżenie", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private String sprawdzDane()
        {
            String uwagi = "";
            if (String.IsNullOrWhiteSpace(tbMiasto.Text))
            {
                uwagi += "Nazwa miasta nie może być pusta!\n";
            }
            if (tbMiasto.Text.Length > 50)
            {
                uwagi += "Nazwa miasta nie może być dłuższa niż 50 znaków!\n";
            }
            if (String.IsNullOrWhiteSpace(tbAdres.Text))
            {
                uwagi += "Adres nie może być pusty!\n";
            }
            if (tbAdres.Text.Length > 50)
            {
                uwagi += "Adres nie może być dłuższy niż 50 znaków!\n";
            }
            return uwagi;
        }
    }
}
