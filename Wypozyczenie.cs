using System;

namespace Wypozyczalnia
{
    // Ta klasa trzyma informacje o jednym wypozyczeniu - kto, co, kiedy i czy juz oddal.
    // Zamiast trzymac to jako pola bezposrednio w Pozycja albo w Czytelnik,
    // robimy dla tego osobna klase, bo jedna pozycja moze byc wypozyczana wiele razy
    // (raz przez jednego czytelnika, potem przez innego) i chcemy to wszystko zapamietac
    public class Wypozyczenie
    {
        private int idPozycji;
        private int idCzytelnika;
        private DateTime dataWypozyczenia;
        private DateTime terminZwrotu;
        private DateTime? dataZwrotu; // dopoki nie zwrocone to bedzie null
        private decimal naliczonaKara;

        public Wypozyczenie(int idPozycji, int idCzytelnika, DateTime dataWypozyczenia, DateTime terminZwrotu)
        {
            this.idPozycji = idPozycji;
            this.idCzytelnika = idCzytelnika;
            this.dataWypozyczenia = dataWypozyczenia;
            this.terminZwrotu = terminZwrotu;
            this.dataZwrotu = null;
            this.naliczonaKara = 0m;
        }

        public int IdPozycji
        {
            get { return idPozycji; }
        }

        public int IdCzytelnika
        {
            get { return idCzytelnika; }
        }

        public DateTime DataWypozyczenia
        {
            get { return dataWypozyczenia; }
        }

        public DateTime TerminZwrotu
        {
            get { return terminZwrotu; }
        }

        public DateTime? DataZwrotu
        {
            get { return dataZwrotu; }
        }

        public decimal NaliczonaKara
        {
            get { return naliczonaKara; }
        }

        // jak dataZwrotu ciagle jest null to znaczy ze jeszcze nie oddane
        public bool JestZwrocone
        {
            get { return dataZwrotu != null; }
        }

        // ta metoda jest wywolywana jak czytelnik oddaje pozycje z powrotem
        public void ZapiszZwrot(DateTime kiedy, decimal kara)
        {
            dataZwrotu = kiedy;
            naliczonaKara = kara;
        }

        public string DoLiniiTekstu()
        {
            string tekstDatyZwrotu;
            if (dataZwrotu == null)
            {
                tekstDatyZwrotu = "brak";
            }
            else
            {
                tekstDatyZwrotu = dataZwrotu.Value.ToString("yyyy-MM-dd");
            }

            return idPozycji + ";" + idCzytelnika + ";" + dataWypozyczenia.ToString("yyyy-MM-dd") + ";"
                + terminZwrotu.ToString("yyyy-MM-dd") + ";" + tekstDatyZwrotu + ";" + naliczonaKara;
        }
    }
}
