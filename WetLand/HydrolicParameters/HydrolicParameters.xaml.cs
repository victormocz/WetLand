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
    /// Interaction logic for HydrolicParameters.xaml
    /// </summary>
    using System.IO;
    public partial class HydrolicParameters : Window
    {
        public List<item> items = new List<item>();
        public HydrolicParameters()
        {
            InitializeComponent();
            resizeCol();
            addDataSourceToListView();
        }

        private void addDataSourceToListView()
        {
            
            string filename = Global.projectName + @"\InputFiles\12_hydro_climate.txt";
            string[] contents = File.ReadAllLines(filename);

            //update the number of days in fixedparameter
            FixedParameterFolder.FixedData changeNumOfDays = new FixedParameterFolder.FixedData();
            if (contents.Length > 2)
            {
                Global.nofDays = contents.Length - 2;
                changeNumOfDays.saveFile();
            }
            else {
                MessageBox.Show("Your 12_hydro_climate.txt's length is too short");
                return;
            }
            DateTime current = Global.startDate.AddDays(-1);//TODO start date
            for (int i = 2; i < contents.Length; i++)
            {
                string[] parameters = contents[i].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                items.Add(new item
                {
                    date = current.AddDays(1),
                    Qin = parameters[0],
                    QOut = parameters[1],
                    Vw = parameters[2],
                    Area = parameters[3],
                    ET = parameters[4],
                    ip = parameters[5],
                    H = parameters[6],
                    Qg = parameters[7],
                    Uw = parameters[8],
                    temp = parameters[9]
                });
                current = current.AddDays(1);
            }
            hydroParameters.ItemsSource = items;
        }

        private void resizeCol()
        {
            double winsize = this.Width / 11.5;
            col0.Width = winsize;
            col1.Width = winsize;
            col2.Width = winsize;
            col3.Width = winsize;
            col4.Width = winsize;
            col5.Width = winsize;
            col6.Width = winsize;
            col7.Width = winsize;
            col8.Width = winsize;
            col9.Width = winsize;
            col10.Width = winsize;
        }

        private void HydrolicParametersWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            resizeCol();
        }

        private void viewGraph_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            HydrolicParametersReport reportwin = new HydrolicParametersReport();
            reportwin.ShowDialog();
        }

        private void viewGraph_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter|| e.Key == Key.Space) { 
                HydrolicParametersReport reportwin = new HydrolicParametersReport();
                reportwin.ShowDialog();
            }
        }

        private void hydroParameters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(hydroParameters.SelectedIndex<0|| hydroParameters.SelectedIndex >= items.Count)
            {
                Save.IsEnabled = false;
                return;
            }
            eDate.Text = items[hydroParameters.SelectedIndex].date.ToString("MM/dd/yyyy");
            eQin.Text = items[hydroParameters.SelectedIndex].Qin.ToString();
            eQOut.Text = items[hydroParameters.SelectedIndex].QOut.ToString();
            eVw.Text = items[hydroParameters.SelectedIndex].Vw.ToString();
            eArea.Text = items[hydroParameters.SelectedIndex].Area.ToString();
            eET.Text = items[hydroParameters.SelectedIndex].ET.ToString();
            eip.Text = items[hydroParameters.SelectedIndex].ip.ToString();
            eH.Text = items[hydroParameters.SelectedIndex].H.ToString();
            eQg.Text = items[hydroParameters.SelectedIndex].Qg.ToString();
            eUw.Text = items[hydroParameters.SelectedIndex].Uw.ToString();
            etemp.Text = items[hydroParameters.SelectedIndex].temp.ToString();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (checkOne(eQin) && checkOne(eQOut) && checkOne(eVw) && checkOne(eArea) && checkOne(eET) && checkOne(eip) && checkOne(eH) && checkOne(eQg) && checkOne(eUw) && checkOne(etemp))
            {
                try {
                    items[hydroParameters.SelectedIndex].Qin = eQin.Text;
                    items[hydroParameters.SelectedIndex].QOut = eQOut.Text;
                    items[hydroParameters.SelectedIndex].Vw = eVw.Text;
                    items[hydroParameters.SelectedIndex].Area = eArea.Text;
                    items[hydroParameters.SelectedIndex].ET = eET.Text;
                    items[hydroParameters.SelectedIndex].ip = eip.Text;
                    items[hydroParameters.SelectedIndex].H = eH.Text;
                    items[hydroParameters.SelectedIndex].Qg = eQg.Text;
                    items[hydroParameters.SelectedIndex].Uw = eUw.Text;
                    items[hydroParameters.SelectedIndex].temp = etemp.Text;
                    hydroParameters.Items.Refresh();
                    String[] content = new String[items.Count + 2];
                    content[0] = "Qin	Qout	Vw	Area	ET	ip	H		Qg	Uw	temp   ";
                    content[1] = "(m3/day) (m3/day) (m3)	(m2)	(cm/day)(cm)	(m)		(m3/day)(m/s)	(day)";
                    for (int i = 0; i < items.Count; i++)
                    {
                        content[i + 2] = items[i].line;
                    }
                    File.WriteAllLines(Global.projectName + @"\InputFiles\12_hydro_climate.txt", content);
                }catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
            else
            {
                return;
            }
        }

        private bool checkOne(TextBox boxData)
        {
            if (boxData.Text.Length == 0)
            {
                boxData.BorderBrush = Brushes.Red;
                status.Visibility = Visibility.Visible;
                status.Text = "This Field Cannot Be Empty";
                return false;
            }
            try
            {
                Double.Parse(boxData.Text);
                boxData.BorderBrush = Brushes.Black;
                return true;
            }
            catch (Exception)
            {
                boxData.BorderBrush = Brushes.Red;
                status.Text = "This Field Can have only Decimal Values";
                return false;
            }
        }
    }
    public class item {
        //{ "Qin", "QOut","Vw","Area","ET","ip","H","Qg","Uw","temp"};
        public DateTime date { get; set; }
        public string Qin { get; set; }
        public string QOut { get; set; }
        public string Vw { get; set; }
        public string Area { get; set; }
        public string ET { get; set; }
        public string ip { get; set; }
        public string H { get; set; }
        public string Qg { get; set; }
        public string Uw { get; set; }
        public string temp { get; set; }
        public string line
        {
            get
            {
                return Qin + "\t" + QOut + "\t" + Vw + "\t" + Area + "\t" + ET + "\t" + ip + "\t" + H + "\t" + Qg + "\t" + Uw + "\t" + temp;
            }
        }
    }

}
