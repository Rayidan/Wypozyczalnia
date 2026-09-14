using System;

namespace Wypozyczalnia
{
    // To jest klasa abstrakcyjna. Nie mozna zrobic czegos w stylu "new Pozycja(...)",
    // bo Pozycja sama w sobie to tylko wzor / szkielet dla Ksiazki i Sprzetu.
    // To jest ta abstrakcja o ktorej mowia na wykladzie.
    public abstract class Pozycja
    {
        // pola sa prywatne, czyli nikt z zewnatrz (np. z Program.cs) nie moze
        // sobie po prostu wpisac pozycja.dostepna = true, tylko musi uzyc metody.
        // to jest enkapsulacja - chowamy dane w srodku klasy
        private int id;
        private string tytul;
        private bool dostepna;
        private int ileRazyWypozyczona;

        // konstruktor, ustawia dane poczatkowe kiedy tworzymy nowa pozycje
        public Pozycja(int id, string tytul)
        {
            this.id = id;
            this.tytul = tytul;
            this.dostepna = true; // nowa pozycja od razu jest wolna, nikt jej jeszcze nie wypozyczyl
            this.ileRazyWypozyczona = 0;
        }

        // ponizej same gettery zeby dalo sie odczytac wartosci z zewnatrz klasy
        public int Id
        {
            get { return id; }
        }

        public string Tytul
        {
            get { return tytul; }
        }

        public bool Dostepna
        {
            get { return dostepna; }
        }

        public int IleRazyWypozyczona
        {
            get { return ileRazyWypozyczona; }
        }

        // ta metoda jest wywolywana kiedy ktos wypozycza pozycje
        public void OznaczWypozyczenie()
        {
            dostepna = false;
            ileRazyWypozyczona = ileRazyWypozyczona + 1;
        }

        // a ta jak sie oddaje pozycje z powrotem
        public void OznaczZwrot()
        {
            dostepna = true;
        }

        // metoda abstrakcyjna - to znaczy ze KAZDA klasa ktora dziedziczy po Pozycja
        // (czyli Ksiazka i Sprzet) musi sama napisac jak to policzyc.
        // dzieki temu jak wywolamy ObliczKare() na liscie roznych pozycji,
        // to dla ksiazki policzy sie inaczej a dla sprzetu inaczej - to jest polimorfizm
        public abstract decimal ObliczKare(int dniSpoznienia);

        // tez abstrakcyjna, po prostu ma zwrocic np "Ksiazka" albo "Sprzet"
        public abstract string PodajTyp();

        // metoda do zamiany pozycji na linijke tekstu, zeby zapisac ja do pliku
        // "virtual" znaczy ze mozna ja nadpisac w klasie potomnej (i tak robimy)
        public virtual string DoLiniiTekstu()
        {
            return id + ";" + tytul + ";" + dostepna + ";" + ileRazyWypozyczona;
        }

        // prosty opis do wyswietlenia w konsoli, wspolny dla wszystkich pozycji
        public string KrotkiOpis()
        {
            string stanDostepnosci;
            if (dostepna == true)
            {
                stanDostepnosci = "dostepna";
            }
            else
            {
                stanDostepnosci = "wypozyczona";
            }

            return "[" + id + "] " + PodajTyp() + " - " + tytul + " (" + stanDostepnosci + ")";
        }
    }
}
