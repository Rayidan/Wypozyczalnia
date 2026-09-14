using System;

namespace Wypozyczalnia
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Magazyn magazyn = new Magazyn();

            // przy starcie probujemy wczytac dane z poprzedniego uruchomienia
            magazyn.WczytajZPliku();

            // jak to pierwsze uruchomienie (pliki nie istnialy) to wgrywamy troche danych
            // testowych zeby program od razu bylo czym pokazac
            if (magazyn.LiczbaPozycji == 0)
            {
                Console.WriteLine("Pierwsze uruchomienie - wgrywam przykladowe dane.");
                magazyn.WgrajPrzykladoweDane();
            }

            Console.WriteLine();
            Console.WriteLine("=== WYPOZYCZALNIA - projekt zaliczeniowy ===");
            Console.WriteLine("Rafal Dolbniak, numer albumu 79621");

            bool koniecProgramu = false;

            while (koniecProgramu == false)
            {
                PokazMenu();
                string wybor = Console.ReadLine();
                Console.WriteLine();

                switch (wybor)
                {
                    case "1":
                        DodajKsiazkeZKonsoli(magazyn);
                        break;

                    case "2":
                        DodajSprzetZKonsoli(magazyn);
                        break;

                    case "3":
                        Console.Write("Podaj id pozycji do usuniecia: ");
                        int idDoUsuniecia = WczytajLiczbe();
                        magazyn.UsunPozycje(idDoUsuniecia);
                        break;

                    case "4":
                        magazyn.PokazWszystkiePozycje();
                        break;

                    case "5":
                        DodajCzytelnikaZKonsoli(magazyn);
                        break;

                    case "6":
                        magazyn.PokazCzytelnikow();
                        break;

                    case "7":
                        WypozyczZKonsoli(magazyn);
                        break;

                    case "8":
                        Console.Write("Podaj id pozycji ktora chcesz zwrocic: ");
                        int idDoZwrotu = WczytajLiczbe();
                        magazyn.ZwrocPozycje(idDoZwrotu);
                        break;

                    case "9":
                        Console.Write("Wpisz szukany tytul (albo jego fragment): ");
                        string fraza = Console.ReadLine();
                        magazyn.WyszukajPozycje(fraza);
                        break;

                    case "10":
                        Console.Write("Podaj id czytelnika: ");
                        int idCzytelnikaHist = WczytajLiczbe();
                        magazyn.PokazHistorieCzytelnika(idCzytelnikaHist);
                        break;

                    case "11":
                        magazyn.PokazAktualneWypozyczenia();
                        break;

                    case "12":
                        magazyn.PokazRaportPopularnosci();
                        break;

                    case "13":
                        magazyn.ZapiszDoPliku();
                        break;

                    case "0":
                        magazyn.ZapiszDoPliku();
                        Console.WriteLine("Dane zapisane. Do zobaczenia!");
                        koniecProgramu = true;
                        break;

                    default:
                        Console.WriteLine("Nie ma takiej opcji, sprobuj jeszcze raz.");
                        break;
                }

                Console.WriteLine();
            }
        }

        static void PokazMenu()
        {
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine(" 1  - Dodaj ksiazke");
            Console.WriteLine(" 2  - Dodaj sprzet");
            Console.WriteLine(" 3  - Usun pozycje");
            Console.WriteLine(" 4  - Pokaz wszystkie pozycje");
            Console.WriteLine(" 5  - Dodaj czytelnika");
            Console.WriteLine(" 6  - Pokaz czytelnikow");
            Console.WriteLine(" 7  - Wypozycz pozycje");
            Console.WriteLine(" 8  - Zwroc pozycje");
            Console.WriteLine(" 9  - Wyszukaj pozycje po tytule");
            Console.WriteLine("10  - Historia wypozyczen czytelnika");
            Console.WriteLine("11  - Pokaz aktualne wypozyczenia (kto/co/czy przetrzymane)");
            Console.WriteLine("12  - Raport najpopularniejszych pozycji");
            Console.WriteLine("13  - Zapisz dane do pliku recznie");
            Console.WriteLine(" 0  - Zapisz i wyjdz");
            Console.WriteLine("---------------------------------------------");
            Console.Write("Wybierz opcje: ");
        }

        static void DodajKsiazkeZKonsoli(Magazyn magazyn)
        {
            Console.Write("Tytul ksiazki: ");
            string tytul = Console.ReadLine();

            Console.Write("Autor: ");
            string autor = Console.ReadLine();

            Console.Write("Liczba stron: ");
            int strony = WczytajLiczbe();

            magazyn.DodajKsiazke(tytul, autor, strony);
        }

        static void DodajSprzetZKonsoli(Magazyn magazyn)
        {
            Console.Write("Nazwa sprzetu: ");
            string nazwa = Console.ReadLine();

            Console.Write("Kategoria (np. elektronika, sport): ");
            string kategoria = Console.ReadLine();

            Console.Write("Wartosc w zl: ");
            string wartoscTekst = Console.ReadLine();
            decimal wartosc;
            bool udaloSie = decimal.TryParse(wartoscTekst, out wartosc);
            if (udaloSie == false)
            {
                wartosc = 0m;
                Console.WriteLine("Nie rozpoznano liczby, ustawiam wartosc na 0.");
            }

            magazyn.DodajSprzet(nazwa, kategoria, wartosc);
        }

        static void DodajCzytelnikaZKonsoli(Magazyn magazyn)
        {
            Console.Write("Imie: ");
            string imie = Console.ReadLine();

            Console.Write("Nazwisko: ");
            string nazwisko = Console.ReadLine();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            magazyn.DodajCzytelnika(imie, nazwisko, email);
        }

        static void WypozyczZKonsoli(Magazyn magazyn)
        {
            Console.Write("Id pozycji: ");
            int idPozycji = WczytajLiczbe();

            Console.Write("Id czytelnika: ");
            int idCzytelnika = WczytajLiczbe();

            Console.Write("Na ile dni (np. 14): ");
            int naIleDni = WczytajLiczbe();

            if (naIleDni <= 0)
            {
                naIleDni = 14; // domyslny termin jak ktos wpisze cos dziwnego
            }

            magazyn.WypozyczPozycje(idPozycji, idCzytelnika, naIleDni);
        }

        // pomocnicza metoda do bezpiecznego wczytania liczby z konsoli,
        // zeby program sie nie wywalal jak ktos wpisze litery zamiast cyfr
        static int WczytajLiczbe()
        {
            string tekst = Console.ReadLine();
            int wynik;
            bool udaloSie = int.TryParse(tekst, out wynik);

            if (udaloSie == false)
            {
                Console.WriteLine("To nie byla poprawna liczba, przyjmuje 0.");
                return 0;
            }

            return wynik;
        }
    }
}
