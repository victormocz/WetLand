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
        public List<item> items = new List<item>();
        public TimeDependentParameters()
        {
            InitializeComponent();
            resizeCol();
            string filename = Global.projectName + @"\InputFiles\13_time_dependent_parameters.txt";
            string[] contents = File.ReadAllLines(filename);
            DateTime current = Global.startDate;
            for (int i = 2; i < contents.Length; i++)
            {
                string[] parameters = contents[i].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                items.Add(new item
                {
                    date = current.AddDays(1),
                    H1 = parameters[0],
                    ONin = parameters[1],
                    NO3in = parameters[2],
                    Nwin = parameters[3],
                    NO3g = parameters[4],
                    Ng = parameters[5],
                    Owin = parameters[6],
                    PO4in = parameters[7],
                    Pg = parameters[8],
                    mwin = parameters[9],
                    NH4air = parameters[10],
                    NO3air = parameters[11],
                    Qa = parameters[12],
                    Qn = parameters[13],
                    LPOCin = parameters[14],
                    RPOCin = parameters[15],
                    DOCin = parameters[16],
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

        private void viewGraph_Click(object sender, RoutedEventArgs e)
        {
            TimeDependentReport reportwin = new TimeDependentReport();
            reportwin.ShowDialog();
        }

        private bool checkOne(TextBox boxData)
        {
            if (boxData.Text.Length == 0)
            {
                boxData.BorderBrush = Brushes.Red;
                MessageBox.Show("This Field Cannot Be Empty", "Error");
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
                MessageBox.Show("This Field Can have only Decimal Values", "Error");
                return false;
            }
        }

        private void timeParameters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (timeParameters.SelectedIndex < 0 || timeParameters.SelectedIndex > items.Count) {
                return;
            }
            eDate.IsEnabled = true;
            eH1.IsEnabled = true;
            eONin.IsEnabled = true;
            eNO3in.IsEnabled = true;
            eNwin.IsEnabled = true;
            eNO3g.IsEnabled = true;
            eNg.IsEnabled = true;
            eOwin.IsEnabled = true;
            ePO4in.IsEnabled = true;
            ePg.IsEnabled = true;
            emwin.IsEnabled = true;
            eNH4air.IsEnabled = true;
            eNO3air.IsEnabled = true;
            eQa.IsEnabled = true;
            eQn.IsEnabled = true;
            eLPOCin.IsEnabled = true;
            eRPOCin.IsEnabled = true;
            eDOCin.IsEnabled = true;
            eTOCatm.IsEnabled = true;
            eTOCgw.IsEnabled = true;
            save.IsEnabled = true;
            eDate.Text = items[timeParameters.SelectedIndex].date.ToString("MM/dd");
            eH1.Text = items[timeParameters.SelectedIndex].H1.ToString();
            eONin.Text = items[timeParameters.SelectedIndex].ONin.ToString();
            eNO3in.Text = items[timeParameters.SelectedIndex].NO3in.ToString();
            eNwin.Text = items[timeParameters.SelectedIndex].Nwin.ToString();
            eNO3g.Text = items[timeParameters.SelectedIndex].NO3g.ToString();
            eNg.Text = items[timeParameters.SelectedIndex].Ng.ToString();
            eOwin.Text = items[timeParameters.SelectedIndex].Owin.ToString();
            ePO4in.Text = items[timeParameters.SelectedIndex].PO4in.ToString();
            ePg.Text = items[timeParameters.SelectedIndex].Pg.ToString();
            emwin.Text = items[timeParameters.SelectedIndex].mwin.ToString();
            eNH4air.Text = items[timeParameters.SelectedIndex].NH4air.ToString();
            eNO3air.Text = items[timeParameters.SelectedIndex].NO3air.ToString();
            eQa.Text = items[timeParameters.SelectedIndex].Qa.ToString();
            eQn.Text = items[timeParameters.SelectedIndex].Qn.ToString();
            eLPOCin.Text = items[timeParameters.SelectedIndex].LPOCin.ToString();
            eRPOCin.Text = items[timeParameters.SelectedIndex].RPOCin.ToString();
            eDOCin.Text = items[timeParameters.SelectedIndex].DOCin.ToString();
            eTOCatm.Text = items[timeParameters.SelectedIndex].TOCatm.ToString();
            eTOCgw.Text = items[timeParameters.SelectedIndex].TOCgw.ToString();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                items[timeParameters.SelectedIndex].H1 = eH1.Text;
                items[timeParameters.SelectedIndex].ONin = eONin.Text;
                items[timeParameters.SelectedIndex].NO3in = eNO3in.Text;
                items[timeParameters.SelectedIndex].Nwin = eNwin.Text;
                items[timeParameters.SelectedIndex].NO3g = eNO3g.Text;
                items[timeParameters.SelectedIndex].Ng = eNg.Text;
                items[timeParameters.SelectedIndex].Owin = eOwin.Text;
                items[timeParameters.SelectedIndex].PO4in = ePO4in.Text;
                items[timeParameters.SelectedIndex].Pg = ePg.Text;
                items[timeParameters.SelectedIndex].mwin = emwin.Text;
                items[timeParameters.SelectedIndex].NH4air = eNH4air.Text;
                items[timeParameters.SelectedIndex].NO3air = eNO3air.Text;
                items[timeParameters.SelectedIndex].Qa = eQa.Text;
                items[timeParameters.SelectedIndex].Qn = eQn.Text;
                items[timeParameters.SelectedIndex].LPOCin = eLPOCin.Text;
                items[timeParameters.SelectedIndex].RPOCin = eRPOCin.Text;
                items[timeParameters.SelectedIndex].DOCin = eDOCin.Text;
                items[timeParameters.SelectedIndex].TOCatm = eTOCatm.Text;
                items[timeParameters.SelectedIndex].TOCgw = eTOCgw.Text;
                timeParameters.Items.Refresh();
                String[] content = new String[items.Count + 2];
                content[0] = " H1	ONin	NO3in	Nwin	NO3g	 Ng	 Owin	PO4in	Pg	 mwin 	NH4air	NO3air	Qa	Qn	LPOCin	RPOCin	DOCin	TOCatm	TOCgw";
                content[1] = "( C)	(mg/L)	(mg/L)	(mg/L)	(mg/L)	(mg/L)	(mg/L)	(mg/L)	(mg/L)	(mg/L)	(mg/L)	(mg/L)	(mg/L)	(mg/L)	(mg/L)	(mg/L)	(mg/L)	(mg/L)	(mg/lit)";
                for (int i = 0; i < items.Count; i++)
                {
                    content[i + 2] = items[i].line;
                }
                File.WriteAllLines(Global.projectName + @"\InputFiles\13_time_dependent_parameters.txt", content);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error");
            }
        }
    }

    public class item
    {
        //{ "Qin", "QOut","Vw","Area","ET","ip","H","Qg","Uw","temp"};
        public DateTime date { get; set; }
        public string H1 { get; set; }
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
        public string TOCatm { get; set; }
        public string TOCgw { get; set; }
        public string line
        {
            get
            {
                return H1 + "\t" + ONin + "\t" + NO3in + "\t" + Nwin + "\t" + NO3g + "\t" + Ng + "\t" + Owin + "\t" + PO4in + "\t" + Pg + "\t" + mwin+"\t"+ NH4air + "\t" + NO3air + "\t" + Qa + "\t" + Qn + "\t" + LPOCin + "\t" + RPOCin + "\t" + DOCin + "\t" + TOCatm + "\t" + TOCgw;
            }
        }
    }
}
