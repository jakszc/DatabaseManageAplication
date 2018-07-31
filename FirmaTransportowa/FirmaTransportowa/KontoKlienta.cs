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
    public partial class KontoKlienta : Form
    {
        String conS;
        SqlConnection sqlC_k, sqlC_d;
        SqlCommand cmd_k = new SqlCommand();
        SqlCommand cmd_d = new SqlCommand();
        SqlDataReader rd_k, rd_d;

        Boolean edytowanoDaneKonta = false;
        Boolean edytowanoDaneOsobowe = false;
        Boolean zapisanoDaneKonta = true;
        Boolean zapisanoDaneOsobowe = true;
        Boolean edycjaLoginu = false;
        Boolean edycjaHasla = false;

        String loginKlienta = "";
        String hasloKlienta = "";
        String imieNazwiskoKlienta = "", nazwaFirmy = "", nipFirmy = "", ulicaNr = "", miasto = "", kodPocztowy = "";
        String typKlienta = "";

        public KontoKlienta()
        {
            InitializeComponent();
            Properties.Settings.Default.poprawneHaslo = false;
            labelHaslo1.Text = "Hasło:";
            labelHaslo2.Visible = false;
            tbHaslo2.Visible = false;
            linkZmienHaslo.Visible = false;
            linkZmienLogin.Visible = false;

            labelImieNazwisko.Visible = false;
            tbImieNazwisko.Visible = false;
            labelNazwaFirmy.Visible = false;
            labelNIP.Visible = false;
            tbNazwaFirmy.Visible = false;
            tbNIP.Visible = false;

            conS = Properties.Settings.Default.projektCS;

            sqlC_d = new SqlConnection(conS);
            cmd_d.Connection = sqlC_d;
            cmd_d.Parameters.Add(new SqlParameter("@id_klienta", SqlDbType.Int));
            cmd_d.Parameters.Add(new SqlParameter("@login", SqlDbType.VarChar));
            cmd_d.Parameters.Add(new SqlParameter("@haslo", SqlDbType.VarChar));

            cmd_d.CommandText = "select [login], [haslo] from dbo.DaneLogowania where [id_klienta] = @id_klienta";
            cmd_d.CommandType = CommandType.Text;
            cmd_d.Parameters["@id_klienta"].Value = Properties.Settings.Default.id_klienta;
            cmd_d.Parameters["@login"].Value = "";
            cmd_d.Parameters["@haslo"].Value = "";
            sqlC_d.Open();
            rd_d = cmd_d.ExecuteReader();
            if (rd_d.Read())
            {
                tbLogin.Text = rd_d[0].ToString();
                tbHaslo1.Text = rd_d[1].ToString();
            }
            sqlC_d.Close();

            sqlC_k = new SqlConnection(conS);
            cmd_k.Connection = sqlC_k;
            cmd_k.CommandText = "select [imie], [nazwisko], [nazwa_firmy], [NIP], [miasto], [kod_pocztowy], [ulica_nr_domu] from dbo.Klient where [id_klienta] = @id_klienta";
            cmd_k.CommandType = CommandType.Text;
            cmd_k.Parameters.Add(new SqlParameter("@id_klienta", SqlDbType.Int));
            cmd_k.Parameters.Add(new SqlParameter("@imie", SqlDbType.VarChar));
            cmd_k.Parameters.Add(new SqlParameter("@nazwisko", SqlDbType.VarChar));
            cmd_k.Parameters.Add(new SqlParameter("@nazwa_firmy", SqlDbType.VarChar));
            cmd_k.Parameters.Add(new SqlParameter("@NIP", SqlDbType.VarChar));
            cmd_k.Parameters.Add(new SqlParameter("@miasto", SqlDbType.VarChar));
            cmd_k.Parameters.Add(new SqlParameter("@kod_pocztowy", SqlDbType.VarChar));
            cmd_k.Parameters.Add(new SqlParameter("@ulica_nr_domu", SqlDbType.VarChar));
            cmd_k.Parameters["@id_klienta"].Value = Properties.Settings.Default.id_klienta;
            cmd_k.Parameters["@imie"].Value = "";
            cmd_k.Parameters["@nazwisko"].Value = "";
            cmd_k.Parameters["@nazwa_firmy"].Value = "";
            cmd_k.Parameters["@NIP"].Value = "";
            cmd_k.Parameters["@miasto"].Value = "";
            cmd_k.Parameters["@kod_pocztowy"].Value = "";
            cmd_k.Parameters["@ulica_nr_domu"].Value = "";
            sqlC_k.Open();
            rd_k = cmd_k.ExecuteReader();
            if (rd_k.Read())
            {
                if (rd_k[0].ToString().Length == 0)
                {
                    labelKlientDuzy.Text = "Klient firmowy";
                    typKlienta = "firma";
                    labelNazwaFirmy.Visible = true;
                    labelNIP.Visible = true;
                    tbNazwaFirmy.Visible = true;
                    tbNIP.Visible = true;
                }
                else
                {
                    labelKlientDuzy.Text = "Klient indywidualny";
                    typKlienta = "indywidualny";
                    labelImieNazwisko.Visible = true;
                    tbImieNazwisko.Visible = true;
                }
                if (rd_k[0].ToString().Length != 0) tbImieNazwisko.Text = rd_k[0].ToString() + " " + rd_k[1].ToString();
                else tbImieNazwisko.Text = "";
                tbNazwaFirmy.Text = rd_k[2].ToString();
                tbNIP.Text = rd_k[3].ToString();
                tbMiasto.Text = rd_k[4].ToString();
                tbKodPocztowy.Text = rd_k[5].ToString();
                tbUlicaNr.Text = rd_k[6].ToString();
            }
            sqlC_k.Close();
        }

        private void buttonPowrot_Click(object sender, EventArgs e)
        {
            Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!zapisanoDaneOsobowe || !zapisanoDaneKonta)
            {
                DialogResult result = MessageBox.Show("Niezatwierdzone zmiany nie zostaną zapisane. Czy na pewno kontynuować?", "Ostrzeżenie", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void linkEdytujDaneKonta_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!edytowanoDaneKonta)
            {
                PotwierdzenieHasla form = new PotwierdzenieHasla();
                form.ShowDialog();
                form = null;
                
                if(Properties.Settings.Default.poprawneHaslo)
                {
                    linkZmienHaslo.Visible = true;
                    linkZmienLogin.Visible = true;

                    loginKlienta = tbLogin.Text;
                    hasloKlienta = tbHaslo1.Text;
                }
            }
        }

        private void linkEdytujDaneOsobowe_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!edytowanoDaneOsobowe)
            {
                edytowanoDaneOsobowe = true;
                zapisanoDaneOsobowe = false;
                buttonZapiszDaneOsobowe.ForeColor = Color.Red;

                tbImieNazwisko.Enabled = true;
                tbNazwaFirmy.Enabled = true;
                tbNIP.Enabled = true;
                tbUlicaNr.Enabled = true;
                tbMiasto.Enabled = true;
                tbKodPocztowy.Enabled = true;

                imieNazwiskoKlienta = tbImieNazwisko.Text;
                nazwaFirmy = tbNazwaFirmy.Text;
                nipFirmy = tbNIP.Text;
                miasto = tbMiasto.Text;
                kodPocztowy = tbKodPocztowy.Text;
                ulicaNr = tbUlicaNr.Text;
            }
        }

        private void linkZmienLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (zapisanoDaneKonta && !edycjaHasla)
            {
                edycjaLoginu = true;
                edytowanoDaneKonta = true;
                zapisanoDaneKonta = false;
                buttonZapiszDaneKonta.ForeColor = Color.Red;
                tbLogin.Enabled = true;
            }
        }

        private void linkZmienHaslo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (zapisanoDaneKonta && !edycjaLoginu)
            {
                edycjaHasla = true;
                edytowanoDaneKonta = true;
                zapisanoDaneKonta = false;
                buttonZapiszDaneKonta.ForeColor = Color.Red;
                labelHaslo1.Text = "Nowe hasło:";
                labelHaslo2.Visible = true;
                tbHaslo2.Visible = true;
                tbHaslo1.Clear();
                tbHaslo2.Clear();

                tbHaslo1.Enabled = true;
                tbHaslo2.Enabled = true;
            }
        }

        private String sprawdzLogin()
        {
            String uwagi = "";
            if(String.IsNullOrWhiteSpace(tbLogin.Text))
            {
                uwagi = "Login nie może być pusty!";
            }
            if (tbLogin.Text.Length > 50)
            {
                uwagi = "Login nie może być dłuższy niż 50 znaków!";
            }
            else if (tbLogin.Text.StartsWith(" "))
            {
                uwagi = "Login nie może zaczynać się od spacji!";
            }
            else if (tbLogin.Text.EndsWith(" "))
            {
                uwagi = "Login nie może kończyć się spacją!";
            }
            else if (!tbLogin.Text.Equals(loginKlienta))
            {
                cmd_d.CommandText = "select 1 from dbo.DaneLogowania where [login] = @login";
                cmd_d.CommandType = CommandType.Text;
                cmd_d.Parameters["@login"].Value = tbLogin.Text;
                sqlC_d.Open();
                rd_d = cmd_d.ExecuteReader();
                if (rd_d.Read())
                {
                    sqlC_d.Close();
                    uwagi = "Ten login już istnieje!\n\nWprowadź inną nazwę użytkownika.";
                }
                sqlC_d.Close();
                if (!uwagi.StartsWith("T"))
                {
                    uwagi = "";
                }
            }
            return uwagi;
        }

        private String sprawdzHaslo()
        {
            if (tbHaslo1.Text.Equals("") || tbHaslo2.Text.Equals(""))
            {
                return "Pola nowego hasła nie mogą być puste!";
            }
            if (tbHaslo1.Text.Length > 50)
            {
                return "Hasło nie może być dłuższe niż 50 znaków!";
            }
            if (tbHaslo1.Text.Any(Char.IsWhiteSpace))
            {
                return "Hasło nie może zawierać spacji!";
            }
            if (!tbHaslo1.Text.Equals(tbHaslo2.Text))
            {
                return "Wpisano niezgodne hasła.";
            }
            else return "";
        }

        private void buttonZapiszDaneKonta_Click(object sender, EventArgs e)
        {
            Boolean zatwierdzonoZmianeLoginu = false;
            Boolean zatwierdzonoZmianeHasla = false;
            Boolean decyzjaCzyAnulowac = true;
            String varSprawdzLogin = "";
            String varSprawdzHaslo = "";

            if (edytowanoDaneKonta)
            {
                if (edycjaLoginu)
                {
                    varSprawdzLogin = sprawdzLogin();
                    if (varSprawdzLogin.Length == 0)
                    {
                        cmd_d.CommandText = "update dbo.DaneLogowania set [login] = @login where [id_klienta] = @id_klienta";
                        cmd_d.CommandType = CommandType.Text;
                        cmd_d.Parameters["@id_klienta"].Value = Properties.Settings.Default.id_klienta;
                        cmd_d.Parameters["@login"].Value = tbLogin.Text;                      
                        sqlC_d.Open();
                        if (cmd_d.ExecuteNonQuery() == 1)
                        {
                            Kontener.login = tbLogin.Text;
                            zatwierdzonoZmianeLoginu = true;
                            loginKlienta = tbLogin.Text;
                            zapisanoDaneKonta = true;
                            edycjaLoginu = false;
                        }
                        sqlC_d.Close();
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show(varSprawdzLogin + "\n\n -OK- aby wrócić do edycji\n-Anuluj- aby anulować edycję danych", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                        if (result == DialogResult.OK) decyzjaCzyAnulowac = false;
                        else decyzjaCzyAnulowac = true;
                    }
                }

                if (edycjaHasla)
                {
                    varSprawdzHaslo = sprawdzHaslo();
                    if (varSprawdzHaslo.Length == 0)
                    {
                        cmd_d.CommandText = "update dbo.DaneLogowania set [haslo] = @haslo where [id_klienta] = @id_klienta";
                        cmd_d.CommandType = CommandType.Text;
                        cmd_d.Parameters["@id_klienta"].Value = Properties.Settings.Default.id_klienta;
                        cmd_d.Parameters["@haslo"].Value = tbHaslo1.Text;
                        sqlC_d.Open();
                        if (cmd_d.ExecuteNonQuery() == 1)
                        {
                            zatwierdzonoZmianeHasla = true;
                            hasloKlienta = tbHaslo1.Text;
                            zapisanoDaneKonta = true;
                            edycjaHasla = false;
                        }
                        sqlC_d.Close();
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show(varSprawdzHaslo + "\n\n -OK- aby wrócić do edycji\n-Anuluj- aby anulować edycję danych", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                        if (result == DialogResult.OK) decyzjaCzyAnulowac = false;
                        else decyzjaCzyAnulowac = true;
                    }
                }

                if (decyzjaCzyAnulowac || zatwierdzonoZmianeLoginu || zatwierdzonoZmianeHasla)
                {
                    zapisanoDaneKonta = true;
                    buttonZapiszDaneKonta.ForeColor = Color.Black;
                    edycjaLoginu = false;
                    edycjaHasla = false;
                    labelHaslo1.Text = "Hasło:";
                    labelHaslo2.Visible = false;
                    tbHaslo2.Visible = false;

                    tbLogin.Text = loginKlienta;
                    tbHaslo1.Text = hasloKlienta;
                    tbHaslo2.Text = "";

                    tbLogin.Enabled = false;
                    tbHaslo1.Enabled = false;
                    tbHaslo2.Enabled = false;
                }
            }
        }

        private String sprawdzDaneOsobowe()
        {
            KontrolaDanych kontrola = new KontrolaDanych();
            String uwagi = "";

            if (typKlienta.Equals("firma"))
            {
                if (String.IsNullOrWhiteSpace(tbNazwaFirmy.Text) || String.IsNullOrWhiteSpace(tbNIP.Text))
                {
                    uwagi += "Nazwa firmy lub NIP są puste\n";
                }
                if (tbNazwaFirmy.Text.Length > 50)
                {
                    uwagi += "Nazwa firmy nie może być dłuższa niż 50 znaków\n";
                }
                if (!kontrola.CheckOnlyNumber(tbNIP.Text) || tbNIP.Text.Length != 10)
                {
                    uwagi += "NIP musi składać się z 10 cyfr\n";
                }
            }
            else if (typKlienta.Equals("indywidualny"))
            {
                String[] imieNazwisko = tbImieNazwisko.Text.Split(' ');

                if (String.IsNullOrWhiteSpace(tbImieNazwisko.Text))
                {
                    uwagi += "Proszę podać imię i nazwisko\n";
                }
                else if (imieNazwisko.Length != 2 || tbImieNazwisko.Text.StartsWith(" ") || tbImieNazwisko.Text.EndsWith(" "))
                {
                    uwagi += "W polu \"Imię i nazwisko\" należy podać dwa ciągi znaków oddzielone spacją, np. 'Jan Kowalski'\n";
                }
                else if (imieNazwisko.Length == 2 && (imieNazwisko[0].Length > 50 || imieNazwisko[1].Length > 50))
                {
                    uwagi += "Imię i nazwisko nie mogą mieć więcej niż po 50 znaków każde\n";
                }
            }
            if (String.IsNullOrWhiteSpace(tbUlicaNr.Text) || String.IsNullOrWhiteSpace(tbMiasto.Text) || String.IsNullOrWhiteSpace(tbKodPocztowy.Text))
            {
                uwagi += "Pola Adres, Miasto i Kod nie mogą być puste\n";
            }
            if (tbUlicaNr.Text.Length > 50 || tbMiasto.Text.Length > 50)
            {
                uwagi += "Pola Adres oraz Miasto nie mogą być dłuższe niż 50 znaków\n";
            }
            if (!kontrola.CheckPostCode(tbKodPocztowy.Text) || tbKodPocztowy.Text.Length > 10)
            {
                uwagi += "Kod pocztowy może składać się z maksymalnie 10 znaków (cyfr i opcjonalnie jednego myślnika).\n";
            }
            return uwagi;
        }

        private void buttonZapiszDaneOsobowe_Click(object sender, EventArgs e)
        {
            Boolean zatwierdzonoZmianeDanychOsobowych = false;
            if (edytowanoDaneOsobowe)
            {
                String wynikSprawdzania = sprawdzDaneOsobowe();
                if (wynikSprawdzania.Length == 0)
                {
                    cmd_k.CommandText = "update dbo.Klient " + "" +
                        "set [imie] = @imie," +
                        "[nazwisko] = @nazwisko," +
                        "[nazwa_firmy] = @nazwa_firmy," +
                        "[NIP] = @NIP," +
                        "[miasto] = @miasto," +
                        "[kod_pocztowy] = @kod_pocztowy," +
                        "[ulica_nr_domu] = @ulica_nr_domu" +
                        " where [id_klienta] = @id_klienta";
                    cmd_k.CommandType = CommandType.Text;
                    cmd_k.Parameters["@miasto"].Value = tbMiasto.Text;
                    cmd_k.Parameters["@kod_pocztowy"].Value = tbKodPocztowy.Text;
                    cmd_k.Parameters["@ulica_nr_domu"].Value = tbUlicaNr.Text;

                    if (typKlienta.Equals("firma"))
                    {
                        cmd_k.Parameters["@nazwa_firmy"].Value = tbNazwaFirmy.Text;
                        cmd_k.Parameters["@NIP"].Value = tbNIP.Text;
                        cmd_k.Parameters["@imie"].Value = DBNull.Value;
                        cmd_k.Parameters["@nazwisko"].Value = DBNull.Value;
                    }
                    else if (typKlienta.Equals("indywidualny"))
                    {
                        cmd_k.Parameters["@imie"].Value = tbImieNazwisko.Text.Split(' ')[0];
                        cmd_k.Parameters["@nazwisko"].Value = tbImieNazwisko.Text.Split(' ')[1];
                        cmd_k.Parameters["@nazwa_firmy"].Value = DBNull.Value;
                        cmd_k.Parameters["@NIP"].Value = DBNull.Value;
                    }

                    sqlC_k.Open();
                    if (cmd_k.ExecuteNonQuery() == 1)
                    {
                        zatwierdzonoZmianeDanychOsobowych = true;
                    }
                    else zatwierdzonoZmianeDanychOsobowych = false;
                    sqlC_k.Close();

                    if (zatwierdzonoZmianeDanychOsobowych)
                    {
                        zapisanoDaneOsobowe = true;
                        edytowanoDaneOsobowe = false;
                        buttonZapiszDaneOsobowe.ForeColor = Color.Black;

                        tbImieNazwisko.Enabled = false;
                        tbNazwaFirmy.Enabled = false;
                        tbNIP.Enabled = false;

                        tbUlicaNr.Enabled = false;
                        tbMiasto.Enabled = false;
                        tbKodPocztowy.Enabled = false;

                        if (typKlienta.Equals("indywidualny")) imieNazwiskoKlienta = tbImieNazwisko.Text;
                        if (typKlienta.Equals("firma")) nazwaFirmy = tbNazwaFirmy.Text;
                        if (typKlienta.Equals("firma")) nipFirmy = tbNIP.Text;
                        miasto = tbMiasto.Text;
                        kodPocztowy = tbKodPocztowy.Text;
                        ulicaNr = tbUlicaNr.Text;
                    }
                }
                else
                {
                    DialogResult result = MessageBox.Show("Wystąpiły następujące błędy:\n"+wynikSprawdzania+"\n-OK- aby wrócić do edycji\n-Anuluj- aby anulować edycję danych","",MessageBoxButtons.OKCancel,MessageBoxIcon.Question);
                    if(result != DialogResult.OK)
                    {
                        zapisanoDaneOsobowe = true;
                        edytowanoDaneOsobowe = false;
                        buttonZapiszDaneOsobowe.ForeColor = Color.Black;
                        if (typKlienta.Equals("indywidualny")) tbImieNazwisko.Text = imieNazwiskoKlienta;
                        if (typKlienta.Equals("firma")) tbNazwaFirmy.Text = nazwaFirmy;
                        if (typKlienta.Equals("firma")) tbNIP.Text = nipFirmy;
                        tbMiasto.Text = miasto;
                        tbKodPocztowy.Text = kodPocztowy;
                        tbUlicaNr.Text = ulicaNr;

                        tbImieNazwisko.Enabled = false;
                        tbNazwaFirmy.Enabled = false;
                        tbNIP.Enabled = false;

                        tbUlicaNr.Enabled = false;
                        tbMiasto.Enabled = false;
                        tbKodPocztowy.Enabled = false;
                    }
                }
            }
        }
    }
}
