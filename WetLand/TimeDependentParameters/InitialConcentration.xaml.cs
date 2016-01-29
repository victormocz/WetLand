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
    /// Interaction logic for InitialConcentration.xaml
    /// </summary>
    public partial class InitialConcentration : Window
    {
        private InitialData data;
        public InitialConcentration()
        {
            InitializeComponent();
            data = new InitialData(this);
            Global.mainWin.Model_Parameters.IsEnabled = true;
        }
        private bool checkAllData()
        {
            bool result = true;
            result = result && checkOne(onw) && checkOne(onsf) && checkOne(onss) && checkOne(nw) && checkOne(ns1) && checkOne(ns2);
            result = result && checkOne(n03w) && checkOne(n03s1) && checkOne(n03s2) && checkOne(a) && checkOne(b) && checkOne(mw);
            result = result && checkOne(ms) && checkOne(ow) && checkOne(pw) && checkOne(ps1) && checkOne(ps2) && checkOne(docw);
            result = result && checkOne(lpocw) && checkOne(rpocw) && checkOne(docs1) && checkOne(lpocs1) && checkOne(rpocs1) && checkOne(docs2);
            result = result && checkOne(lpocs2) && checkOne(rpocs2) && checkOne(ch4w) && checkOne(ch4s1) && checkOne(ch4s2);
            return result;
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

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (checkAllData())
            {
                data.saveFile();
            }
        }
    }

    
}
