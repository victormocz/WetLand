using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WetLand.Routing
{
    using System.IO;
    class RoutingData
    {
        public int n;
        public double deltat;
        public int rows;
        public double hinitial;
        public static string filePath = Global.projectName + @"\InputFiles\Routing\1_inputs.txt";
        private RoutingMainWindow routingWin;
        public RoutingData(RoutingMainWindow routingWin)
        {
            this.routingWin = routingWin;
            n = 198;
            deltat = 1;
            rows = 12;
            hinitial = 0.42;
        }
        public void readData(string filePath)
        {
            string[] content = File.ReadAllLines(filePath);
            if (content.Length < 2)
            {
                throw new Exception(filePath + " is not correct.");
            }
            string[] parameters = content[1].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
            if (parameters.Length != 4)
            {
                throw new Exception(filePath + "'s format is not correct, We have four parameters.");
            }
            n = Convert.ToInt32(parameters[0]);
            deltat = Convert.ToDouble(parameters[1]);
            rows = Convert.ToInt32(parameters[2]);
            hinitial = Convert.ToDouble(parameters[3]);
        }
        public void putValue()
        {
            routingWin.hinitial.Text = this.hinitial.ToString();
        }
        public void getValue()
        {
            this.hinitial = Convert.ToDouble(routingWin.hinitial.Text);
        }
        public void saveFile()
        {
            string[] content = new string[2];
            content[0] = "n	DeltaT	rows	Hinitial(m)";
            content[1] = n.ToString() + "\t" + deltat.ToString() + "\t" + rows.ToString() + "\t" + hinitial.ToString();
            try
            {
                File.WriteAllLines(filePath, content);
                routingWin.status.Text = "Data saved successfully!";

            }
            catch (Exception ex)
            {
                routingWin.status.Text = ex.Message;
            }

        }
    }
}
