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

namespace WetLand.FixedParameterFolder
{
    /// <summary>
    /// Interaction logic for FixedParameters.xaml
    /// </summary>
    public partial class FixedParameters : Window
    {
        private FixedData data;
        public FixedParameters()
        {
            InitializeComponent();
            data = new FixedData(this);
            Global.mainWin.Water_quality.IsEnabled = true;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (checkAllData())
            {
                data.saveFile();
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

        private bool checkAllData()
        {
            bool result = true;
            result = result && checkOne(dt) && checkOne(ronn) && checkOne(rond) && checkOne(roc) && checkOne(sims);
            result = result && checkOne(fnw) && checkOne(fns1) && checkOne(fns2) && checkOne(fno3w) && checkOne(fno3s1) && checkOne(fno3s2) && checkOne(apn) && checkOne(lat) && checkOne(d_bound);
            result = result && checkOne(amc) && checkOne(lamdar);
            return result;
        }
    }
}
