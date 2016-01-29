using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WetLand
{
    public static class Global
    {
        public static string projectName;
        public static string currentDir;
        public static int nofDays;
        public static DateTime startDate;
        public static string simulationNumber;
        public static bool DeterminMode;
        public static MainWindow mainWin;
        public static List<ModelParameters.Parameters> tempCarbon;
        public static List<ModelParameters.Parameters> tempNitrogen;
        public static List<ModelParameters.Parameters> Carbon;
        public static List<ModelParameters.Parameters> Nitrogen;
        public static double percentage;

    }
}
