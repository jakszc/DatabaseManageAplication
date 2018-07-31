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
    public partial class Podsumowanie : Form
    {
        String conS;
        SqlConnection sqlC;
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rd;
        public Podsumowanie()
        {
            InitializeComponent();
            if (Kontener.czyRata)
            {
                label10.Visible = true;
                label11.Visible = true;
                label12.Visible = true;
                label13.Visible = true;
                textBox9.Visible = true;
                textBox10.Visible = true;
                textBox11.Visible = true;
                textBox12.Visible = true;
                textBox9.Text = Kontener.rata;
                textBox10.Text = Kontener.ileRat;
                textBox11.Text = Kontener.dataPierwszejRaty;
            }
            else
            {
                label10.Visible = false;
                label11.Visible = false;
                label12.Visible = false;
                label13.Visible = false;
                textBox9.Visible = false;
                textBox10.Visible = false;
                textBox11.Visible = false;
                textBox12.Visible = false;
            }
            conS = Properties.Settings.Default.projektCS;
            sqlC = new SqlConnection(conS);

            cmd.Connection = sqlC;
            cmd.CommandText = "select [imie], [nazwisko], [nazwa_firmy], [NIP] from dbo.Klient where [id_klienta] = @id_klienta";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@id_klienta", Properties.Settings.Default.id_klienta));
            if(Kontener.przywilej == "admin" || Kontener.przywilej == "superadmin")
            {
                cmd.Parameters["@id_klienta"].Value = Kontener.idKlienta;
            }
            sqlC.Open();
            rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                if (rd[0].ToString().Length == 0)
                {
                    label1.Text = "Nazwa Firmy:";                  
                    label2.Text = "NIP:";
                    label1.Location = new Point(72, label1.Location.Y);
                    label2.Location = new Point(112, label2.Location.Y);
                    textBox1.Text = rd[2].ToString();
                    textBox2.Text = rd[3].ToString();
                }
                else
                {
                    label1.Text = "Imię:";
                    label2.Text = "Nazwisko:";
                    label1.Location = new Point(111, label1.Location.Y);
                    label2.Location = new Point(84, label2.Location.Y);
                    textBox1.Text = rd[0].ToString();
                    textBox2.Text = rd[1].ToString();
                }
            }
            textBox3.Text = Kontener.adresDostawy;
            textBox4.Text = Kontener.dataDostawy;
            textBox5.Text = Math.Round(Convert.ToDouble(Kontener.sumKwota),2).ToString();
            textBox6.Text = Kontener.sumWaga;
            textBox7.Text = Kontener.sumIlosc;
            if (Kontener.czyRata)
            {
                textBox8.Text = (Convert.ToDouble(Kontener.sumKwota) * 1.05).ToString();
            }
            else
            {
                textBox8.Text = Kontener.sumKwota;
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Kontener.czyAnulowal = true;
            Close();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Close();
            Kontener.czyAnulowal = false;
        }
    }
}
