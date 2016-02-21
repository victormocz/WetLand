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
    using Microsoft.Win32;
    using OxyPlot;
    using OxyPlot.Xps;
    using OxyPlot.Series;
    using OxyPlot.Axes;
    using System.IO;
    using System.Diagnostics;
    public partial class HydrolicParametersReport : Window
    {
        private string[] Lengend = { "Qin", "QOut","Vw","Area","ET","ip","H","Qg","Uw","temp"};
        private string[] PTitle = { "Volumetric inflow rate (m³/day)", "Wetland discharge (outflow) rate (m³/day)", "Water Volume of Wetland Surface Water (m³)",
            "Wetland Surface Area (m²)", "Evapotranspiration Rate (cm/day)", "Precipitation Rate (cm/day)",
            "Average Depth of Water In Wetland (m)", "Groundwater Discharge (m³/day)", "Wind speed (m/sec)", "Daily Temperature (°C)" };
        private string[] ytitle = { "Qin (m³/day)", "QOut(m³/day)", "Vw (m³)", "Area (m²)", "ET (cm/day)", "ip (cm)", "H (m)", "Qg (m³/day)", "Uw (m/sec)", "temp (°C)" };
        private string filename;

        public HydrolicParametersReport()
        {
            InitializeComponent();
            ReportModelView myModel = new ReportModelView();
            filename = Global.projectName + @"\InputFiles\12_hydro_climate.txt";
            report.Model = myModel.CreateModel("Qin(m³/day)","Qin", 0, filename,ytitle[0]);
        }

        private void reportIndex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (reportIndex == null || report==null) {
                return;
            }
            ReportModelView myModel = new ReportModelView();
            report.Model = myModel.CreateModel(PTitle[reportIndex.SelectedIndex], Lengend[reportIndex.SelectedIndex], reportIndex.SelectedIndex, filename,ytitle[reportIndex.SelectedIndex]);
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
