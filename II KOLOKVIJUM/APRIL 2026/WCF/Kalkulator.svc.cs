using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.ServiceModel;
using Microsoft.SqlServer.Server;

namespace Kalkulator
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class Kalkulator : IKalkulator
    {
        double rezultat = 0.0;
        string izraz = "";

        // operacije
        public double Obrisi()
        {
            rezultat = 0.0;
            izraz = "";
            Callback.PrikaziIzraz(izraz);
            return rezultat;
        }

        public double Dodaj(double n)
        {
            rezultat += n;
            DodajUIzraz("+", n);
            Callback.PrikaziIzraz(izraz);
            return rezultat;
        }

        public double Oduzmi(double n)
        {
            rezultat -= n;
            DodajUIzraz("-", n);
            Callback.PrikaziIzraz(izraz);
            return rezultat;
        }

        public double Pomnozi(double n)
        {
            rezultat *= n;
            DodajUIzraz("*", n);
            Callback.PrikaziIzraz(izraz);
            return rezultat;
        }

        public double Podeli(double n)
        {
            rezultat /= n;
            DodajUIzraz("/", n);
            Callback.PrikaziIzraz(izraz);
            return rezultat;
        }

        void DodajUIzraz(string op, double n)
        {
            if (izraz == "")
            {
                izraz = n.ToString();
            } else
            {
                izraz += op + n.ToString();
            }
        }


        IKalkulatorCallback Callback
        {
            get { return OperationContext.Current.GetCallbackChannel<IKalkulatorCallback>(); }
        }
    }
}
