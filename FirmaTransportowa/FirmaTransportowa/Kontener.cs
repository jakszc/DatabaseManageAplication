using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirmaTransportowa
{
    public static class Kontener
    {
        public static bool czyRata = false, czyAnulowal = false;
        public static String wyszukanyLogin, przywilej, login, dataZwolnienia, haslo, adresDostawy, sumKwota, rata, sumWaga, sumIlosc, dataDostawy, ileRat, dataPierwszejRaty, idTrans;
        public static int idZamowienia, idFaktury, idKlienta, idPunktu, dzienSplatyRaty;
        public static DateTime dDataDostawy, dataTransportu;
    }
}
