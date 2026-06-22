using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.ServiceModel;

namespace Kalkulator
{
    // callback handler
    class CallbackHandler : IKalkulatorCallback
    {
        public void PrikaziIzraz(string izraz)
        {
            Console.WriteLine("   [callback] izraz: " + izraz);
        }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            InstanceContext context = new InstanceContext(new CallbackHandler());
            KalkulatorClient proxy = new KalkulatorClient(context);


            // pozovi SVE metode i ispisi rezultat
            Console.WriteLine("Dodaj(2)   -> " + proxy.Dodaj(2));
            Console.WriteLine("Dodaj(3)   -> " + proxy.Dodaj(3));
            Console.WriteLine("Oduzmi(5)  -> " + proxy.Oduzmi(5));
            Console.WriteLine("Pomnozi(7) -> " + proxy.Pomnozi(7));
            Console.WriteLine("Podeli(2)  -> " + proxy.Podeli(2));
            Console.WriteLine("Obrisi()   -> " + proxy.Obrisi());

            proxy.Close();
            Console.ReadLine();
        }
    }
}
