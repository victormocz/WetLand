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

namespace WetLand.ModelParameters
{
    /// <summary>
    /// Interaction logic for Carbon.xaml
    /// </summary>
    using System.IO;
    using MathNet.Numerics.Distributions;
    public partial class Carbon : Window
    {
        public List<Parameters> carbonParameters;
        public Carbon()
        {
            InitializeComponent();
            carbonParameters = new List<Parameters>();
            sim.Text = Global.simulationNumber;
            readData();
        }
        
        private void mcs(object sender, RoutedEventArgs e)
        {
            if (!checkAll())
            {
                MessageBox.Show("Please correct your value!", "Error!");
                return;
            }
            int n = Convert.ToInt32(sim.Text);
            int simNum = n;
            Global.simulationNumber = sim.Text;
            int leftNum = n;

            bool memory = true; //memory flag for memory allocation

            File.WriteAllText(Global.projectName + @"\InputFiles\15_generated_parameters_carbon.txt", "aca	FaDOC	FaLPOC	FaRPOC	FbDOC	FbLPOC	FbRPOC	kLPOC	kRPOC	KsatO	KinO	KN	KinN	K1DOC	k2DOC	k3DOC	k4DOC	cp1	cp2	cp3	fbw	k1CH4	k2CH4	Rveg\r\nN.A.	N.A.	N.A.	N.A.	N.A.	N.A.	N.A.	1/day	1/day	mgO/lit	mg/lit	mg/lit	mg/lit	1/day	1/day	1/day	1/day	N.A.	N.A.	N.A.	N.A	1/day	1/day	cm root/ gr chla\r\n");

            #region mcs start
            while (memory)
            {
                string[][] result = new string[24][];
                //10000 string array for buffer
                if (leftNum <= 10000)
                {
                    memory = false;
                    n = leftNum;
                }
                else
                {
                    n = 10000;
                    leftNum -= 10000;
                }
                carbonParameters.Clear();
                result[0] = mcs_start(no01d, no01min, no01tri, no01max, n);
                result[1] = mcs_start(no02d, no02min, no02tri, no02max, n);
                result[2] = mcs_start(no03d, no03min, no03tri, no03max, n);
                result[3] = mcs_start(no04d, no04min, no04tri, no04max, n);
                result[4] = mcs_start(no05d, no05min, no05tri, no05max, n);
                result[5] = mcs_start(no06d, no06min, no06tri, no06max, n);
                result[6] = mcs_start(no07d, no07min, no07tri, no07max, n);
                result[7] = mcs_start(no08d, no08min, no08tri, no08max, n);
                result[8] = mcs_start(no09d, no09min, no09tri, no09max, n);
                result[9] = mcs_start(no10d, no10min, no10tri, no10max, n);
                result[10] = mcs_start(no11d, no11min, no11tri, no11max, n);
                result[11] = mcs_start(no12d, no12min, no12tri, no12max, n);
                result[12] = mcs_start(no13d, no13min, no13tri, no13max, n);
                result[13] = mcs_start(no14d, no14min, no14tri, no14max, n);
                result[14] = mcs_start(no15d, no15min, no15tri, no15max, n);
                result[15] = mcs_start(no16d, no16min, no16tri, no16max, n);
                result[16] = mcs_start(no17d, no17min, no17tri, no17max, n);
                result[17] = mcs_start(no18d, no18min, no18tri, no18max, n);
                result[18] = mcs_start(no19d, no19min, no19tri, no19max, n);
                result[19] = mcs_start(no20d, no20min, no20tri, no20max, n);
                result[20] = mcs_start(no21d, no21min, no21tri, no21max, n);
                result[21] = mcs_start(no22d, no22min, no22tri, no22max, n);
                result[22] = mcs_start(no23d, no23min, no23tri, no23max, n);
                result[23] = mcs_start(no24d, no24min, no24tri, no24max, n);
                //Save Files
                btnSave_Click(sender,e);
                Global.tempCarbon = new List<Parameters>(this.carbonParameters);
                string[] line = new string[n];
                for (int index = 0; index < n; index++)
                {
                    line[index] = result[0][index] + "	" + result[1][index] + "	" + result[2][index] + "	" + result[3][index] + "	" + result[4][index] + "	" + result[5][index] + "	" + result[6][index] + "	" + result[7][index] + "	" + result[8][index] + "	" + result[9][index] + "	" + result[10][index] + "	" + result[11][index] + "	" + result[12][index] + "	" + result[13][index] + "	" + result[14][index] + "	" + result[15][index] + "	" + result[16][index] + "	" + result[17][index] + "	" + result[18][index] + "	" + result[19][index] + "	" + result[20][index] + "	" + result[21][index] + "	" + result[22][index] + "	" + result[23][index];
                }


                File.AppendAllLines(Global.projectName + @"\InputFiles\15_generated_parameters_carbon.txt", line);
            }
            #endregion mcs end
            //generator();

            //change the simulation num in txt file
            UpdateSimInFixedParameter(simNum);
            //Global.DeterminMode = false;
            Global.mainWin.menu_run.IsEnabled = true;
            Global.mainWin.KSTest.IsEnabled = false;
            MessageBox.Show("All parameters for Carbon have been generated.", "Completed!");

        }

