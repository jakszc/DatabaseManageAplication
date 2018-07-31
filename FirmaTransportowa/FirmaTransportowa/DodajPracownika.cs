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
    public partial class DodajKierowcaa : Form
    {
        String conS = Properties.Settings.Default.projektCS;
        SqlConnection sqlC;
        SqlCommand cmd;
        SqlDataReader rd;
        public DodajKierowcaa()
        {
            InitializeComponent();
            
            label8.Hide();
            label9.Hide();
            label10.Hide();
            textBox6.Hide();
            dateTimePicker2.Hide();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label6.Show();
            label7.Show();
            textBox5.Show();
            label8.Hide();
            label9.Hide();
            label10.Hide();
            textBox6.Hide();
            dateTimePicker2.Hide();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label6.Hide();
            label7.Hide();
            textBox5.Hide();
            label8.Show();
            label9.Show();
            label10.Show();
            textBox6.Show();
            dateTimePicker2.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!czyPeselBezLiter())
            {
                MessageBox.Show("PESEL może składać się tylko z cyfr.", "Zły format PESEL.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (!czyPesel())
            {
                MessageBox.Show("Taki numer PESEL już istnieje.", "Duplikat numeru PESEL.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if(textBox1.Text.Length != 11)
            {
                MessageBox.Show("PESEL ma dokładnie 11 cyfr.", "Zły format PESEL.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if(textBox2.Text.Length > 50 || textBox3.Text.Length > 50)
            {
                MessageBox.Show("Pola imię i nazwisko mogą zawierać maksymalnie 50 znaków", "Za długa nazwa.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }else if(textBox2.Text.Equals("") || textBox1.Text.Equals("") || textBox3.Text.Equals(""))
            {
                MessageBox.Show("Formularz nie może zawierać pustych pól.", "Puste pola.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (radioButton1.Checked)
                {
                    if(textBox5.Text.Equals(""))
                    {
                        MessageBox.Show("Formularz nie może zawierać pustych pól.", "Puste pola.", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                    else if (!CheckOnlyNumberAndPoint(textBox5.Text))
                    {
                        MessageBox.Show("Stawka miesięczna może zawierać tylko cyfry i kropkę.", "Zła wartość.", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                    else if (!dlStawki2(textBox5.Text))
                    {
                        MessageBox.Show("Maksymalnie 2 cyfry przed i dwie po kropce w polu stawka godzinowa.", "Zła stawka.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (dateTimePicker1.Value < DateTime.Now.Date)
                    {
                        MessageBox.Show("Data przed aktualną.", "Zła data.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        sqlC = new SqlConnection(conS);
                        cmd = new SqlCommand();
                        cmd.Connection = sqlC;
                        cmd.CommandText = "insert into dbo.Kierowca([status], [PESEL], [imie], [nazwisko], [data_zatrudnienia], [stawka_miesieczna], [wolny_od]) " +
                            "values('aktywny', @PESEL, @imie, @nazwisko, @data_zatrudnienia, @stawka_miesieczna, @wolny_od)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("@PESEL", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@imie", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@nazwisko", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@data_zatrudnienia", SqlDbType.Date));
                        cmd.Parameters.Add(new SqlParameter("@stawka_miesieczna", SqlDbType.Decimal));
                        cmd.Parameters.Add(new SqlParameter("@wolny_od", SqlDbType.Date));


                        cmd.Parameters["@PESEL"].Value = textBox1.Text;
                        cmd.Parameters["@imie"].Value = textBox2.Text;
                        cmd.Parameters["@nazwisko"].Value = textBox3.Text;
                        cmd.Parameters["@data_zatrudnienia"].Value = dateTimePicker1.Value;
                        cmd.Parameters["@stawka_miesieczna"].Value = Convert.ToDouble(textBox5.Text.Replace(".", ","));
                        cmd.Parameters["@wolny_od"].Value = dateTimePicker1.Value;

                        sqlC.Open();
                        cmd.ExecuteNonQuery();
                        sqlC.Close();
                        Close();
                    }
                    
                }
                else
                {
                    if (textBox6.Text.Equals(""))
                    {
                        MessageBox.Show("Formularz nie może zawierać pustych pól.", "Puste pola.", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                    else if (!CheckOnlyNumberAndPoint(textBox6.Text))
                    {
                        MessageBox.Show("Stawka godzinowa może zawierać tylko cyfry i kropkę.", "Zła wartość.", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                    else if(!dlStawki(textBox6.Text))
                    {
                        MessageBox.Show("Maksymalnie 2 cyfry przed i dwie po kropce w polu stawka godzinowa.", "Zła stawka.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (dateTimePicker1.Value < DateTime.Now.Date || dateTimePicker2.Value < DateTime.Now.Date)
                    {
                        MessageBox.Show("Data przed aktualną.", "Zła data.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (dateTimePicker1.Value.AddMonths(1) > dateTimePicker2.Value)
                    {
                        MessageBox.Show("Długość kontraktu musi wynosić minimum jeden miesiąc.", "Zła data.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        sqlC = new SqlConnection(conS);
                        cmd = new SqlCommand();
                        cmd.Connection = sqlC;
                        cmd.CommandText = "insert into dbo.Kierowca([PESEL], [imie],[status], [nazwisko], [stawka_godzinowa], [data_podpisania_umowy], [data_wygasniecia_umowy], [wolny_od]) " +
                                                                "values(@PESEL, @imie,'aktywny', @nazwisko, @stawka_godzinowa, @data_podpisania_umowy, @data_wygasniecia_umowy, @wolny_od)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("@PESEL", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@imie", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@nazwisko", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@stawka_godzinowa", SqlDbType.Decimal));
                        cmd.Parameters.Add(new SqlParameter("@data_podpisania_umowy", SqlDbType.Date));
                        cmd.Parameters.Add(new SqlParameter("@data_wygasniecia_umowy", SqlDbType.Date));
                        cmd.Parameters.Add(new SqlParameter("@wolny_od", SqlDbType.Date));


                        cmd.Parameters["@PESEL"].Value = textBox1.Text;
                        cmd.Parameters["@imie"].Value = textBox2.Text;
                        cmd.Parameters["@nazwisko"].Value = textBox3.Text;
                        cmd.Parameters["@stawka_godzinowa"].Value = Convert.ToDouble(textBox6.Text.Replace(".",","));
                        cmd.Parameters["@data_podpisania_umowy"].Value = dateTimePicker1.Value;
                        cmd.Parameters["@data_wygasniecia_umowy"].Value = dateTimePicker2.Value;
                        cmd.Parameters["@wolny_od"].Value = dateTimePicker1.Value;

                        sqlC.Open();
                        cmd.ExecuteNonQuery();
                        sqlC.Close();
                        Close();
                    }
                    
                }
                
            }
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

        public bool dlStawki(String text)
        {
            bool onlyNum = true;
            int ile = 0;
            for (int i = 0; i < text.Length; i++)
            {
                ile++;
                if (text[i].Equals('.'))
                {
                    ile = 0;
                }  
                if (ile > 2)
                    onlyNum = false;
            }
            return onlyNum;
        }

        public bool dlStawki2(String text)
        {
            bool onlyNum = true;
            int ile = 0, max = 5;
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

            if (datee.Date < DateTime.Now.Date)
                valid = false;
            return valid;
        }

        public bool czyPesel()
        {
            sqlC = new SqlConnection(conS);
            cmd = new SqlCommand();
            cmd.Connection = sqlC;
            bool czy = true;
            cmd.CommandText = "select [PESEL] from dbo.Kierowca";
            cmd.CommandType = CommandType.Text;
            sqlC.Open();
            rd = cmd.ExecuteReader();

            while (rd.Read())
            {
                if (rd[0].ToString() == textBox1.Text)
                    czy = false;
            }
            sqlC.Close();
            return czy;
        }

        public bool czyPeselBezLiter()
        {
            bool czy = true;
            for (int i = 0; i < textBox1.Text.Length; i++)
            {
                if (!Char.IsDigit(textBox1.Text[i]))
                {
                    czy = false;
                }
            }
            return czy;
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

        public DateTime parseDateToDate(String date)
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

            return datee;
        }
    }
}
