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
    public partial class ETWindow : Window
    {
        private List<ETItem> etItems;
        public ETWindow()
        {
            InitializeComponent();
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
    }
}
