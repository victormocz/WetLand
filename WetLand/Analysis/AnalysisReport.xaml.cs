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

namespace WetLand.Analysis
{
    /// <summary>
    /// Interaction logic for AnalysisReport.xaml
    /// </summary>
    using System.IO;
    using System.Threading;
    using Microsoft.Win32;
    using OxyPlot;
    using OxyPlot.Xps;
    using OxyPlot.Series;
    using OxyPlot.Axes;
    using System.Diagnostics;
    public partial class AnalysisReport : Window
    {
        private string[] fileName = { "102_Onw.txt", "103_Onss.txt","104_Onsf.txt", "105_Nw.txt", "106_Ns1.txt", "107_Ns2.txt",
        "108_NO3w.txt","109_NO3s1.txt","110_NO3s2.txt","111_Ow.txt","112_a.txt","113_b.txt","114_Pw.txt","115_Ps1.txt",
        "116_Ps2.txt","118_mw.txt","150_DOCw.txt","151_LPOCw.txt","152_RPOCw.txt","153_DOCs1.txt","154_LPOCs1.txt","155_RPOCs1.txt",
        "156_DOCs2.txt","157_LPOCs2.txt","158_RPOCs2.txt","159_TOCw.txt","160_CH4w.txt","161_CH4s1.txt","162_CH4s2.txt"};
        private string[] lengend = {  "Onw ",  "Onss ", "Onsf ",  "Nw ",  "Ns1 ",  "Ns2 ",
         "NO3w ", "NO3s1 ", "NO3s2 ", "Ow ", "a ", "b ", "Pw ", "Ps1 ",
         "Ps2 ", "mw ", "DOCw ", "LPOCw ", "RPOCw ", "DOCs1 ", "LPOCs1 ", "RPOCs1 ",
         "DOCs2 ", "LPOCs2 ", "RPOCs2 ", "TOCw ", "CH4w ", "CH4s1 ", "CH4s2 "};
        private string[] title = { "Particulate organic nitrogen concentration in free water (gr/cm³)", "Concentration of organic nitrogen in in anaerobic sediment layer (gr/cm³)", "Concentration of organic nitrogen in in aerobic sediment layer (gr/cm³)", "Total ammonia-nitrogen ([NH4+] + [NH3]) concentration in free water (gr/cm³)", "Total ammonia-nitrogen pore-water concentration in upper aerobic layer (gr/cm³)", "Total ammonia-nitrogen pore-water concentration in lower anaerobic layer (gr/cm³)", "Nitrate-nitrogen concentration in free water (gr/cm³)", "Nitrate-nitrogen pore-water concentration in upper aerobic layer (gr/cm³)", "Nitrate-nitrogen pore-water concentration in lower anaerobic layer (gr/cm³)", "Oxygen concentration in free water (gr/cm³)", "Mass of free floating plant (gr chlorophyll a)", "Mass of rooted plants (gr chlorophyll a)", "Total inorganic phosphorus concentration in free water (gr/cm³)", "Total phosphorus concentration in aerobic layer (gr/cm³)", "Total phosphorus concentration in anaerobic layer (gr/cm³)", "Sediment concentration in free water (gr/cm³)", "Concentrations of dissolved organic C in free water (gr/cm³)", "Concentrations of labile (fast reacting) particulate organic C in free water (gr/cm³)", "Concentrations of refractory (slow reacting) particulate organic C in free water (gr/cm³)", "Pore water concentrations of DOC in aerobic sediment layer (gr/cm³)", "Pore water concentrations of LPOC in aerobic sediment layer (gr/cm³)", "Pore water concentrations of RPOC in aerobic sediment layer (gr/cm³)", "Pore water concentrations of DOC in lower anaerobic sediment layer (gr/cm³)", "Pore water concentrations of LPOC in lower anaerobic sediment layer (gr/cm³)", "Pore water concentrations of RPOC in lower anaerobic sediment layer (gr/cm³)", "Concentrations of total organic C in free water (gr/cm³)", "Methane concentration in free water (gr/cm³)", "Methane concentration in aerobic sediment layer (gr/cm³)", "Methane concentration in anaerobic sediment layer (gr/cm³)" };
        private string[] ytitle = { "Onw (gr/cm³)", "Onsf (gr/cm³)", "Onss (gr/cm³)", "Nw (gr/cm³)", "Ns1 (gr/cm³)",
        "Ns2 (gr/cm³)","NO3w (gr/cm³)","NO3s1 (gr/cm³)","NO3s2 (gr/cm³)","Ow (gr/cm³)","a (gr chlorophyll a)",
        "b (gr chlorophyll a)","Pw (gr/cm³)","Ps1 (gr/cm³)","Ps2 (gr/cm³)","mw (gr/cm³)","Docw (gr/cm³)",
        "LPOCw (gr/cm³)","RPOCw (gr/cm³)","DOCs1 (gr/cm³)","LPOCs1 (gr/cm³)","RPOCs1 (gr/cm³)","DOCs2 (gr/cm³)",
        "LPOCs2 (gr/cm³)","RPOCs2 (gr/cm³)","TOCw (gr/cm³)","CH4w (gr/cm³)","CH4s1 (gr/cm³)","CH4s2 (gr/cm³)"};
        private string prefix = Global.projectName + @"\InputFiles\";
        private int lengthOfsimulation;
        private List<item> items;
        public AnalysisReport()
        {
            InitializeComponent();
            if (Global.DeterminMode)
            {
                SimulationNumSelector.Visibility = Visibility.Collapsed;
            }
            else
            {
                SimulationNumSelector.Visibility = Visibility.Visible;
            }
            try
            {
                int count = 0;
                using (var reader = File.OpenText(Global.projectName + @"\InputFiles\102_Onw.txt"))
                {
                    while (reader.ReadLine() != null)
                    {
                        count++;
                    }
                }
                lengthOfsimulation = count / 4;
                status.Text = "Simulation Number (1-" + lengthOfsimulation.ToString() + ")";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!");
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            Thread update = new Thread(updateThread);
            int index = reportIndex.SelectedIndex;
            update.Start(index);
        }

        private void updateThread(Object index)
        {
            if (reportIndex == null)
            {
                return;
            }
            if ((int)index == 0)
            {
                Dispatcher.Invoke(new Action(() =>
                    report.Model = null));
                return;
            }

            int number;
            string simNum = "";
            Dispatcher.Invoke(new Action(() =>
               simNum = simulationNum.Text));
            if (int.TryParse(simNum, out number))
            {
                ReportModelView myModel = new ReportModelView();
                if (number < 1 || number > lengthOfsimulation)
                {
                    MessageBox.Show("Simulation Number should between 1-" + lengthOfsimulation.ToString() + "!", "Error");
                    return;
                }
                if (File.Exists(prefix + fileName[(int)index - 1]))
                {
                    Dispatcher.Invoke(new Action(() =>
               Update.Content = "Loading"));
                    Dispatcher.Invoke(new Action(() =>
                       Update.IsEnabled = false));
                    Dispatcher.Invoke(new Action(() =>
                       reportIndex.IsEnabled = false));
                    myModel.CreateModel(prefix + fileName[(int)index - 1], title[(int)index - 1], lengend[(int)index - 1], number, ytitle[(int)index - 1]);
                    Dispatcher.Invoke(new Action(() =>
                    report.Model = myModel.MyModel
                ));

                }
            }
            readSimulationvalue(number);
            Dispatcher.Invoke(new Action(() =>
               Update.IsEnabled = true));
            Dispatcher.Invoke(new Action(() =>
               Update.Content = "Update"));
            Dispatcher.Invoke(new Action(() =>
               reportIndex.IsEnabled = true));
        }
        private void selectionThread(Object index)
        {
            if (report == null || reportIndex == null || simulationNum == null)
            {
                return;
            }
            if ((int)index == 0)
            {
                Dispatcher.Invoke(new Action(() =>
                    report.Model = null));
                return;
            }

            ReportModelView myModel = new ReportModelView();

            if (File.Exists(prefix + fileName[(int)index - 1]))
            {
                Dispatcher.Invoke(new Action(() =>
               Update.IsEnabled = false));
                Dispatcher.Invoke(new Action(() =>
                   Update.Content = "Loading"));
                Dispatcher.Invoke(new Action(() =>
                   reportIndex.IsEnabled = false));
                Dispatcher.Invoke(new Action(() =>
                   simulationNum.Text = "1"));
                myModel.CreateModel(prefix + fileName[(int)index - 1], title[(int)index - 1], lengend[(int)index - 1], 1, ytitle[(int)index - 1]);
                Dispatcher.Invoke(new Action(() =>
                    report.Model = myModel.MyModel
                ));
                readSimulationvalue(1);
            }
            else
            {
                MessageBox.Show("File " + prefix + fileName[(int)index - 1] + " not found!", "caution");
                return;
            }
            Dispatcher.Invoke(new Action(() =>
               Update.IsEnabled = true));
            Dispatcher.Invoke(new Action(() =>
               Update.Content = "Update"));
            Dispatcher.Invoke(new Action(() =>
               reportIndex.IsEnabled = true));
        }
        private void reportIndex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (report == null || reportIndex == null || simulationNum == null )
            {
                return;
            }
            if (reportIndex.SelectedIndex == 0)
            {
                menu_print.IsEnabled = false;
                menu_pdf.IsEnabled = false;
                return;
            }
            else {
                menu_print.IsEnabled = true;
                menu_pdf.IsEnabled = true;
            }
            Thread selection = new Thread(selectionThread);
            selection.Start(reportIndex.SelectedIndex);
        }

