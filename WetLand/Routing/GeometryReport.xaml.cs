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

namespace WetLand.Routing
{
    /// <summary>
    /// Interaction logic for GeometryReport.xaml
    /// </summary>
    using Microsoft.Win32;
    using OxyPlot;
    using OxyPlot.Xps;
    using OxyPlot.Series;
    using OxyPlot.Axes;
    using System.IO;
    using System.Diagnostics;
    public partial class GeometryReport : Window
    {
        private string[] ytitle = { "Area (m²)", "Volume (m³)", "Outflow (m³/day)" };
        private string[] lengend = { "Area", "Volume", "Outflow" };
        public GeometryReport()
        {
            InitializeComponent();
            setModel();
        }
        private void setModel()
        {
            try
            {
                ReportModelView model = new ReportModelView();
                if (reportIndex != null && report != null)
                {
                    report.Model = model.CreateModel(1, reportIndex.SelectedIndex, reportIndex.Text, lengend[reportIndex.SelectedIndex], ytitle[reportIndex.SelectedIndex]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void reportIndex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            setModel();
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

        private void menu_pdf_Click (object sender, RoutedEventArgs e)
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
    }
}
