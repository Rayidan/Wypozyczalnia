using System;

namespace Wypozyczalnia
{
    // Czytelnik to zwykla klasa, nie dziedziczy po niczym.
    // Tutaj tez mamy enkapsulacje - pola prywatne, dostep tylko przez property
    public class Czytelnik
    {
        private int id;
        private string imie;
        private string nazwisko;
        private string email;

        public Czytelnik(int id, string imie, string nazwisko, string email)
        {
            this.id = id;
            this.imie = imie;
            this.nazwisko = nazwisko;
            this.email = email;
        }

        public int Id
        {
            get { return id; }
        }

        public string Imie
        {
            get { return imie; }
        }

        public string Nazwisko
        {
            get { return nazwisko; }
        }

        public string Email
        {
            get { return email; }
        }

        // male udogodnienie - zeby nie skladac imienia i nazwiska w kilku miejscach programu
        public string ImieINazwisko()
        {
            return imie + " " + nazwisko;
        }

        public string DoLiniiTekstu()
        {
            return id + ";" + imie + ";" + nazwisko + ";" + email;
        }
    }
}
