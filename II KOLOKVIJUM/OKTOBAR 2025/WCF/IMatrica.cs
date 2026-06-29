using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace WcfMatrice
{
    [DataContract]
    public class OperacijaRezultat
    {
        [DataMember]
        public bool IsGreska { get; set; }

        [DataMember]
        public string OpisGreske { get; set; }

        [DataMember]
        public double[][] RezultatMatrica { get; set; }
    }

    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IMatrica
    {
        [OperationContract]
        void SetujMatricu(double[][] matrica);

        [OperationContract]
        double[][] PreuzmiMatricu();

        [OperationContract]
        OperacijaRezultat Sabiranje(double[][] drugaMatrica);

        [OperationContract]
        OperacijaRezultat MnozenjeSkalarom(double skalar);

        [OperationContract]
        OperacijaRezultat MnozenjeMatricom(double[][] drugaMatrica);

        [OperationContract]
        OperacijaRezultat Transponovanje();
    }
}