        private void readSimulationvalue(Object num)
        {
            int number = (int)num;
            items = new List<item>();
            int index = 0;
            Dispatcher.Invoke(new Action(() =>
               index = reportIndex.SelectedIndex));
            if (!File.Exists(prefix + fileName[index - 1]))
            {
                MessageBox.Show("File " + prefix + fileName[index - 1] + " not found!", "caution");
                return;
            }
            DateTime tempdate = Global.startDate.AddDays(-1);
            int count = 0;
            string simStr = "";
            foreach (var line in File.ReadLines(prefix + fileName[index - 1]))
            {
                count++;
                if (count > number + 3 * (number - 1) && count < number + 3 * (number - 1) + 2)
                {
                    simStr += line;
                }
                if (count >= number + 3 * (number - 1) + 2)
                {
                    count = 0;
                    break;
                }
            }
            string[] parameters = simStr.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
            foreach (var parameter in parameters)
            {
                items.Add(new item { date = tempdate, simulationValue = parameter });
                tempdate = tempdate.AddDays(1);
            }
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

        private void timeTable_Click(object sender, RoutedEventArgs e)
        {
            TimeSeriesWindow tsw = new TimeSeriesWindow(items);
            tsw.ShowDialog();
        }
    }
    public class item
    {
        public DateTime date { set; get; }
        public string simulationValue { set; get; }
    }
}
