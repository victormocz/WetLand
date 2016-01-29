using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WetLand.KSTest
{
    using MathNet.Numerics.Distributions;
    class Data14
    {
        //L2	theta	Is	fNup	kd	kep	kga0	kgb0	kmin1s	knw	kminw	kns	kden	
        //rowp	vels_o	vels_s  	velb	ana	rChl	Ss	Sw	c_Uw	frap	
        //c1	c2	PH	S	Kw	apa	Dnp	Ksa	Ksb	RanN1	fW	fact  	a_vr_o	a_vr_s	porw
        public List<double> l2;
        public List<double> theta;
        public List<double> Is;
        public List<double> fnup;
        public List<double> kd;
        public List<double> kep;
        public List<double> kga0;
        public List<double> kgb0;
        public List<double> kmin1s;
        public List<double> knw;
        public List<double> kminw;
        public List<double> kns;
        public List<double> kden;
        public List<double> rowp;
        public List<double> vels_o;
        public List<double> vels_s;
        public List<double> velb;
        public List<Double> ana;
        public List<Double> rchl;
        public List<Double> ss;
        public List<Double> sw;
        public List<Double> c_uw;
        public List<Double> frap;
        public List<Double> c1;
        public List<Double> c2;
        public List<Double> ph;
        public List<Double> s;
        public List<Double> kw;
        public List<Double> apa;
        public List<Double> dnp;
        public List<Double> ksa;
        public List<Double> ksb;
        public List<Double> rann1;
        public List<Double> fw;
        public List<Double> fact;
        public List<Double> a_vr_o;
        public List<Double> a_vr_s;
        public List<Double> porw;

        public Data14() {
            l2 = new List<double>();
            theta= new List<double>();
            Is= new List<double>();
            fnup= new List<double>();
            kd= new List<double>();
            kep= new List<double>();
            kga0= new List<double>();
            kgb0= new List<double>();
            kmin1s= new List<double>();
            knw= new List<double>();
            kminw= new List<double>();
            kns= new List<double>();
            kden= new List<double>();
            rowp= new List<double>();
            vels_o= new List<double>();
            vels_s= new List<double>();
            velb= new List<double>();
            ana= new List<double>();
            rchl= new List<double>();
            ss= new List<double>();
            sw= new List<double>();
            c_uw= new List<double>();
            frap= new List<double>();
            c1= new List<double>();
            c2= new List<double>();
            ph= new List<double>();
            s= new List<double>();
            kw= new List<double>();
            apa= new List<double>();
            dnp= new List<double>();
            ksa= new List<double>();
            ksb= new List<double>();
            rann1= new List<double>();
            fw= new List<double>();
            fact= new List<double>();
            a_vr_o= new List<double>();
            a_vr_s= new List<double>();
            porw= new List<double>();
        }
        public void addOneRowData(string data)
        {
            string[] parameters = data.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
            l2.Add(Convert.ToDouble(parameters[0]));
            theta.Add(Convert.ToDouble(parameters[1]));
            Is.Add(Convert.ToDouble(parameters[2]));
            fnup.Add(Convert.ToDouble(parameters[3]));
            kd.Add(Convert.ToDouble(parameters[4]));
            kep.Add(Convert.ToDouble(parameters[5]));
            kga0.Add(Convert.ToDouble(parameters[6]));
            kgb0.Add(Convert.ToDouble(parameters[7]));
            kmin1s.Add(Convert.ToDouble(parameters[8]));
            knw.Add(Convert.ToDouble(parameters[9]));
            kminw.Add(Convert.ToDouble(parameters[10]));
            kns.Add(Convert.ToDouble(parameters[11]));
            kden.Add(Convert.ToDouble(parameters[12]));
            rowp.Add(Convert.ToDouble(parameters[13]));
            vels_o.Add(Convert.ToDouble(parameters[14]));
            vels_s.Add(Convert.ToDouble(parameters[15]));
            velb.Add(Convert.ToDouble(parameters[16]));
            ana.Add(Convert.ToDouble(parameters[17]));
            rchl.Add(Convert.ToDouble(parameters[18]));
            ss.Add(Convert.ToDouble(parameters[19]));
            sw.Add(Convert.ToDouble(parameters[20]));
            c_uw.Add(Convert.ToDouble(parameters[21]));
            frap.Add(Convert.ToDouble(parameters[22]));
            c1.Add(Convert.ToDouble(parameters[23]));
            c2.Add(Convert.ToDouble(parameters[24]));
            ph.Add(Convert.ToDouble(parameters[25]));
            s.Add(Convert.ToDouble(parameters[26]));
            kw.Add(Convert.ToDouble(parameters[27]));
            apa.Add(Convert.ToDouble(parameters[28]));
            dnp.Add(Convert.ToDouble(parameters[29]));
            ksa.Add(Convert.ToDouble(parameters[30]));
            ksb.Add(Convert.ToDouble(parameters[31]));
            rann1.Add(Convert.ToDouble(parameters[32]));
            fw.Add(Convert.ToDouble(parameters[33]));
            fact.Add(Convert.ToDouble(parameters[34]));
            a_vr_o.Add(Convert.ToDouble(parameters[35]));
            a_vr_s.Add(Convert.ToDouble(parameters[36]));
            porw.Add(Convert.ToDouble(parameters[37]));
        }
        public void insertOneRowData(double[] parameters) {
            l2.Add(parameters[0]);
            theta.Add(parameters[1]);
            Is.Add(parameters[2]);
            fnup.Add(parameters[3]);
            kd.Add(parameters[4]);
            kep.Add(parameters[5]);
            kga0.Add(parameters[6]);
            kgb0.Add(parameters[7]);
            kmin1s.Add(parameters[8]);
            knw.Add(parameters[9]);
            kminw.Add(parameters[10]);
            kns.Add(parameters[11]);
            kden.Add(parameters[12]);
            rowp.Add(parameters[13]);
            vels_o.Add(parameters[14]);
            vels_s.Add(parameters[15]);
            velb.Add(parameters[16]);
            ana.Add(parameters[17]);
            rchl.Add(parameters[18]);
            ss.Add(parameters[19]);
            sw.Add(parameters[20]);
            c_uw.Add(parameters[21]);
            frap.Add(parameters[22]);
            c1.Add(parameters[23]);
            c2.Add(parameters[24]);
            ph.Add(parameters[25]);
            s.Add(parameters[26]);
            kw.Add(parameters[27]);
            apa.Add(parameters[28]);
            dnp.Add(parameters[29]);
            ksa.Add(parameters[30]);
            ksb.Add(parameters[31]);
            rann1.Add(parameters[32]);
            fw.Add(parameters[33]);
            fact.Add(parameters[34]);
            a_vr_o.Add(parameters[35]);
            a_vr_s.Add(parameters[36]);
            porw.Add(parameters[37]);
        }
        public void sortDescending() {
            l2 = l2.OrderByDescending(i => i).ToList();
            theta = theta.OrderByDescending(i => i).ToList();
            Is = Is.OrderByDescending(i => i).ToList();
            fnup = fnup.OrderByDescending(i => i).ToList();
            kd = kd.OrderByDescending(i => i).ToList();
            kep = kep.OrderByDescending(i => i).ToList();
            kga0 = kga0.OrderByDescending(i => i).ToList();
            kgb0 = kgb0.OrderByDescending(i => i).ToList();
            kmin1s = kmin1s.OrderByDescending(i => i).ToList();
            knw = knw.OrderByDescending(i => i).ToList();
            kminw = kminw.OrderByDescending(i => i).ToList();
            kns = kns.OrderByDescending(i => i).ToList();
            kden = kden.OrderByDescending(i => i).ToList();
            rowp = rowp.OrderByDescending(i => i).ToList();
            vels_o = vels_o.OrderByDescending(i => i).ToList();
            vels_s = vels_s.OrderByDescending(i => i).ToList();
            velb = velb.OrderByDescending(i => i).ToList();
            ana = ana.OrderByDescending(i => i).ToList();
            rchl = rchl.OrderByDescending(i => i).ToList();
            ss = ss.OrderByDescending(i => i).ToList();
            sw = sw.OrderByDescending(i => i).ToList();
            c_uw = c_uw.OrderByDescending(i => i).ToList();
            frap = frap.OrderByDescending(i => i).ToList();
            c1 = c1.OrderByDescending(i => i).ToList();
            c2 = c2.OrderByDescending(i => i).ToList();
            ph = ph.OrderByDescending(i => i).ToList();
            s = s.OrderByDescending(i => i).ToList();
            kw = kw.OrderByDescending(i => i).ToList();
            apa = apa.OrderByDescending(i => i).ToList();
            dnp = dnp.OrderByDescending(i => i).ToList();
            ksa = ksa.OrderByDescending(i => i).ToList();
            ksb = ksb.OrderByDescending(i => i).ToList();
            rann1 = rann1.OrderByDescending(i => i).ToList();
            fw = fw.OrderByDescending(i => i).ToList();
            fact = fact.OrderByDescending(i => i).ToList();
            a_vr_o = a_vr_o.OrderByDescending(i => i).ToList();
            a_vr_s = a_vr_s.OrderByDescending(i => i).ToList();
            porw = porw.OrderByDescending(i => i).ToList();
        }
        public void sort() {
            l2.Sort();
            theta.Sort();
            Is.Sort();
            fnup.Sort();
            kd.Sort();
            kep.Sort();
            kga0.Sort();
            kgb0.Sort();
            kmin1s.Sort();
            knw.Sort();
            kminw.Sort();
            kns.Sort();
            kden.Sort();
            rowp.Sort();
            vels_o.Sort();
            vels_s.Sort();
            velb.Sort();
            ana.Sort();
            rchl.Sort();
            ss.Sort();
            sw.Sort();
            c_uw.Sort();
            frap.Sort();
            c1.Sort();
            c2.Sort();
            ph.Sort();
            s.Sort();
            kw.Sort();
            apa.Sort();
            dnp.Sort();
            ksa.Sort();
            ksb.Sort();
            rann1.Sort();
            fw.Sort();
            fact.Sort();
            a_vr_o.Sort();
            a_vr_s.Sort();
            porw.Sort();
        }
        public void cdf(int type,List<double> parameter,double a,double b) {
            if (type == 0)//Uniform
            {
                for (int i = 0; i < parameter.Count; i++)
                {
                    parameter[i] = ContinuousUniform.CDF(a, b, parameter[i]);
                }
            }
            else if (type == 1)//Lognormal
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
            else if (type == 2) {
                double mu = a;
                double sigma = b;
                for (int i = 0; i < parameter.Count; i++)
                {
                    parameter[i] = LogNormal.CDF(mu, sigma, parameter[i]);
                }
            }
        }

        public void cdf(int type, List<double> parameter, double a, double b,double c)
        {
            if (type == 0)
            {
                for (int i = 0; i < parameter.Count; i++)
                {
                    parameter[i] = ContinuousUniform.CDF(a, b, parameter[i]);
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
            else if (type == 3) {
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
