using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WetLand.KSTest
{
    using MathNet.Numerics.Distributions;
    //aca	FaDOC	FaLPOC	FaRPOC	FbDOC	FbLPOC	FbRPOC	kLPOC	kRPOC	KsatO	KinO	KN	KinN	K1DOC	k2DOC	k3DOC	k4DOC	cp1	cp2	cp3	fbw	k1CH4	k2CH4	Rveg
    class Data15
    {
        public List<double> aca;
        public List<double> fadoc;
        public List<double> falpoc;
        public List<double> farpoc;
        public List<double> fbdoc;
        public List<double> fblpoc;
        public List<double> fbrpoc;
        public List<double> klpoc;
        public List<double> krpoc;
        public List<double> ksato;
        public List<double> kino;
        public List<double> kn;
        public List<double> kinn;
        public List<double> k1doc;
        public List<double> k2doc;
        public List<double> k3doc;
        public List<double> k4doc;
        public List<double> cp1;
        public List<double> cp2;
        public List<double> cp3;
        public List<double> fbw;
        public List<double> k1ch4;
        public List<double> k2ch4;
        public List<double> rveg;
        public Data15() {
            aca = new List<double>();
            fadoc = new List<double>();
            falpoc = new List<double>();
            farpoc = new List<double>();
            fbdoc = new List<double>();
            fblpoc = new List<double>();
            fbrpoc = new List<double>();
            klpoc = new List<double>();
            krpoc = new List<double>();
            ksato = new List<double>();
            kino = new List<double>();
            kn = new List<double>();
            kinn = new List<double>();
            k1doc = new List<double>();
            k2doc = new List<double>();
            k3doc = new List<double>();
            k4doc = new List<double>();
            cp1 = new List<double>();
            cp2 = new List<double>();
            cp3 = new List<double>();
            fbw = new List<double>();
            k1ch4 = new List<double>();
            k2ch4 = new List<double>();
            rveg = new List<double>();

        }
        public void addOneRowData(string data) {
            string[] parameters = data.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
            aca.Add(Convert.ToDouble(parameters[0]));
            fadoc.Add(Convert.ToDouble(parameters[1]));
            falpoc.Add(Convert.ToDouble(parameters[2]));
            farpoc.Add(Convert.ToDouble(parameters[3]));
            fbdoc.Add(Convert.ToDouble(parameters[4]));
            fblpoc.Add(Convert.ToDouble(parameters[5]));
            fbrpoc.Add(Convert.ToDouble(parameters[6]));
            klpoc.Add(Convert.ToDouble(parameters[7]));
            krpoc.Add(Convert.ToDouble(parameters[8]));
            ksato.Add(Convert.ToDouble(parameters[9]));
            kino.Add(Convert.ToDouble(parameters[10]));
            kn.Add(Convert.ToDouble(parameters[11]));
            kinn.Add(Convert.ToDouble(parameters[12]));
            k1doc.Add(Convert.ToDouble(parameters[13]));
            k2doc.Add(Convert.ToDouble(parameters[14]));
            k3doc.Add(Convert.ToDouble(parameters[15]));
            k4doc.Add(Convert.ToDouble(parameters[16]));
            cp1.Add(Convert.ToDouble(parameters[17]));
            cp2.Add(Convert.ToDouble(parameters[18]));
            cp3.Add(Convert.ToDouble(parameters[19]));
            fbw.Add(Convert.ToDouble(parameters[20]));
            k1ch4.Add(Convert.ToDouble(parameters[21]));
            k2ch4.Add(Convert.ToDouble(parameters[22]));
            rveg.Add(Convert.ToDouble(parameters[23]));
        }
        public void insertOneRowData(double[] parameters)
        {
            aca.Add(parameters[0]);
            fadoc.Add(parameters[1]);
            falpoc.Add(parameters[2]);
            farpoc.Add(parameters[3]);
            fbdoc.Add(parameters[4]);
            fblpoc.Add(parameters[5]);
            fbrpoc.Add(parameters[6]);
            klpoc.Add(parameters[7]);
            krpoc.Add(parameters[8]);
            ksato.Add(parameters[9]);
            kino.Add(parameters[10]);
            kn.Add(parameters[11]);
            kinn.Add(parameters[12]);
            k1doc.Add(parameters[13]);
            k2doc.Add(parameters[14]);
            k3doc.Add(parameters[15]);
            k4doc.Add(parameters[16]);
            cp1.Add(parameters[17]);
            cp2.Add(parameters[18]);
            cp3.Add(parameters[19]);
            fbw.Add(parameters[20]);
            k1ch4.Add(parameters[21]);
            k2ch4.Add(parameters[22]);
            rveg.Add(parameters[23]);
        }
        public void sort() {
            aca.Sort();
            fadoc.Sort();
            falpoc.Sort();
            farpoc.Sort();
            fbdoc.Sort();
            fblpoc.Sort();
            fbrpoc.Sort();
            klpoc.Sort();
            krpoc.Sort();
            ksato.Sort();
            kino.Sort();
            kn.Sort();
            kinn.Sort();
            k1doc.Sort();
            k2doc.Sort();
            k3doc.Sort();
            k4doc.Sort();
            cp1.Sort();
            cp2.Sort();
            cp3.Sort();
            fbw.Sort();
            k1ch4.Sort();
            k2ch4.Sort();
            rveg.Sort();
        }
        public void sortDescinding() {
            aca = aca.OrderByDescending(i => i).ToList();
            fadoc = fadoc.OrderByDescending(i => i).ToList();
            falpoc = falpoc.OrderByDescending(i => i).ToList();
            farpoc = farpoc.OrderByDescending(i => i).ToList();
            fbdoc = fbdoc.OrderByDescending(i => i).ToList();
            fblpoc = fblpoc.OrderByDescending(i => i).ToList();
            fbrpoc = fbrpoc.OrderByDescending(i => i).ToList();
            klpoc = klpoc.OrderByDescending(i => i).ToList();
            krpoc = krpoc.OrderByDescending(i => i).ToList();
            ksato = ksato.OrderByDescending(i => i).ToList();
            kino = kino.OrderByDescending(i => i).ToList();
            kn = kn.OrderByDescending(i => i).ToList();
            kinn = kinn.OrderByDescending(i => i).ToList();
            k1doc = k1doc.OrderByDescending(i => i).ToList();
            k2doc = k2doc.OrderByDescending(i => i).ToList();
            k3doc = k3doc.OrderByDescending(i => i).ToList();
            k4doc = k4doc.OrderByDescending(i => i).ToList();
            cp1 = cp1.OrderByDescending(i => i).ToList();
            cp2 = cp2.OrderByDescending(i => i).ToList();
            cp3 = cp3.OrderByDescending(i => i).ToList();
            fbw = fbw.OrderByDescending(i => i).ToList();
            k1ch4 = k1ch4.OrderByDescending(i => i).ToList();
            k2ch4 = k2ch4.OrderByDescending(i => i).ToList();
            rveg = rveg.OrderByDescending(i => i).ToList();
        }

        public void cdf(int type, List<double> parameter, double a, double b)
        {
            if (type == 0)
            {
                double numerator = b - a;
                for (int i = 0; i < parameter.Count; i++)
                {
                    parameter[i] = (parameter[i] - a) / numerator;
                }
            }
            else if (type == 1)
            {
                double Xl = a;
                double Xu = b;
                double Finv_Rl = Normal.InvCDF(0, 1, 0.1 / 100);
                double Finv_Ru = Normal.InvCDF(0, 1, 99.9 / 100);
                double mu = (Math.Log(Xu) * Finv_Rl - Math.Log(Xl) * Finv_Ru) / (Finv_Rl - Finv_Ru);
                double sigma = Math.Log(Xu / Xl) / (Finv_Ru - Finv_Rl);
                for (int i = 0; i < parameter.Count; i++)
                {
                    parameter[i] = LogNormal.CDF(mu, sigma, parameter[i]);
                }
            }
            else if (type == 2)
            {
                double mu = a;
                double sigma = b;
                for (int i = 0; i < parameter.Count; i++)
                {
                    parameter[i] = LogNormal.CDF(mu, sigma, parameter[i]);
                }
            }
        }

        public void cdf(int type, List<double> parameter, double a, double b, double c)
        {
            if (type == 0)
            {
                double numerator = b - a;
                for (int i = 0; i < parameter.Count; i++)
                {
                    parameter[i] = (parameter[i] - a) / numerator;
                }
            }
            else if (type == 1)
            {
                double Xl = a;
                double Xu = b;
                double Finv_Rl = Normal.InvCDF(0, 1, 0.1 / 100);
                double Finv_Ru = Normal.InvCDF(0, 1, 99.9 / 100);
                double mu = (Math.Log(Xu) * Finv_Rl - Math.Log(Xl) * Finv_Ru) / (Finv_Rl - Finv_Ru);
                double sigma = Math.Log(Xu / Xl) / (Finv_Ru - Finv_Rl);
                for (int i = 0; i < parameter.Count; i++)
                {
                    parameter[i] = LogNormal.CDF(mu, sigma, parameter[i]);
                }
            }
            else if (type == 2)
            {
                double mu = a;
                double sigma = b;
                for (int i = 0; i < parameter.Count; i++)
                {
                    parameter[i] = LogNormal.CDF(mu, sigma, parameter[i]);
                }
            }
            else if (type == 3)
            {
                for (int i = 0; i < parameter.Count; i++)
                {
                    parameter[i] = Triangular.CDF(a, c, b, parameter[i]);
                }
            }
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
