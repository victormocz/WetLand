using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WetLand.PostProcess
{
    using System.Windows;
    using System.IO;
    using System.Text.RegularExpressions;

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

                //string simStr = "";
                int simNum=0;

                StringBuilder simStr = new StringBuilder();
                foreach (var line in File.ReadLines(filePath))
                {
                    string[] para = line.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                    if (para.Length == 1 && Regex.IsMatch(para[0], @"^\d+$")) {
                        simStr.Clear();
                        simNum = Convert.ToInt32(para[0]);
                    }
                    else
                    {
                        simStr.Append(line);
                    }
                    string[] parameter = simStr.ToString().Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                    if (parameter.Length == Global.nofDays && !simStr.ToString().Contains("*"))
                    {
                        double[] inputSimulationSeries = new double[parameter.Length];
                        for (int i = 0; i < parameter.Length; i++)
                        {
                            inputSimulationSeries[i] = Convert.ToDouble(parameter[i]);
                        }
                        if (observedCount == 0)
                        {
                            simulationSeries.Add(new SimulationData(simNum, inputSimulationSeries));
                        }
                        else
                        {
                            simulationSeries.Add(new SimulationData(simNum, inputSimulationSeries, observedValue));
                        }
                    }


                    //if (count == 0)
                    //{
                    //    simNum = Convert.ToInt32(line);
                    //    count++;
                    //}
                    //else if (count == 3)
                    //{
                    //    simStr += line;
                    //    string[] stringValue = simStr.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                    //    double[] inputSimulationSeries = new double[stringValue.Length];
                    //    if (simStr.Contains("*") || stringValue.Length != Global.nofDays)
                    //    {
                    //        count = 0;
                    //        simStr = "";
                    //        continue;
                    //    }
                    //    for (int i = 0; i < stringValue.Length; i++)
                    //    {
                    //        inputSimulationSeries[i] = Convert.ToDouble(stringValue[i]);
                    //    }
                    //    if (observedCount == 0)
                    //    {
                    //        simulationSeries.Add(new SimulationData(simNum, inputSimulationSeries));
                    //    }
                    //    else
                    //    {
                    //        simulationSeries.Add(new SimulationData(simNum, inputSimulationSeries, observedValue));
                    //    }
                    //    count = 0;
                    //    simStr = "";

                    //}
                    //else
                    //{
                    //    simStr += line + "\t";
                    //    count++;
                    //}
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
