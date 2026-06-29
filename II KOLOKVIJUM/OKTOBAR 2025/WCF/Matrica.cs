using System;
using System.ServiceModel;

namespace WcfMatrice
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class MatricaService : IMatrica
    {
        private double[][] serverskaMatrica = null;

        public void SetujMatricu(double[][] matrica)
        {
            this.serverskaMatrica = matrica;
        }

        public double[][] PreuzmiMatricu()
        {
            return this.serverskaMatrica;
        }

        public OperacijaRezultat Sabiranje(double[][] drugaMatrica)
        {
            OperacijaRezultat rez = new OperacijaRezultat();

            if (serverskaMatrica == null || drugaMatrica == null)
            {
                rez.IsGreska = true;
                rez.OpisGreske = "Matrice nisu inicijalizovane.";
                return rez;
            }

            int r1 = serverskaMatrica.Length;
            int c1 = serverskaMatrica[0].Length;
            int r2 = drugaMatrica.Length;
            int c2 = drugaMatrica[0].Length;

            if (r1 != r2 || c1 != c2)
            {
                rez.IsGreska = true;
                rez.OpisGreske = "Dimenzije matrica se ne poklapaju.";
                return rez;
            }

            double[][] nova = InicijalizujMatricu(r1, c1);
            for (int i = 0; i < r1; i++)
            {
                for (int j = 0; j < c1; j++)
                {
                    nova[i][j] = serverskaMatrica[i][j] + drugaMatrica[i][j];
                }
            }

            rez.IsGreska = false;
            rez.RezultatMatrica = nova;
            return rez;
        }

        public OperacijaRezultat MnozenjeSkalarom(double skalar)
        {
            OperacijaRezultat rez = new OperacijaRezultat();

            if (serverskaMatrica == null)
            {
                rez.IsGreska = true;
                rez.OpisGreske = "Matrica na serveru nije setovana.";
                return rez;
            }

            int r = serverskaMatrica.Length;
            int c = serverskaMatrica[0].Length;
            double[][] nova = InicijalizujMatricu(r, c);

            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    nova[i][j] = serverskaMatrica[i][j] * skalar;
                }
            }

            rez.IsGreska = false;
            rez.RezultatMatrica = nova;
            return rez;
        }

        public OperacijaRezultat MnozenjeMatricom(double[][] drugaMatrica)
        {
            OperacijaRezultat rez = new OperacijaRezultat();

            if (serverskaMatrica == null || drugaMatrica == null)
            {
                rez.IsGreska = true;
                rez.OpisGreske = "Matrice nisu inicijalizovane.";
                return rez;
            }

            int r1 = serverskaMatrica.Length;
            int c1 = serverskaMatrica[0].Length;
            int r2 = drugaMatrica.Length;

            if (c1 != r2)
            {
                rez.IsGreska = true;
                rez.OpisGreske = "Matrice nisu kompatibilne za mnozenje.";
                return rez;
            }

            int c2 = drugaMatrica[0].Length;
            double[][] nova = InicijalizujMatricu(r1, c2);

            for (int i = 0; i < r1; i++)
            {
                for (int j = 0; j < c2; j++)
                {
                    nova[i][j] = 0;
                    for (int k = 0; k < c1; k++)
                    {
                        nova[i][j] += serverskaMatrica[i][k] * drugaMatrica[k][j];
                    }
                }
            }

            rez.IsGreska = false;
            rez.RezultatMatrica = nova;
            return rez;
        }

        public OperacijaRezultat Transponovanje()
        {
            OperacijaRezultat rez = new OperacijaRezultat();

            if (serverskaMatrica == null)
            {
                rez.IsGreska = true;
                rez.OpisGreske = "Matrica na serveru nije setovana.";
                return rez;
            }

            int r = serverskaMatrica.Length;
            int c = serverskaMatrica[0].Length;
            double[][] nova = InicijalizujMatricu(c, r);

            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    nova[j][i] = serverskaMatrica[i][j];
                }
            }

            rez.IsGreska = false;
            rez.RezultatMatrica = nova;
            return rez;
        }

        private double[][] InicijalizujMatricu(int rows, int cols)
        {
            double[][] m = new double[rows][];
            for (int i = 0; i < rows; i++)
            {
                m[i] = new double[cols];
            }
            return m;
        }
    }
}
