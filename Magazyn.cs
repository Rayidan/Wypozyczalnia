using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace Wypozyczalnia
{
    // To jest glowna klasa calego programu - trzyma wszystkie listy
    // (pozycje, czytelnicy, wypozyczenia) i ma metody ktore cos z nimi robia.
    // Nazwalem ja Magazyn bo najlepiej pasowalo, moglo by byc tez "Wypozyczalnia"
    // ale to samo co nazwa projektu wiec zrobilby sie balagan z namespace
    public class Magazyn
    {
        private List<Pozycja> pozycje;
        private List<Czytelnik> czytelnicy;
        private List<Wypozyczenie> wypozyczenia;

        // licznik do nadawania nowych numerkow id, zeby sie nie powtarzaly
        private int nastepneIdPozycji;
        private int nastepneIdCzytelnika;

        // nazwy plikow w ktorych trzymamy dane, w tym samym folderze co program
        private string plikPozycje = "dane_pozycje.txt";
        private string plikCzytelnicy = "dane_czytelnicy.txt";
        private string plikWypozyczenia = "dane_wypozyczenia.txt";

        public Magazyn()
        {
            pozycje = new List<Pozycja>();
            czytelnicy = new List<Czytelnik>();
            wypozyczenia = new List<Wypozyczenie>();
            nastepneIdPozycji = 1;
            nastepneIdCzytelnika = 1;
        }

        // ========== FUNKCJONALNOSC: dodawanie ksiazki ==========
        public void DodajKsiazke(string tytul, string autor, int liczbaStron)
        {
            Ksiazka nowa = new Ksiazka(nastepneIdPozycji, tytul, autor, liczbaStron);
            pozycje.Add(nowa);
            nastepneIdPozycji = nastepneIdPozycji + 1;
            Console.WriteLine("Dodano ksiazke, nadany numer id to: " + nowa.Id);
        }

        // ========== FUNKCJONALNOSC: dodawanie sprzetu ==========
        public void DodajSprzet(string nazwa, string kategoria, decimal wartosc)
        {
            Sprzet nowy = new Sprzet(nastepneIdPozycji, nazwa, kategoria, wartosc);
            pozycje.Add(nowy);
            nastepneIdPozycji = nastepneIdPozycji + 1;
            Console.WriteLine("Dodano sprzet, nadany numer id to: " + nowy.Id);
        }

        // ========== FUNKCJONALNOSC: usuwanie pozycji ==========
        public void UsunPozycje(int id)
        {
            Pozycja znaleziona = ZnajdzPozycjePoId(id);

            if (znaleziona == null)
            {
                Console.WriteLine("Nie ma pozycji o takim id.");
                return;
            }

            if (znaleziona.Dostepna == false)
            {
                Console.WriteLine("Ta pozycja jest aktualnie wypozyczona, nie mozna jej usunac.");
                return;
            }

            pozycje.Remove(znaleziona);
            Console.WriteLine("Usunieto pozycje: " + znaleziona.Tytul);
        }

        // ========== FUNKCJONALNOSC: wyswietlanie wszystkich pozycji ==========
        public void PokazWszystkiePozycje()
        {
            if (pozycje.Count == 0)
            {
                Console.WriteLine("Magazyn jest pusty, nie ma zadnych pozycji.");
                return;
            }

            for (int i = 0; i < pozycje.Count; i++)
            {
                Console.WriteLine(pozycje[i].KrotkiOpis());
            }
        }

        // ========== FUNKCJONALNOSC: dodawanie czytelnika ==========
        public void DodajCzytelnika(string imie, string nazwisko, string email)
        {
            Czytelnik nowy = new Czytelnik(nastepneIdCzytelnika, imie, nazwisko, email);
            czytelnicy.Add(nowy);
            nastepneIdCzytelnika = nastepneIdCzytelnika + 1;
            Console.WriteLine("Dodano czytelnika, nadany numer id to: " + nowy.Id);
        }

        // ========== FUNKCJONALNOSC: wyswietlanie czytelnikow ==========
        public void PokazCzytelnikow()
        {
            if (czytelnicy.Count == 0)
            {
                Console.WriteLine("Nie ma jeszcze zadnych czytelnikow.");
                return;
            }

            for (int i = 0; i < czytelnicy.Count; i++)
            {
                Czytelnik c = czytelnicy[i];
                Console.WriteLine("[" + c.Id + "] " + c.ImieINazwisko() + " - " + c.Email);
            }
        }

        // ========== FUNKCJONALNOSC: wypozyczanie pozycji ==========
        public void WypozyczPozycje(int idPozycji, int idCzytelnika, int naIleDni)
        {
            Pozycja pozycja = ZnajdzPozycjePoId(idPozycji);
            Czytelnik czytelnik = ZnajdzCzytelnikaPoId(idCzytelnika);

            if (pozycja == null)
            {
                Console.WriteLine("Nie ma pozycji o takim id.");
                return;
            }

            if (czytelnik == null)
            {
                Console.WriteLine("Nie ma czytelnika o takim id.");
                return;
            }

            if (pozycja.Dostepna == false)
            {
                Console.WriteLine("Ta pozycja jest juz wypozyczona przez kogos innego.");
                return;
            }

            DateTime dzisiaj = DateTime.Now.Date;
            DateTime termin = dzisiaj.AddDays(naIleDni);

            Wypozyczenie noweWypozyczenie = new Wypozyczenie(idPozycji, idCzytelnika, dzisiaj, termin);
            wypozyczenia.Add(noweWypozyczenie);

            pozycja.OznaczWypozyczenie();

            Console.WriteLine(czytelnik.ImieINazwisko() + " wypozyczyl(a) \"" + pozycja.Tytul + "\".");
            Console.WriteLine("Termin zwrotu: " + termin.ToString("yyyy-MM-dd"));
        }

        // ========== FUNKCJONALNOSC: zwrot pozycji (z naliczeniem kary) ==========
        public void ZwrocPozycje(int idPozycji)
        {
            Pozycja pozycja = ZnajdzPozycjePoId(idPozycji);

            if (pozycja == null)
            {
                Console.WriteLine("Nie ma pozycji o takim id.");
                return;
            }

            // szukamy wypozyczenia dla tej pozycji ktore jeszcze nie ma zwrotu
            Wypozyczenie aktywne = null;
            for (int i = 0; i < wypozyczenia.Count; i++)
            {
                if (wypozyczenia[i].IdPozycji == idPozycji && wypozyczenia[i].JestZwrocone == false)
                {
                    aktywne = wypozyczenia[i];
                }
            }

            if (aktywne == null)
            {
                Console.WriteLine("Ta pozycja nie jest aktualnie wypozyczona.");
                return;
            }

            DateTime dzisiaj = DateTime.Now.Date;
            int dniSpoznienia = (dzisiaj - aktywne.TerminZwrotu).Days;

            // wywolanie ObliczKare na obiekcie typu Pozycja - a to ze w srodku
            // wykona sie kod z Ksiazka albo ze Sprzet, to wlasnie jest polimorfizm
            decimal kara = pozycja.ObliczKare(dniSpoznienia);

            aktywne.ZapiszZwrot(dzisiaj, kara);
            pozycja.OznaczZwrot();

            Console.WriteLine("Zwrocono: " + pozycja.Tytul);

            if (kara > 0)
            {
                Console.WriteLine("Uwaga, zwrot spozniony o " + dniSpoznienia + " dni. Naliczona kara: " + kara + " zl");
            }
            else
            {
                Console.WriteLine("Zwrot w terminie, brak kary.");
            }
        }

        // ========== FUNKCJONALNOSC: wyszukiwanie pozycji po tytule ==========
        public void WyszukajPozycje(string szukanyTekst)
        {
            bool cosZnaleziono = false;
            string szukanyMaleLitery = szukanyTekst.ToLower();

            for (int i = 0; i < pozycje.Count; i++)
            {
                string tytulMaleLitery = pozycje[i].Tytul.ToLower();
                if (tytulMaleLitery.Contains(szukanyMaleLitery))
                {
                    Console.WriteLine(pozycje[i].KrotkiOpis());
                    cosZnaleziono = true;
                }
            }

            if (cosZnaleziono == false)
            {
                Console.WriteLine("Nic nie znaleziono dla frazy: " + szukanyTekst);
            }
        }

        // ========== FUNKCJONALNOSC: historia wypozyczen danego czytelnika ==========
        public void PokazHistorieCzytelnika(int idCzytelnika)
        {
            Czytelnik czytelnik = ZnajdzCzytelnikaPoId(idCzytelnika);
            if (czytelnik == null)
            {
                Console.WriteLine("Nie ma czytelnika o takim id.");
                return;
            }

            bool cosZnaleziono = false;

            for (int i = 0; i < wypozyczenia.Count; i++)
            {
                Wypozyczenie w = wypozyczenia[i];
                if (w.IdCzytelnika == idCzytelnika)
                {
                    cosZnaleziono = true;
                    Pozycja pozycja = ZnajdzPozycjePoId(w.IdPozycji);
                    string tytulPozycji = "(usunieta pozycja)";
                    if (pozycja != null)
                    {
                        tytulPozycji = pozycja.Tytul;
                    }

                    string status;
                    if (w.JestZwrocone)
                    {
                        status = "zwrocono " + w.DataZwrotu.Value.ToString("yyyy-MM-dd") + ", kara: " + w.NaliczonaKara + " zl";
                    }
                    else
                    {
                        status = "nadal wypozyczone, termin: " + w.TerminZwrotu.ToString("yyyy-MM-dd");
                    }

                    Console.WriteLine(" - " + tytulPozycji + " | wypozyczono: " + w.DataWypozyczenia.ToString("yyyy-MM-dd") + " | " + status);
                }
            }

            if (cosZnaleziono == false)
            {
                Console.WriteLine(czytelnik.ImieINazwisko() + " nie ma jeszcze zadnych wypozyczen.");
            }
        }

        // ========== FUNKCJONALNOSC: lista aktualnie wypozyczonych pozycji ==========
        public void PokazAktualneWypozyczenia()
        {
            bool cosZnaleziono = false;
            DateTime dzisiaj = DateTime.Now.Date;

            for (int i = 0; i < wypozyczenia.Count; i++)
            {
                Wypozyczenie w = wypozyczenia[i];
                if (w.JestZwrocone == false)
                {
                    cosZnaleziono = true;
                    Pozycja pozycja = ZnajdzPozycjePoId(w.IdPozycji);
                    Czytelnik czytelnik = ZnajdzCzytelnikaPoId(w.IdCzytelnika);

                    string nazwaPozycji = "?";
                    if (pozycja != null)
                    {
                        nazwaPozycji = pozycja.Tytul;
                    }

                    string nazwaCzytelnika = "?";
                    if (czytelnik != null)
                    {
                        nazwaCzytelnika = czytelnik.ImieINazwisko();
                    }

                    string uwaga = "";
                    if (dzisiaj > w.TerminZwrotu)
                    {
                        int dniPoTerminie = (dzisiaj - w.TerminZwrotu).Days;
                        uwaga = "  <-- PRZETRZYMANE juz " + dniPoTerminie + " dni!";
                    }

                    Console.WriteLine(nazwaPozycji + " -> " + nazwaCzytelnika + " (termin: " + w.TerminZwrotu.ToString("yyyy-MM-dd") + ")" + uwaga);
                }
            }

            if (cosZnaleziono == false)
            {
                Console.WriteLine("Aktualnie nic nie jest wypozyczone.");
            }
        }

        // ========== FUNKCJONALNOSC: raport najpopularniejszych pozycji ==========
        public void PokazRaportPopularnosci()
        {
            if (pozycje.Count == 0)
            {
                Console.WriteLine("Brak danych do raportu.");
                return;
            }

            // robie kopie listy zeby posortowac i nie namieszac w oryginalnej kolejnosci
            List<Pozycja> posortowane = new List<Pozycja>(pozycje);

            // zwykle sortowanie babelkowe, od najwiecej wypozyczanych do najmniej
            for (int i = 0; i < posortowane.Count - 1; i++)
            {
                for (int j = 0; j < posortowane.Count - 1 - i; j++)
                {
                    if (posortowane[j].IleRazyWypozyczona < posortowane[j + 1].IleRazyWypozyczona)
                    {
                        Pozycja tymczasowa = posortowane[j];
                        posortowane[j] = posortowane[j + 1];
                        posortowane[j + 1] = tymczasowa;
                    }
                }
            }

            Console.WriteLine("Ranking wg liczby wypozyczen:");
            for (int i = 0; i < posortowane.Count; i++)
            {
                Console.WriteLine((i + 1) + ". " + posortowane[i].Tytul + " - " + posortowane[i].IleRazyWypozyczona + " razy");
            }
        }

        // pomocnicza metoda do szukania pozycji po id, zeby nie kopiowac tej samej petli wszedzie
        private Pozycja ZnajdzPozycjePoId(int id)
        {
            for (int i = 0; i < pozycje.Count; i++)
            {
                if (pozycje[i].Id == id)
                {
                    return pozycje[i];
                }
            }
            return null;
        }

        private Czytelnik ZnajdzCzytelnikaPoId(int id)
        {
            for (int i = 0; i < czytelnicy.Count; i++)
            {
                if (czytelnicy[i].Id == id)
                {
                    return czytelnicy[i];
                }
            }
            return null;
        }

        // ========== FUNKCJONALNOSC: zapis danych do plikow ==========
        public void ZapiszDoPliku()
        {
            List<string> liniePozycje = new List<string>();
            for (int i = 0; i < pozycje.Count; i++)
            {
                liniePozycje.Add(pozycje[i].DoLiniiTekstu());
            }
            File.WriteAllLines(plikPozycje, liniePozycje);

            List<string> linieCzytelnicy = new List<string>();
            for (int i = 0; i < czytelnicy.Count; i++)
            {
                linieCzytelnicy.Add(czytelnicy[i].DoLiniiTekstu());
            }
            File.WriteAllLines(plikCzytelnicy, linieCzytelnicy);

            List<string> linieWypozyczenia = new List<string>();
            for (int i = 0; i < wypozyczenia.Count; i++)
            {
                linieWypozyczenia.Add(wypozyczenia[i].DoLiniiTekstu());
            }
            File.WriteAllLines(plikWypozyczenia, linieWypozyczenia);

            Console.WriteLine("Zapisano dane do plikow.");
        }

        // ========== FUNKCJONALNOSC: wczytywanie danych z plikow przy starcie ==========
        public void WczytajZPliku()
        {
            // jak plik nie istnieje to znaczy ze program uruchamiamy pierwszy raz,
            // wiec po prostu nic nie wczytujemy i konczymy metode
            if (!File.Exists(plikPozycje))
            {
                return;
            }

            string[] liniePozycje = File.ReadAllLines(plikPozycje);
            for (int i = 0; i < liniePozycje.Length; i++)
            {
                if (liniePozycje[i].Trim() == "")
                {
                    continue; // pusta linia, pomijamy
                }

                string[] czesci = liniePozycje[i].Split(';');
                // czesci[0] to litera K albo S, czesci[1] to id, [2] tytul, [3] dostepna, [4] ile razy

                int id = int.Parse(czesci[1]);
                string tytul = czesci[2];
                bool dostepna = bool.Parse(czesci[3]);
                int ileRazy = int.Parse(czesci[4]);

                if (czesci[0] == "K")
                {
                    string autor = czesci[5];
                    int strony = int.Parse(czesci[6]);
                    Ksiazka k = new Ksiazka(id, tytul, autor, strony);
                    PrzywrocStanPozycji(k, dostepna, ileRazy);
                    pozycje.Add(k);
                }
                else if (czesci[0] == "S")
                {
                    string kategoria = czesci[5];
                    decimal wartosc = decimal.Parse(czesci[6], CultureInfo.InvariantCulture);
                    Sprzet s = new Sprzet(id, tytul, kategoria, wartosc);
                    PrzywrocStanPozycji(s, dostepna, ileRazy);
                    pozycje.Add(s);
                }

                if (id >= nastepneIdPozycji)
                {
                    nastepneIdPozycji = id + 1;
                }
            }

            if (File.Exists(plikCzytelnicy))
            {
                string[] linieCzytelnicy = File.ReadAllLines(plikCzytelnicy);
                for (int i = 0; i < linieCzytelnicy.Length; i++)
                {
                    if (linieCzytelnicy[i].Trim() == "")
                    {
                        continue;
                    }

                    string[] czesci = linieCzytelnicy[i].Split(';');
                    int id = int.Parse(czesci[0]);
                    Czytelnik c = new Czytelnik(id, czesci[1], czesci[2], czesci[3]);
                    czytelnicy.Add(c);

                    if (id >= nastepneIdCzytelnika)
                    {
                        nastepneIdCzytelnika = id + 1;
                    }
                }
            }

            if (File.Exists(plikWypozyczenia))
            {
                string[] linieWyp = File.ReadAllLines(plikWypozyczenia);
                for (int i = 0; i < linieWyp.Length; i++)
                {
                    if (linieWyp[i].Trim() == "")
                    {
                        continue;
                    }

                    string[] czesci = linieWyp[i].Split(';');
                    int idPoz = int.Parse(czesci[0]);
                    int idCzyt = int.Parse(czesci[1]);
                    DateTime dataWyp = DateTime.Parse(czesci[2]);
                    DateTime termin = DateTime.Parse(czesci[3]);

                    Wypozyczenie w = new Wypozyczenie(idPoz, idCzyt, dataWyp, termin);

                    if (czesci[4] != "brak")
                    {
                        DateTime dataZwr = DateTime.Parse(czesci[4]);
                        decimal kara = decimal.Parse(czesci[5], CultureInfo.InvariantCulture);
                        w.ZapiszZwrot(dataZwr, kara);
                    }

                    wypozyczenia.Add(w);
                }
            }

            Console.WriteLine("Wczytano zapisane dane (" + pozycje.Count + " pozycji, " + czytelnicy.Count + " czytelnikow).");
        }

        // male obejscie - dostepnosc i licznik wypozyczen sa prywatne w klasie Pozycja
        // (specjalnie, zeby nikt z zewnatrz nie mogl ich sobie zmienic w dowolnym momencie),
        // ale przy wczytywaniu z pliku musimy je jakos ustawic z powrotem na to co bylo zapisane
        private void PrzywrocStanPozycji(Pozycja pozycja, bool dostepna, int ileRazy)
        {
            for (int i = 0; i < ileRazy; i++)
            {
                pozycja.OznaczWypozyczenie();
            }

            if (dostepna)
            {
                pozycja.OznaczZwrot();
            }
        }

        // dane przykladowe zeby przy pierwszym uruchomieniu program nie byl zupelnie pusty,
        // przyda sie tez do nagrywania filmiku z prezentacja
        public void WgrajPrzykladoweDane()
        {
            DodajKsiazke("Wiedzmin", "Andrzej Sapkowski", 300);
            DodajKsiazke("Pan Tadeusz", "Adam Mickiewicz", 250);
            DodajSprzet("Projektor Epson", "elektronika", 1500m);
            DodajSprzet("Namiot 4-osobowy", "turystyka", 450m);
            DodajCzytelnika("Rafal", "Dolbniak", "rafal@przyklad.pl");
            DodajCzytelnika("Jan", "Kowalski", "jan.kowalski@przyklad.pl");
        }

        public int LiczbaPozycji
        {
            get { return pozycje.Count; }
        }
    }
}
