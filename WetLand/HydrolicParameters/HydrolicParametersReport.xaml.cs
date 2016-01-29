using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WetLand.HydrolicParameters
{
    /// <summary>
    /// Interaction logic for HydrolicParametersReport.xaml
    /// </summary>
    /// 
    using OxyPlot;
    using OxyPlot.Series;
    using OxyPlot.Axes;
    public partial class HydrolicParametersReport : Window
    {
        public string[] Lengend = { "Qin", "QOut","Vw","Area","ET","ip","H","Qg","Uw","temp"};
        public string[] PTitle = { "Qin(m³/day)", "QOut(m³/day)", "Water Volume of Wetland Surface Water (m³)",
            "Wetland Surface Area (m²)", "Evapotranspiration Rate (cm/day)", "Precipitation Rate (cm/day)",
            "Average Depth of Water In Wetland (m)", "Groundwater Discharge (m³/day)", "Wind speed (m/sec)", "Daily Temperature (°C)" };
        public string filename;

        public HydrolicParametersReport()
        {
            InitializeComponent();
            ReportModelView myModel = new ReportModelView();
            filename = Global.projectName + @"\InputFiles\12_hydro_climate.txt";
            report.Model = myModel.CreateModel("Qin(m³/day)","Qin", 0, filename);
        }

        private void reportIndex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (reportIndex == null || report==null) {
                return;
            }
            ReportModelView myModel = new ReportModelView();
            report.Model = myModel.CreateModel(PTitle[reportIndex.SelectedIndex], Lengend[reportIndex.SelectedIndex], reportIndex.SelectedIndex, filename);
        }
    }
}
