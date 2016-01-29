using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WetLand.PostProcess
{
    class SimulationData : IComparable<SimulationData>
    {
        public int simulationNumber;
        public double[] daysData;
        public double likelihood;
        public double Ens;
        public double MBE;
        public double PBIAS;

        private double avgObservedValue;
        private double denominator;
        private double totalObservedValue;
        public SimulationData(int simulationNumber,double[] daysData,double?[] observedValue)
        {
            this.simulationNumber = simulationNumber;
            this.daysData = daysData;

            if (observedValue == null)
            {
                this.avgObservedValue = 0.0;
                this.denominator = 0.0;
            }
            else
            {
                double totalOfObs = 0.0;
                double totalOfdenominator = 0.0;
                int count = 0;
                for (int i = 0; i < observedValue.Length; i++)
                {
                    if (observedValue[i].HasValue)
                    {
                        totalOfObs += observedValue[i].Value;
                        totalOfdenominator += (observedValue[i].Value - avgObservedValue) * (observedValue[i].Value - avgObservedValue);
                        count++;
                    }
                }
                this.totalObservedValue = totalOfObs;
                this.avgObservedValue = totalOfObs / count;
                this.denominator = totalOfdenominator;
            }
            caculate(observedValue);
        }

        public SimulationData(int simulationNumber, double[] daysData)
        {
            this.simulationNumber = simulationNumber;
            this.daysData = daysData;
        }


        public double TotalOfSimulation
        {
            get
            {
                double total = 0.0;
                if (daysData == null)
                {
                    return 0.0;
                }
                else
                {
                    for (int i = 0; i < daysData.Length; i++)
                    {
                        total += daysData[i];
                    }
                    return total;
                }
            }
        }


        public void caculate(double?[] observedValue) {
            double numerator = 0.0;
            double numeratorOfPBIAS = 0.0;
            for (int i = 0; i < daysData.Length; i++) {
                if (!observedValue[i].HasValue) {
                    continue;
                }
                else {
                    numerator += (daysData[i] - observedValue[i].Value) * (daysData[i] - observedValue[i].Value);
                    numeratorOfPBIAS += daysData[i] - observedValue[i].Value;
                }
            }
            if (totalObservedValue != 0 && denominator != 0)
            {
                Ens = 1.0 - numerator / denominator;
                MBE = (TotalOfSimulation - totalObservedValue) / totalObservedValue;
                likelihood = 0.5 * (Ens + Math.Exp(-Math.Abs(MBE / 100)));
                PBIAS = numeratorOfPBIAS*100 / totalObservedValue;
            }
            else
            {
                Ens = -1000;
                MBE = -1000;
                likelihood = -1000;
                PBIAS = 0;
            }
        }

        public int CompareTo(SimulationData a) {
            return a.likelihood.CompareTo(likelihood);
        }
    }
}
