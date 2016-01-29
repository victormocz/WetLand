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
    /// Interaction logic for TimeDependentParameters.xaml
    /// </summary>
    using System.IO;
    public partial class TimeDependentParameters : Window
    {
        public TimeDependentParameters()
        {
            InitializeComponent();
            resizeCol();
            List<item> items = new List<item>();
            string filename = Global.projectName + @"\InputFiles\13_time_dependent_parameters.txt";
            string[] contents = File.ReadAllLines(filename);
            DateTime current = Global.startDate;
            for (int i = 2; i < contents.Length; i++)
            {
                string[] parameters = contents[i].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                items.Add(new item
                {
                    date = current.AddDays(1),
                    ONin = parameters[0],
                    NO3in = parameters[1],
                    Nwin = parameters[2],
                    NO3g = parameters[3],
                    Ng = parameters[4],
                    Owin = parameters[5],
                    PO4in = parameters[6],
                    Pg = parameters[7],
                    mwin = parameters[8],
                    NH4air = parameters[9],
                    NO3air = parameters[10],
                    Qa = parameters[11],
                    Qn = parameters[12],
                    LPOCin = parameters[13],
                    RPOCin = parameters[14],
                    DOCin = parameters[15],
                    TOCin = parameters[16],
                    TOCatm = parameters[17],
                    TOCgw = parameters[18],
                });
                current = current.AddDays(1);
            }
            timeParameters.ItemsSource = items;
            Global.mainWin.Model_Parameters.IsEnabled = true;
        }

        private void resizeCol()
        {
            double winsize = this.Width / 21.5;
            col0.Width = winsize*1.5;
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
            col11.Width = winsize;
            col12.Width = winsize;
            col13.Width = winsize;
            col14.Width = winsize;
            col15.Width = winsize;
            col16.Width = winsize;
            col17.Width = winsize;
            col18.Width = winsize;
            col19.Width = winsize;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            resizeCol();
        }

        private void viewGraph_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            TimeDependentReport reportwin = new TimeDependentReport();
            reportwin.ShowDialog();
        }

        private void viewGraph_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                TimeDependentReport reportwin = new TimeDependentReport();
                reportwin.ShowDialog();
            }
        }
    }

    public class item
    {
        //{ "Qin", "QOut","Vw","Area","ET","ip","H","Qg","Uw","temp"};
        public DateTime date { get; set; }
        public string ONin { get; set; }
        public string NO3in { get; set; }
        public string Nwin { get; set; }
        public string NO3g { get; set; }
        public string Ng { get; set; }
        public string Owin { get; set; }
        public string PO4in { get; set; }
        public string Pg { get; set; }
        public string mwin { get; set; }
        public string NH4air { get; set; }
        public string NO3air { get; set; }
        public string Qa { get; set; }
        public string Qn { get; set; }
        public string LPOCin { get; set; }
        public string RPOCin { get; set; }
        public string DOCin { get; set; }
        public string TOCin { get; set; }
        public string TOCatm { get; set; }
        public string TOCgw { get; set; }
    }
}
