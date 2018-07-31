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
    public partial class DodajProdukty : Form
    {
        String conS = Properties.Settings.Default.projektCS;
        SqlConnection sqlC;
        SqlCommand cmd;
        SqlDataReader rd;
        List<String> nazwyProduktów = new List<String>();
        
        public DodajProdukty()
        {
            InitializeComponent();
            sqlC = new SqlConnection(conS);
            cmd = new SqlCommand();
            cmd.Connection = sqlC;
            cmd.CommandText = "select nazwa from dbo.Katalog_produktow";
            cmd.CommandType = CommandType.Text;
            sqlC.Open();
            rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                comboBox1.Items.Add(rd[0].ToString());
                nazwyProduktów.Add(rd[0].ToString());
            }
            sqlC.Close();
            comboBox1.SelectedIndex = 1;
            label4.Hide();
            label5.Hide();
            label6.Hide();
            textBox2.Hide();
            textBox3.Hide();
            textBox4.Hide();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1.Show();
            label2.Show();
            label4.Hide();
            label5.Hide();
            label6.Hide();
            textBox2.Hide();
            textBox3.Hide();
            textBox4.Hide();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1.Hide();
            label2.Hide();
            label4.Show();
            label5.Show();
            label6.Show();
            textBox2.Show();
            textBox3.Show();
            textBox4.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool nowy = true;
            if (radioButton2.Checked)
            {
                if(textBox2.Text.Length > 100)
                {
                    nowy = false;
                    MessageBox.Show("Za długa nazwa produktu. Maksymalnie 100 znaków.", "Za długa nazwa.", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
                else if(textBox2.Text.Equals("") || textBox3.Text.Equals("") || textBox4.Text.Equals(""))
                {
                    nowy = false;
                    MessageBox.Show("W formularzu pozostały puste pola.", "Puste pola.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if(!CheckOnlyNumberAndPoint(textBox3.Text) || !CheckOnlyNumberAndPoint(textBox4.Text))
                {
                    nowy = false;
                    MessageBox.Show("W polu waga i cena dopuszczalne są tylko cyfry i kropka.", "Zły format liczby.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (!checkName(textBox2.Text))
                {
                    nowy = false;
                    MessageBox.Show("Podana nazwa produktu już istnieje.", "Duplikat nazwy.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            if (nowy)
            {
                if (!dlStawki(textBox3.Text, 6) || !dlStawki(textBox4.Text, 6))
                {
                    MessageBox.Show("Maksymalnie 6 cyfry przed i dwie po kropce w polu waga lub cena.", "Zła wartosc.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (CheckOnlyNumber(textBox1.Text) && !textBox1.Text.Equals("") && textBox1.Text.Length <= 20)
                {
                    if (radioButton2.Checked)
                    {
                        cmd.CommandText = "insert into dbo.Katalog_produktow([nazwa],[cena],[waga],[dostepna_ilosc]) " +
                            "values(@nazwa, @cena, @waga, @dostepna_ilosc)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("@nazwa", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@cena", SqlDbType.Decimal));
                        cmd.Parameters.Add(new SqlParameter("@waga", SqlDbType.Decimal));
                        cmd.Parameters.Add(new SqlParameter("@dostepna_ilosc", SqlDbType.Int));
                        

                        cmd.Parameters["@nazwa"].Value = textBox2.Text;
                        cmd.Parameters["@cena"].Value = Convert.ToDouble(textBox3.Text.Replace(".", ","));
                        cmd.Parameters["@waga"].Value = Convert.ToDouble(textBox4.Text.Replace(".", ","));
                        cmd.Parameters["@dostepna_ilosc"].Value = Convert.ToInt32(textBox1.Text);
                        sqlC.Open();
                        cmd.ExecuteNonQuery();
                        sqlC.Close();

                        cmd.CommandText = "select [id_katalogowe] from dbo.Katalog_produktow where nazwa = @nazwa";
                        cmd.CommandType = CommandType.Text;
                        sqlC.Open();
                        rd = cmd.ExecuteReader();
                        String id_kata = "";
                        if (rd.Read())
                        {
                            id_kata = rd[0].ToString();
                        }
                        sqlC.Close();

                        cmd.CommandText = "insert into dbo.Produkt([id_katalogowe], id_zamowienia) " +
                            "values(@id_kata , NULL)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("@id_kata", SqlDbType.Int));
                        cmd.Parameters["@id_kata"].Value = Convert.ToInt32(id_kata);
                        for (int i = 0; i < Convert.ToInt32(textBox1.Text); i++)
                        {
                            sqlC.Open();
                            cmd.ExecuteNonQuery();
                            sqlC.Close();
                        }

                    }
                    else if (radioButton1.Checked)
                    {
                        cmd.CommandText = "select [id_katalogowe] from dbo.Katalog_produktow where nazwa = @nazwa";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("@nazwa", SqlDbType.VarChar));
                        
                        

                        cmd.Parameters["@nazwa"].Value = comboBox1.Text;

                        sqlC.Open();
                        rd = cmd.ExecuteReader();
                        String id_kata = "";
                        if (rd.Read())
                        {
                            id_kata = rd[0].ToString();
                        }
                        sqlC.Close();

                        cmd.CommandText = "insert into dbo.Produkt([id_katalogowe], id_zamowienia) " +
                            "values(@id_kata, NULL)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("@id_kata", SqlDbType.Int));
                        cmd.Parameters["@id_kata"].Value = Convert.ToInt32(id_kata);

                        for (int i = 0; i < Convert.ToInt32(textBox1.Text); i++)
                        {
                            sqlC.Open();
                            cmd.ExecuteNonQuery();
                            sqlC.Close();
                        }

                        cmd.CommandText = "update dbo.Katalog_produktow set [dostepna_ilosc] = [dostepna_ilosc] + @dostepna_ilosc where [id_katalogowe] = " + id_kata;
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("@dostepna_ilosc", SqlDbType.Int));
                        cmd.Parameters["@dostepna_ilosc"].Value = Convert.ToInt32(textBox1.Text);
                        sqlC.Open();
                        cmd.ExecuteNonQuery();
                        sqlC.Close();
                    }



                    Close();
                }
                else
                {
                    MessageBox.Show("Zła wartość w polu ilość do dodania.", "Zły format liczby.", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            
        }

        public bool checkName(String text)
        {
            bool onlyNum = true;
            for (int i = 0; i < nazwyProduktów.Count; i++)
            {
                if (nazwyProduktów.ElementAt(i) == text)
                {
                    onlyNum = false;
                }
            }
            return onlyNum;
        }

        public bool dlStawki(String text, int max)
        {
            bool onlyNum = true;
            int ile = 0;
            for (int i = 0; i < text.Length; i++)
            {
                ile++;
                if (text[i].Equals('.'))
                {
                    ile = 0;
                    max = 2;
                }
                if (ile > max)
                    onlyNum = false;
            }
            return onlyNum;
        }

        public bool CheckOnlyNumber(String text)
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

        public bool CheckOnlyNumberAndPoint(String text)
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
    }
}
