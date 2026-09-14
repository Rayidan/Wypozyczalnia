# Projekt zaliczeniowy - Programowanie Obiektowe

**Student:** Rafał Dołbniak
**Numer albumu:** 79621
**Uczelnia:** Akademia Finansów i Biznesu Vistula
**Prowadzący:** Artur Karwatka
**Zespół:** projekt indywidualny (1 osoba) - wszystkie funkcjonalności RD

## Temat

Konsolowa aplikacja w C# do obsługi wypożyczalni - program pozwala zarządzać
wypożyczalnią, w której można wypożyczać zarówno książki jak i sprzęt (np. projektor,
namiot itp). Dwa różne rodzaje rzeczy do wypożyczenia, każdy ma trochę inne zasady
naliczania kary za spóźniony zwrot - to jest miejsce gdzie wykorzystałem dziedziczenie
i polimorfizm (opisane niżej).

Program działa w pętli z menu tekstowym, dane zapisują się do plików tekstowych
w folderze programu, więc po ponownym uruchomieniu wszystko jest tam gdzie było.

## Lista funkcjonalności (RD - całość, projekt indywidualny)

1. Dodawanie nowej książki do magazynu (tytuł, autor, liczba stron)
2. Dodawanie nowego sprzętu do magazynu (nazwa, kategoria, wartość)
3. Usuwanie pozycji z magazynu (tylko gdy nie jest aktualnie wypożyczona)
4. Wyświetlanie listy wszystkich pozycji wraz ze statusem (dostępna/wypożyczona)
5. Dodawanie nowego czytelnika (imię, nazwisko, email)
6. Wyświetlanie listy czytelników
7. Wypożyczanie pozycji wybranemu czytelnikowi z ustawieniem terminu zwrotu
8. Zwrot pozycji wraz z automatycznym naliczeniem kary za spóźnienie (jeśli jest)
9. Wyszukiwanie pozycji po fragmencie tytułu
10. Historia wypożyczeń dla konkretnego czytelnika
11. Podgląd aktualnie wypożyczonych pozycji wraz z oznaczeniem tych przetrzymanych
12. Raport najpopularniejszych pozycji (ranking wg liczby wypożyczeń)
13. Zapis danych do plików tekstowych (ręczny z menu oraz automatyczny przy wyjściu)
14. Automatyczne wczytanie danych z plików przy starcie programu

(punkty 13 i 14 to jedna "para" zapis/odczyt, więc realnie liczę to jako
13 samodzielnych funkcjonalności - i tak więcej niż wymagane minimum 8)

## Wykorzystane elementy programowania obiektowego

- **Abstrakcja** - klasa `Pozycja` jest klasą abstrakcyjną (`abstract class`), nie da
  się utworzyć jej obiektu wprost, jest tylko wzorem dla `Ksiazka` i `Sprzet`.
  Metody `ObliczKare` i `PodajTyp` są abstrakcyjne.
- **Enkapsulacja** - wszystkie pola we wszystkich klasach są prywatne (`private`),
  dostęp do nich jest tylko przez property (gettery) albo przez konkretne metody
  jak `OznaczWypozyczenie()` / `OznaczZwrot()`, nie da się tych pól nadpisać
  bezpośrednio z zewnątrz klasy.
- **Dziedziczenie** - `Ksiazka` i `Sprzet` dziedziczą po `Pozycja` (`: Pozycja`),
  dzięki temu obie klasy mają wspólne pola/metody (Id, Tytul, Dostepna...) bez
  przepisywania tego dwa razy.
- **Polimorfizm** - metoda `ObliczKare(int dniSpoznienia)` jest inaczej liczona
  dla książki (0,50 zł/dzień) a inaczej dla sprzętu (2 zł/dzień + dodatkowa opłata
  po 30 dniach). W klasie `Magazyn` wywołuję `pozycja.ObliczKare(...)` na zmiennej
  typu `Pozycja` i program sam wybiera właściwą wersję metody w zależności od tego,
  czy pod spodem jest `Ksiazka` czy `Sprzet`.

## Struktura plików

- `Pozycja.cs` - klasa abstrakcyjna, bazowa dla wszystkiego co można wypożyczyć
- `Ksiazka.cs` - książka (dziedziczy po Pozycja)
- `Sprzet.cs` - sprzęt (dziedziczy po Pozycja)
- `Czytelnik.cs` - osoba wypożyczająca
- `Wypozyczenie.cs` - pojedynczy zapis wypożyczenia (co, kto, kiedy, czy zwrócone)
- `Magazyn.cs` - główna logika programu, listy obiektów i wszystkie metody
- `Program.cs` - menu w konsoli, wczytywanie danych od użytkownika

## Jak uruchomić

Wymagany zainstalowany .NET SDK (projekt robiony i testowany na .NET 10 SDK,
`dotnet --version` -> 10.0.301).

W folderze z plikiem `Wypozyczalnia.csproj`:

```
dotnet build
dotnet run
```

Przy pierwszym uruchomieniu program sam wgra kilka przykładowych pozycji i
czytelników, żeby od razu było na czym testować menu. Dane są potem zapisywane
do plików `dane_pozycje.txt`, `dane_czytelnicy.txt`, `dane_wypozyczenia.txt`
w tym samym folderze (zwykły tekst rozdzielany średnikami, można podejrzeć
w notatniku).
