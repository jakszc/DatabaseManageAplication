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
    public partial class DodajAdmin : Form
    {
        KontrolaDanych kontrola;
        public DodajAdmin()
        {
            InitializeComponent();
            kontrola = new KontrolaDanych();
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Length > 50 || textBox2.Text.Length > 50)
            {
                MessageBox.Show("Maksymalnie 50 znaków w loginie i haśle.", "Za długa nazwa lub hasło", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }else if(textBox1.Text.Equals("") || textBox2.Text.Equals("") || textBox3.Text.Equals("") || textBox4.Text.Equals("") || textBox5.Text.Equals(""))
            {
                MessageBox.Show("Puste pola w formularzu.", "Puste pola.", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else if (textBox5.Text.Length != 11 || !kontrola.czyPeselBezLiter(textBox5.Text) || !kontrola.czyPeselPracownik(textBox5.Text))
            {
                MessageBox.Show("Niepoprawny PESEL.", "PESEL.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (textBox3.Text.Length > 50 || textBox4.Text.Length > 50)
            {
                MessageBox.Show("Maksymalnie 50 znaków w polach imię i nazwisko.", "Imię i nazwisko", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (dateTimePicker1.Value < DateTime.Now.Date)
            {
                MessageBox.Show("Nie można wybrać daty wcześniejszej niż dzisiejszy dzień.", "Zła data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                String conS = Properties.Settings.Default.projektCS;
                SqlConnection sqlC = new SqlConnection(conS);
                SqlCommand cmd = new SqlCommand();
                SqlDataReader rd;
                cmd.Connection = sqlC;

                cmd.CommandText = "select count(*) from dbo.DaneLogowania where [login] = @login";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@login", SqlDbType.VarChar));
                cmd.Parameters["@login"].Value = textBox1.Text;
                sqlC.Open();
                rd = cmd.ExecuteReader();
                int val = 0;
                if (rd.Read())
                {
                    val = Convert.ToInt32(rd[0].ToString());
                }
                sqlC.Close();

                if(val == 0)
                {
                    cmd.CommandText = "insert into dbo.Pracownik([imię],[nazwisko],[PESEL],[data_zatrudnienia]) " +
                        "values(@imie , @nazwisko, @PESEL, @data_zatrudnienia)";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("@imie", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@nazwisko", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@PESEL", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@data_zatrudnienia", SqlDbType.Date));
                    cmd.Parameters["@imie"].Value = textBox3.Text;
                    cmd.Parameters["@nazwisko"].Value = textBox4.Text;
                    cmd.Parameters["@PESEL"].Value = textBox5.Text;
                    cmd.Parameters["@data_zatrudnienia"].Value = dateTimePicker1.Value;
                    sqlC.Open();
                    cmd.ExecuteNonQuery();
                    sqlC.Close();


                    cmd.CommandText = "select current_value from sys.sequences where name = 'Pracownik_seq'";
                    cmd.CommandType = CommandType.Text;
                    
                    sqlC.Open();
                    rd = cmd.ExecuteReader();
                    int id_admina = 0;
                    if (rd.Read())
                    {
                        id_admina = Convert.ToInt32(rd[0].ToString());
                    }
                    sqlC.Close();


                    cmd.CommandText = "insert into dbo.DaneLogowania([login],[haslo],[przywileje],[id_klienta],[status], [id_admina]) " +
                        "values(@login , @haslo, @przywileje, NULL, 'aktywny', @id_admina)";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("@haslo", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@przywileje", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@id_admina", SqlDbType.Int));
                    cmd.Parameters["@haslo"].Value = textBox2.Text;
                    cmd.Parameters["@przywileje"].Value = comboBox1.Text;
                    cmd.Parameters["@id_admina"].Value = id_admina;
                    sqlC.Open();
                    cmd.ExecuteNonQuery();
                    sqlC.Close();
                    Close();
                }
                else
                {
                    MessageBox.Show("Taki login już istenieje. Wybierz inny.", "Powtarzający się login.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
