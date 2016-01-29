using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WetLand.PostProcess
{
    using System.Windows;
    using System.IO;
    class PostProcessData
    {
        public List<SimulationData> simulationSeries;
        public double?[] observedValue;
        private int observedCount;
        
        public PostProcessData() {
            simulationSeries = new List<SimulationData>();
            observedCount = 0;
        }

        public int getObservedValue(string filePath)
        {
            observedValue = new double?[Global.nofDays];
            if (File.Exists(filePath))
            {
                string[] content = File.ReadAllLines(filePath);
                double tmpForConvert = 0.0;
                for (int i = 0; i < content.Length; i++)
                {
                    if (content[i].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries).Length != 2)
                    {
                        continue;
                    }
                    else
                    {
                        if (Double.TryParse(content[i].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries)[1], out tmpForConvert))
                        {
                            observedValue[i] = tmpForConvert;
                            observedCount++;
                        }
                        else
                        {
                            continue;
                        }
                    }

                }
                return observedCount;
            }
            else
            {
                return 0;
            }
        }

        public void getSimulationValue(string filePath) {
            if (!File.Exists(filePath))
            {
                throw new IOException("Cannot fine the file"+filePath);
            }
            else {
                int count = 0;
                string simStr = "";
                int simNum=0;
                foreach (var line in File.ReadLines(filePath))
                {
                    if (count == 0)
                    {
                        simNum = Convert.ToInt32(line);
                        count++;
                    }
                    else if (count == 3)
                    {
                        simStr += line;
                        string[] stringValue = simStr.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                        double[] inputSimulationSeries = new double[stringValue.Length];
                        if (simStr.Contains("*") || stringValue.Length != Global.nofDays)
                        {
                            count = 0;
                            simStr = "";
                            continue;
                        }
                        for (int i = 0; i < stringValue.Length; i++)
                        {
                            inputSimulationSeries[i] = Convert.ToDouble(stringValue[i]);
                        }
                        if (observedCount == 0)
                        {
                            simulationSeries.Add(new SimulationData(simNum, inputSimulationSeries));
                        }
                        else
                        {
                            simulationSeries.Add(new SimulationData(simNum, inputSimulationSeries, observedValue));
                        }
                        count = 0;
                        simStr = "";

                    }
                    else
                    {
                        simStr += line + "\t";
                        count++;
                    }
                }
            }
        }

        public void sort() {
            if (observedCount > 0)
            {
                simulationSeries.Sort();
            }
        }
    }
}
