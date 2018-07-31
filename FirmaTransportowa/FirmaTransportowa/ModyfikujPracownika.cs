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
    
    public partial class ModyfikujKierowcaa : Form
    {
        String idObiektu;
        String conS;
        SqlConnection sqlC, sqlCP;
        SqlCommand cmd = new SqlCommand();
        SqlCommand cmdP = new SqlCommand();
        SqlDataReader rd;
        Boolean edytowanoBoxy;
        Boolean zapisanoZmiany;
        Boolean czyUmowaOPrace;
        bool zwolniony = false, chorobowe = false;
        //tutaj zmiana nazw zmiennych do backupow
        int bStatus;
        String bPESEL, bImie, bNazwisko, bStawka;
        DateTime bDataZwolnienia;
        KontrolaDanych kontrola;
        String status;

        public ModyfikujKierowcaa(String id)
        {
            InitializeComponent();

            kontrola = new KontrolaDanych();
            idObiektu = id;
            edytowanoBoxy = false;
            zapisanoZmiany = true;

            conS = Properties.Settings.Default.projektCS;
            sqlC = new SqlConnection(conS);
            cmd.Connection = sqlC;
            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));

            //tutaj aktualizacja całego zapytania
            cmd.CommandText = "select [PESEL], [imie], [nazwisko], [data_zatrudnienia], [stawka_miesieczna], [stawka_godzinowa], [data_zwolnienia], [data_wygasniecia_umowy], [status] from dbo.Kierowca where [id_pracownika] = @id";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters["@id"].Value = Convert.ToInt32(idObiektu);

            sqlC.Open();
            rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                labelTytul.Text = "Dane kierowcy: " + rd[1].ToString() + " " + rd[2].ToString();
                bPESEL = tbPESEL.Text = rd[0].ToString();
                bImie = tbImie.Text = rd[1].ToString();
                bNazwisko = tbNazwisko.Text = rd[2].ToString();
                if (rd[3].ToString() != "")
                {
                    bStawka = tbStawka.Text = rd[4].ToString();
                    if(!rd[6].ToString().Equals(""))
                        bDataZwolnienia = dateTimePicker1.Value = DateTime.Parse(rd[6].ToString());
                    else
                    {
                        bDataZwolnienia = DateTime.Now;
                        dateTimePicker1.Checked = false;
                    }
                        

                    czyUmowaOPrace = true;
                }
                else
                {
                    bStawka = tbStawka.Text = rd[5].ToString();
                    bDataZwolnienia = dateTimePicker1.Value = DateTime.Parse(rd[7].ToString());
                    czyUmowaOPrace = false;
                }
                if(rd[8].ToString().Equals("aktywny"))
                    bStatus = comboBox1.SelectedIndex = 0;
                else if(rd[8].ToString().Equals("zwolniony"))
                    bStatus = comboBox1.SelectedIndex = 2;
                else
                    bStatus = comboBox1.SelectedIndex = 1;
                status = comboBox1.Text;

            }
            sqlC.Close();
        }

        private void buttonZatwierdz_Click(object sender, EventArgs e)
        {
            String varSprawdzDane = "";
            // tutaj zmiana sprawdzanych textBoxow
            if (true || !tbPESEL.Text.Equals(bPESEL) || !tbImie.Text.Equals(bImie) || !tbNazwisko.Text.Equals(bNazwisko) || !tbStawka.Text.Equals(bStawka) || comboBox1.SelectedIndex != bStatus)
            {
                edytowanoBoxy = true;
                zapisanoZmiany = false;
            }

            if (edytowanoBoxy)
            {
                varSprawdzDane = sprawdzDane();
                if (varSprawdzDane.Length == 0)
                {
                    cmd.Parameters.Add(new SqlParameter("@PESEL", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@imie", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@nazwisko", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@stawka", SqlDbType.Decimal));                 
                    cmd.Parameters.Add(new SqlParameter("@dataZwolnienia", SqlDbType.Date));
                    cmd.Parameters.Add(new SqlParameter("@status", SqlDbType.VarChar));
                    if (czyUmowaOPrace)
                    {
                        cmd.CommandText = "update dbo.Kierowca set [PESEL] = @PESEL, [imie] = @imie, [nazwisko] = @nazwisko, [stawka_miesieczna] = @stawka, [data_zwolnienia] = @dataZwolnienia, [status] = @status where [id_pracownika] = @id";
                    }
                    else
                    {
                        cmd.CommandText = "update dbo.Kierowca set [PESEL] = @PESEL, [imie] = @imie, [nazwisko] = @nazwisko, [stawka_godzinowa] = @stawka, [data_wygasniecia_umowy] = @dataZwolnienia, [status] = @status where [id_pracownika] = @id";

                    }
                    //tutaj zmiana całego zapytania, i parametrow
                    cmd.CommandType = CommandType.Text;
                    
                    cmd.Parameters["@PESEL"].Value = tbPESEL.Text;
                    cmd.Parameters["@imie"].Value = tbImie.Text;
                    cmd.Parameters["@nazwisko"].Value = tbNazwisko.Text;
                    cmd.Parameters["@stawka"].Value = Convert.ToDouble(tbStawka.Text.Replace(".",","));
                    if(czyUmowaOPrace)
                    {
                        if (!dateTimePicker1.Checked)
                            cmd.Parameters["@dataZwolnienia"].Value = DBNull.Value;
                        else
                            cmd.Parameters["@dataZwolnienia"].Value = dateTimePicker1.Value;
                    }
                    else
                    {
                        if (!dateTimePicker1.Checked)
                            cmd.Parameters["@dataZwolnienia"].Value = bDataZwolnienia;
                        else
                            cmd.Parameters["@dataZwolnienia"].Value = dateTimePicker1.Value;
                    }
                        

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

                    if(chorobowe || zwolniony)
                    {
                        DateTime podanaData = DateTime.Now.Date;
                        if (chorobowe) podanaData = DateTime.Now.Date;
                        if (zwolniony && czyUmowaOPrace) podanaData = dateTimePicker1.Value;
                        if (zwolniony && !czyUmowaOPrace) podanaData = bDataZwolnienia;

                        String conS2;
                        SqlConnection sqlC2;
                        SqlCommand cmd2 = new SqlCommand();

                        conS2 = Properties.Settings.Default.projektCS;
                        sqlC2 = new SqlConnection(conS2);
                        cmd2.Connection = sqlC2;


                        cmd2.CommandText = "zmienTransportyKierowcy";
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.Add(new SqlParameter("@id_kierowcy", SqlDbType.Int));
                        cmd2.Parameters.Add(new SqlParameter("@data_zmiany", SqlDbType.Date));

                        cmd2.Parameters["@data_zmiany"].Value = podanaData;
                        cmd2.Parameters["@id_kierowcy"].Value = Convert.ToInt32(idObiektu);
                        sqlC2.Open();
                        cmd2.ExecuteNonQuery();
                        sqlC2.Close();
                    }
                    if(!chorobowe && !zwolniony && (status.Equals("chorobowe (L4)") || status.Equals("zwolniony")))
                    {
                        String conS2;
                        SqlConnection sqlC2;
                        SqlCommand cmd2 = new SqlCommand();

                        conS2 = Properties.Settings.Default.projektCS;
                        sqlC2 = new SqlConnection(conS2);
                        cmd2.Connection = sqlC2;

                        if (status.Equals("chorobowe (L4)"))
                        {
                            cmd2.CommandText = "update dbo.Kierowca set wolny_od = GETDATE() where id_pracownika = @id_pracownika";
                            cmd2.CommandType = CommandType.Text;
                            cmd2.Parameters.Add(new SqlParameter("@id_pracownika", SqlDbType.Int));

                            cmd2.Parameters["@id_pracownika"].Value = Convert.ToInt32(idObiektu);
                            sqlC2.Open();
                            cmd2.ExecuteNonQuery();
                            sqlC2.Close();
                        }

                        if (status.Equals("zwolniony"))
                        {
                            if(czyUmowaOPrace)
                            {
                                cmd2.CommandText = "update dbo.Kierowca set wolny_od = GETDATE(), data_zatrudnienia = GETDATE(), data_zwolnienia = NULL where id_pracownika = @id_pracownika";
                                cmd2.CommandType = CommandType.Text;
                                cmd2.Parameters.Add(new SqlParameter("@id_pracownika", SqlDbType.Int));

                                cmd2.Parameters["@id_pracownika"].Value = Convert.ToInt32(idObiektu);
                                sqlC2.Open();
                                cmd2.ExecuteNonQuery();
                                sqlC2.Close();
                            }
                            else
                            {
                                cmd2.CommandText = "update dbo.Kierowca set wolny_od = GETDATE(), data_podpisania_umowy = GETDATE(), data_wygasniecia_umowy = DATEADD(MONTH,1,GETDATE()) where id_pracownika = @id_pracownika";
                                cmd2.CommandType = CommandType.Text;
                                cmd2.Parameters.Add(new SqlParameter("@id_pracownika", SqlDbType.Int));

                                cmd2.Parameters["@id_pracownika"].Value = Convert.ToInt32(idObiektu);
                                sqlC2.Open();
                                cmd2.ExecuteNonQuery();
                                sqlC2.Close();
                            }
    
                        }
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
                        tbPESEL.Text = bPESEL;
                        tbImie.Text = bImie;
                        tbNazwisko.Text = bNazwisko;
                        tbStawka.Text = bStawka;
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

        public bool czyPesel()
        {
            sqlCP = new SqlConnection(conS);
            cmdP = new SqlCommand();
            cmdP.Connection = sqlCP;
            bool czy = true;
            cmdP.CommandText = "select [PESEL] from dbo.Kierowca";
            cmdP.CommandType = CommandType.Text;
            sqlCP.Open();
            rd = cmdP.ExecuteReader();

            while (rd.Read())
            {
                if (rd[0].ToString() == tbPESEL.Text && rd[0].ToString() != bPESEL)
                    czy = false;
            }
            sqlCP.Close();
            return czy;
        }

        public bool czyPeselBezLiter()
        {
            bool czy = true;
            for (int i = 0; i < tbPESEL.Text.Length; i++)
            {
                if (!Char.IsDigit(tbPESEL.Text[i]))
                {
                    czy = false;
                }
            }
            return czy;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (czyUmowaOPrace)
            {
                if (dateTimePicker1.Checked)
                {
                    comboBox1.SelectedIndex = 2;
                    zwolniony = true;
                    chorobowe = false;
                }
                else
                {
                    comboBox1.SelectedIndex = 0;
                    zwolniony = false;
                    chorobowe = false;
                }
            }
            else
            {
                if (dateTimePicker1.Checked)
                {
                    comboBox1.SelectedIndex = 0;
                    zwolniony = false;
                    chorobowe = false;
                }
            }
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (czyUmowaOPrace)
            {
                if (comboBox1.SelectedIndex == 2)
                {
                    dateTimePicker1.Checked = true;
                    zwolniony = true;
                    chorobowe = false;
                }
                else if (comboBox1.SelectedIndex == 0)
                {
                    dateTimePicker1.Checked = false;
                    zwolniony = false;
                    chorobowe = false;
                }
                else
                {
                    dateTimePicker1.Checked = false;
                    chorobowe = true;
                    zwolniony = false;
                }
            }
            else
            {
                if (comboBox1.SelectedIndex == 2)
                {
                    dateTimePicker1.Checked = false;
                    zwolniony = true;
                    chorobowe = false;
                }
                else if (comboBox1.SelectedIndex == 0)
                {
                    zwolniony = false;
                    chorobowe = false;
                }
                else
                {
                    dateTimePicker1.Checked = false;
                    chorobowe = true;
                    zwolniony = false;
                }
            }
        }

        public bool CheckIfDecimal82(String text)
        {
            text = text.Replace(",", ".");
            bool onlyOne = false;
            int dotPosition = -1;

            for (int i = 0; i < text.Length; i++)
            {
                if (!Char.IsDigit(text[i]))
                {
                    if (text[i].Equals('.') && !onlyOne)
                    {
                        onlyOne = true;
                        dotPosition = i;
                    }
                    else if (onlyOne)
                    {
                        return false;
                    }
                    if (text[i].Equals('-')) return false;
                }
            }
            if (dotPosition != -1)
            {
                if (text.Length > 9) return false;
                if (dotPosition < (text.Length - 3)) return false;
            }
            if (dotPosition == -1 && text.Length > 6) return false;

            return true;
        }

        public bool dlStawki(String text)
        {
            text = text.Replace(',', '.');
            bool onlyNum = true;
            int ileKropek = 0;
            int ile = 0;
            for (int i = 0; i < text.Length; i++)
            {
                ile++;
                if (text[i].Equals('.'))
                {
                    ileKropek++;
                    ile = 0;
                }
                if (ileKropek > 1) return false;
                if (ile > 2)
                    onlyNum = false;
            }
            return onlyNum;
        }

        public bool dlStawki2(String text)
        {
            text = text.Replace(',', '.');
            bool onlyNum = true;
            int ileKropek = 0;
            int ile = 0, max = 4;
            for (int i = 0; i < text.Length; i++)
            {
                ile++;
                if (text[i].Equals('.'))
                {
                    ileKropek++;
                    ile = 0;
                    max = 2;
                }
                if (ileKropek > 1) return false;
                if (ile > max)
                {
                    onlyNum = false;
                    MessageBox.Show(text, ile.ToString());
                }
            }
            return onlyNum;
        }

        private String sprawdzDane()
        {
            String uwagi = "";
            //PESEL
            if (!czyPeselBezLiter())
            {
                uwagi += "PESEL może składać się tylko z cyfr!\n";
            }
            if (!czyPesel())
            {
                uwagi += "Taki numer PESEL już istnieje!\n";
            }
            else if (tbPESEL.Text.Length != 11)
            {
                uwagi += "PESEL musi mieć 11 cyfr!\n";
            }
            //Imie
            if (String.IsNullOrWhiteSpace(tbImie.Text))
            {
                uwagi += "Imię nie może być puste!\n";
            }
            if(tbImie.Text.Length > 50)
            {
                uwagi += "Imię nie może być dłuższe niż 50 znaków!\n";
            }
            //Nazwisko
            if (String.IsNullOrWhiteSpace(tbNazwisko.Text))
            {
                uwagi += "Imię nie może być puste!\n";
            }
            if (tbNazwisko.Text.Length > 50)
            {
                uwagi += "Imię nie może być dłuższe niż 50 znaków!\n";
            }
            //stawka
            if (String.IsNullOrWhiteSpace(tbStawka.Text))
            {
                uwagi += "Stawka nie może być pusta!\n";
            }
            else if (czyUmowaOPrace)
            {
                if (!dlStawki2(tbStawka.Text))
                {
                    uwagi += "W polu Stawka maksymalnie 4 cyfry przed i 2 po kropce.\n";
                }
                
            }
            else
            {
                if (!dlStawki(tbStawka.Text))
                {
                    uwagi += "W polu Stawka maksymalnie 2 cyfry przed i 2 po kropce.\n";
                }
            }
            if(dateTimePicker1.Value < bDataZwolnienia && !czyUmowaOPrace && !chorobowe)
            {
                uwagi += "Data zwolnienia wcześniejsza niż wygaśnięcie kontraktu pracownika.\n";
            }
            if(zwolniony && dateTimePicker1.Value < DateTime.Now.Date)
            {
                uwagi += "Data zwolnienia wcześniejsza niż dzisiejsza data.\n";
            }
            if (zwolniony && bDataZwolnienia > DateTime.Now.Date && !czyUmowaOPrace)
            {
                uwagi += "Nie można zwolnić pracownika kontraktowego przed końcem jego kontraktu.\n";
            }
            return uwagi;
        }
    }
}
