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
    public partial class ModyfikujDaneLogowania : Form
    {
        String idObiektu;
        String conS;
        SqlConnection sqlC;
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rd;
        Boolean edytowanoBoxy;
        Boolean zapisanoZmiany;
        //tutaj zmiana nazw zmiennych do backupow
        int bPrzywileje, id_admina;
        String bLogin, bHaslo, bImie, bNazwisko, bPESEL;

        public ModyfikujDaneLogowania(String id)
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
            cmd.CommandText = "select [login], [haslo], [przywileje], [imię], [nazwisko], [PESEL], d.[id_admina] from dbo.DaneLogowania d join dbo.Pracownik p on d.id_admina = p.id_admina where d.[id] = @id";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters["@id"].Value = Convert.ToInt32(idObiektu);

            sqlC.Open();
            rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                labelTytul.Text = "Dane użytkownika: " + rd[0].ToString();
                bLogin = tbLogin.Text = rd[0].ToString();
                bHaslo = tbHaslo.Text = rd[1].ToString();
                if (rd[2].ToString().Equals("admin"))
                {
                    bPrzywileje = comboBox1.SelectedIndex = 0;
                  
                }
                else
                {
                    bPrzywileje = comboBox1.SelectedIndex = 1;
                }
                bImie = textBox1.Text = rd[3].ToString();
                bNazwisko = textBox2.Text = rd[4].ToString();
                bPESEL = textBox3.Text = rd[5].ToString();
                id_admina = Convert.ToInt32(rd[6].ToString());

            }
            sqlC.Close();

            cmd.Parameters.Add(new SqlParameter("@login", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("@haslo", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("@przywileje", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("@imię", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("@nazwisko", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("@PESEL", SqlDbType.VarChar));
            cmd.Parameters["@login"].Value = "";
            cmd.Parameters["@haslo"].Value = "";
            cmd.Parameters["@przywileje"].Value = "";
            cmd.Parameters["@imię"].Value = "";
            cmd.Parameters["@nazwisko"].Value = "";
            cmd.Parameters["@PESEL"].Value = "";
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

        private void buttonZatwierdz_Click(object sender, EventArgs e)
        {
            String varSprawdzDane = "";
            // tutaj zmiana sprawdzanych textBoxow
            if (!tbLogin.Text.Equals(bLogin) || !tbHaslo.Text.Equals(bHaslo) || comboBox1.SelectedIndex != bPrzywileje || !textBox1.Text.Equals(bImie) || !textBox2.Text.Equals(bNazwisko) || !textBox3.Text.Equals(bPESEL))
            {
                edytowanoBoxy = true;
                zapisanoZmiany = false;
            }

            if (edytowanoBoxy)
            {
                varSprawdzDane = sprawdzDane();
                if(varSprawdzDane.Length == 0)
                {
                    //tutaj zmiana całego zapytania, i parametrow
                    cmd.CommandText = "update dbo.DaneLogowania set [login] = @login, [haslo] = @haslo, [przywileje] = @przywileje where [id] = @id";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters["@login"].Value = tbLogin.Text;
                    cmd.Parameters["@haslo"].Value = tbHaslo.Text;
                    cmd.Parameters["@przywileje"].Value = comboBox1.Text;
                    
                    sqlC.Open();
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        zapisanoZmiany = true;
                        edytowanoBoxy = false;
                    }
                    else
                    {
                        MessageBox.Show("Wystąpił nieoczekiwany błąd, sprawdź bazę danych.","UWAGA",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                    sqlC.Close();

                    cmd.CommandText = "update dbo.Pracownik set [imię] = @imię, [nazwisko] = @nazwisko, [PESEL] = @PESEL where [id_admina] = @id";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters["@id"].Value = id_admina;
                    cmd.Parameters["@imię"].Value = textBox1.Text;
                    cmd.Parameters["@nazwisko"].Value = textBox2.Text;
                    cmd.Parameters["@PESEL"].Value = textBox3.Text;
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
                        tbLogin.Text = bLogin;
                        tbHaslo.Text = bHaslo;
                        comboBox1.SelectedIndex = bPrzywileje;
                        textBox1.Text = bImie;
                        textBox2.Text = bNazwisko;
                        textBox3.Text = bPESEL;
                        //
                        edytowanoBoxy = false;
                        zapisanoZmiany = true;
                    }
                }
            }
            else Close();
        }


        private String sprawdzDane()
        {
            KontrolaDanych kontrola = new KontrolaDanych();
            String uwagi = "";
            //login
            if (String.IsNullOrWhiteSpace(tbLogin.Text))
            {
                uwagi += "Login nie może być pusty!\n";
            }
            else
            {
                if (tbLogin.Text.StartsWith(" "))
                {
                    uwagi += "Login nie może zaczynać się od spacji!\n";
                }
                if (tbLogin.Text.EndsWith(" "))
                {
                    uwagi += "Login nie może kończyć się spacją!\n";
                }
            }
            if (tbLogin.Text.Length > 50)
            {
                uwagi += "Login nie może być dłuższy niż 50 znaków!\n";
            }
            if (uwagi.Length == 0 && !tbLogin.Text.Equals(bLogin))
            {
                cmd.CommandText = "select 1 from dbo.DaneLogowania where [login] = @login";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters["@login"].Value = tbLogin.Text;
                sqlC.Open();
                rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    uwagi += "Ten login już istnieje!\n";
                }
                sqlC.Close();
            }

            //haslo
            if (tbHaslo.Text.Equals(""))
            {
                uwagi += "Hasło nie może być puste!\n";
            }
            if (tbHaslo.Text.Length > 50)
            {
                uwagi += "Hasło nie może być dłuższe niż 50 znaków!\n";
            }
            if (tbHaslo.Text.Any(Char.IsWhiteSpace))
            {
                uwagi += "Hasło nie może zawierać spacji!\n";
            }

            //Imie
            if (textBox1.Text.Equals(""))
            {
                uwagi += "Imię nie może być puste!\n";
            }
            if (textBox1.Text.Length > 50)
            {
                uwagi += "Imię nie może być dłuższe niż 50 znaków!\n";
            }
            if (textBox1.Text.Any(Char.IsWhiteSpace))
            {
                uwagi += "Imię nie może zawierać spacji!\n";
            }

            //Nazwisko

            if (textBox2.Text.Equals(""))
            {
                uwagi += "Nazwisko nie może być puste!\n";
            }
            if (textBox2.Text.Length > 50)
            {
                uwagi += "Nazwisko nie może być dłuższe niż 50 znaków!\n";
            }
            if (textBox2.Text.Any(Char.IsWhiteSpace))
            {
                uwagi += "Nazwisko nie może zawierać spacji!\n";
            }

            //PESEL

            if (textBox3.Text.Equals(""))
            {
                uwagi += "PESEL nie może być pusty!\n";
            }
            if (textBox3.Text.Length != 11)
            {
                uwagi += "PESEL musi zawierać 11 cyfr!\n";
            }
            if (!kontrola.czyPeselBezLiter(textBox3.Text))
            {
                uwagi += "PESEL zawiera tylko cyfry!\n";
            }
            if (!kontrola.czyPeselPracownik(textBox3.Text) && !bPESEL.Equals(textBox3.Text))
            {
                uwagi += "Taki PESEL już istnieje!\n";
            }

            return uwagi;
        }
    }
}
