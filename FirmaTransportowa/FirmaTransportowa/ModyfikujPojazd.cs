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
    public partial class ModyfikujPojazd : Form
    {
        String idObiektu;
        String conS;
        SqlConnection sqlC;
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rd;
        Boolean edytowanoBoxy;
        Boolean zapisanoZmiany;
        //tutaj zmiana nazw zmiennych do backupow
        String bRejestracja;
        int bStatus;
        String status;

        public ModyfikujPojazd(String id)
        {
            InitializeComponent();
            idObiektu = id;
            edytowanoBoxy = false;
            zapisanoZmiany = true;

            conS = Properties.Settings.Default.projektCS;
            sqlC = new SqlConnection(conS);
            cmd.Connection = sqlC;
            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("@rejestracja", SqlDbType.VarChar));
            //tutaj aktualizacja całego zapytania
            cmd.CommandText = "select [rejestracja], [status] from dbo.Pojazd where [rejestracja] = @id";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters["@id"].Value = idObiektu;
            cmd.Parameters["@rejestracja"].Value = "";


            sqlC.Open();
            rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                labelTytul.Text = "Dane pojazdu: " + rd[0].ToString();
                bRejestracja = textBox1.Text = rd[0].ToString();
                if(rd[1].ToString().Equals("aktywny"))
                    bStatus = comboBox1.SelectedIndex = 0;
                else if(rd[1].ToString().Equals("w serwisie"))
                    bStatus = comboBox1.SelectedIndex = 1;
                else
                    bStatus = comboBox1.SelectedIndex = 2;
                status = comboBox1.Text;

            }
            sqlC.Close();
        }

        private void buttonZatwierdz_Click(object sender, EventArgs e)
        {
            String varSprawdzDane = "";
            // tutaj zmiana sprawdzanych textBoxow
            if (comboBox1.SelectedIndex != bStatus || !textBox1.Text.Equals(bRejestracja))
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
                    cmd.CommandText = "update dbo.Pojazd set [status] = @status, [rejestracja] = @rejestracja where [rejestracja] = @id";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("@status", SqlDbType.VarChar));

                    cmd.Parameters["@status"].Value = comboBox1.Text;
                    cmd.Parameters["@rejestracja"].Value = textBox1.Text;


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

                    if(comboBox1.SelectedIndex == 1 || comboBox1.SelectedIndex == 2)
                    {                      
                        DateTime podanaData = DateTime.Now.Date;

                        String conS2;
                        SqlConnection sqlC2;
                        SqlCommand cmd2 = new SqlCommand();

                        conS2 = Properties.Settings.Default.projektCS;
                        sqlC2 = new SqlConnection(conS2);
                        cmd2.Connection = sqlC2;


                        cmd2.CommandText = "zmienTransportyPojazdu";
                        cmd2.CommandType = CommandType.StoredProcedure;                        
                        cmd2.Parameters.Add(new SqlParameter("@rej_pojazdu", SqlDbType.VarChar));
                        cmd2.Parameters.Add(new SqlParameter("@data_zmiany", SqlDbType.Date));

                        cmd2.Parameters["@data_zmiany"].Value = podanaData;
                        cmd2.Parameters["@rej_pojazdu"].Value = idObiektu;
                        sqlC2.Open();
                        cmd2.ExecuteNonQuery();
                        sqlC2.Close();
                    }

                    if(comboBox1.SelectedIndex == 0 && (status.Equals("w serwisie") || status.Equals("nieużytkowany")))
                    {
                        String conS2;
                        SqlConnection sqlC2;
                        SqlCommand cmd2 = new SqlCommand();

                        conS2 = Properties.Settings.Default.projektCS;
                        sqlC2 = new SqlConnection(conS2);
                        cmd2.Connection = sqlC2;


                        cmd2.CommandText = "update dbo.Pojazd set wolny_od = GETDATE() where rejestracja = @rej_pojazdu";
                        cmd2.CommandType = CommandType.Text;
                        cmd2.Parameters.Add(new SqlParameter("@rej_pojazdu", SqlDbType.VarChar));

                        cmd2.Parameters["@rej_pojazdu"].Value = idObiektu;
                        sqlC2.Open();
                        cmd2.ExecuteNonQuery();
                        sqlC2.Close();
                    }


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
                        textBox1.Text = bRejestracja;
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
            //status
            if (!textBox1.Text.Equals(bRejestracja))
            {
                if (!checkRejestracja())
                {
                    uwagi += "Taka rejestracja już istnieje!\n";
                }else if(textBox1.Text.Length > 10)
                {
                    uwagi += "Rejestracja ma maksymalnie 10 znaków!\n";
                }
            }
            return uwagi;
        }

        public bool checkRejestracja()
        {
            cmd.CommandText = "select 1 from dbo.Pojazd where [rejestracja] = @rejestracja";
            cmd.CommandType = CommandType.Text;
            

            cmd.Parameters["@rejestracja"].Value = textBox1.Text;

            sqlC.Open();

            rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                sqlC.Close();
                return false;
            }
            sqlC.Close();
            return true;
        }
    }
}
