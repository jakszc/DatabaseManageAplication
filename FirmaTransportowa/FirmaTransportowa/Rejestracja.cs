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
    public partial class Rejestracja : Form
    {
        public Rejestracja()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label1.Text = "Imię:";
            label2.Text = "Nazwisko:";
            textBox3.Clear();
            textBox4.Clear();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label1.Text = "Nazwa firmy:";
            label2.Text = "NIP:";
            textBox3.Clear();
            textBox4.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String conS = Properties.Settings.Default.projektCS;
            SqlConnection sqlC = new SqlConnection(conS);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader rd;
            KontrolaDanych kontrola = new KontrolaDanych();

            if (textBox1.Text.Equals("") || textBox2.Text.Equals("") || textBox3.Text.Equals("") || textBox4.Text.Equals("") || textBox5.Text.Equals("") ||
                textBox6.Text.Equals("") || textBox7.Text.Equals(""))
            {
                MessageBox.Show("Proszę uzupełnić wszystkie pola.","Uwaga",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            else if((textBox3.Text.Length > 50 || textBox4.Text.Length > 50) && radioButton1.Checked )
            {
                MessageBox.Show("Maksymalnie 50 znaków dla pól: Imię, Nazwisko.", "Uwaga", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (textBox3.Text.Length > 50 && radioButton2.Checked)
            {
                MessageBox.Show("Maksymalnie 50 znaków dla pola: Nazwa Firmy.", "Uwaga", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (textBox4.Text.Length != 10 && radioButton2.Checked)
            {
                MessageBox.Show("NIP posiada dokładnie 10 znaków.", "Uwaga", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if(radioButton2.Checked && !kontrola.CheckOnlyNumber(textBox4.Text))
            {
                MessageBox.Show("NIP składa się tylko z 10 cyfr.", "Uwaga", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if(textBox5.Text.Length > 50 || textBox6.Text.Length > 50)
            {
                MessageBox.Show("Maksymalnie 50 znaków dla pól: Miasto, Adres.", "Uwaga", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!kontrola.CheckPostCode(textBox7.Text) || textBox7.Text.Length > 10)
            {
                MessageBox.Show("Kod pocztowy składa się z maksymalnie 10 znaków (cyfr i opcjonalnie jednego myślnika).", "Uwaga", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {                
                
                cmd.Connection = sqlC;
                cmd.CommandText = "select 1 from dbo.DaneLogowania where [login] = @login";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@login", textBox1.Text));

                sqlC.Open();
                rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    MessageBox.Show("Wprowadź inną nazwę użytkownika.", "Ten login już istnieje!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    sqlC.Close();
                }
                else
                {
                    sqlC.Close();
                    cmd.CommandText = "SELECT current_value FROM sys.sequences WHERE name = 'Klient_seq'";
                    cmd.CommandType = CommandType.Text;
                    sqlC.Open();
                    rd = cmd.ExecuteReader();
                    string id_klienta = "";
                    if (rd.Read()) { id_klienta = (Convert.ToInt32(rd[0].ToString()) + 1).ToString(); }
                    sqlC.Close();

                    

                    cmd.CommandText = "insert into dbo.Klient ( [miasto],[kod_pocztowy], [ulica_nr_domu],[imie],[nazwisko],[nazwa_firmy],[NIP]) values " +
                        "( @miasto, @kod_pocztowy, @ulica, @imie, @nazwisko, @firma, @NIP)";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("@miasto", textBox5.Text));
                    cmd.Parameters.Add(new SqlParameter("@kod_pocztowy", textBox7.Text));
                    cmd.Parameters.Add(new SqlParameter("@ulica", textBox6.Text));


                    if (radioButton1.Checked)
                    {
                        cmd.Parameters.Add(new SqlParameter("@imie", textBox3.Text));
                        cmd.Parameters.Add(new SqlParameter("@nazwisko", textBox4.Text));
                        cmd.Parameters.Add(new SqlParameter("@firma", DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@NIP", DBNull.Value));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@firma", textBox3.Text));
                        cmd.Parameters.Add(new SqlParameter("@NIP", textBox4.Text));
                        cmd.Parameters.Add(new SqlParameter("@imie", DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@nazwisko", DBNull.Value));
                    }

                    sqlC.Open();
                    cmd.ExecuteNonQuery();
                    sqlC.Close();
                    

                    cmd.CommandText = "insert into dbo.DaneLogowania ([login], [haslo], [przywileje], [id_klienta], [status]) values " +
                        "(@login2, @haslo, 'user', @id_klienta, 'aktywny')";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("@login2", textBox1.Text));
                    cmd.Parameters.Add(new SqlParameter("@haslo", textBox2.Text));
                    cmd.Parameters.Add(new SqlParameter("@id_klienta", id_klienta));

                    sqlC.Open();
                    cmd.ExecuteNonQuery();
                    sqlC.Close();

                    Close();
                }               

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
