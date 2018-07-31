using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirmaTransportowa
{
    class KontrolaDanych
    {
        String conS = Properties.Settings.Default.projektCS;
        SqlConnection sqlC;
        SqlCommand cmd;
        SqlDataReader rd;

        public KontrolaDanych()
        {
            sqlC = new SqlConnection(conS);
            cmd = new SqlCommand();
            cmd.Connection = sqlC;
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

        public bool CheckPostCode(String text)
        {
            bool onlyNum = true;
            bool onlyOne = false;
            for (int i = 0; i < text.Length; i++)
            {
                if (!Char.IsDigit(text[i]))
                {
                    if (text[i].Equals('-') && !onlyOne)
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

            if (datee.Date < DateTime.Now.Date)
                valid = false;
            return valid;
        }

        public bool czyPeselPracownik(String text)
        {
            sqlC = new SqlConnection(conS);
            cmd = new SqlCommand();
            cmd.Connection = sqlC;
            bool czy = true;
            cmd.CommandText = "select [PESEL] from dbo.Pracownik";
            cmd.CommandType = CommandType.Text;
            sqlC.Open();
            rd = cmd.ExecuteReader();

            while (rd.Read())
            {
                if (rd[0].ToString() == text)
                    czy = false;
            }
            sqlC.Close();
            return czy;
        }

        public bool czyPeselKierowca(String text)
        {
            sqlC = new SqlConnection(conS);
            cmd = new SqlCommand();
            cmd.Connection = sqlC;
            bool czy = true;
            cmd.CommandText = "select [PESEL] from dbo.Pracownik";
            cmd.CommandType = CommandType.Text;
            sqlC.Open();
            rd = cmd.ExecuteReader();

            while (rd.Read())
            {
                if (rd[0].ToString() == text)
                    czy = false;
            }
            sqlC.Close();
            return czy;
        }

        public bool czyPeselBezLiter(String text)
        {
            bool czy = true;
            for (int i = 0; i < text.Length; i++)
            {
                if (!Char.IsDigit(text[i]))
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
