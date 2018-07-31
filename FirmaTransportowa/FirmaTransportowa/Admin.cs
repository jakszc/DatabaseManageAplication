using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FirmaTransportowa
{
    public partial class Admin : Form
    {
        String conS;
        SqlConnection sqlC;
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rd, rd2;
        String[,] columnType;
        String nazwaTabeli = "", where = "";
        List<String> whereTable;
        int rowToDel;
        int comboIndex = 0;
        bool zmianaTabeli = false;
        public Admin()
        {
            InitializeComponent();
            whereTable = new List<String>();
            columnType = new String[55, 2];
            fillColumnTypeTable();
            conS = Properties.Settings.Default.projektCS;
            sqlC = new SqlConnection(conS);
            cmd.Connection = sqlC;
            cmd.Parameters.Add(new SqlParameter("@nazwa", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("@where", SqlDbType.NVarChar));
            nazwaTabeli = "Zamowienia";
            where = "order by final.id_faktury desc";
            
            pobierzDane(nazwaTabeli, where);
            zmianaTabeli = false;
            contextMenuStrip1.Items[0].Visible = false;
            contextMenuStrip1.Items[1].Visible = false;
            contextMenuStrip1.Items[2].Visible = false;
            contextMenuStrip1.Items[3].Visible = false;
            contextMenuStrip1.Items[4].Visible = false;
            contextMenuStrip1.Items[5].Visible = false;
            contextMenuStrip1.Items[6].Visible = false;
            contextMenuStrip1.Items[7].Visible = false;
            contextMenuStrip1.Items[8].Visible = false;
            contextMenuStrip1.Items[9].Visible = false;
            contextMenuStrip1.Items[10].Visible = false;
            contextMenuStrip1.Items[11].Visible = false;
            contextMenuStrip1.Items[12].Visible = true;
            contextMenuStrip1.Items[13].Visible = true;
            contextMenuStrip1.Items[14].Visible = true;
            contextMenuStrip1.Items[15].Visible = true;
            contextMenuStrip1.Items[16].Visible = false;
            contextMenuStrip1.Items[17].Visible = false;
            contextMenuStrip1.Items[18].Visible = false;
        }

        public void fillColumnTypeTable()
        {
            columnType[0, 0] = "id"; columnType[0, 1] = "int";
            columnType[1, 0] = "Login"; columnType[1, 1] = "varchar";
            columnType[2, 0] = "Hasło"; columnType[2, 1] = "varchar";
            columnType[3, 0] = "przywileje"; columnType[3, 1] = "varchar";
            columnType[4, 0] = "id_klienta"; columnType[4, 1] = "int";

            columnType[5, 0] = "Numer faktury"; columnType[5, 1] = "int";
            columnType[6, 0] = "id_zamowienia"; columnType[6, 1] = "int";
            columnType[7, 0] = "Kwota całkowita"; columnType[7, 1] = "double";
            columnType[46, 0] = "Typ płatności"; columnType[46, 1] = "varchar";
            columnType[8, 0] = "Liczba rat"; columnType[8, 1] = "int";
            columnType[9, 0] = "Rata miesięczna"; columnType[9, 1] = "double";
            columnType[10, 0] = "Dzień m-ca płatności"; columnType[10, 1] = "int";

            columnType[11, 0] = "ID katalogowe"; columnType[11, 1] = "int";
            columnType[12, 0] = "Nazwa produktu"; columnType[12, 1] = "varchar";
            columnType[13, 0] = "Cena[zł]"; columnType[13, 1] = "double";
            columnType[14, 0] = "Waga[kg]"; columnType[14, 1] = "double";
            columnType[15, 0] = "Szt. w magazynie"; columnType[15, 1] = "int";

            columnType[16, 0] = "Miasto"; columnType[16, 1] = "varchar";
            columnType[17, 0] = "Kod"; columnType[17, 1] = "varchar";
            columnType[18, 0] = "Adres"; columnType[18, 1] = "varchar";
            columnType[19, 0] = "Imię"; columnType[19, 1] = "varchar";
            columnType[20, 0] = "Nazwisko"; columnType[20, 1] = "varchar";
            columnType[21, 0] = "Nazwa Firmy"; columnType[21, 1] = "varchar";
            columnType[22, 0] = "NIP"; columnType[22, 1] = "varchar";

            columnType[23, 0] = "Rejestracja"; columnType[23, 1] = "varchar";
            columnType[24, 0] = "Typ pojazdu"; columnType[24, 1] = "varchar";
            columnType[25, 0] = "Waga dopuszczalna"; columnType[25, 1] = "double";
            columnType[26, 0] = "Wolny od"; columnType[26, 1] = "date";

            columnType[27, 0] = "id_pracownika"; columnType[27, 1] = "int";
            columnType[28, 0] = "PESEL"; columnType[28, 1] = "varchar";
            columnType[31, 0] = "nr_umowy"; columnType[31, 1] = "varchar";
            columnType[32, 0] = "Data zatrudnienia"; columnType[32, 1] = "date";
            columnType[33, 0] = "Data zwolnienia"; columnType[33, 1] = "date";
            columnType[34, 0] = "Stawka[zł]"; columnType[34, 1] = "double";
            columnType[35, 0] = "stawka_godzinowa"; columnType[35, 1] = "double";
            columnType[36, 0] = "data_podpisania_umowy"; columnType[36, 1] = "date";
            columnType[37, 0] = "data_wygasniecia_umowy"; columnType[37, 1] = "date";

            columnType[38, 0] = "id_produktu"; columnType[38, 1] = "int";

            columnType[39, 0] = "id_punktu"; columnType[39, 1] = "int";
            columnType[40, 0] = "adres"; columnType[40, 1] = "varchar";

            columnType[41, 0] = "id_transportu"; columnType[41, 1] = "int";
            columnType[42, 0] = "Waga całkowita"; columnType[42, 1] = "double";
            columnType[43, 0] = "Data wyjazdu"; columnType[43, 1] = "date";
            columnType[44, 0] = "Miasto dostawy"; columnType[44, 1] = "varchar";

            columnType[45, 0] = "adres_dostawy"; columnType[45, 1] = "varchar";
            columnType[47, 0] = "Status"; columnType[47, 1] = "varchar";
            columnType[48, 0] = "Rodzaj umowy"; columnType[48, 1] = "varchar";
            columnType[49, 0] = "Nazwisko kierowcy"; columnType[49, 1] = "varchar";
            columnType[50, 0] = "Rejestracja pojazdu"; columnType[50, 1] = "varchar";
            columnType[51, 0] = "Dane zamawiającego"; columnType[51, 1] = "varchar";
            columnType[52, 0] = "Adres dostawy"; columnType[52, 1] = "varchar";
            columnType[53, 0] = "Data dostawy"; columnType[53, 1] = "varchar";
            columnType[54, 0] = "Dane klienta"; columnType[54, 1] = "varchar";

        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("To spowoduje wylogowanie się. Czy chcesz kontynuować?", "Uwaga!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }

        }

        private void wylogujSięToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void daneLogowaniaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Kontener.przywilej.Equals("superadmin"))
            {
                contextMenuStrip1.Items[0].Visible = true;
                contextMenuStrip1.Items[1].Visible = true;
                contextMenuStrip1.Items[2].Visible = true;
                contextMenuStrip1.Items[3].Visible = false;
                contextMenuStrip1.Items[4].Visible = false;
                contextMenuStrip1.Items[5].Visible = false;
                contextMenuStrip1.Items[6].Visible = false;
                contextMenuStrip1.Items[7].Visible = false;
                contextMenuStrip1.Items[8].Visible = false;
                contextMenuStrip1.Items[9].Visible = false;
                contextMenuStrip1.Items[10].Visible = false;
                contextMenuStrip1.Items[11].Visible = false;
                contextMenuStrip1.Items[12].Visible = false;
                contextMenuStrip1.Items[13].Visible = false;
                contextMenuStrip1.Items[14].Visible = false;
                contextMenuStrip1.Items[15].Visible = false;
                contextMenuStrip1.Items[16].Visible = false;
                contextMenuStrip1.Items[17].Visible = false;
                contextMenuStrip1.Items[18].Visible = false;

            }
            zmianaTabeli = true;
            nazwaTabeli = "Pracownicy";
            where = "";
            pobierzDane(nazwaTabeli, where );
        }

        private void fakturaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Items[0].Visible = false;
            contextMenuStrip1.Items[1].Visible = false;
            contextMenuStrip1.Items[2].Visible = false;
            contextMenuStrip1.Items[3].Visible = true;
            contextMenuStrip1.Items[4].Visible = true;
            contextMenuStrip1.Items[5].Visible = false;
            contextMenuStrip1.Items[6].Visible = false;
            contextMenuStrip1.Items[7].Visible = false;
            contextMenuStrip1.Items[8].Visible = false;
            contextMenuStrip1.Items[9].Visible = false;
            contextMenuStrip1.Items[10].Visible = false;
            contextMenuStrip1.Items[11].Visible = false;
            contextMenuStrip1.Items[12].Visible = false;
            contextMenuStrip1.Items[13].Visible = false;
            contextMenuStrip1.Items[14].Visible = false;
            contextMenuStrip1.Items[15].Visible = false;
            contextMenuStrip1.Items[16].Visible = false;
            contextMenuStrip1.Items[17].Visible = false;
            contextMenuStrip1.Items[18].Visible = false;
            zmianaTabeli = true;
            nazwaTabeli = "Faktury";
            where = "";
            pobierzDane(nazwaTabeli, where);
        }

        private void katalogProduktówToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Items[0].Visible = true;
            contextMenuStrip1.Items[1].Visible = true;
            contextMenuStrip1.Items[2].Visible = true;
            contextMenuStrip1.Items[3].Visible = false;
            contextMenuStrip1.Items[4].Visible = false;
            contextMenuStrip1.Items[5].Visible = false;
            contextMenuStrip1.Items[6].Visible = false;
            contextMenuStrip1.Items[7].Visible = false;
            contextMenuStrip1.Items[8].Visible = false;
            contextMenuStrip1.Items[9].Visible = false;
            contextMenuStrip1.Items[10].Visible = false;
            contextMenuStrip1.Items[11].Visible = false;
            contextMenuStrip1.Items[12].Visible = false;
            contextMenuStrip1.Items[13].Visible = false;
            contextMenuStrip1.Items[14].Visible = false;
            contextMenuStrip1.Items[15].Visible = false;
            contextMenuStrip1.Items[16].Visible = false;
            contextMenuStrip1.Items[17].Visible = false;
            contextMenuStrip1.Items[18].Visible = false;
            zmianaTabeli = true;
            nazwaTabeli = "KatalogProduktow";
            where = "";
            pobierzDane(nazwaTabeli, where);
        }

        private void klientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Items[0].Visible = false;
            contextMenuStrip1.Items[1].Visible = false;
            contextMenuStrip1.Items[2].Visible = false;
            contextMenuStrip1.Items[3].Visible = false;
            contextMenuStrip1.Items[4].Visible = false;
            contextMenuStrip1.Items[5].Visible = true;
            contextMenuStrip1.Items[6].Visible = false;
            contextMenuStrip1.Items[7].Visible = false;
            contextMenuStrip1.Items[8].Visible = false;
            contextMenuStrip1.Items[9].Visible = false;
            contextMenuStrip1.Items[10].Visible = false;
            contextMenuStrip1.Items[11].Visible = false;
            contextMenuStrip1.Items[12].Visible = false;
            contextMenuStrip1.Items[13].Visible = false;
            contextMenuStrip1.Items[14].Visible = false;
            contextMenuStrip1.Items[15].Visible = false;
            contextMenuStrip1.Items[16].Visible = false;
            contextMenuStrip1.Items[17].Visible = true;
            contextMenuStrip1.Items[18].Visible = true;

            zmianaTabeli = true;
            nazwaTabeli = "Klienci";
            where = "";
            pobierzDane(nazwaTabeli, where);
        }

        private void pojazdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Items[0].Visible = true;
            contextMenuStrip1.Items[1].Visible = true;
            contextMenuStrip1.Items[2].Visible = false;
            contextMenuStrip1.Items[3].Visible = false;
            contextMenuStrip1.Items[4].Visible = false;
            contextMenuStrip1.Items[5].Visible = false;
            contextMenuStrip1.Items[6].Visible = true;
            contextMenuStrip1.Items[7].Visible = false;
            contextMenuStrip1.Items[8].Visible = false;
            contextMenuStrip1.Items[9].Visible = false;
            contextMenuStrip1.Items[10].Visible = false;
            contextMenuStrip1.Items[11].Visible = false;
            contextMenuStrip1.Items[12].Visible = false;
            contextMenuStrip1.Items[13].Visible = false;
            contextMenuStrip1.Items[14].Visible = false;
            contextMenuStrip1.Items[15].Visible = false;
            contextMenuStrip1.Items[16].Visible = false;
            contextMenuStrip1.Items[17].Visible = false;
            contextMenuStrip1.Items[18].Visible = false;
            zmianaTabeli = true;
            nazwaTabeli = "Pojazdy";
            where = "";
            pobierzDane(nazwaTabeli, where);
        }

        private void KierowcaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Items[0].Visible = true;
            contextMenuStrip1.Items[1].Visible = true;
            contextMenuStrip1.Items[2].Visible = false;
            contextMenuStrip1.Items[3].Visible = false;
            contextMenuStrip1.Items[4].Visible = false;
            contextMenuStrip1.Items[5].Visible = false;
            contextMenuStrip1.Items[6].Visible = false;
            contextMenuStrip1.Items[7].Visible = true;
            contextMenuStrip1.Items[8].Visible = false;
            contextMenuStrip1.Items[9].Visible = false;
            contextMenuStrip1.Items[10].Visible = false;
            contextMenuStrip1.Items[11].Visible = false;
            contextMenuStrip1.Items[12].Visible = false;
            contextMenuStrip1.Items[13].Visible = false;
            contextMenuStrip1.Items[14].Visible = false;
            contextMenuStrip1.Items[15].Visible = false;
            contextMenuStrip1.Items[16].Visible = false;
            contextMenuStrip1.Items[17].Visible = false;
            contextMenuStrip1.Items[18].Visible = false;

            zmianaTabeli = true;
            nazwaTabeli = "Kierowcy";
            where = "";
            pobierzDane(nazwaTabeli, where);
        }


        private void punktDystrybucjiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Items[0].Visible = true;
            contextMenuStrip1.Items[1].Visible = true;
            contextMenuStrip1.Items[2].Visible = true;
            contextMenuStrip1.Items[3].Visible = false;
            contextMenuStrip1.Items[4].Visible = false;
            contextMenuStrip1.Items[5].Visible = false;
            contextMenuStrip1.Items[6].Visible = false;
            contextMenuStrip1.Items[7].Visible = false;
            contextMenuStrip1.Items[8].Visible = true;
            contextMenuStrip1.Items[9].Visible = false;
            contextMenuStrip1.Items[10].Visible = false;
            contextMenuStrip1.Items[11].Visible = false;
            contextMenuStrip1.Items[12].Visible = false;
            contextMenuStrip1.Items[13].Visible = false;
            contextMenuStrip1.Items[14].Visible = false;
            contextMenuStrip1.Items[15].Visible = false;
            contextMenuStrip1.Items[16].Visible = false;
            contextMenuStrip1.Items[17].Visible = false;
            contextMenuStrip1.Items[18].Visible = false;

            zmianaTabeli = true;
            nazwaTabeli = "PunktyDystrybucji";
            where = "";
            pobierzDane(nazwaTabeli, where);
        }

        private void transportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Items[0].Visible = false;
            contextMenuStrip1.Items[1].Visible = false;
            contextMenuStrip1.Items[2].Visible = false;
            contextMenuStrip1.Items[3].Visible = false;
            contextMenuStrip1.Items[4].Visible = false;
            contextMenuStrip1.Items[5].Visible = false;
            contextMenuStrip1.Items[6].Visible = false;
            contextMenuStrip1.Items[7].Visible = false;
            contextMenuStrip1.Items[8].Visible = false;
            contextMenuStrip1.Items[9].Visible = true;
            contextMenuStrip1.Items[10].Visible = true;
            contextMenuStrip1.Items[11].Visible = true;
            contextMenuStrip1.Items[12].Visible = false;
            contextMenuStrip1.Items[13].Visible = false;
            contextMenuStrip1.Items[14].Visible = false;
            contextMenuStrip1.Items[15].Visible = false;
            contextMenuStrip1.Items[16].Visible = true;
            contextMenuStrip1.Items[17].Visible = false;
            contextMenuStrip1.Items[18].Visible = false;

            zmianaTabeli = true;
            nazwaTabeli = "Transporty";
            where = "";
            pobierzDane(nazwaTabeli, where);
        }

        private void zamówienieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Items[0].Visible = false;
            contextMenuStrip1.Items[1].Visible = false;
            contextMenuStrip1.Items[2].Visible = false;
            contextMenuStrip1.Items[3].Visible = false;
            contextMenuStrip1.Items[4].Visible = false;
            contextMenuStrip1.Items[5].Visible = false;
            contextMenuStrip1.Items[6].Visible = false;
            contextMenuStrip1.Items[7].Visible = false;
            contextMenuStrip1.Items[8].Visible = false;
            contextMenuStrip1.Items[9].Visible = false;
            contextMenuStrip1.Items[10].Visible = false;
            contextMenuStrip1.Items[11].Visible = false;
            contextMenuStrip1.Items[12].Visible = true;
            contextMenuStrip1.Items[13].Visible = true;
            contextMenuStrip1.Items[14].Visible = true;
            contextMenuStrip1.Items[15].Visible = true;
            contextMenuStrip1.Items[16].Visible = false;
            contextMenuStrip1.Items[17].Visible = false;
            contextMenuStrip1.Items[18].Visible = false;

            zmianaTabeli = true;
            nazwaTabeli = "Zamowienia";
            where = "";
            pobierzDane(nazwaTabeli, where);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!textBox1.Text.Equals(""))
            {
                comboIndex = comboBox1.SelectedIndex;
                String type = "";
                bool isOk = true;
                for (int i = 0; i < 55; i++)
                {
                    if (columnType[i, 0] == comboBox1.Text)
                    {
                        type = columnType[i, 1];
                    }
                }
                if (type == "varchar")
                {
                    String toWhereTable = "final.[" + comboBox1.Text + "] like '%" + textBox1.Text + "%'";
                    if(!whereTable.Contains(toWhereTable))
                        whereTable.Add(toWhereTable);
                }
                else if (type == "int")
                {
                    if (checkInt(textBox1.Text))
                    {
                        String toWhereTable = "final.[" + comboBox1.Text + "] = " + textBox1.Text;
                        if (!whereTable.Contains(toWhereTable))
                            whereTable.Add(toWhereTable);
                    }
                    else
                    {
                        isOk = false;
                        MessageBox.Show("Podano błędną wartość w polu wyszukiwania dla columny typu int", "Błędna wartość w polu wyszukiwania.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (type == "double")
                {
                    if (checkDouble(textBox1.Text))
                    {
                        String toWhereTable = "final.[" + comboBox1.Text + "] = " + textBox1.Text;
                        if (!whereTable.Contains(toWhereTable))
                            whereTable.Add(toWhereTable);
                    }
                    else
                    {
                        isOk = false;
                        MessageBox.Show("Columna typu Double przyjmuje tylko cyfry i kropkę.", "Błędna wartość w polu wyszukiwania.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (type == "date")
                {
                    if (checkDate(textBox1.Text))
                    {
                        String date = parseDate(textBox1.Text);
                        String toWhereTable = "final.[" + comboBox1.Text + "] = '" + date + "'";
                        if (!whereTable.Contains(toWhereTable))
                            whereTable.Add(toWhereTable);
                    }
                    else
                    {
                        isOk = false;
                        MessageBox.Show("Data nie jest w poprawny formacie.", "Błędna wartość w polu wyszukiwania.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Nieoczekiwany błąd.", "Błędna wartość w polu wyszukiwania.", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                if (isOk)
                {
                    where = createWhereStatement();
                    pobierzDane(nazwaTabeli, where);
                }

            }

        }

        public String createWhereStatement()
        {
            String result = "where";
            if (whereTable.Count == 0)
            {
                result = "";
            }
            else
            {
                for (int i = 0; i < whereTable.Count; i++)
                {
                    if (whereTable.Count - 1 == i)
                    {
                        result += " " + whereTable[i];
                    }
                    else
                    {
                        result += " " + whereTable[i] + " and ";
                    }
                }
            }
            return result;
        }

        public bool checkInt(String text)
        {
            bool onlyNum = true;
            for (int i = 0; i < text.Length; i++)
            {
                if (!Char.IsDigit(text[i]))
                {
                    onlyNum = false;
                }
            }
            return onlyNum;
        }

        public bool checkDouble(String text)
        {
            bool onlyNum = true;
            bool onlyOne = false;
            for (int i = 0; i < text.Length; i++)
            {
                if (!Char.IsDigit(text[i]))
                {
                    if (text[i].Equals('.') && !onlyOne)
                    {
                        onlyOne = true;
                    }
                    else
                    {
                        onlyNum = false;
                    }
                }
            }
            return onlyNum;
        }

        public bool checkDate(String date)
        {
            bool valid = true;
            string[] formats = {"M/d/yyyy h:mm:ss tt", "M/d/yyyy h:mm tt",
                   "MM/dd/yyyy hh:mm:ss", "M/d/yyyy h:mm:ss",
                   "M/d/yyyy hh:mm tt", "M/d/yyyy hh tt",
                   "M/d/yyyy h:mm", "M/d/yyyy h:mm",
                   "MM/dd/yyyy hh:mm", "M/dd/yyyy hh:mm", "dd.MM.yyyy", "dd-MM-yyyy", "dd/MM/yyyy", "MM/dd/yyyy",
                   "MM/dd/yyyy","MM.dd.yyyy","MM-dd-yyyy", "yyyy.MM.dd","yyyy-MM-dd","yyyy/MM/dd" };
            DateTime datee;
            if (DateTime.TryParseExact(date, formats,
                                        new CultureInfo("en-US"),
                                        DateTimeStyles.None,
                                        out datee))
                valid = true;
            else
                valid = false;

            return valid;
        }

        public String parseDate(String date)
        {
            string[] formats = {"M/d/yyyy h:mm:ss tt", "M/d/yyyy h:mm tt",
                   "MM/dd/yyyy hh:mm:ss", "M/d/yyyy h:mm:ss",
                   "M/d/yyyy hh:mm tt", "M/d/yyyy hh tt",
                   "M/d/yyyy h:mm", "M/d/yyyy h:mm",
                   "MM/dd/yyyy hh:mm", "M/dd/yyyy hh:mm", "dd.MM.yyyy", "dd-MM-yyyy", "dd/MM/yyyy", "MM/dd/yyyy",
                   "MM/dd/yyyy","MM.dd.yyyy","MM-dd-yyyy", "yyyy.MM.dd","yyyy-MM-dd","yyyy/MM/dd" };
            DateTime datee;
            DateTime.TryParseExact(date, formats,
                                        new CultureInfo("en-US"),
                                        DateTimeStyles.None,
                                        out datee);

            return datee.ToString("yyyy-MM-dd");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (whereTable.Count > 0)
            {
                whereTable.RemoveAt(whereTable.Count - 1);
                where = createWhereStatement();
                pobierzDane(nazwaTabeli, where);
            }

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

        private void usuńToolStripMenuItem_Click(object sender, EventArgs e)
        {

                Int32 rowToDelete = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
                rowToDel = rowToDelete;
                if (rowToDelete >= 0)
                {
                    usunZBazy(dataGridView1.Rows[rowToDelete].Cells[0].Value.ToString());
                    where = createWhereStatement();
                    pobierzDane(nazwaTabeli, where);
                }
            

        }

        public void usunZBazy(String id)
        {
            String conS = Properties.Settings.Default.projektCS;
            SqlConnection sqlC = new SqlConnection(conS);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlC;
            if(nazwaTabeli == "Pracownicy" && dataGridView1.Rows[rowToDel].Cells[3].Value.ToString() != "admin")
            {
                cmd.CommandText = "select id_admina from dbo.DaneLogowania where [id] = @id";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                cmd.Parameters["@id"].Value = Convert.ToInt32(id);
                sqlC.Open();
                rd = cmd.ExecuteReader();
                int id_admina = 0;
                if (rd.Read())
                {
                    id_admina = Convert.ToInt32(rd[0].ToString());
                }
                sqlC.Close();

                cmd.CommandText = "delete from dbo.Pracownik where [id_admina] = @id";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters["@id"].Value = id_admina;
                sqlC.Open();
                cmd.ExecuteNonQuery();
                sqlC.Close();
            }
            else if (nazwaTabeli == "KatalogProduktow")
            {
                cmd.CommandText = "update dbo.Katalog_produktow set [dostepna_ilosc] = 0 where [id_katalogowe] = @id";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                cmd.Parameters["@id"].Value = Convert.ToInt32(id);
                sqlC.Open();
                cmd.ExecuteNonQuery();
                sqlC.Close();

                cmd.CommandText = "delete from dbo.Produkt where [id_zamowienia] is null and [id_katalogowe] = @id";
                cmd.CommandType = CommandType.Text;
                sqlC.Open();
                cmd.ExecuteNonQuery();
                sqlC.Close();
            }
            else if (nazwaTabeli == "Pojazdy")
            {
                cmd.CommandText = "update dbo.Pojazd set [status] = 'usunięty' where [rejestracja] = @rejestracja";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@rejestracja", SqlDbType.VarChar));
                cmd.Parameters["@rejestracja"].Value = id;
                sqlC.Open();
                cmd.ExecuteNonQuery();
                sqlC.Close();
            }
           
            else if (nazwaTabeli == "PunktyDystrybucji")
            {
                cmd.CommandText = "update dbo.Punkt_dystrybucji set status = 'nieaktywny' where id_punktu = " + id;
                cmd.CommandType = CommandType.Text;
                sqlC.Open();
                cmd.ExecuteNonQuery();
                sqlC.Close();
            }

        }

        private void dodajToolStripMenuItem_Click(object sender, EventArgs e)
        {

            dodajDoBazy();
            where = createWhereStatement();
            pobierzDane(nazwaTabeli, where);
            
        }

        public void dodajDoBazy()
        {
            String conS = Properties.Settings.Default.projektCS;
            SqlConnection sqlC = new SqlConnection(conS);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlC;

            if (nazwaTabeli == "Pracownicy")
            {
                DodajAdmin form = new DodajAdmin();
                form.ShowDialog();
                form = null;
            }
            else if (nazwaTabeli == "KatalogProduktow")
            {
                DodajProdukty form = new DodajProdukty();
                form.ShowDialog();
                form = null;
            }
            else if (nazwaTabeli == "Pojazdy")
            {
                DodajPojazd form = new DodajPojazd();
                form.ShowDialog();
                form = null;
            }
            else if (nazwaTabeli == "Kierowcy")
            {
                DodajKierowcaa form = new DodajKierowcaa();
                form.ShowDialog();
                form = null;
            }
            else if (nazwaTabeli == "PunktyDystrybucji")
            {
                DodajPunktDystrybucji form = new DodajPunktDystrybucji();
                form.ShowDialog();
                form = null;
            }
        }

        private void modyfikujToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Int32 rowToDelete = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            rowToDel = rowToDelete;
            if (rowToDelete >= 0)
            {
                modyfikujWBazie(dataGridView1.Rows[rowToDelete].Cells[0].Value.ToString());
                where = createWhereStatement();
                pobierzDane(nazwaTabeli, where);
            }
            
        }

        public void modyfikujWBazie(String id)
        {
            String conS = Properties.Settings.Default.projektCS;
            SqlConnection sqlC = new SqlConnection(conS);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlC;

            if (nazwaTabeli == "Pracownicy")
            {
                ModyfikujDaneLogowania form = new ModyfikujDaneLogowania(id);
                form.ShowDialog();
                form = null;
            }
            else if (nazwaTabeli == "KatalogProduktow")
            {
                ModyfikujKatalogProduktow form = new ModyfikujKatalogProduktow(id);
                form.ShowDialog();
                form = null;
            }
            else if (nazwaTabeli == "Pojazdy")
            {
                ModyfikujPojazd form = new ModyfikujPojazd(id);
                form.ShowDialog();
                form = null;
            }
            else if (nazwaTabeli == "Kierowcy")
            {
                ModyfikujKierowcaa form = new ModyfikujKierowcaa(id);
                form.ShowDialog();
                form = null;
            }
            else if (nazwaTabeli == "PunktyDystrybucji")
            {
                ModyfikujPunktDystrybucji form = new ModyfikujPunktDystrybucji(id);
                form.ShowDialog();
                form = null;
            }
        }

        private void nazwaćToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NoweZamowienie form = new NoweZamowienie();
            form.ShowDialog();
            pobierzDane(nazwaTabeli, where);
        }

        private void zobaczSzczegółyZamówieniaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Int32 rowToDelete = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (rowToDelete >= 0)
            {
                Kontener.idZamowienia = Convert.ToInt32(dataGridView1.Rows[rowToDelete].Cells[0].Value.ToString());
                Kontener.sumWaga = dataGridView1.Rows[rowToDelete].Cells[8].Value.ToString();
                Kontener.sumKwota = dataGridView1.Rows[rowToDelete].Cells[7].Value.ToString();
                Kontener.dataTransportu = DateTime.Parse(dataGridView1.Rows[rowToDelete].Cells[9].Value.ToString()).AddDays(-1) ;
                PodgladZamowieniaAdmin form = new PodgladZamowieniaAdmin();
                form.ShowDialog();
                form = null;
                pobierzDane(nazwaTabeli, where);
            }
        }

        private void przejdźDoZamówieńToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Int32 rowToDelete = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (rowToDelete >= 0)
            {
                whereTable.Clear();
                nazwaTabeli = "Zamowienia";
                whereTable.Add("final.[" + dataGridView1.Columns[0].HeaderText + "] = " + dataGridView1.Rows[rowToDelete].Cells[0].Value.ToString());
                where = createWhereStatement();
                pobierzDane(nazwaTabeli, where);
                contextMenuStrip1.Items[0].Visible = false;
                contextMenuStrip1.Items[1].Visible = false;
                contextMenuStrip1.Items[2].Visible = false;
                contextMenuStrip1.Items[3].Visible = false;
                contextMenuStrip1.Items[4].Visible = false;
                contextMenuStrip1.Items[5].Visible = false;
                contextMenuStrip1.Items[6].Visible = false;
                contextMenuStrip1.Items[7].Visible = false;
                contextMenuStrip1.Items[8].Visible = false;
                contextMenuStrip1.Items[9].Visible = false;
                contextMenuStrip1.Items[10].Visible = false;
                contextMenuStrip1.Items[11].Visible = false;
                contextMenuStrip1.Items[12].Visible = true;
                contextMenuStrip1.Items[13].Visible = true;
                contextMenuStrip1.Items[14].Visible = true;
                contextMenuStrip1.Items[15].Visible = true;
                contextMenuStrip1.Items[16].Visible = false;
                contextMenuStrip1.Items[17].Visible = false;
                contextMenuStrip1.Items[18].Visible = false;
                comboIndex = 0;
            }           
        }

        private void przejdźDoKlientaFakturyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Int32 rowToDelete = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (rowToDelete >= 0)
            {
                whereTable.Clear();
                nazwaTabeli = "Klienci";
                whereTable.Add("final.[" + dataGridView1.Columns[1].HeaderText + "] = " + dataGridView1.Rows[rowToDelete].Cells[1].Value.ToString());
                where = createWhereStatement();
                pobierzDane(nazwaTabeli, where);
                contextMenuStrip1.Items[0].Visible = false;
                contextMenuStrip1.Items[1].Visible = false;
                contextMenuStrip1.Items[2].Visible = false;
                contextMenuStrip1.Items[3].Visible = false;
                contextMenuStrip1.Items[4].Visible = false;
                contextMenuStrip1.Items[5].Visible = true;
                contextMenuStrip1.Items[6].Visible = false;
                contextMenuStrip1.Items[7].Visible = false;
                contextMenuStrip1.Items[8].Visible = false;
                contextMenuStrip1.Items[9].Visible = false;
                contextMenuStrip1.Items[10].Visible = false;
                contextMenuStrip1.Items[11].Visible = false;
                contextMenuStrip1.Items[12].Visible = false;
                contextMenuStrip1.Items[13].Visible = false;
                contextMenuStrip1.Items[14].Visible = false;
                contextMenuStrip1.Items[15].Visible = false;
                contextMenuStrip1.Items[16].Visible = false;
                contextMenuStrip1.Items[17].Visible = true;
                contextMenuStrip1.Items[18].Visible = true;
                comboIndex = 0; 
            }
        }

        private void przejdźDoZamówieńKlientaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Int32 rowToDelete = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (rowToDelete >= 0)
            {
                whereTable.Clear();
                nazwaTabeli = "Zamowienia";
                whereTable.Add("final.[" + dataGridView1.Columns[0].HeaderText + "] = " + dataGridView1.Rows[rowToDelete].Cells[0].Value.ToString());
                where = createWhereStatement();
                pobierzDane(nazwaTabeli, where);
                contextMenuStrip1.Items[0].Visible = false;
                contextMenuStrip1.Items[1].Visible = false;
                contextMenuStrip1.Items[2].Visible = false;
                contextMenuStrip1.Items[3].Visible = false;
                contextMenuStrip1.Items[4].Visible = false;
                contextMenuStrip1.Items[5].Visible = false;
                contextMenuStrip1.Items[6].Visible = false;
                contextMenuStrip1.Items[7].Visible = false;
                contextMenuStrip1.Items[8].Visible = false;
                contextMenuStrip1.Items[9].Visible = false;
                contextMenuStrip1.Items[10].Visible = false;
                contextMenuStrip1.Items[11].Visible = false;
                contextMenuStrip1.Items[12].Visible = true;
                contextMenuStrip1.Items[13].Visible = true;
                contextMenuStrip1.Items[14].Visible = true;
                contextMenuStrip1.Items[15].Visible = true;
                contextMenuStrip1.Items[16].Visible = false;
                contextMenuStrip1.Items[17].Visible = false;
                contextMenuStrip1.Items[18].Visible = false;
                comboIndex = 0;
            }
        }

        private void przejdźDoTransportówToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Int32 rowToDelete = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (rowToDelete >= 0)
            {
                whereTable.Clear();
                nazwaTabeli = "Transporty";
                whereTable.Add("final.[Rejestracja pojazdu] = '" + dataGridView1.Rows[rowToDelete].Cells[0].Value.ToString() + "'");
                where = createWhereStatement();
                pobierzDane(nazwaTabeli, where);
                contextMenuStrip1.Items[0].Visible = false;
                contextMenuStrip1.Items[1].Visible = false;
                contextMenuStrip1.Items[2].Visible = false;
                contextMenuStrip1.Items[3].Visible = false;
                contextMenuStrip1.Items[4].Visible = false;
                contextMenuStrip1.Items[5].Visible = false;
                contextMenuStrip1.Items[6].Visible = false;
                contextMenuStrip1.Items[7].Visible = false;
                contextMenuStrip1.Items[8].Visible = false;
                contextMenuStrip1.Items[9].Visible = true;
                contextMenuStrip1.Items[10].Visible = true;
                contextMenuStrip1.Items[11].Visible = true;
                contextMenuStrip1.Items[12].Visible = false;
                contextMenuStrip1.Items[13].Visible = false;
                contextMenuStrip1.Items[14].Visible = false;
                contextMenuStrip1.Items[15].Visible = false;
                contextMenuStrip1.Items[16].Visible = true;
                contextMenuStrip1.Items[17].Visible = false;
                contextMenuStrip1.Items[18].Visible = false;
                comboIndex = 0;
            }
        }

        private void przejdźDoTransportówKierowcyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Int32 rowToDelete = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (rowToDelete >= 0)
            {
                whereTable.Clear();
                nazwaTabeli = "Transporty";
                whereTable.Add("final.[id_pracownika] = " + dataGridView1.Rows[rowToDelete].Cells[0].Value.ToString());
                where = createWhereStatement();
                pobierzDane(nazwaTabeli, where);
                contextMenuStrip1.Items[0].Visible = false;
                contextMenuStrip1.Items[1].Visible = false;
                contextMenuStrip1.Items[2].Visible = false;
                contextMenuStrip1.Items[3].Visible = false;
                contextMenuStrip1.Items[4].Visible = false;
                contextMenuStrip1.Items[5].Visible = false;
                contextMenuStrip1.Items[6].Visible = false;
                contextMenuStrip1.Items[7].Visible = false;
                contextMenuStrip1.Items[8].Visible = false;
                contextMenuStrip1.Items[9].Visible = true;
                contextMenuStrip1.Items[10].Visible = true;
                contextMenuStrip1.Items[11].Visible = true;
                contextMenuStrip1.Items[12].Visible = false;
                contextMenuStrip1.Items[13].Visible = false;
                contextMenuStrip1.Items[14].Visible = false;
                contextMenuStrip1.Items[15].Visible = false;
                contextMenuStrip1.Items[16].Visible = true;
                contextMenuStrip1.Items[17].Visible = false;
                contextMenuStrip1.Items[18].Visible = false;
                comboIndex = 0;
            }
        }

        private void przejdźDoZamówieńPunktuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Int32 rowToDelete = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (rowToDelete >= 0)
            {
                whereTable.Clear();
                nazwaTabeli = "Zamowienia";
                whereTable.Add("final.[" + dataGridView1.Columns[0].HeaderText + "] = " + dataGridView1.Rows[rowToDelete].Cells[0].Value.ToString());
                where = createWhereStatement();
                pobierzDane(nazwaTabeli, where);
                contextMenuStrip1.Items[0].Visible = false;
                contextMenuStrip1.Items[1].Visible = false;
                contextMenuStrip1.Items[2].Visible = false;
                contextMenuStrip1.Items[3].Visible = false;
                contextMenuStrip1.Items[4].Visible = false;
                contextMenuStrip1.Items[5].Visible = false;
                contextMenuStrip1.Items[6].Visible = false;
                contextMenuStrip1.Items[7].Visible = false;
                contextMenuStrip1.Items[8].Visible = false;
                contextMenuStrip1.Items[9].Visible = false;
                contextMenuStrip1.Items[10].Visible = false;
                contextMenuStrip1.Items[11].Visible = false;
                contextMenuStrip1.Items[12].Visible = true;
                contextMenuStrip1.Items[13].Visible = true;
                contextMenuStrip1.Items[14].Visible = true;
                contextMenuStrip1.Items[15].Visible = true;
                contextMenuStrip1.Items[16].Visible = false;
                contextMenuStrip1.Items[17].Visible = false;
                contextMenuStrip1.Items[18].Visible = false;
                comboIndex = 0;
            }
        }

        private void przejdźDoZamówieńWTransporcieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Int32 rowToDelete = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (rowToDelete >= 0)
            {
                whereTable.Clear();
                nazwaTabeli = "Zamowienia";
                whereTable.Add("final.[" + dataGridView1.Columns[0].HeaderText + "] = " + dataGridView1.Rows[rowToDelete].Cells[0].Value.ToString());
                where = createWhereStatement();
                pobierzDane(nazwaTabeli, where);
                contextMenuStrip1.Items[0].Visible = false;
                contextMenuStrip1.Items[1].Visible = false;
                contextMenuStrip1.Items[2].Visible = false;
                contextMenuStrip1.Items[3].Visible = false;
                contextMenuStrip1.Items[4].Visible = false;
                contextMenuStrip1.Items[5].Visible = false;
                contextMenuStrip1.Items[6].Visible = false;
                contextMenuStrip1.Items[7].Visible = false;
                contextMenuStrip1.Items[8].Visible = false;
                contextMenuStrip1.Items[9].Visible = false;
                contextMenuStrip1.Items[10].Visible = false;
                contextMenuStrip1.Items[11].Visible = false;
                contextMenuStrip1.Items[12].Visible = true;
                contextMenuStrip1.Items[13].Visible = true;
                contextMenuStrip1.Items[14].Visible = true;
                contextMenuStrip1.Items[15].Visible = true;
                contextMenuStrip1.Items[16].Visible = false;
                contextMenuStrip1.Items[17].Visible = false;
                contextMenuStrip1.Items[18].Visible = false;
                comboIndex = 0;
            }
        }

        private void przejdźDoPracownikaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Int32 rowToDelete = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (rowToDelete >= 0)
            {
                whereTable.Clear();
                nazwaTabeli = "Kierowcy";
                whereTable.Add("final.[id_pracownika] = " + dataGridView1.Rows[rowToDelete].Cells[1].Value.ToString());
                where = createWhereStatement();
                pobierzDane(nazwaTabeli, where);
                contextMenuStrip1.Items[0].Visible = true;
                contextMenuStrip1.Items[1].Visible = true;
                contextMenuStrip1.Items[2].Visible = false;
                contextMenuStrip1.Items[3].Visible = false;
                contextMenuStrip1.Items[4].Visible = false;
                contextMenuStrip1.Items[5].Visible = false;
                contextMenuStrip1.Items[6].Visible = false;
                contextMenuStrip1.Items[7].Visible = true;
                contextMenuStrip1.Items[8].Visible = false;
                contextMenuStrip1.Items[9].Visible = false;
                contextMenuStrip1.Items[10].Visible = false;
                contextMenuStrip1.Items[11].Visible = false;
                contextMenuStrip1.Items[12].Visible = false;
                contextMenuStrip1.Items[13].Visible = false;
                contextMenuStrip1.Items[14].Visible = false;
                contextMenuStrip1.Items[15].Visible = false;
                contextMenuStrip1.Items[16].Visible = false;
                contextMenuStrip1.Items[17].Visible = false;
                contextMenuStrip1.Items[18].Visible = false;
                comboIndex = 0;
            }
        }

        private void przejdźDoPojazduToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Int32 rowToDelete = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (rowToDelete >= 0)
            {
                whereTable.Clear();
                nazwaTabeli = "Pojazdy";
                whereTable.Add("final.[Rejestracja] = '" + dataGridView1.Rows[rowToDelete].Cells[6].Value.ToString() + "'");
                where = createWhereStatement();
                pobierzDane(nazwaTabeli, where);
                contextMenuStrip1.Items[0].Visible = true;
                contextMenuStrip1.Items[1].Visible = true;
                contextMenuStrip1.Items[2].Visible = false;
                contextMenuStrip1.Items[3].Visible = false;
                contextMenuStrip1.Items[4].Visible = false;
                contextMenuStrip1.Items[5].Visible = false;
                contextMenuStrip1.Items[6].Visible = true;
                contextMenuStrip1.Items[7].Visible = false;
                contextMenuStrip1.Items[8].Visible = false;
                contextMenuStrip1.Items[9].Visible = false;
                contextMenuStrip1.Items[10].Visible = false;
                contextMenuStrip1.Items[11].Visible = false;
                contextMenuStrip1.Items[12].Visible = false;
                contextMenuStrip1.Items[13].Visible = false;
                contextMenuStrip1.Items[14].Visible = false;
                contextMenuStrip1.Items[15].Visible = false;
                contextMenuStrip1.Items[16].Visible = false;
                contextMenuStrip1.Items[17].Visible = false;
                contextMenuStrip1.Items[18].Visible = false;
                comboIndex = 0;
            }
        }

        private void przejdźDoKlientaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Int32 rowToDelete = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (rowToDelete >= 0)
            {
                whereTable.Clear();
                nazwaTabeli = "Klienci";
                whereTable.Add("final.[" + dataGridView1.Columns[3].HeaderText + "] = " + dataGridView1.Rows[rowToDelete].Cells[3].Value.ToString());
                where = createWhereStatement();
                pobierzDane(nazwaTabeli, where);
                contextMenuStrip1.Items[0].Visible = false;
                contextMenuStrip1.Items[1].Visible = false;
                contextMenuStrip1.Items[2].Visible = false;
                contextMenuStrip1.Items[3].Visible = false;
                contextMenuStrip1.Items[4].Visible = false;
                contextMenuStrip1.Items[5].Visible = true;
                contextMenuStrip1.Items[6].Visible = false;
                contextMenuStrip1.Items[7].Visible = false;
                contextMenuStrip1.Items[8].Visible = false;
                contextMenuStrip1.Items[9].Visible = false;
                contextMenuStrip1.Items[10].Visible = false;
                contextMenuStrip1.Items[11].Visible = false;
                contextMenuStrip1.Items[12].Visible = false;
                contextMenuStrip1.Items[13].Visible = false;
                contextMenuStrip1.Items[14].Visible = false;
                contextMenuStrip1.Items[15].Visible = false;
                contextMenuStrip1.Items[16].Visible = false;
                contextMenuStrip1.Items[17].Visible = true;
                contextMenuStrip1.Items[18].Visible = true;
                comboIndex = 0;
            }
        }

        private void przejdźDoTransportuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Int32 rowToDelete = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (rowToDelete >= 0)
            {
                whereTable.Clear();
                nazwaTabeli = "Transporty";
                whereTable.Add("final.[" + dataGridView1.Columns[1].HeaderText + "] = " + dataGridView1.Rows[rowToDelete].Cells[1].Value.ToString());
                where = createWhereStatement();
                pobierzDane(nazwaTabeli, where);
                contextMenuStrip1.Items[0].Visible = false;
                contextMenuStrip1.Items[1].Visible = false;
                contextMenuStrip1.Items[2].Visible = false;
                contextMenuStrip1.Items[3].Visible = false;
                contextMenuStrip1.Items[4].Visible = false;
                contextMenuStrip1.Items[5].Visible = false;
                contextMenuStrip1.Items[6].Visible = false;
                contextMenuStrip1.Items[7].Visible = false;
                contextMenuStrip1.Items[8].Visible = false;
                contextMenuStrip1.Items[9].Visible = true;
                contextMenuStrip1.Items[10].Visible = true;
                contextMenuStrip1.Items[11].Visible = true;
                contextMenuStrip1.Items[12].Visible = false;
                contextMenuStrip1.Items[13].Visible = false;
                contextMenuStrip1.Items[14].Visible = false;
                contextMenuStrip1.Items[15].Visible = false;
                contextMenuStrip1.Items[16].Visible = true;
                contextMenuStrip1.Items[17].Visible = false;
                contextMenuStrip1.Items[18].Visible = false;
                comboIndex = 0;
            }
        }

        private void przejdźDoFakturyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Int32 rowToDelete = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (rowToDelete >= 0)
            {
                whereTable.Clear();
                nazwaTabeli = "Faktury";
                whereTable.Add("final.[Numer faktury] = " + dataGridView1.Rows[rowToDelete].Cells[2].Value.ToString());
                where = createWhereStatement(); 
                pobierzDane(nazwaTabeli, where);
                contextMenuStrip1.Items[0].Visible = false;
                contextMenuStrip1.Items[1].Visible = false;
                contextMenuStrip1.Items[2].Visible = false;
                contextMenuStrip1.Items[3].Visible = true;
                contextMenuStrip1.Items[4].Visible = true;
                contextMenuStrip1.Items[5].Visible = false;
                contextMenuStrip1.Items[6].Visible = false;
                contextMenuStrip1.Items[7].Visible = false;
                contextMenuStrip1.Items[8].Visible = false;
                contextMenuStrip1.Items[9].Visible = false;
                contextMenuStrip1.Items[10].Visible = false;
                contextMenuStrip1.Items[11].Visible = false;
                contextMenuStrip1.Items[12].Visible = false;
                contextMenuStrip1.Items[13].Visible = false;
                contextMenuStrip1.Items[14].Visible = false;
                contextMenuStrip1.Items[15].Visible = false;
                contextMenuStrip1.Items[16].Visible = false;
                contextMenuStrip1.Items[17].Visible = false;
                contextMenuStrip1.Items[18].Visible = false;
                comboIndex = 0;
            }
        }

        private void przesuńWyjazdTransportuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Int32 rowToDelete = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (rowToDelete >= 0)
            {
                if(DateTime.Parse(dataGridView1.Rows[rowToDelete].Cells[2].Value.ToString()) > DateTime.Now.Date)
                {
                    String conS2;
                    SqlConnection sqlC2;
                    SqlCommand cmd2 = new SqlCommand();
                    conS2 = Properties.Settings.Default.projektCS;
                    sqlC2 = new SqlConnection(conS2);
                    cmd2.Connection = sqlC2;

                    cmd2.CommandText = "select id_zamowienia from dbo.Zamowienie where id_transportu = @id_trans";
                    cmd2.CommandType = CommandType.Text;
                    cmd2.Parameters.Add(new SqlParameter("@id_trans", SqlDbType.Int));

                    cmd2.Parameters["@id_trans"].Value = dataGridView1.Rows[rowToDelete].Cells[0].Value;
                    sqlC2.Open();
                    rd = cmd2.ExecuteReader();
                    while (rd.Read())
                    {
                        String conS3 = Properties.Settings.Default.projektCS;
                        SqlConnection sqlC3 = new SqlConnection(conS3);
                        SqlCommand cmd3 = new SqlCommand();
                        cmd3.Connection = sqlC3;

                        cmd3.CommandText = "anulujProdukty";
                        cmd3.CommandType = CommandType.StoredProcedure;
                        cmd3.Parameters.Add(new SqlParameter("@id_zamowienia", SqlDbType.Int));
                        cmd3.Parameters["@id_zamowienia"].Value = Convert.ToInt32(rd[0].ToString());
                        sqlC3.Open();
                        cmd3.ExecuteNonQuery();
                        sqlC3.Close();


                        cmd3.CommandText = "select * from dbo.policzProdukty()";
                        cmd3.CommandType = CommandType.Text;
                        sqlC3.Open();
                        rd2 = cmd3.ExecuteReader();
                        while (rd2.Read())
                        {
                            String conS4 = Properties.Settings.Default.projektCS;
                            SqlConnection sqlC4 = new SqlConnection(conS4);
                            SqlCommand cmd4 = new SqlCommand();
                            cmd4.Connection = sqlC4;

                            cmd4.CommandText = "update dbo.Katalog_produktow set [dostepna_ilosc] = @dostepna_ilosc where [id_katalogowe] = @id_katalogowe";
                            cmd4.CommandType = CommandType.Text;
                            cmd4.Parameters.Add(new SqlParameter("@dostepna_ilosc", SqlDbType.Int));
                            cmd4.Parameters.Add(new SqlParameter("@id_katalogowe", SqlDbType.Int));
                            cmd4.Parameters["@dostepna_ilosc"].Value = Convert.ToInt32(rd2[1].ToString());
                            cmd4.Parameters["@id_katalogowe"].Value = Convert.ToInt32(rd2[0].ToString());
                            sqlC4.Open();
                            cmd4.ExecuteNonQuery();
                            sqlC4.Close();

                        }
                        sqlC3.Close();

                        cmd3.CommandText = "anulujZamowienie";
                        cmd3.CommandType = CommandType.StoredProcedure;
                        cmd3.Parameters["@id_zamowienia"].Value = Convert.ToInt32(rd[0].ToString());
                        sqlC3.Open();
                        cmd3.ExecuteNonQuery();
                        sqlC3.Close();
                    }
                    sqlC2.Close();
                    pobierzDane(nazwaTabeli, where);
                }
                else
                {
                    MessageBox.Show("Nie można anulować transportu który już wyjechał.", "Błąd.", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
        }

        private void usuńKlientaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Int32 rowToDelete = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (rowToDelete >= 0)
            {
                String conS2;
                SqlConnection sqlC2;
                SqlCommand cmd2 = new SqlCommand();
                conS2 = Properties.Settings.Default.projektCS;
                sqlC2 = new SqlConnection(conS2);
                cmd2.Connection = sqlC2;

                cmd2.CommandText = "select count(*) from dbo.Zamowienie where id_klienta = @id_klienta";
                cmd2.CommandType = CommandType.Text;
                cmd2.Parameters.Add(new SqlParameter("@id_klienta", SqlDbType.Int));

                cmd2.Parameters["@id_klienta"].Value = dataGridView1.Rows[rowToDelete].Cells[0].Value;
                sqlC2.Open();
                rd = cmd2.ExecuteReader();
                int liczbaZamowien = 0;
                if(rd.Read())
                {
                    liczbaZamowien = Convert.ToInt32(rd[0].ToString());
                }
                sqlC2.Close();

                cmd2.CommandText = "niezrealizowaneZamowienia";
                cmd2.CommandType = CommandType.StoredProcedure;

                cmd2.Parameters["@id_klienta"].Value = dataGridView1.Rows[rowToDelete].Cells[0].Value;
                sqlC2.Open();
                rd = cmd2.ExecuteReader();
                int liczbaNiezrealizownychZamowien = 0;
                if(rd.Read())
                    liczbaNiezrealizownychZamowien = Convert.ToInt32(rd[0].ToString());
                if (liczbaZamowien != liczbaNiezrealizownychZamowien)
                {
                    DialogResult result = MessageBox.Show("Nie można usunąć danych klienta z bazy gdyż ma już zrealizowane zamówienia.\nCzy chcesz dezaktywować klienta i usunąć jego niezrealizowane zamówienia?", "Ostrzeżenie.", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if(DialogResult.Yes == result)
                    {
                        while (rd.Read())
                        {
                            String conS3 = Properties.Settings.Default.projektCS;
                            SqlConnection sqlC3 = new SqlConnection(conS3);
                            SqlCommand cmd3 = new SqlCommand();
                            cmd3.Connection = sqlC3;

                            cmd3.CommandText = "anulujProdukty";
                            cmd3.CommandType = CommandType.StoredProcedure;
                            cmd3.Parameters.Add(new SqlParameter("@id_zamowienia", SqlDbType.Int));
                            cmd3.Parameters["@id_zamowienia"].Value = Convert.ToInt32(rd[0].ToString());
                            sqlC3.Open();
                            cmd3.ExecuteNonQuery();
                            sqlC3.Close();


                            cmd3.CommandText = "select * from dbo.policzProdukty()";
                            cmd3.CommandType = CommandType.Text;
                            sqlC3.Open();
                            rd2 = cmd3.ExecuteReader();
                            while (rd2.Read())
                            {
                                String conS4 = Properties.Settings.Default.projektCS;
                                SqlConnection sqlC4 = new SqlConnection(conS4);
                                SqlCommand cmd4 = new SqlCommand();
                                cmd4.Connection = sqlC4;

                                cmd4.CommandText = "update dbo.Katalog_produktow set [dostepna_ilosc] = @dostepna_ilosc where [id_katalogowe] = @id_katalogowe";
                                cmd4.CommandType = CommandType.Text;
                                cmd4.Parameters.Add(new SqlParameter("@dostepna_ilosc", SqlDbType.Int));
                                cmd4.Parameters.Add(new SqlParameter("@id_katalogowe", SqlDbType.Int));
                                cmd4.Parameters["@dostepna_ilosc"].Value = Convert.ToInt32(rd2[1].ToString());
                                cmd4.Parameters["@id_katalogowe"].Value = Convert.ToInt32(rd2[0].ToString());
                                sqlC4.Open();
                                cmd4.ExecuteNonQuery();
                                sqlC4.Close();

                            }
                            sqlC3.Close();

                            cmd3.CommandText = "anulujZamowienie";
                            cmd3.CommandType = CommandType.StoredProcedure;
                            cmd3.Parameters["@id_zamowienia"].Value = Convert.ToInt32(rd[0].ToString());
                            sqlC3.Open();
                            cmd3.ExecuteNonQuery();
                            sqlC3.Close();

                        }
                        String conS30 = Properties.Settings.Default.projektCS;
                        SqlConnection sqlC30 = new SqlConnection(conS30);
                        SqlCommand cmd30 = new SqlCommand();
                        cmd30.Connection = sqlC30;

                        cmd30.CommandText = "update dbo.DaneLogowania set status = 'nieaktywny' where id_klienta = @id_klienta";
                        cmd30.CommandType = CommandType.Text;
                        cmd30.Parameters.Add(new SqlParameter("@id_klienta", SqlDbType.Int));
                        cmd30.Parameters["@id_klienta"].Value = dataGridView1.Rows[rowToDelete].Cells[0].Value;
                        sqlC30.Open();
                        cmd30.ExecuteNonQuery();
                        sqlC30.Close();
                    }

                }
                else
                {
                    DialogResult result = MessageBox.Show("Czy na pewno chcesz usunąć klienta?\n UWAGA zostaną usunięte też jego wszystkie zamówienia!", "Ostrzeżenie.", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (DialogResult.Yes == result)
                    {
                        while (rd.Read())
                        {
                            String conS3 = Properties.Settings.Default.projektCS;
                            SqlConnection sqlC3 = new SqlConnection(conS3);
                            SqlCommand cmd3 = new SqlCommand();
                            cmd3.Connection = sqlC3;

                            cmd3.CommandText = "anulujProdukty";
                            cmd3.CommandType = CommandType.StoredProcedure;
                            cmd3.Parameters.Add(new SqlParameter("@id_zamowienia", SqlDbType.Int));
                            cmd3.Parameters["@id_zamowienia"].Value = Convert.ToInt32(rd[0].ToString());
                            sqlC3.Open();
                            cmd3.ExecuteNonQuery();
                            sqlC3.Close();


                            cmd3.CommandText = "select * from dbo.policzProdukty()";
                            cmd3.CommandType = CommandType.Text;
                            sqlC3.Open();
                            rd2 = cmd3.ExecuteReader();
                            while (rd2.Read())
                            {
                                String conS4 = Properties.Settings.Default.projektCS;
                                SqlConnection sqlC4 = new SqlConnection(conS4);
                                SqlCommand cmd4 = new SqlCommand();
                                cmd4.Connection = sqlC4;

                                cmd4.CommandText = "update dbo.Katalog_produktow set [dostepna_ilosc] = @dostepna_ilosc where [id_katalogowe] = @id_katalogowe";
                                cmd4.CommandType = CommandType.Text;
                                cmd4.Parameters.Add(new SqlParameter("@dostepna_ilosc", SqlDbType.Int));
                                cmd4.Parameters.Add(new SqlParameter("@id_katalogowe", SqlDbType.Int));
                                cmd4.Parameters["@dostepna_ilosc"].Value = Convert.ToInt32(rd2[1].ToString());
                                cmd4.Parameters["@id_katalogowe"].Value = Convert.ToInt32(rd2[0].ToString());
                                sqlC4.Open();
                                cmd4.ExecuteNonQuery();
                                sqlC4.Close();

                            }
                            sqlC3.Close();

                            cmd3.CommandText = "anulujZamowienie";
                            cmd3.CommandType = CommandType.StoredProcedure;
                            cmd3.Parameters["@id_zamowienia"].Value = Convert.ToInt32(rd[0].ToString());
                            sqlC3.Open();
                            cmd3.ExecuteNonQuery();
                            sqlC3.Close();

                        }
                        String conS30 = Properties.Settings.Default.projektCS;
                        SqlConnection sqlC30 = new SqlConnection(conS30);
                        SqlCommand cmd30 = new SqlCommand();
                        cmd30.Connection = sqlC30;

                        cmd30.CommandText = "delete from dbo.Klient where id_klienta = @id_klienta";
                        cmd30.CommandType = CommandType.Text;
                        cmd30.Parameters.Add(new SqlParameter("@id_klienta", SqlDbType.Int));
                        cmd30.Parameters["@id_klienta"].Value = dataGridView1.Rows[rowToDelete].Cells[0].Value;
                        sqlC30.Open();
                        cmd30.ExecuteNonQuery();
                        sqlC30.Close();
                    }
                }
                
                sqlC2.Close();
                pobierzDane(nazwaTabeli, where);
            }
        }

        private void resetujHasłoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Int32 rowToDelete = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (rowToDelete >= 0)
            {
                DialogResult result = MessageBox.Show("Jesteś pewny że chcesz zresetować hasło klientowi: " + dataGridView1.Rows[rowToDelete].Cells[2].Value.ToString() + "?", "Ostrzeżenie.", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if(DialogResult.Yes == result)
                {
                    var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                    var stringChars = new char[8];
                    var random = new Random();

                    for (int i = 0; i < stringChars.Length; i++)
                    {
                        stringChars[i] = chars[random.Next(chars.Length)];
                    }

                    var finalString = new String(stringChars);
                    String haslo = finalString;

                    String conS2;
                    SqlConnection sqlC2;
                    SqlCommand cmd2 = new SqlCommand();
                    conS2 = Properties.Settings.Default.projektCS;
                    sqlC2 = new SqlConnection(conS2);
                    cmd2.Connection = sqlC2;

                    cmd2.CommandText = "update dbo.DaneLogowania set [haslo] = @haslo where [id_klienta] = @id_klienta";
                    cmd2.CommandType = CommandType.Text;
                    cmd2.Parameters.Add(new SqlParameter("@haslo", SqlDbType.VarChar));
                    cmd2.Parameters.Add(new SqlParameter("@id_klienta", SqlDbType.Int));

                    cmd2.Parameters["@haslo"].Value = haslo;
                    cmd2.Parameters["@id_klienta"].Value = dataGridView1.Rows[rowToDelete].Cells[0].Value;
                    sqlC2.Open();
                    int tmp = cmd2.ExecuteNonQuery();
                    if(tmp != 1)
                    {
                        MessageBox.Show("Dziwne w tytule ilość zmienionych haseł.", tmp.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    }
                    sqlC2.Close();
                    MessageBox.Show("Nowe hasło to:\n" + haslo, "Zmiana hasla" , MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
        }

        public void pobierzDane(String nazwaTabeli, String where)
        {
            cmd.CommandText = "dbo.adminSelect";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters["@nazwa"].Value = nazwaTabeli;
            cmd.Parameters["@where"].Value = where;
            sqlC.Open();
            cmd.ExecuteNonQuery();
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                adap.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            
            sqlC.Close();


            if (nazwaTabeli == "Pracownicy")
            {
                dataGridView1.Columns[0].Visible = false;
                if (Kontener.przywilej.Equals("admin"))
                {
                    dataGridView1.Columns[2].Visible = false;
                    dataGridView1.Columns[5].Visible = false;
                    dataGridView1.Columns[6].Visible = false;
                    dataGridView1.Columns[7].Visible = false;
                }
            }
            if (nazwaTabeli == "Kierowcy" || nazwaTabeli == "PunktyDystrybucji" || nazwaTabeli == "Klienci")
            {
                dataGridView1.Columns[0].Visible = false;
            }
            if (nazwaTabeli == "Zamowienia")
            {
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[2].Visible = false;
                dataGridView1.Columns[3].Visible = false;
                dataGridView1.Columns[4].Visible = false;
            }
            if (nazwaTabeli == "Faktury")
            {
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Visible = false;
            }
            if (nazwaTabeli == "Transporty")
            {
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Visible = false;
            }


            comboBox1.Items.Clear();
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                if(dataGridView1.Columns[i].Visible)
                    comboBox1.Items.Add(dataGridView1.Columns[i].HeaderText);

            }
            if (zmianaTabeli)
            {
                comboIndex = 0;
                zmianaTabeli = false;
                whereTable.Clear();
            }
            comboBox1.SelectedIndex = comboIndex;

            
            
        }
    }
}