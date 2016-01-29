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

namespace WetLand.TimeDependentParameters
{
    /// <summary>
    /// Interaction logic for TimeDependentReport.xaml
    /// </summary>
    public partial class TimeDependentReport : Window
    {
        public string[] Lengend = { "ONin", "NO3in", "Nwin", "NO3g", "Ng", "Owin", "PO4in", "Pg", "mwin", "NH4air", "NO3air", "Qa", "Qn", "LPOCin", "RPOCin", "DOCin", "TOCin", "TOCatm", "TOCgw" };
        public string[] PTitle = { "Organic Nitrogen Concentration In Incoming Flow (mg/L)",
            "Nitrate-Nitrogen Concentration In Incoming Flow (mg/L)",
            "Total Ammonia-Nitrogen Concentration In Incoming Flow (mg/L)",
            "Nitrate-Nitrogen Concentration In Groundwater Discharge (mg/L)",
            "Total Ammonia-Nitrogen Concentration In Groundwater Discharge (mg/L)",
            "Oxygen Concentration In Incoming Flow (mg/L)",
            "Phosphate Concentration In Incoming Flow (mg/L)",
            "Total Phosphorus Concentration In Groundwater Discharge (mg/L)",
            "Sediment Concentration In Incoming Flow (mg/L)",
            "Ammonium Concentration In Precipitation (mg/L)",
            "Nitrate-Nitrogen Concentrations In Precipitation (mg/L)",
            "Dry Depositional Rates Of Total Ammonia Nitrogen (mg/m²/day)",
            "Dry Depositional Rates Of Total Nitrate-Nitrogen (mg/m²/day)",
            "Labile Particulate Organic Carbon Concentration In Incoming Flow (mg/L)",
            "Refractory Particulate Organic Carbon Concentration In Incoming Flow (mg/L)",
            "Dissolved Organic Carbon Concentration In Incoming Flow (mg/L)",
            "Atmospheric Deposition For Total Organic Carbon (mg/m²/day)",
            "Total Organic Carbon Concentration In Groundwater Discharge (mg/L)"
        };
        public string fileName;

        public TimeDependentReport()
        {
            InitializeComponent();
            fileName = Global.projectName + @"\InputFiles\13_time_dependent_parameters.txt";
            ReportModelView myModel = new ReportModelView();
            report.Model = myModel.CreateModel("Organic Nitrogen Concentration In Incoming Flow (mg/L)", "ONin", new DateTime(1995, 1, 1), 0, fileName);
        }

        private void reportIndex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (reportIndex == null|| report == null) {
                return;
            }
            ReportModelView myModel = new ReportModelView();
            report.Model = myModel.CreateModel(PTitle[reportIndex.SelectedIndex], Lengend[reportIndex.SelectedIndex], new DateTime(1995, 1, 1), reportIndex.SelectedIndex, fileName);
        }
    }
}
