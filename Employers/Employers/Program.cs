using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employers
{
    public class Alkalmazott
    {
        public int Azonosito { get; set; }
        public string Nev { get; set; }
        public int Kor { get; set; }
        public decimal Kereset { get; set; }

        public Alkalmazott(int azonosito, string nev, int kor, decimal kereset)
        {
            Azonosito = azonosito;
            Nev = nev;
            Kor = kor;
            Kereset = kereset;
        }
    }

    static void Main(string[] args)
    {
        string fajlPath = "tulajdonsagok_100sor.txt";
        List<Alkalmazott> alkalmazottak = new List<Alkalmazott>();

        try
        {
            foreach (var sor in File.ReadLines(fajlPath))
            {
                var reszek = sor.Split(';');

                if (reszek.Length != 4)
                {
                    Console.WriteLine($"Hibás sor: {sor}");
                    continue;
                }

                try
                {
                    var azonosito = int.Parse(reszek[0]);
                    var nev = reszek[1].Trim();
                    var kor = int.Parse(reszek[2]);
                    var kereset = decimal.Parse(reszek[3]);

                    alkalmazottak.Add(new Alkalmazott(azonosito, nev, kor, kereset));
                }
                catch (FormatException e)
                {
                    Console.WriteLine($"Formátum hiba az alábbi sorban: {sor}. Hiba: {e.Message}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Hiba az alábbi sor feldolgozása közben: {sor}. Hiba: {e.Message}");
                }
            }
        }
        catch (IOException e)
        {
            Console.WriteLine($"Hiba a fájl beolvasása közben: {e.Message}");
            return;
        }

        if (alkalmazottak.Count == 0)
        {
            Console.WriteLine("Nincsenek alkalmazottak az adatbázisban.");
            return;
        }

        Console.WriteLine("Alkalmazottak nevei:");
        foreach (var alkalmazott in alkalmazottak)
        {
            Console.WriteLine(alkalmazott.Nev);
        }

        try
        {
            var maxKereset = alkalmazottak.Max(a => a.Kereset);
            var legmagasabbKeresetuek = alkalmazottak.Where(a => a.Kereset == maxKereset);

            Console.WriteLine("\nLegmagasabb keresetű alkalmazottak:");
            foreach (var alkalmazott in legmagasabbKeresetuek)
            {
                Console.WriteLine($"Azonosító: {alkalmazott.Azonosito}, Név: {alkalmazott.Nev}");
            }
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine($"Hiba a legmagasabb kereset meghatározása közben: {e.Message}");
        }

        const int nyugdijKor = 65;
        var nyugdijhozKozeliek = alkalmazottak.Where(a => (nyugdijKor - a.Kor) == 10);

        Console.WriteLine("\nNyugdíjhoz közel álló alkalmazottak:");
        foreach (var alkalmazott in nyugdijhozKozeliek)
        {
            Console.WriteLine($"Név: {alkalmazott.Nev}, Kor: {alkalmazott.Kor}");
        }

        decimal keresetKuszob = 50000;
        var magasKeresetuAlkalmazottak = alkalmazottak.Count(a => a.Kereset > keresetKuszob);

        Console.WriteLine($"\n50000 forint felett kereső alkalmazottak száma: {magasKeresetuAlkalmazottak}");
    }
}
