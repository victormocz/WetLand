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

namespace WetLand.Date
{
    /// <summary>
    /// Interaction logic for Date.xaml
    /// </summary>
    using System.IO;
    public partial class Date : Window
    {
        public Date()
        {
            InitializeComponent();
        }

        private void confirm_Click(object sender, RoutedEventArgs e)
        {
            if (startDate.Text.Length == 0)
            {
                MessageBox.Show("Please select a start date", "Caution");
                return;
            }
            try {
                Global.startDate = Convert.ToDateTime(startDate.Text);

                Global.mainWin.Hydro_Climate.IsEnabled = true;
                this.Close();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Error");
            }
        }
    }
}
