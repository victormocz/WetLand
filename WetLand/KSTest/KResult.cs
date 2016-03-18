using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WetLand.KSTest
{
    using Accord.Statistics.Testing;
    public class KResult : IComparable<KResult>
    {
        public string name;
        public double Dmax;
        public double ks2stat;
        public double p;
        public List<double> Interp_BData;
        public double[] NBData;
        public KResult(string name,List<double> listB,List<double> originalList1, List<Double> originalList2, List<Double> sortedList1, List<Double> sortedList2) {
            if (originalList1.Count == 0 || originalList2.Count == 0 || sortedList1.Count == 0 || sortedList2.Count == 0) {
                throw new Exception("Length of Interp_B and NB dataset Should greater than 0");
            }
            this.Dmax = -1;
            for (int i = 0; i < sortedList1.Count; i++)
            {
                double difference = Math.Abs(sortedList1[i] - sortedList2[i]);
                if (this.Dmax < difference)
                {
                    this.Dmax = difference;
                }
            }
            this.name = name;
            this.Interp_BData = listB;
            //TODO debug
            double[] originalArray1 = originalList1.ToArray();
            double[] originalArray2 = originalList2.ToArray();
            TwoSampleKolmogorovSmirnovTest ktest = new TwoSampleKolmogorovSmirnovTest(originalArray1, originalArray2);
            Array.Sort(originalArray2);
            this.NBData = originalArray2;
            this.ks2stat = ktest.Statistic;
            this.p = ktest.PValue;
            
            
        }
        public int CompareTo(KResult a)
        {
            return a.Dmax.CompareTo(Dmax);
        }
    }
}
