using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ1_Radunovic_Vuk.Assets.Klase
{
    [Serializable]
    public class Serije
    {
        string poster;
        string datum_izlaska; //koristicemo regex za proveru, mislim da je jednostavnija implementacija
        int broj_sezona;
        string naslov;
        string zanr;
        string rtb_ime;

        public Serije() { }

        public Serije(string poster, string datum_izlaska, int broj_sezona, string naslov, string zanr, string rtb_ime)
        {
            Poster = poster;
            Datum_izlaska = datum_izlaska;
            Broj_sezona = broj_sezona;
            Naslov = naslov;
            Zanr = zanr;
            Rtb_ime = rtb_ime;
        }

        public string Poster { get => poster; set => poster = value; }
        public string Datum_izlaska { get => datum_izlaska; set => datum_izlaska = value; }
        public int Broj_sezona { get => broj_sezona; set => broj_sezona = value; }
        public string Naslov { get => naslov; set => naslov = value; }
        public string Zanr { get => zanr; set => zanr = value; }
        public string Rtb_ime { get => rtb_ime; set => rtb_ime = value; }
    }
}