        private static void UpdateSimInFixedParameter(int simNum)
        {
            try {
                string[] content = File.ReadAllLines(Global.projectName + "\\InputFiles\\10_fixed_parameters.txt");
                string[] words = content[1].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                words[5] = simNum.ToString();
                string simChanged = words[0] + "\t" + words[1] + "\t" + words[2] + "\t" + words[3] + "\t" + words[4] + "\t" + words[5];
                content[1] = simChanged;
                File.WriteAllLines(Global.projectName + "\\InputFiles\\10_fixed_parameters.txt", content); }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Error");
            }
            
        }


        #region mcs start function
        private string[] mcs_start(ComboBox nod, TextBox nomin, TextBox notri, TextBox nomax, int n)
        {
            string[] result = new string[n];
            Random random = new Random();
            int m = nod.SelectedIndex + 1;

            if (1 == m)
            {
                double Xl = Convert.ToDouble(nomin.Text);
                double Xu = Convert.ToDouble(nomax.Text);
                if (Xu < Xl)
                {
                    MessageBox.Show("Xu should be greater than Xl");
                    return null;
                }
                for (int index = 0; index < n; index++)
                {
                    result[index] = (Xl + (Xu - Xl) * random.NextDouble()).ToString("f6");
                }
            }
            else if (2 == m)
            {
                double Xl = Convert.ToDouble(nomin.Text);
                double Xu = Convert.ToDouble(nomax.Text);
                double Finv_Rl = Normal.InvCDF(0, 1, 0.1 / 100);
                double Finv_Ru = Normal.InvCDF(0, 1, 99.9 / 100);
                double mu = (Math.Log(Xu) * Finv_Rl - Math.Log(Xl) * Finv_Ru) / (Finv_Rl - Finv_Ru);
                double sigma = Math.Log(Xu / Xl) / (Finv_Ru - Finv_Rl);

                for (int index = 0; index < n; index++)
                {
                    result[index] = LogNormal.InvCDF(mu, sigma, random.NextDouble()).ToString("f6");
                }
            }
            else if (m == 3)
            {
                double mu = Convert.ToDouble(nomin.Text);
                double sigma = Convert.ToDouble(nomax.Text);
                for (int index = 0; index < n; index++)
                {
                    result[index] = LogNormal.InvCDF(mu, sigma, random.NextDouble()).ToString("f6");
                }
            }
            else if (m == 4)
            {
                double a = Convert.ToDouble(nomin.Text);
                double b = Convert.ToDouble(nomax.Text);
                double c = Convert.ToDouble(notri.Text);
                double Fc = (c - a) / (b - a);
                double ranNum = 0;
                for (int index = 0; index < n; index++)
                {
                    ranNum = random.NextDouble();
                    if (ranNum < Fc)
                    {
                        result[index] = (a + Math.Sqrt(ranNum * (b - a) * (c - a))).ToString("f6");
                    }
                    else
                    {
                        result[index] = (b - Math.Sqrt((1 - ranNum) * (b - a) * (b - c))).ToString("f6");
                    }
                }
            }
            int type = Convert.ToInt16(nod.SelectedIndex);
            double minValue = Convert.ToDouble(nomin.Text);
            double maxValue = Convert.ToDouble(nomax.Text);
            double cValue = Convert.ToDouble(notri.Text);
            carbonParameters.Add(new Parameters(type, minValue, maxValue, cValue));
            return result;
        }
        #endregion

