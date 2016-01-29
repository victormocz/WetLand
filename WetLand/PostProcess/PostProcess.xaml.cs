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
    public partial class PostProcess : Window
    {
        public PostProcess()
        {
            InitializeComponent();
            try
            {
                //ReportModelView myModel = new ReportModelView();
                //report.Model = myModel.CreateModel(reportIndex.SelectedIndex,reportIndex.Text);
                percentage.Text = (Global.percentage*100).ToString();
                if (Global.DeterminMode == true)
                {
                    percentPanel.Visibility = Visibility.Hidden;
                }
                else {
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
            try
            {
                if (reportIndex.SelectedIndex <= 0) {
                    return;
                }
                ReportModelView myModel = new ReportModelView();
                report.Model = myModel.CreateModel(reportIndex.SelectedIndex-1, reportIndex.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            ReportModelView myModel = new ReportModelView();
            try
            {
                double percent = Convert.ToDouble(percentage.Text);
                if (percent > 10 || percent < 1) {
                    throw new Exception("Percentage should between 1% to 10%");
                }
                percent = percent * 0.01;
                Global.percentage = percent;
                report.Model = myModel.CreateModel(reportIndex.SelectedIndex-1, reportIndex.Text);
            }
            catch (FormatException) {
                MessageBox.Show("Please input the correct percentage","Error");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error");
            }

        }
    }
}
