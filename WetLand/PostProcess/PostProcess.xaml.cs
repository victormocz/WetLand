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

namespace WetLand.PostProcess
{
    /// <summary>
    /// Interaction logic for PostProcess.xaml
    /// </summary>
    using System.Threading;
    using Microsoft.Win32;
    using OxyPlot;
    using OxyPlot.Xps;
    using OxyPlot.Series;
    using OxyPlot.Axes;
    using System.Diagnostics;
    using System.IO;
    public partial class PostProcess : Window
    {
        private string[] title = { "Particulate organic nitrogen concentration in free water (gr/cm³)", "Concentration of organic nitrogen in in anaerobic sediment layer (gr/cm³)", "Concentration of organic nitrogen in in aerobic sediment layer (gr/cm³)", "Total ammonia-nitrogen ([NH4+] + [NH3]) concentration in free water (gr/cm³)", "Total ammonia-nitrogen pore-water concentration in upper aerobic layer (gr/cm³)", "Total ammonia-nitrogen pore-water concentration in lower anaerobic layer (gr/cm³)", "Nitrate-nitrogen concentration in free water (gr/cm³)", "Nitrate-nitrogen pore-water concentration in upper aerobic layer (gr/cm³)", "Nitrate-nitrogen pore-water concentration in lower anaerobic layer (gr/cm³)", "Oxygen concentration in free water (gr/cm³)", "Mass of free floating plant (gr chlorophyll a)", "Mass of rooted plants (gr chlorophyll a)", "Total inorganic phosphorus concentration in free water (gr/cm³)", "Total phosphorus concentration in aerobic layer (gr/cm³)", "Total phosphorus concentration in anaerobic layer (gr/cm³)", "Sediment concentration in free water (gr/cm³)", "Concentrations of dissolved organic C in free water (gr/cm³)", "Concentrations of labile (fast reacting) particulate organic C in free water (gr/cm³)", "Concentrations of refractory (slow reacting) particulate organic C in free water (gr/cm³)", "Pore water concentrations of DOC in aerobic sediment layer (gr/cm³)", "Pore water concentrations of LPOC in aerobic sediment layer (gr/cm³)", "Pore water concentrations of RPOC in aerobic sediment layer (gr/cm³)", "Pore water concentrations of DOC in lower anaerobic sediment layer (gr/cm³)", "Pore water concentrations of LPOC in lower anaerobic sediment layer (gr/cm³)", "Pore water concentrations of RPOC in lower anaerobic sediment layer (gr/cm³)", "Concentrations of total organic C in free water (gr/cm³)", "Methane concentration in free water (gr/cm³)", "Methane concentration in aerobic sediment layer (gr/cm³)", "Methane concentration in anaerobic sediment layer (gr/cm³)" };
        private string[] ytitle = { "Onw (gr/cm³)", "Onsf (gr/cm³)", "Onss (gr/cm³)", "Nw (gr/cm³)", "Ns1 (gr/cm³)",
        "Ns2 (gr/cm³)","NO3w (gr/cm³)","NO3s1 (gr/cm³)","NO3s2 (gr/cm³)","Ow (gr/cm³)","a (gr chlorophyll a)",
        "b (gr chlorophyll a)","Pw (gr/cm³)","Ps1 (gr/cm³)","Ps2 (gr/cm³)","mw (gr/cm³)","Docw (gr/cm³)",
        "LPOCw (gr/cm³)","RPOCw (gr/cm³)","DOCs1 (gr/cm³)","LPOCs1 (gr/cm³)","RPOCs1 (gr/cm³)","DOCs2 (gr/cm³)",
        "LPOCs2 (gr/cm³)","RPOCs2 (gr/cm³)","TOCw (gr/cm³)","CH4w (gr/cm³)","CH4s1 (gr/cm³)","CH4s2 (gr/cm³)"};
        private ReportModelView myModel;
        public PostProcess()
        {
            InitializeComponent();
            try
            {
                //ReportModelView myModel = new ReportModelView();
                //report.Model = myModel.CreateModel(reportIndex.SelectedIndex,reportIndex.Text);
                percentage.Text = (Global.percentage * 100).ToString();
                if (Global.DeterminMode == true)
                {
                    percentPanel.Visibility = Visibility.Hidden;
                }
                else
                {
                    percentPanel.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void reportIndex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (reportIndex == null || report == null)
            {
                return;
            }
            if (reportIndex.SelectedIndex == 0)
            {
                menu_print.IsEnabled = false;
                menu_pdf.IsEnabled = false;
                table.IsEnabled = false;
                return;
            }
            else
            {
                menu_print.IsEnabled = true;
                menu_pdf.IsEnabled = true;
                table.IsEnabled = true;
            }
            Thread selection = new Thread(selectionThread);
            selection.Start();
        }

        private void selectionThread() {
            int index = 0;
            Dispatcher.Invoke(new Action(() =>
                index = reportIndex.SelectedIndex
                ));
            Dispatcher.Invoke(new Action(() =>
                reportIndex.IsEnabled = false
                ));
            Dispatcher.Invoke(new Action(() =>
               updateButton.Content = "Loading"
               ));
            Dispatcher.Invoke(new Action(() =>
                updateButton.IsEnabled = false
                ));
            if (reportIndex == null || report == null)
            {
                return;
            }
            try
            {
                if (index <= 0)
                {
                    return;
                }
                myModel = new ReportModelView();
                myModel.CreateModel(index - 1, title[index - 1], ytitle[index - 1]);
                Dispatcher.Invoke(new Action(() =>
                report.Model = myModel.MyModel
                ));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            Dispatcher.Invoke(new Action(() =>
                reportIndex.IsEnabled = true
                ));
            Dispatcher.Invoke(new Action(() =>
                updateButton.IsEnabled = true
                ));
            Dispatcher.Invoke(new Action(() =>
               updateButton.Content = "Update"
               ));
        }

        private void updateThread()
        {
            myModel = new ReportModelView();
            string pertext = "1";
            int index = 1;
            Dispatcher.Invoke(new Action(() =>
                pertext = percentage.Text
                ));
            Dispatcher.Invoke(new Action(() =>
                index = reportIndex.SelectedIndex
                ));
            Dispatcher.Invoke(new Action(() =>
               updateButton.Content = "Loading"
               ));
            Dispatcher.Invoke(new Action(() =>
                reportIndex.IsEnabled = false
                ));
            Dispatcher.Invoke(new Action(() =>
                updateButton.IsEnabled = false
                ));
            try
            {
                double percent = Convert.ToDouble(pertext);
                if (percent > 10 || percent < 1)
                {
                    throw new Exception("Percentage should between 1% to 10%");
                }
                percent = percent * 0.01;
                Global.percentage = percent;
                myModel.CreateModel(index - 1, title[index - 1], ytitle[index - 1]);
                Dispatcher.Invoke(new Action(() =>
                report.Model = myModel.MyModel
                ));
            }
            catch (FormatException)
            {
                MessageBox.Show("Please input the correct percentage", "Error");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            Dispatcher.Invoke(new Action(() =>
                reportIndex.IsEnabled = true
                ));
            Dispatcher.Invoke(new Action(() =>
                updateButton.IsEnabled = true
                ));
            Dispatcher.Invoke(new Action(() =>
               updateButton.Content = "Update"
               ));
        }
        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            Thread update = new Thread(updateThread);
            update.Start();
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

        private void table_Click(object sender, RoutedEventArgs e)
        {
            if (myModel != null) {
                List<RankData> items = new List<RankData>();
                for (int i = 0; i < myModel.ppdata.simulationSeries.Count; i++) {
                    items.Add(new RankData
                    {
                        rank = (i + 1).ToString(),
                        percentage = ((double)(i + 1) / (double)myModel.ppdata.simulationSeries.Count).ToString(),
                        simnum = myModel.ppdata.simulationSeries[i].simulationNumber.ToString(),
                        likelihood = myModel.ppdata.simulationSeries[i].likelihood.ToString()
                    });
                }
                RankTable rtw = new RankTable(items);
                rtw.ShowDialog();
            }
        }
    }
}
