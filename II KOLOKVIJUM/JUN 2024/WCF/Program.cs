using Skladiste;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConsoleSkladista
{
    class Program
    {
        static void Main(string[] args)
        {
            SkladistaClient proxy = new SkladistaClient();

            Vlasnik pera = new Vlasnik { Ime = "Pera", Prezime = "Peric", Jmbg = "111" };
            Vlasnik mika = new Vlasnik { Ime = "Mika", Prezime = "Mikic", Jmbg = "222" };

            proxy.Zakupi(pera, new Skladiste
            {
                IdSkladista = 1,
                PocetakZakupa = DateTime.Now.AddDays(-5),
                KrajZakupa = DateTime.Now.AddDays(5),
                Cena = 1000
            });
            proxy.Zakupi(pera, new Skladiste
            {
                IdSkladista = 2,
                PocetakZakupa = DateTime.Now.AddDays(-20),
                KrajZakupa = DateTime.Now.AddDays(-10),
                Cena = 500
            });
            proxy.Zakupi(mika, new Skladiste
            {
                IdSkladista = 3,
                PocetakZakupa = DateTime.Now.AddDays(-1),
                KrajZakupa = DateTime.Now.AddDays(30),
                Cena = 2000
            });

            Console.WriteLine("Aktivna skladista Pere:");
            foreach (var s in proxy.AktivnaSkladistaVlasnika("111"))
                Console.WriteLine("  Skladiste " + s.IdSkladista + ", cena " + s.Cena);

            Console.WriteLine("Vlasnici aktivnih skladista:");
            foreach (var v in proxy.VlasniciAktivnihSkladista())
                Console.WriteLine("  " + v.Ime + " " + v.Prezime);

            Console.WriteLine("Istorija svih zakupa:");
            foreach (var z in proxy.SvaSkladistaSaIstorijom())
                Console.WriteLine("  Skladiste " + z.Skladiste.IdSkladista +
                                  " <- " + z.Vlasnik.Ime + " " + z.Vlasnik.Prezime);

            proxy.Close();
            Console.ReadLine();
        }
    }
}
