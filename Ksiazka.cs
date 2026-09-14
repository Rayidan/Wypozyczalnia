using System;

namespace Wypozyczalnia
{
    // Ksiazka dziedziczy po Pozycja (to jest to dziedziczenie - slowko ": Pozycja")
    // czyli automatycznie ma Id, Tytul, Dostepna itd, nie trzeba tego pisac jeszcze raz
    public class Ksiazka : Pozycja
    {
        private string autor;
        private int liczbaStron;

        // base(id, tytul) - wywoluje konstruktor klasy Pozycja zeby ustawil te wspolne pola
        public Ksiazka(int id, string tytul, string autor, int liczbaStron) : base(id, tytul)
        {
            this.autor = autor;
            this.liczbaStron = liczbaStron;
        }

        public string Autor
        {
            get { return autor; }
        }

        public int LiczbaStron
        {
            get { return liczbaStron; }
        }

        // dla ksiazki kara jest mala, 50 groszy za kazdy dzien spoznienia
        public override decimal ObliczKare(int dniSpoznienia)
        {
            if (dniSpoznienia <= 0)
            {
                // jak nie ma spoznienia albo oddal wczesniej to kara = 0
                return 0m;
            }

            return dniSpoznienia * 0.50m;
        }

        public override string PodajTyp()
        {
            return "Ksiazka";
        }

        // dopisujemy na poczatku litere K zeby przy wczytywaniu z pliku
        // program wiedzial ze to jest ksiazka a nie sprzet,
        // a na koncu dopisujemy jeszcze autora i liczbe stron
        public override string DoLiniiTekstu()
        {
            return "K;" + base.DoLiniiTekstu() + ";" + autor + ";" + liczbaStron;
        }
    }
}
