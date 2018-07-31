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
    public partial class ModyfikujKatalogProduktow : Form
    {
        String idObiektu;
        String conS;
        SqlConnection sqlC;
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rd;
        Boolean edytowanoBoxy;
        Boolean zapisanoZmiany;
        //tutaj zmiana nazw zmiennych do backupow
        String bNazwa, bCena, bWaga;

        public ModyfikujKatalogProduktow(String id)
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
            cmd.CommandText = "select [nazwa], [cena], [waga] from dbo.Katalog_produktow where [id_katalogowe] = @id";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters["@id"].Value = Convert.ToInt32(idObiektu);

            sqlC.Open();
            rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                labelTytul.Text = "Dane produktu";
                bNazwa = tbNazwa.Text = rd[0].ToString();
                bCena = tbCena.Text = rd[1].ToString();
                bWaga = tbWaga.Text = rd[2].ToString();
            }
            sqlC.Close();

            cmd.Parameters.Add(new SqlParameter("@nazwa", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("@cena", SqlDbType.Decimal));
            cmd.Parameters.Add(new SqlParameter("@waga", SqlDbType.Decimal));
            cmd.Parameters["@nazwa"].Value = "";
            cmd.Parameters["@cena"].Value = 0.00;
            cmd.Parameters["@waga"].Value = 0.00;
        }

        private void buttonZatwierdz_Click(object sender, EventArgs e)
        {
            String varSprawdzDane = "";
            // tutaj zmiana sprawdzanych textBoxow
            if (!tbNazwa.Text.Equals(bNazwa) || !tbCena.Text.Equals(bCena) || !tbWaga.Text.Equals(bWaga))
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
                    cmd.CommandText = "update dbo.Katalog_produktow set [nazwa] = @nazwa, [cena] = @cena, [waga] = @waga where [id_katalogowe] = @id";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters["@nazwa"].Value = tbNazwa.Text;
                    cmd.Parameters["@cena"].Value = Convert.ToDouble(tbCena.Text.Replace(".", ","));
                    cmd.Parameters["@waga"].Value = Convert.ToDouble(tbWaga.Text.Replace(".", ","));
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
                        tbNazwa.Text = bNazwa;
                        tbCena.Text = bCena;
                        tbWaga.Text = bWaga;
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
                if(text.Length > 9) return false;
                if (dotPosition < (text.Length - 3)) return false;
            }
            if (dotPosition == -1 && text.Length > 6) return false;

            return true;
        }

        private String sprawdzDane()
        {
            String uwagi = "";
            //nazwa
            if (String.IsNullOrWhiteSpace(tbNazwa.Text))
            {
                uwagi += "Nazwa produktu nie może być pusta!\n";
            }
            if(tbNazwa.Text.Length > 100)
            {
                uwagi += "Nazwa produktu nie może być dłuższa niż 100 znaków!\n";
            }
            else if (!tbNazwa.Text.Equals(bNazwa))
            {
                cmd.CommandText = "select 1 from dbo.Katalog_produktow where [nazwa] = @nazwa";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters["@nazwa"].Value = tbNazwa.Text;
                sqlC.Open();
                rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    uwagi += "Ten produkt już istnieje w bazie!\n";
                }
                sqlC.Close();
            }
            //cena
            if (String.IsNullOrWhiteSpace(tbCena.Text))
            {
                uwagi += "Cena produktu nie może być pusta!\n";
            }
            if (!CheckIfDecimal82(tbCena.Text))
            {
                uwagi += "W polu Cena maksymalnie 6 cyfr przed i 2 po kropce.\n";
            }
            //waga
            if (String.IsNullOrWhiteSpace(tbWaga.Text))
            {
                uwagi += "Waga produktu nie może być pusta!\n";
            }
            if (!CheckIfDecimal82(tbWaga.Text))
            {
                uwagi += "W polu Waga maksymalnie 6 cyfr przed i 2 po kropce.\n";
            }

            return uwagi;
        }
    }
}
