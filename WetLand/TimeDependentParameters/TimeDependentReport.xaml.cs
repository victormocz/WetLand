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
    using Microsoft.Win32;
    using OxyPlot;
    using OxyPlot.Xps;
    using OxyPlot.Series;
    using OxyPlot.Axes;
    using System.IO;
    using System.Diagnostics;
    public partial class TimeDependentReport : Window
    {
        private string[] Lengend = { "H1","ONin", "NO3in", "Nwin", "NO3g", "Ng", "Owin",
            "PO4in", "Pg", "mwin", "NH4air", "NO3air", "Qa", "Qn", "LPOCin",
            "RPOCin", "DOCin", "TOCatm", "TOCgw" };
        private string[] PTitle = { "H1","Organic Nitrogen Concentration In Incoming Flow (mg/L)",
            "Nitrate-Nitrogen Concentration In Incoming Flow (mg/L)",
            "Total Ammonia-Nitrogen Concentration In Incoming Flow (mg/L)",
            "Nitrate-Nitrogen Concentration In Groundwater Discharge (mg/L)",
            "Total Ammonia-Nitrogen Concentration In Groundwater Discharge (mg/L)",
            "Oxygen Concentration In Incoming Flow (mg/L)",
            "Phosphate Concentration In Incoming Flow (mg/L)",//7
            "Total Phosphorus Concentration In Groundwater Discharge (mg/L)",
            "Sediment Concentration In Incoming Flow (mg/L)",
            "Ammonium Concentration In Precipitation (mg/L)",//10
            "Nitrate-Nitrogen Concentrations In Precipitation (mg/L)",
            "Dry Depositional Rates Of Total Ammonia Nitrogen (mg/m²/day)",
            "Dry Depositional Rates Of Total Nitrate-Nitrogen (mg/m²/day)",
            "Labile Particulate Organic Carbon Concentration In Incoming Flow (mg/L)",
            "Refractory Particulate Organic Carbon Concentration In Incoming Flow (mg/L)",//15
            "Dissolved Organic Carbon Concentration In Incoming Flow (mg/L)",
            "Atmospheric Deposition For Total Organic Carbon (mg/m²/day)",
            "Total Organic Carbon Concentration In Groundwater Discharge (mg/L)"
        };
        private string fileName;
        private string[] ytitle = { "H1 (C)","ONin (mg/L)", "NO3in (mg/L)", "Nwin (mg/L)", "NO3g (mg/L)",
            "Ng (mg/L)", "Owin (mg/L)", "PO4in (mg/L)", "Pg (mg/L)", "mwin (mg/L)", "NH4air (mg/L)",
            "NO3air (mg/L)", "Qa (mg/m²/day)", "Qn (mg/m²/day)", "LPOCin (mg/L)", "RPOCin (mg/L)",
            "DOCin (mg/L)", "TOCatm (mg/m²/day)", "TOCgw (mg/L)" };
        public TimeDependentReport()
        {
            InitializeComponent();
            fileName = Global.projectName + @"\InputFiles\13_time_dependent_parameters.txt";
            ReportModelView myModel = new ReportModelView();
            report.Model = myModel.CreateModel("Organic Nitrogen Concentration In Incoming Flow (mg/L)", "ONin", 0, fileName, ytitle[0]);
        }

        private void reportIndex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (reportIndex == null|| report == null) {
                return;
            }
            ReportModelView myModel = new ReportModelView();
            report.Model = myModel.CreateModel(PTitle[reportIndex.SelectedIndex], Lengend[reportIndex.SelectedIndex], reportIndex.SelectedIndex, fileName, ytitle[reportIndex.SelectedIndex]);
        }

        private void menu_pdf_Click(object sender, RoutedEventArgs e)
        {
            var path = this.GetFilename(".pdf files|*.pdf", ".pdf");
            if (path != null)
            {
                using (var stream = File.Create(path))
                {
                    OxyPlot.PdfExporter.Export(report.Model, stream, report.ActualWidth, report.ActualHeight);
                }

                OpenContainingFolder(path);
            }
        }
        private void menu_print_Click(object sender, RoutedEventArgs e)
        {
            XpsExporter.Print(report.Model, report.ActualWidth, report.ActualHeight);
        }
        private string GetFilename(string filter, string defaultExt)
        {
            var dlg = new SaveFileDialog { Filter = filter, DefaultExt = defaultExt };
            return dlg.ShowDialog(this.Owner).Value ? dlg.FileName : null;
        }
        private static void OpenContainingFolder(string fileName)
        {
            // var folder = Path.GetDirectoryName(fileName);
            var psi = new ProcessStartInfo("Explorer.exe", "/select," + fileName);
            Process.Start(psi);
        }
    }
}
