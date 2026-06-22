using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Kalkulator
{

    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IKalkulatorCallback))]

    public interface IKalkulator
    {
        [OperationContract]
        double Obrisi(); // resetuje rezultat na 0

        [OperationContract]
        double Dodaj(double n);

        [OperationContract]
        double Oduzmi(double n);

        [OperationContract]
        double Pomnozi(double n);

        [OperationContract]
        double Podeli(double n);
    }

    public interface IKalkulatorCallback
    {
        [OperationContract(IsOneWay = true)]
        void PrikaziIzraz(string izraz);
    }

}
