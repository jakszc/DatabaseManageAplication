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
    public partial class DodajPojazd : Form
    {
        String conS = Properties.Settings.Default.projektCS;
        SqlConnection sqlC;
        SqlCommand cmd;
        SqlDataReader rd;
        public DodajPojazd()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            sqlC = new SqlConnection(conS);
            cmd = new SqlCommand();
            cmd.Connection = sqlC;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!czyZajete())
            {
                MessageBox.Show("Taka nazwa rejestracji już istnieje.", "Duplikat rejestracji.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!CheckOnlyNumberAndPoint(textBox3.Text))
            {
                MessageBox.Show("W polu waga dopuszczalne są tylko cyfry i kropka.", "Zły format liczby.", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else if (!dlStawki(textBox3.Text))
            {
                MessageBox.Show("Maksymalnie 6 cyfry przed i dwie po kropce w polu waga.", "Zła wartość.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if(textBox1.Text.Length > 10)
            {
                MessageBox.Show("Maksymalnie 10 znaków w polu rejestracja.", "Za duża liczba zanków.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if(textBox1.Text.Equals("") || textBox3.Text.Equals(""))
            {
                MessageBox.Show("W formularzu znajdują się puste pola.", "Puste pola.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                cmd.CommandText = "insert into dbo.Pojazd([rejestracja],[typ_pojazdu],[waga_dopuszczalna],[wolny_od],[status]) " +
                    "values(@rejestracja, @typ_pojazdu, @waga_dopuszczalna, @wolny_od, 'aktywny')";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@rejestracja", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@typ_pojazdu", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@waga_dopuszczalna", SqlDbType.Decimal));
                cmd.Parameters.Add(new SqlParameter("@wolny_od", SqlDbType.Date));

                cmd.Parameters["@rejestracja"].Value = textBox1.Text;
                cmd.Parameters["@typ_pojazdu"].Value = comboBox1.Text;
                cmd.Parameters["@waga_dopuszczalna"].Value = Convert.ToDouble(textBox3.Text.Replace(".", ","));
                cmd.Parameters["@wolny_od"].Value = DateTime.Now.Date;


                sqlC.Open();
                cmd.ExecuteNonQuery();
                sqlC.Close();
                Close();
            }
            
        }

        public bool dlStawki(String text)
        {
            bool onlyNum = true;
            int ile = 0, max = 6;
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

        
        public bool czyZajete()
        {
            bool czy = true;
            cmd.CommandText = "select [rejestracja] from dbo.Pojazd";
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
    }
}
