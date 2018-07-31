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
    public partial class NoweZamowienie : Form
    {
        bool done = false, adminDostep = true;
        bool zatwierdzone = false;
        int ileProduktu = 0;
        Double sumKwota;
        Double sumWaga;
        int sumIloscProduktow = 0;
        Double waga = 0, cena = 0;
        BindingList<String[]> list = new BindingList<String[]>();
        List<int[]> wKoszyku = new List<int[]>();
        public NoweZamowienie()
        {
            InitializeComponent();

            if(Kontener.przywilej == "admin" || Kontener.przywilej == "superadmin")
            {
                label14.Visible = true;
                textBox5.Visible = true;
                button3.Visible = true;
            }
        }

        public void sumy()
        {
            sumIloscProduktow = 0;
            sumKwota = 0;
            sumWaga = 0;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {

                sumIloscProduktow += Convert.ToInt32(dataGridView1.Rows[i].Cells["Ilosc"].Value);
                sumKwota += Convert.ToDouble(dataGridView1.Rows[i].Cells["Cena"].Value);
                sumWaga += Convert.ToDouble(dataGridView1.Rows[i].Cells["Waga"].Value);
                
            }
            label10.Text = "Łączna kwota: " + sumKwota.ToString();
            label11.Text = "Łączna waga: " + sumWaga.ToString();
            label12.Text = "Łączna ilość produktów: " + sumIloscProduktow.ToString();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!done)
            {
                if (!zatwierdzone)
                {
                    DialogResult result = MessageBox.Show("Czy na pewno chcesz wyjść? Twój koszyk zostanie anulowany.", "Potwierdzenie wyjścia.", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {
                        String conS = Properties.Settings.Default.projektCS;
                        SqlConnection sqlC = new SqlConnection(conS);
                        SqlCommand cmd = new SqlCommand();

                        cmd.Connection = sqlC;
                        cmd.CommandText = "UPDATE dbo.Katalog_produktow SET [dostepna_ilosc] = [dostepna_ilosc] + @ilosc " +
                            "where [id_katalogowe] = @id_kat2";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("@id_kat2", SqlDbType.Int);
                        cmd.Parameters.Add("@ilosc", SqlDbType.Int);
                        for (int i = 0; i < wKoszyku.Count; i++)
                        {
                            cmd.Parameters["@id_kat2"].Value = wKoszyku[i][0];
                            cmd.Parameters["@ilosc"].Value = wKoszyku[i][1];
                            sqlC.Open();
                            cmd.ExecuteNonQuery();
                            sqlC.Close();
                        }
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
                else
                {
                    zatwierdzone = false;
                }
            }
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String conS = Properties.Settings.Default.projektCS;
            SqlConnection sqlC = new SqlConnection(conS);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader rd;

            if (Kontener.przywilej == "admin" || Kontener.przywilej == "superadmin")
            {
                if (textBox5.Text.Equals(""))
                {
                    MessageBox.Show("Podaj login użytkownika.", "Błąd.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    adminDostep = false;
                }
                else
                {
                    cmd.Connection = sqlC;
                    cmd.CommandText = "select 1 from dbo.DaneLogowania where [login] = @login22";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("@login22", textBox5.Text));

                    sqlC.Open();
                    rd = cmd.ExecuteReader();
                    if (rd.Read())
                    {
                        Kontener.login = textBox5.Text;
                        sqlC.Close();

                        cmd.CommandText = "select id_klienta from dbo.DaneLogowania where [login] = @login22";
                        cmd.CommandType = CommandType.Text;
                        sqlC.Open();
                        rd = cmd.ExecuteReader();
                        if (rd.Read())
                        {
                            Kontener.idKlienta = Convert.ToInt32(rd[0].ToString());
                            sqlC.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Nie istnieje taki użytkownik.", "Ten login nie istnieje!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        adminDostep = false;
                    }
                }

            }
            if (adminDostep)
            {
                if (wKoszyku.Count == 0)
                {
                    MessageBox.Show("Nie dodałeś żadnych produktów!", "Brak produktów.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if ((checkBox1.Checked && textBox2.Text.Equals("")))
                {
                    MessageBox.Show("Nie podałeś ilości rat!", "Brak ilości rat.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {

                    Kontener.adresDostawy = comboBox2.Text;
                    Kontener.ileRat = textBox2.Text;
                    Kontener.rata = textBox3.Text;
                    Kontener.sumIlosc = sumIloscProduktow.ToString();
                    Kontener.sumKwota = sumKwota.ToString();
                    Kontener.sumWaga = sumWaga.ToString();
                    Kontener.dzienSplatyRaty = Convert.ToInt32(textBox4.Text);
                    czasTransportu();
                    Kontener.dDataDostawy = DateTime.Parse(Kontener.dataDostawy);
                    Kontener.dataDostawy = Kontener.dDataDostawy.ToString("dd.MM.yyyy");
                    Kontener.dataPierwszejRaty = Kontener.dDataDostawy.AddMonths(1).ToString("dd.MM.yyyy");


                    if (checkBox1.Checked)
                    {
                        Kontener.czyRata = true;
                    }
                    else
                    {
                        Kontener.czyRata = false;
                    }

                    zatwierdzone = true;
                    Podsumowanie form = new Podsumowanie();
                    form.ShowDialog();
                    form = null;
                    zatwierdzone = false;
                    if (!Kontener.czyAnulowal)
                    {

                        dodajTransportZamowienieFaktury();
                        done = true;
                        przypiszProdukty();
                        Close();
                    }
                    else
                    {
                        Kontener.czyAnulowal = false;
                    }
                }

            }
            else
            {
                adminDostep = true;
            }


        }

        private void przypiszProdukty()
        {
            String conS = Properties.Settings.Default.projektCS;
            SqlConnection sqlC = new SqlConnection(conS);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader rd;

            cmd.Connection = sqlC;
            cmd.Parameters.Add(new SqlParameter("@id_katalogowe", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@ilosc", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@id_zamowienia", SqlDbType.Int));
            cmd.CommandText = "update top (@ilosc) dbo.Produkt set [id_zamowienia] = @id_zamowienia where [id_katalogowe] = @id_katalogowe and [id_zamowienia] is null";

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                cmd.Parameters["@id_katalogowe"].Value = Convert.ToInt32(dataGridView1.Rows[i].Cells["id"].Value.ToString());
                cmd.Parameters["@id_zamowienia"].Value = Kontener.idZamowienia + 1;
                cmd.Parameters["@ilosc"].Value = Convert.ToInt32(dataGridView1.Rows[i].Cells["Ilosc"].Value.ToString());

                sqlC.Open();
                cmd.ExecuteNonQuery();
                sqlC.Close();
            }
        }

        public void dodajTransportZamowienieFaktury()
        {
            String conS = Properties.Settings.Default.projektCS;
            SqlConnection sqlC = new SqlConnection(conS);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader rd;

            cmd.Connection = sqlC;

            //Transport
            String miastoDost = Kontener.adresDostawy.Split(' ')[0].Remove(Kontener.adresDostawy.Split(' ')[0].Length - 1);
            cmd.CommandText = "dbo.dobierzTransport";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@waga", sumWaga));
            cmd.Parameters.Add(new SqlParameter("@miasto_dostawy", miastoDost));

            sqlC.Open();
            rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                String idTrans = rd[0].ToString();
                String dataTrans = rd[1].ToString();
                Kontener.idTrans = idTrans;
                Kontener.dataDostawy = dataTrans;
            }
            sqlC.Close();
            //pobierz id_zamówienia i id_faktury
            cmd.CommandText = "SELECT current_value FROM sys.sequences where [name] = 'Zamowienie_seq'";
            cmd.CommandType = CommandType.Text;
            sqlC.Open();
            rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                Kontener.idZamowienia = Convert.ToInt32(rd[0].ToString());
            }
            sqlC.Close();

            cmd.CommandText = "SELECT current_value FROM sys.sequences where [name] = 'Faktura_seq'";
            cmd.CommandType = CommandType.Text;
            sqlC.Open();
            rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                Kontener.idFaktury = Convert.ToInt32(rd[0].ToString());
            }
            sqlC.Close();

            //pobierz id_klienta
            cmd.CommandText = "select [id_klienta] from dbo.DaneLogowania where [login] = @login";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@login", Kontener.login));
            sqlC.Open();
            rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                Kontener.idKlienta = Convert.ToInt32(rd[0].ToString());
            }
            sqlC.Close();

            //pobierz id_punktu
            Kontener.idPunktu = Convert.ToInt32(comboBox2.SelectedValue.ToString());

            //Zamówienie
            cmd.CommandText = "INSERT INTO dbo.Zamowienie([waga],[id_transportu],[id_faktury] ,[id_klienta], [id_punktu]) " +
                "VALUES(@waga1, @id_trans, @id_fakt, @id_klienta, @id_punktu)";
            cmd.Parameters.Add(new SqlParameter("@waga1", Convert.ToDouble(Kontener.sumWaga)));
            cmd.Parameters.Add(new SqlParameter("@id_trans", Kontener.idTrans));
            cmd.Parameters.Add(new SqlParameter("@id_fakt", Kontener.idFaktury + 1));
            cmd.Parameters.Add(new SqlParameter("@id_klienta", Kontener.idKlienta));
            cmd.Parameters.Add(new SqlParameter("@id_punktu", Kontener.idPunktu));
            sqlC.Open();
            cmd.ExecuteNonQuery();
            sqlC.Close();



            //Faktury
            cmd.CommandText = "INSERT INTO dbo.Faktura([id_zamowienia],[kwota_calkowita],[typ_platnosci],[liczba_rat],[rata_miesieczna],[dzien_splaty_raty]) " +
                "VALUES(@id_zamowienia, @kwota_calkowita, @typ_platnosci, @liczba_rat, @rata_miesieczna, @dzien_splaty_raty)";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@id_zamowienia", Kontener.idZamowienia + 1));
            if (Kontener.czyRata)
            {
                cmd.Parameters.Add(new SqlParameter("@kwota_calkowita", SqlDbType.Money));
                cmd.Parameters.Add(new SqlParameter("@typ_platnosci", "ratalna"));
                cmd.Parameters.Add(new SqlParameter("@liczba_rat", Convert.ToInt32(Kontener.ileRat)));
                cmd.Parameters.Add(new SqlParameter("@rata_miesieczna", SqlDbType.Money));
                cmd.Parameters.Add(new SqlParameter("@dzien_splaty_raty", Kontener.dzienSplatyRaty));
                cmd.Parameters["@kwota_calkowita"].Value = Convert.ToDouble(Kontener.sumKwota) * 1.05;
                cmd.Parameters["@rata_miesieczna"].Value = Convert.ToDouble(Kontener.rata);
            }
            else
            {
                cmd.Parameters.Add(new SqlParameter("@kwota_calkowita", SqlDbType.Money));
                cmd.Parameters.Add(new SqlParameter("@typ_platnosci", "jednorazowa"));
                cmd.Parameters.Add(new SqlParameter("@liczba_rat", DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@rata_miesieczna", DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@dzien_splaty_raty", DBNull.Value));
                cmd.Parameters["@kwota_calkowita"].Value = Convert.ToDouble(Kontener.sumKwota);
            }


            sqlC.Open();
            cmd.ExecuteNonQuery();
            sqlC.Close();
           
        }

        public void czasTransportu()
        {
            String conS = Properties.Settings.Default.projektCS;
            SqlConnection sqlC = new SqlConnection(conS);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader rd;

            cmd.Connection = sqlC;

            //Transport
            String miastoDost = Kontener.adresDostawy.Split(' ')[0].Remove(Kontener.adresDostawy.Split(' ')[0].Length - 1);
            cmd.CommandText = "dbo.szacowanaDataTransportu";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@waga", sumWaga));
            cmd.Parameters.Add(new SqlParameter("@miasto_dostawy", miastoDost));

            sqlC.Open();
            rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                String dataTrans = rd[0].ToString();
                Kontener.dataDostawy = dataTrans;
            }
            sqlC.Close();
        }


        private void NoweZamowienie_Load(object sender, EventArgs e)
        {
            // TODO: Ten wiersz kodu wczytuje dane do tabeli 'zamowienie_listaProduktow.listaProduktow' . Możesz go przenieść lub usunąć.
            this.listaProduktowTableAdapter.Fill(this.zamowienie_listaProduktow.listaProduktow);
            // TODO: Ten wiersz kodu wczytuje dane do tabeli 'zamowienie_adresDostawy.listaAdresowMiasta' . Możesz go przenieść lub usunąć.
            this.listaAdresowMiastaTableAdapter.Fill(this.zamowienie_adresDostawy.listaAdresowMiasta);
            getCount();
            sumy();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool onlyNum = true;
            for (int i = 0; i < textBox1.Text.Length; i++)
            {
                if (!Char.IsDigit(textBox1.Text[i]))
                {
                    onlyNum = false;
                }
            }
            if (textBox1.Text.Equals("") || !onlyNum )
            {
                MessageBox.Show("To nie jest poprawna ilość produktów.", "Błędna ilość.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if(ileProduktu < Convert.ToInt32(textBox1.Text))
            {
                MessageBox.Show("Wybrana ilość produktów jest większa niż dostępna na magazynie! Proszę wybrać mniejszą ilość.", "Błędna ilość.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                addToKoszyk();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            getCount();
        }

        public void getCount()
        {
            String conS = Properties.Settings.Default.projektCS;
            SqlConnection sqlC = new SqlConnection(conS);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader rd;

            cmd.Connection = sqlC;
            cmd.CommandText = "dbo.ileProduktu";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@id_kat", Convert.ToInt32(comboBox1.SelectedValue)));

            sqlC.Open();
            rd = cmd.ExecuteReader();

            if (rd.Read())
            {
                String val = rd[0].ToString();
                ileProduktu = Convert.ToInt32(val);
                label5.Text = "Ilość w magazynie: " + val;
                val = rd[1].ToString();
                cena = Math.Round(Convert.ToDouble(val),2);
                label8.Text = "Cena: " + cena.ToString();
                val = rd[2].ToString();
                waga = Math.Round(Convert.ToDouble(val),2);
                label9.Text = "Waga: " + waga.ToString();
            }
            else
            {
                MessageBox.Show("Wystąpił nieoczekiwany błąd.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            sqlC.Close();
        }

        public void addToKoszyk()
        {
            String conS = Properties.Settings.Default.projektCS;
            SqlConnection sqlC = new SqlConnection(conS);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader rd;

            cmd.Connection = sqlC;
            int id_katalogowe = Convert.ToInt32(comboBox1.SelectedValue);
            
            cmd.CommandText = "UPDATE dbo.Katalog_produktow SET [dostepna_ilosc] = [dostepna_ilosc] - @ilosc " +
                "where [id_katalogowe] = @id_kat2";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@id_kat2", id_katalogowe));
            cmd.Parameters.Add(new SqlParameter("@ilosc", Convert.ToInt32(textBox1.Text)));

            sqlC.Open();
            cmd.ExecuteNonQuery();
            sqlC.Close();

            getCount();

            cmd.CommandText = "select [nazwa],[cena],[waga] from dbo.Katalog_produktow where [id_katalogowe] = @id_kat";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@id_kat", id_katalogowe));

            sqlC.Open();
            rd = cmd.ExecuteReader();

            if (rd.Read())
            {
                String naz, cen, wag;
                naz = rd[0].ToString();
                cen = rd[1].ToString();
                wag = rd[2].ToString();
                cen = (Convert.ToDouble(cen) * Convert.ToInt32(textBox1.Text)).ToString();
                wag = (Convert.ToDouble(wag) * Convert.ToInt32(textBox1.Text)).ToString();

                int duplikat = -1;
                for(int i = 0; i < wKoszyku.Count; i++)
                {
                    if(wKoszyku[i][0] == id_katalogowe)
                    {
                        duplikat = i;
                    }
                }

                

                if (duplikat == -1)
                {
                    wKoszyku.Add(new int[] { id_katalogowe, Convert.ToInt32(textBox1.Text) });
                    int rowId = dataGridView1.Rows.Add();

                    DataGridViewRow row = dataGridView1.Rows[rowId];
                    row.Cells["id"].Value = id_katalogowe;
                    row.Cells["Nazwa"].Value = naz;
                    row.Cells["Ilosc"].Value = textBox1.Text;
                    row.Cells["Cena"].Value = cen;
                    row.Cells["Waga"].Value = wag;
                }
                else
                {
                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        if (dataGridView1.Rows[i].Cells["id"].Value.Equals(id_katalogowe))
                        {
                            dataGridView1.Rows[i].Cells["Ilosc"].Value = (Convert.ToInt32(dataGridView1.Rows[i].Cells["Ilosc"].Value) + Convert.ToInt32(textBox1.Text)).ToString();
                            dataGridView1.Rows[i].Cells["Cena"].Value = (Convert.ToDouble(dataGridView1.Rows[i].Cells["Cena"].Value) + Convert.ToDouble(cen)).ToString();
                            dataGridView1.Rows[i].Cells["Waga"].Value = (Convert.ToDouble(dataGridView1.Rows[i].Cells["Waga"].Value) + Convert.ToDouble(wag)).ToString();
                        }
                    }
                    wKoszyku[duplikat][1] += Convert.ToInt32(textBox1.Text);
                }
                

            }
            else
            {
                MessageBox.Show("Wystąpił nieoczekiwany błąd.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            sqlC.Close();
            sumy();
            rata();
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




        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Int32 rowToDelete = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if(rowToDelete >= 0)
            {
                przywrocDoBazy(rowToDelete);
                dataGridView1.Rows.RemoveAt(rowToDelete);
                dataGridView1.ClearSelection();
                getCount();
                sumy();
                rata();
            }
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                label6.Visible = true;
                label7.Visible = true;
                label13.Visible = true;
                textBox2.Visible = true;
                textBox3.Visible = true;
                textBox4.Visible = true;
            }
            else
            {
                label6.Visible = false;
                label7.Visible = false;
                label13.Visible = false;
                textBox2.Visible = false;
                textBox3.Visible = false;
                textBox4.Visible = false;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            rata();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Kontener.wyszukanyLogin = "";
            SzukajLoginu form = new SzukajLoginu();
            form.ShowDialog();
            textBox5.Text = Kontener.wyszukanyLogin;
        }


        public void rata()
        {
            bool onlyNum = true;
            for (int i = 0; i < textBox2.Text.Length; i++)
            {
                if (!Char.IsDigit(textBox2.Text[i]))
                {
                    onlyNum = false;
                }
            }
            if (textBox2.Text.Equals(""))
            {
                textBox3.Text = "0";
                textBox4.Text = DateTime.Now.Day.ToString();
            }
            else if (!onlyNum || textBox2.Text.Equals("0"))
            {
                MessageBox.Show("To nie jest poporawna liczba.", "Błędna liczba.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                textBox3.Text = Math.Round((sumKwota * 1.05 / Convert.ToDouble(textBox2.Text)),2).ToString();
                textBox4.Text = DateTime.Now.Day.ToString();
            }
        }

        public void przywrocDoBazy(int rowToDelete)
        {
            String conS = Properties.Settings.Default.projektCS;
            SqlConnection sqlC = new SqlConnection(conS);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = sqlC;
            cmd.CommandText = "UPDATE dbo.Katalog_produktow SET [dostepna_ilosc] = [dostepna_ilosc] + @ilosc " +
                "where [id_katalogowe] = @id_kat2";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@id_kat2", SqlDbType.Int);
            cmd.Parameters.Add("@ilosc", SqlDbType.Int);

            cmd.Parameters["@id_kat2"].Value = wKoszyku[rowToDelete][0];
            cmd.Parameters["@ilosc"].Value = wKoszyku[rowToDelete][1];
            sqlC.Open();
            cmd.ExecuteNonQuery();
            sqlC.Close();
            wKoszyku.RemoveAt(rowToDelete);
            
        }
    }

}
