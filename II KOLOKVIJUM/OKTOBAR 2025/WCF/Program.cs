using System;
using System.ServiceModel;

namespace WcfMatrice
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(MatricaService));
            host.Open();
            Console.WriteLine("Servis je pokrenut.");

            ChannelFactory<IMatrica> factory = new ChannelFactory<IMatrica>(new WSHttpBinding(), "http://localhost:8080/MatricaService/");
            IMatrica klijent = factory.CreateChannel();

            double[][] matricaA = new double[][]
            {
                new double[] { 1.0, 2.0, 3.0 },
                new double[] { 4.0, 5.0, 6.0 }
            };

            Console.WriteLine("Poziv: SetujMatricu");
            klijent.SetujMatricu(matricaA);

            Console.WriteLine("Poziv: PreuzmiMatricu");
            double[][] preuzeta = klijent.PreuzmiMatricu();
            PrikaziMatricu(preuzeta);

            double[][] matricaB = new double[][]
            {
                new double[] { 10.0, 20.0, 30.0 },
                new double[] { 40.0, 50.0, 60.0 }
            };
            Console.WriteLine("Poziv: Sabiranje");
            OperacijaRezultat rezSabiranja = klijent.Sabiranje(matricaB);
            PrikaziRezultat(rezSabiranja);

            Console.WriteLine("Poziv: MnozenjeSkalarom");
            OperacijaRezultat rezSkalar = klijent.MnozenjeSkalarom(2.0);
            PrikaziRezultat(rezSkalar);

            double[][] matricaC = new double[][]
            {
                new double[] { 1.0, 2.0 },
                new double[] { 3.0, 4.0 },
                new double[] { 5.0, 6.0 }
            };
            Console.WriteLine("Poziv: MnozenjeMatricom");
            OperacijaRezultat rezMnozenja = klijent.MnozenjeMatricom(matricaC);
            PrikaziRezultat(rezMnozenja);

            Console.WriteLine("Poziv: Transponovanje");
            OperacijaRezultat rezTransponovanja = klijent.Transponovanje();
            PrikaziRezultat(rezTransponovanja);

            ((IClientChannel)klijent).Close();
            factory.Close();
            host.Close();

            Console.ReadKey();
        }

        static void PrikaziRezultat(OperacijaRezultat rez)
        {
            if (rez.IsGreska)
            {
                Console.WriteLine("Greska: " + rez.OpisGreske);
            }
            else
            {
                PrikaziMatricu(rez.RezultatMatrica);
            }
        }

        static void PrikaziMatricu(double[][] m)
        {
            if (m == null) return;
            for (int i = 0; i < m.Length; i++)
            {
                for (int j = 0; j < m[i].Length; j++)
                {
                    Console.Write(m[i][j] + "\t");
                }
                Console.WriteLine();
            }
        }
    }
}
