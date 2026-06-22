using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace Skladiste
{
    public class Skladista : ISkladista
    {
        public void Zakupi(Vlasnik vlasnik, Skladiste skladiste)
        {
            Repository.Instance.Zakupi.Add(new Zakup
            {
                Vlasnik = vlasnik,
                Skladiste = skladiste
            });
        }

        // pomocna, jel zakup traje jos uvek
        private bool JeAktivan(Zakup z)
        {
            DateTime danas = DateTime.Now;
            return z.Skladiste.PocetakZakupa <= danas && danas <= z.Skladiste.KrajZakupa;
        }

        public IList<Skladiste> AktivnaSkladistaVlasnika(string jmbg)
        {
            return Repository.Instance.Zakupi
                .Where(z => z.Vlasnik.Jmbg == jmbg && JeAktivan(z))
                .Select(z => z.Skladiste)
                .ToList();
        }

        public IList<Vlasnik> VlasniciAktivnihSkladista()
        {
            return Repository.Instance.Zakupi
                .Where(z => JeAktivan(z))
                .Select(z => z.Vlasnik)
                .ToList();
        }

        public IList<Zakup> SvaSkladistaSaIstorijom()
        {
            return Repository.Instance.Zakupi;
        }
    }
}
