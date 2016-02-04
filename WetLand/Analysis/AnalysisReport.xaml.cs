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
        private string prefix = Global.projectName + @"\InputFiles\";
        private int lengthOfsimulation;
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

            ReportModelView myModel = new ReportModelView();
            if (File.Exists(prefix + fileName[0]))
            {
                report.Model = myModel.CreateModel(prefix + fileName[0], "Particulate organic nitrogen concentration in free water (gr/cm³)", "Onw", 1);
                readSimulationvalue(1);
            }
            else
            {
                MessageBox.Show("File " + prefix + fileName[0] + " not found!", "caution");
                return;
            }
            try
            {
                string[] content = File.ReadAllLines(prefix + fileName[0]);
                lengthOfsimulation = content.Length / 4;
                status.Text = "Simulation Number (1-" + lengthOfsimulation.ToString() + ")";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (reportIndex == null)
            {
                return;
            }
            int number;
            if (int.TryParse(simulationNum.Text, out number))
            {
                ReportModelView myModel = new ReportModelView();
                if (number < 1 || number > lengthOfsimulation)
                {
                    MessageBox.Show("Simulation Number should between 1-" + lengthOfsimulation.ToString() + "!", "Error");
                    return;
                }
                if (File.Exists(prefix + fileName[reportIndex.SelectedIndex]))
                {
                    report.Model = myModel.CreateModel(prefix + fileName[reportIndex.SelectedIndex], reportIndex.Text, lengend[reportIndex.SelectedIndex], number);
                }
            }
            readSimulationvalue(number);
        }

        private void reportIndex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (report == null || reportIndex == null || simulationNum == null)
            {
                return;
            }
            ReportModelView myModel = new ReportModelView();

            if (File.Exists(prefix + fileName[reportIndex.SelectedIndex]))
            {
                simulationNum.Text = "1";
                report.Model = myModel.CreateModel(prefix + fileName[reportIndex.SelectedIndex], reportIndex.Text, lengend[reportIndex.SelectedIndex], 1);
                readSimulationvalue(1);
            }
            else
            {
                MessageBox.Show("File " + prefix + fileName[reportIndex.SelectedIndex] + " not found!", "caution");
                return;
            }


        }

        private void readSimulationvalue(int number)
        {
            List<item> items = new List<item>();

            if (!File.Exists(prefix + fileName[reportIndex.SelectedIndex]))
            {
                MessageBox.Show("File " + prefix + fileName[reportIndex.SelectedIndex] + " not found!", "caution");
                return;
            }

            string[] contents = File.ReadAllLines(prefix + fileName[reportIndex.SelectedIndex]);
            DateTime tempdate = Global.startDate.AddDays(-1);
            int count = 0;
            string simStr = "";
            //new
            foreach (var line in File.ReadLines(prefix + fileName[reportIndex.SelectedIndex]))
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
            analysisSimulation.ItemsSource = items;
        }
    }
    public class item
    {
        public DateTime date { set; get; }
        public string simulationValue { set; get; }
    }
}
