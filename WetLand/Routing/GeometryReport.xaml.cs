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
    /// Interaction logic for GeometryReport.xaml
    /// </summary>
    public partial class GeometryReport : Window
    {
        private string[] ytitle = { "Area (m²)", "Volume (m³)", "Outflow (m³/day)" };
        private string[] lengend = { "Area", "Volume", "Outflow" };
        public GeometryReport()
        {
            InitializeComponent();
            setModel();
        }
        private void setModel()
        {
            try
            {
                ReportModelView model = new ReportModelView();
                if (reportIndex != null && report != null)
                {
                    report.Model = model.CreateModel(1, reportIndex.SelectedIndex, reportIndex.Text, lengend[reportIndex.SelectedIndex], ytitle[reportIndex.SelectedIndex]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void reportIndex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            setModel();
        }
    }
}
