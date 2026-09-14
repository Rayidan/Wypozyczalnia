using System;

namespace Wypozyczalnia
{
    // Sprzet tez dziedziczy po Pozycja, tak samo jak Ksiazka.
    // To jest przyklad ze dwie rozne klasy moga dziedziczyc po jednej klasie bazowej
    public class Sprzet : Pozycja
    {
        private string kategoria; // np "elektronika", "sport", "narzedzia"
        private decimal wartosc;  // ile sprzet jest wart w zlotowkach

        public Sprzet(int id, string tytul, string kategoria, decimal wartosc) : base(id, tytul)
        {
            this.kategoria = kategoria;
            this.wartosc = wartosc;
        }

        public string Kategoria
        {
            get { return kategoria; }
        }

        public decimal Wartosc
        {
            get { return wartosc; }
        }

        // sprzet ma wieksza kare bo jest zazwyczaj drozszy niz ksiazka - 2 zl za dzien
        // i dodatkowo jak ktos przetrzyma sprzet ponad miesiac to dokladamy jeszcze 20 zl,
        // dla ksiazki takiej zasady nie ma - to wlasnie jest ten polimorfizm w praktyce,
        // ta sama metoda ObliczKare ale co innego sie dzieje w srodku
        public override decimal ObliczKare(int dniSpoznienia)
        {
            if (dniSpoznienia <= 0)
            {
                return 0m;
            }

            decimal kara = dniSpoznienia * 2.00m;

            if (dniSpoznienia > 30)
            {
                kara = kara + 20m;
            }

            return kara;
        }

        public override string PodajTyp()
        {
            return "Sprzet";
        }

        // litera S na poczatku zeby odroznic od ksiazki przy wczytywaniu pliku
        public override string DoLiniiTekstu()
        {
            return "S;" + base.DoLiniiTekstu() + ";" + kategoria + ";" + wartosc;
        }
    }
}
