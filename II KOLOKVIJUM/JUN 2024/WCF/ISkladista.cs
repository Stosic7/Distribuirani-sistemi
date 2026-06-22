using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Skladiste
{
    [ServiceContract]
    public interface ISkladista
    {
        [OperationContract]
        void Zakupi(Vlasnik vlasnik, Skladiste skladiste);

        [OperationContract]
        IList<Skladiste> AktivnaSkladistaVlasnika(string jmbg);

        [OperationContract]
        IList<Vlasnik> VlasniciAktivnihSkladista();

        [OperationContract]
        IList<Zakup> SvaSkladistaSaIstorijom();
    }

    [DataContract]
    public class Vlasnik
    {
        [DataMember] public string Ime { get; set; }
        [DataMember] public string Prezime { get; set; }
        [DataMember] public string Jmbg { get; set; }
    }

    [DataContract]
    public class Skladiste
    {
        [DataMember] public int IdSkladista { get; set; }
        [DataMember] public DateTime PocetakZakupa { get; set; }
        [DataMember] public DateTime KrajZakupa { get; set; }
        [DataMember] public decimal Cena { get; set; }
    }

    [DataContract]
    public class Zakup
    {
        [DataMember] public Vlasnik Vlasnik { get; set; }
        [DataMember] public Skladiste Skladiste { get; set; }
    }
}