        //Only accept float number and one point
        private void no01min_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char ch = Convert.ToChar(e.Text);
            TextBox tb = (TextBox)sender;
            if (ch == 46 && tb.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }
            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        #region selection change event
        private void no01d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no01d != null && no01tri != null)
            {
                if (no01d.SelectedIndex == 3)
                {
                    no01tri.IsEnabled = true;
                }
                else
                {
                    no01tri.IsEnabled = false;
                }
            }
        }

        private void no02d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no02d != null && no02tri != null)
            {
                if (no02d.SelectedIndex == 3)
                {
                    no02tri.IsEnabled = true;
                }
                else
                {
                    no02tri.IsEnabled = false;
                }
            }
        }

        private void no03d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no03d != null && no03tri != null)
            {
                if (no03d.SelectedIndex == 3)
                {
                    no03tri.IsEnabled = true;
                }
                else
                {
                    no03tri.IsEnabled = false;
                }
            }
        }

        private void no04d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no04d != null && no04tri != null)
            {
                if (no04d.SelectedIndex == 3)
                {
                    no04tri.IsEnabled = true;
                }
                else
                {
                    no04tri.IsEnabled = false;
                }
            }
        }

        private void no05d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no05d != null && no05tri != null)
            {
                if (no05d.SelectedIndex == 3)
                {
                    no05tri.IsEnabled = true;
                }
                else
                {
                    no05tri.IsEnabled = false;
                }
            }
        }

        private void no06d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no06d != null && no06tri != null)
            {
                if (no06d.SelectedIndex == 3)
                {
                    no06tri.IsEnabled = true;
                }
                else
                {
                    no06tri.IsEnabled = false;
                }
            }
        }

        private void no07d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no07d != null && no07tri != null)
            {
                if (no07d.SelectedIndex == 3)
                {
                    no07tri.IsEnabled = true;
                }
                else
                {
                    no07tri.IsEnabled = false;
                }
            }
        }

        private void no08d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no08d != null && no08tri != null)
            {
                if (no08d.SelectedIndex == 3)
                {
                    no08tri.IsEnabled = true;
                }
                else
                {
                    no08tri.IsEnabled = false;
                }
            }
        }

        private void no09d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no09d != null && no09tri != null)
            {
                if (no09d.SelectedIndex == 3)
                {
                    no09tri.IsEnabled = true;
                }
                else
                {
                    no09tri.IsEnabled = false;
                }
            }
        }

        private void no10d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no10d != null && no10tri != null)
            {
                if (no10d.SelectedIndex == 3)
                {
                    no10tri.IsEnabled = true;
                }
                else
                {
                    no10tri.IsEnabled = false;
                }
            }
        }

        private void no11d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no11d != null && no11tri != null)
            {
                if (no11d.SelectedIndex == 3)
                {
                    no11tri.IsEnabled = true;
                }
                else
                {
                    no11tri.IsEnabled = false;
                }
            }
        }

        private void no12d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no12d != null && no12tri != null)
            {
                if (no12d.SelectedIndex == 3)
                {
                    no12tri.IsEnabled = true;
                }
                else
                {
                    no12tri.IsEnabled = false;
                }
            }
        }

        private void no13d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no13d != null && no13tri != null)
            {
                if (no13d.SelectedIndex == 3)
                {
                    no13tri.IsEnabled = true;
                }
                else
                {
                    no13tri.IsEnabled = false;
                }
            }
        }

        private void no14d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no14d != null && no14tri != null)
            {
                if (no14d.SelectedIndex == 3)
                {
                    no14tri.IsEnabled = true;
                }
                else
                {
                    no14tri.IsEnabled = false;
                }
            }
        }

        private void no15d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no15d != null && no15tri != null)
            {
                if (no15d.SelectedIndex == 3)
                {
                    no15tri.IsEnabled = true;
                }
                else
                {
                    no15tri.IsEnabled = false;
                }
            }
        }

        private void no16d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no16d != null && no16tri != null)
            {
                if (no16d.SelectedIndex == 3)
                {
                    no16tri.IsEnabled = true;
                }
                else
                {
                    no16tri.IsEnabled = false;
                }
            }
        }

        private void no17d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no17d != null && no17tri != null)
            {
                if (no17d.SelectedIndex == 3)
                {
                    no17tri.IsEnabled = true;
                }
                else
                {
                    no17tri.IsEnabled = false;
                }
            }
        }

        private void no18d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no18d != null && no18tri != null)
            {
                if (no18d.SelectedIndex == 3)
                {
                    no18tri.IsEnabled = true;
                }
                else
                {
                    no18tri.IsEnabled = false;
                }
            }
        }

        private void no19d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no19d != null && no19tri != null)
            {
                if (no19d.SelectedIndex == 3)
                {
                    no19tri.IsEnabled = true;
                }
                else
                {
                    no19tri.IsEnabled = false;
                }
            }
        }

        private void no20d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no20d != null && no20tri != null)
            {
                if (no20d.SelectedIndex == 3)
                {
                    no20tri.IsEnabled = true;
                }
                else
                {
                    no20tri.IsEnabled = false;
                }
            }
        }

        private void no21d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no21d != null && no21tri != null)
            {
                if (no21d.SelectedIndex == 3)
                {
                    no21tri.IsEnabled = true;
                }
                else
                {
                    no21tri.IsEnabled = false;
                }
            }
        }

        private void no22d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no22d != null && no22tri != null)
            {
                if (no22d.SelectedIndex == 3)
                {
                    no22tri.IsEnabled = true;
                }
                else
                {
                    no22tri.IsEnabled = false;
                }
            }
        }

        private void no23d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no23d != null && no23tri != null)
            {
                if (no23d.SelectedIndex == 3)
                {
                    no23tri.IsEnabled = true;
                }
                else
                {
                    no23tri.IsEnabled = false;
                }
            }
        }

        private void no24d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no24d != null && no24tri != null)
            {
                if (no24d.SelectedIndex == 3)
                {
                    no24tri.IsEnabled = true;
                }
                else
                {
                    no24tri.IsEnabled = false;
                }
            }
        }
        #endregion

        private void numeric(object sender, TextCompositionEventArgs e)
        {
            char ch = Convert.ToChar(e.Text);
            TextBox tb = (TextBox)sender;
            if (!Char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private bool checkOne(TextBox boxData)
        {
            if (boxData.Text.Length == 0)
            {
                boxData.BorderBrush = Brushes.Red;
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
                return false;
            }
        }
        private bool checkRow(ComboBox index, TextBox min, TextBox max, TextBox mid)
        {
            bool result = checkOne(min) && checkOne(max) && checkOne(mid);
            double dmin = Convert.ToDouble(min.Text);
            double dmax = Convert.ToDouble(max.Text);
            double dmid = Convert.ToDouble(mid.Text);
            if (result == false)
            {
                return result;
            }
            if (index.SelectedIndex == 0 || index.SelectedIndex == 1)
            {
                if (dmin > dmax)
                {
                    min.BorderBrush = Brushes.Red;
                    max.BorderBrush = Brushes.Red;
                    return false;
                }
                min.BorderBrush = Brushes.Black;
                max.BorderBrush = Brushes.Black;
            }
            else if (index.SelectedIndex == 3)
            {
                if (!(dmin <= dmid && dmid <= dmax))
                {
                    min.BorderBrush = Brushes.Red;
                    max.BorderBrush = Brushes.Red;
                    mid.BorderBrush = Brushes.Red;
                    return false;
                }
                min.BorderBrush = Brushes.Black;
                max.BorderBrush = Brushes.Black;
                mid.BorderBrush = Brushes.Black;
            }
            return true;
        }
        private bool checkAll()
        {
            bool result = true;
            result = result && checkRow(no01d, no01min, no01max, no01tri);
            result = result && checkRow(no02d, no02min, no02max, no02tri);
            result = result && checkRow(no03d, no03min, no03max, no03tri);
            result = result && checkRow(no04d, no04min, no04max, no04tri);
            result = result && checkRow(no05d, no05min, no05max, no05tri);
            result = result && checkRow(no06d, no06min, no06max, no06tri);
            result = result && checkRow(no07d, no07min, no07max, no07tri);
            result = result && checkRow(no08d, no08min, no08max, no08tri);
            result = result && checkRow(no09d, no09min, no09max, no09tri);
            result = result && checkRow(no10d, no10min, no10max, no10tri);
            result = result && checkRow(no11d, no11min, no11max, no11tri);
            result = result && checkRow(no12d, no12min, no12max, no12tri);
            result = result && checkRow(no13d, no13min, no13max, no13tri);
            result = result && checkRow(no14d, no14min, no14max, no14tri);
            result = result && checkRow(no15d, no15min, no15max, no15tri);
            result = result && checkRow(no16d, no16min, no16max, no16tri);
            result = result && checkRow(no17d, no17min, no17max, no17tri);
            result = result && checkRow(no18d, no18min, no18max, no18tri);
            result = result && checkRow(no19d, no19min, no19max, no19tri);
            result = result && checkRow(no20d, no20min, no20max, no20tri);
            result = result && checkRow(no21d, no21min, no21max, no21tri);
            result = result && checkRow(no22d, no22min, no22max, no22tri);
            result = result && checkRow(no23d, no23min, no23max, no23tri);
            result = result && checkRow(no24d, no24min, no24max, no24tri);
            result = result && checkOne(sim);
            return result;
        }

        private void btnNitrogen_Click(object sender, RoutedEventArgs e)
        {
            Nitrogen nw = new Nitrogen();
            nw.Show();
            this.Close();
        }
        private void addOneRowData(ComboBox distribution, TextBox min, TextBox max, TextBox c)
        {
            string row = distribution.SelectedIndex.ToString() + "\t" + min.Text.ToString() + "\t" + max.Text.ToString() + "\t" + c.Text.ToString() + "\r\n";
           

            

            try
            {
                File.AppendAllText(Global.projectName + "\\InputFiles\\System\\15mon.bat", row);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error");
            }
        }

        private void readOneRowData(String row, ComboBox distribution, TextBox min, TextBox max, TextBox c)
        {
            string[] parameters = row.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
            if (parameters.Length == 4)
            {
                distribution.SelectedIndex = Convert.ToInt32(parameters[0]);
                min.Text = parameters[1];
                max.Text = parameters[2];
                c.Text = parameters[3];
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                File.WriteAllText(Global.projectName + "\\InputFiles\\System\\15mon.bat", "");
                addOneRowData(no01d, no01min, no01max, no01tri);
                addOneRowData(no02d, no02min, no02max, no02tri);
                addOneRowData(no03d, no03min, no03max, no03tri);
                addOneRowData(no04d, no04min, no04max, no04tri);
                addOneRowData(no05d, no05min, no05max, no05tri);
                addOneRowData(no06d, no06min, no06max, no06tri);
                addOneRowData(no07d, no07min, no07max, no07tri);
                addOneRowData(no08d, no08min, no08max, no08tri);
                addOneRowData(no09d, no09min, no09max, no09tri);
                addOneRowData(no10d, no10min, no10max, no10tri);
                addOneRowData(no11d, no11min, no11max, no11tri);
                addOneRowData(no12d, no12min, no12max, no12tri);
                addOneRowData(no13d, no13min, no13max, no13tri);
                addOneRowData(no14d, no14min, no14max, no14tri);
                addOneRowData(no15d, no15min, no15max, no15tri);
                addOneRowData(no16d, no16min, no16max, no16tri);
                addOneRowData(no17d, no17min, no17max, no17tri);
                addOneRowData(no18d, no18min, no18max, no18tri);
                addOneRowData(no19d, no19min, no19max, no19tri);
                addOneRowData(no20d, no20min, no20max, no20tri);
                addOneRowData(no21d, no21min, no21max, no21tri);
                addOneRowData(no22d, no22min, no22max, no22tri);
                addOneRowData(no23d, no23min, no23max, no23tri);
                addOneRowData(no24d, no24min, no24max, no24tri);
                File.AppendAllText(Global.projectName + "\\InputFiles\\System\\15mon.bat", sim.Text);

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error!");
            }
        }
        private void readData()
        {
            if (File.Exists(Global.projectName + "\\InputFiles\\System\\15mon.bat"))
            {
                try
                {
                    string[] rows = File.ReadAllLines(Global.projectName + "\\InputFiles\\System\\15mon.bat");
                    if (rows.Length == 25)
                    {
                        readOneRowData(rows[0], no01d, no01min, no01max, no01tri);
                        readOneRowData(rows[1], no02d, no02min, no02max, no02tri);
                        readOneRowData(rows[2], no03d, no03min, no03max, no03tri);
                        readOneRowData(rows[3], no04d, no04min, no04max, no04tri);
                        readOneRowData(rows[4], no05d, no05min, no05max, no05tri);
                        readOneRowData(rows[5], no06d, no06min, no06max, no06tri);
                        readOneRowData(rows[6], no07d, no07min, no07max, no07tri);
                        readOneRowData(rows[7], no08d, no08min, no08max, no08tri);
                        readOneRowData(rows[8], no09d, no09min, no09max, no09tri);
                        readOneRowData(rows[9], no10d, no10min, no10max, no10tri);
                        readOneRowData(rows[10], no11d, no11min, no11max, no11tri);
                        readOneRowData(rows[11], no12d, no12min, no12max, no12tri);
                        readOneRowData(rows[12], no13d, no13min, no13max, no13tri);
                        readOneRowData(rows[13], no14d, no14min, no14max, no14tri);
                        readOneRowData(rows[14], no15d, no15min, no15max, no15tri);
                        readOneRowData(rows[15], no16d, no16min, no16max, no16tri);
                        readOneRowData(rows[16], no17d, no17min, no17max, no17tri);
                        readOneRowData(rows[17], no18d, no18min, no18max, no18tri);
                        readOneRowData(rows[18], no19d, no19min, no19max, no19tri);
                        readOneRowData(rows[19], no20d, no20min, no20max, no20tri);
                        readOneRowData(rows[20], no21d, no21min, no21max, no21tri);
                        readOneRowData(rows[21], no22d, no22min, no22max, no22tri);
                        readOneRowData(rows[22], no23d, no23min, no23max, no23tri);
                        readOneRowData(rows[23], no24d, no24min, no24max, no24tri);
                        //sim.Text = rows[24].Trim();
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message, "Error!");
                }
            }
        }

        private void default_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Global.projectName + "\\InputFiles\\System\\15mon.bat"))
            {
                File.Delete(Global.projectName + "\\InputFiles\\System\\15mon.bat");
            }
            Carbon carbon = new Carbon();
            carbon.Show();
            this.Close();

        }
    }
}
