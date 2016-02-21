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
    /// Interaction logic for ETWindow.xaml
    /// </summary>
    using System.IO;
    using System.Diagnostics;
    public partial class ETWindow : Window
    {
        private List<ETItem> etItems;
        private RoutingMainWindow rmw;
        public ETWindow(RoutingMainWindow rmw)
        {
            InitializeComponent();
            this.rmw = rmw;
            etItems = new List<ETItem>();
            try
            {
                addData();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error");
            }
            etData.ItemsSource = etItems;

        }

        private void addData() {
            string[] contents = File.ReadAllLines(Global.projectName+ @"\InputFiles\Routing\ET Module\1_inputs.txt");
            if (contents.Length < 4) {
                throw new Exception("File Length of inputs.txt is too short!");
            }
            for (int i = 3; i < contents.Length; i++) {
                string[] parameters = contents[i].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                if (parameters.Length != 2) {
                    throw new Exception("We need two parameters in line "+ i.ToString());
                }
                etItems.Add(new ETItem{julian = parameters[0],ta = parameters[1] });
            }
        }

        private void executeET() {
            if (!File.Exists(Global.projectName + @"\InputFiles\Routing\ET Module\1_inputs.txt")) {
                throw new Exception("Can not find the 1_inputs.txt file, Please prepare it for calculating the ET data");
            }
            ProcessStartInfo info = new ProcessStartInfo();
            info.WorkingDirectory = Global.projectName + @"\InputFiles\Routing\ET Module";
            info.FileName = Global.projectName + @"\InputFiles\Routing\ET Module\ET.exe";
            info.UseShellExecute = false;
            info.CreateNoWindow = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            var process = Process.Start(info);
            process.WaitForExit();
        }
        private void addETDate() {
            string[] etcontents = File.ReadAllLines(Global.projectName + @"\InputFiles\Routing\ET Module\ET.txt");
            string[] contents = File.ReadAllLines(Global.projectName + @"\InputFiles\Routing\2_input_time_series.txt");
            if (etcontents.Length != contents.Length)
            {
                throw new Exception("Length of days do not equal.(ET Module)");
            }
            for (int i = 1; i < contents.Length; i++) {
                string[] parameters = contents[i].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                if (parameters.Length == 4)
                {
                    contents[i] = parameters[0] + "\t" + parameters[1] + "\t" + etcontents[i] + "\t" + parameters[2] + "\t" + parameters[3];
                }
                else if (parameters.Length == 5) {
                    contents[i] = parameters[0] + "\t" + parameters[1] + "\t" + etcontents[i] + "\t" + parameters[3] + "\t" + parameters[4];
                }
            }
            File.WriteAllLines(Global.projectName + @"\InputFiles\Routing\2_input_time_series.txt", contents);
        }

        private void calculate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                executeET();
                addETDate();
                rmw.addDataSourceToInput();
                rmw.timeSeriesData.Items.Refresh();
                MessageBox.Show("ET data are updated!", "Caution");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
    }
}
