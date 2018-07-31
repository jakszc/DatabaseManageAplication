using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FirmaTransportowa
{
    public partial class PodajDate : Form
    {
        public PodajDate()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Kontener.czyAnulowal = true;
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value < DateTime.Now.Date)
            {
                MessageBox.Show("Data jest wcześniejsza niż aktualna.", "Błędna data.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Kontener.dataZwolnienia = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                Close();
            }
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
    }
}
