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
    /// Interaction logic for Nitrogen.xaml
    /// </summary>
    using MathNet.Numerics.Distributions;
    using System.IO;

    public partial class Nitrogen : Window
    {
        public List<Parameters> nitrogenParameters;
        public Nitrogen()
        {
            InitializeComponent();
            nitrogenParameters = new List<Parameters>();
            sim.Text = Global.simulationNumber.ToString();
            readData();
        }

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
        private void numeric(object sender, TextCompositionEventArgs e)
        {
            char ch = Convert.ToChar(e.Text);
            TextBox tb = (TextBox)sender;
            if (!Char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void btnGoToCarbon_Click(object sender, RoutedEventArgs e)
        {
            Carbon carbon = new Carbon();
            carbon.Show();
            this.Close();
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

            File.WriteAllText(Global.projectName + @"\InputFiles\14_generated_parameters.txt", "L2\ttheta\tIs\tfNup\tkd\tkep\tkga0\tkgb0\tkmin1s\tknw\tkminw\tkns\tkden\trowp\tvels_o\tvels_s  \tvelb\tana\trChl\tSs\tSw\tc_Uw\tfrap\tc1\tc2\tPH\tS\tKw\tapa\tDnp\tKsa\tKsb\tRanN1\tfW\tfact  \ta_vr_o\ta_vr_s\tporw\r\n");

            #region mcs start
            while (memory)
            {
                string[][] result = new string[38][];
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
                nitrogenParameters.Clear();
                result[0] = mcs_start(no02d, no02min, no02tri, no02max, n);
                result[1] = mcs_start(no04d, no04min, no04tri, no04max, n);
                result[2] = mcs_start(no05d, no05min, no05tri, no05max, n);
                result[3] = mcs_start(no06d, no06min, no06tri, no06max, n);
                result[4] = mcs_start(no07d, no07min, no07tri, no07max, n);
                result[5] = mcs_start(no08d, no08min, no08tri, no08max, n);
                result[6] = mcs_start(no09d, no09min, no09tri, no09max, n);
                result[7] = mcs_start(no10d, no10min, no10tri, no10max, n);
                result[8] = mcs_start(no11d, no11min, no11tri, no11max, n);
                result[9] = mcs_start(no12d, no12min, no12tri, no12max, n);
                result[10] = mcs_start(no13d, no13min, no13tri, no13max, n);
                result[11] = mcs_start(no14d, no14min, no14tri, no14max, n);
                result[12] = mcs_start(no15d, no15min, no15tri, no15max, n);
                result[13] = mcs_start(no16d, no16min, no16tri, no16max, n);
                result[14] = mcs_start(no17d, no17min, no17tri, no17max, n);
                result[15] = mcs_start(no18d, no18min, no18tri, no18max, n);
                result[16] = mcs_start(no19d, no19min, no19tri, no19max, n);
                result[17] = mcs_start(no20d, no20min, no20tri, no20max, n);
                result[18] = mcs_start(no21d, no21min, no21tri, no21max, n);
                result[19] = mcs_start(no22d, no22min, no22tri, no22max, n);
                result[20] = mcs_start(no23d, no23min, no23tri, no23max, n);
                result[21] = mcs_start(no24d, no24min, no24tri, no24max, n);
                result[22] = mcs_start(no25d, no25min, no25tri, no25max, n);
                result[23] = mcs_start(no26d, no26min, no26tri, no26max, n);
                result[24] = mcs_start(no27d, no27min, no27tri, no27max, n);
                result[25] = mcs_start(no28d, no28min, no28tri, no28max, n);
                result[26] = mcs_start(no29d, no29min, no29tri, no29max, n);
                result[27] = mcs_start(no30d, no30min, no30tri, no30max, n);
                result[28] = mcs_start(no31d, no31min, no31tri, no31max, n);
                result[29] = mcs_start(no32d, no32min, no32tri, no32max, n);
                result[30] = mcs_start(no33d, no33min, no33tri, no33max, n);
                result[31] = mcs_start(no34d, no34min, no34tri, no34max, n);
                result[32] = mcs_start(no35d, no35min, no35tri, no35max, n);
                result[33] = mcs_start(no36d, no36min, no36tri, no36max, n);
                result[34] = mcs_start(no37d, no37min, no37tri, no37max, n);
                result[35] = mcs_start(no38d, no38min, no38tri, no38max, n);
                result[36] = mcs_start(no39d, no39min, no39tri, no39max, n);
                result[37] = mcs_start(no40d, no40min, no40tri, no40max, n);
                //Save result
                btnSave_Click(sender, e);
                Global.tempNitrogen = new List<Parameters>(nitrogenParameters);
                string[] line = new string[n];
                for (int index = 0; index < n; index++)
                {
                    line[index] = result[0][index] + "	" + result[1][index] + "	" + result[2][index] + "	" + result[3][index] + "	" + result[4][index] + "	" + result[5][index] + "	" + result[6][index] + "	" + result[7][index] + "	" + result[8][index] + "	" + result[9][index] + "	" + result[10][index] + "	" + result[11][index] + "	" + result[12][index] + "	" + result[13][index] + "	" + result[14][index] + "	" + result[15][index] + "	" + result[16][index] + "	" + result[17][index] + "	" + result[18][index] + "	" + result[19][index] + "	" + result[20][index] + "	" + result[21][index] + "	" + result[22][index] + "	" + result[23][index] + "	" + result[24][index] + "	" + result[25][index] + "	" + result[26][index] + "	" + result[27][index] + "	" + result[28][index] + "	" + result[29][index] + "	" + result[30][index] + "	" + result[31][index] + "	" + result[32][index] + "	" + result[33][index] + "	" + result[34][index] + "	" + result[35][index] + "	" + result[36][index] + "	" + result[37][index];
                }
                File.AppendAllLines(Global.projectName + @"\InputFiles\14_generated_parameters.txt", line);
            }
            UpdateSimInFixedParameter(simNum);
            //Global.DeterminMode = false;

            Global.mainWin.menu_run.IsEnabled = true;
            Global.mainWin.KSTest.IsEnabled = false;
            MessageBox.Show("All parameters for Nitrogen and Phosphorus have been generated.", "Completed!");
        }

        private static void UpdateSimInFixedParameter(int simNum)
        {
            try
            {
                string[] content = File.ReadAllLines(Global.projectName + "\\InputFiles\\10_fixed_parameters.txt");
                string[] parameters = content[1].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                parameters[5] = simNum.ToString();
                string simChanged = parameters[0] + "\t" + parameters[1] + "\t" + parameters[2] + "\t" + parameters[3] + "\t" + parameters[4] + "\t" + parameters[5];
                content[1] = simChanged;
                File.WriteAllLines(Global.projectName + "\\InputFiles\\10_fixed_parameters.txt", content);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error");
            }
            
        }
        #endregion mcs end

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
            nitrogenParameters.Add(new Parameters(type, minValue, maxValue, cValue));

            return result;
        }
        #endregion

        #region selection change event
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

        private void no25d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no25d != null && no25tri != null)
            {
                if (no25d.SelectedIndex == 3)
                {
                    no25tri.IsEnabled = true;
                }
                else
                {
                    no25tri.IsEnabled = false;
                }
            }
        }

        private void no26d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no26d != null && no26tri != null)
            {
                if (no26d.SelectedIndex == 3)
                {
                    no26tri.IsEnabled = true;
                }
                else
                {
                    no26tri.IsEnabled = false;
                }
            }
        }

        private void no27d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no27d != null && no27tri != null)
            {
                if (no27d.SelectedIndex == 3)
                {
                    no27tri.IsEnabled = true;
                }
                else
                {
                    no27tri.IsEnabled = false;
                }
            }
        }

        private void no28d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no28d != null && no28tri != null)
            {
                if (no28d.SelectedIndex == 3)
                {
                    no28tri.IsEnabled = true;
                }
                else
                {
                    no28tri.IsEnabled = false;
                }
            }
        }

        private void no29d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no29d != null && no29tri != null)
            {
                if (no29d.SelectedIndex == 3)
                {
                    no29tri.IsEnabled = true;
                }
                else
                {
                    no29tri.IsEnabled = false;
                }
            }
        }

        private void no30d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no30d != null && no30tri != null)
            {
                if (no30d.SelectedIndex == 3)
                {
                    no30tri.IsEnabled = true;
                }
                else
                {
                    no30tri.IsEnabled = false;
                }
            }
        }

        private void no31d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no31d != null && no31tri != null)
            {
                if (no31d.SelectedIndex == 3)
                {
                    no31tri.IsEnabled = true;
                }
                else
                {
                    no31tri.IsEnabled = false;
                }
            }
        }

        private void no32d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no32d != null && no32tri != null)
            {
                if (no32d.SelectedIndex == 3)
                {
                    no32tri.IsEnabled = true;
                }
                else
                {
                    no32tri.IsEnabled = false;
                }
            }
        }

        private void no33d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no33d != null && no33tri != null)
            {
                if (no33d.SelectedIndex == 3)
                {
                    no33tri.IsEnabled = true;
                }
                else
                {
                    no33tri.IsEnabled = false;
                }
            }
        }

        private void no34d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no34d != null && no34tri != null)
            {
                if (no34d.SelectedIndex == 3)
                {
                    no34tri.IsEnabled = true;
                }
                else
                {
                    no34tri.IsEnabled = false;
                }
            }
        }

        private void no35d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no35d != null && no35tri != null)
            {
                if (no35d.SelectedIndex == 3)
                {
                    no35tri.IsEnabled = true;
                }
                else
                {
                    no35tri.IsEnabled = false;
                }
            }
        }

        private void no36d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no36d != null && no36tri != null)
            {
                if (no36d.SelectedIndex == 3)
                {
                    no36tri.IsEnabled = true;
                }
                else
                {
                    no36tri.IsEnabled = false;
                }
            }
        }

        private void no37d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no37d != null && no37tri != null)
            {
                if (no37d.SelectedIndex == 3)
                {
                    no37tri.IsEnabled = true;
                }
                else
                {
                    no37tri.IsEnabled = false;
                }
            }
        }

        private void no38d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no38d != null && no38tri != null)
            {
                if (no38d.SelectedIndex == 3)
                {
                    no38tri.IsEnabled = true;
                }
                else
                {
                    no38tri.IsEnabled = false;
                }
            }
        }

        private void no39d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no39d != null && no39tri != null)
            {
                if (no39d.SelectedIndex == 3)
                {
                    no39tri.IsEnabled = true;
                }
                else
                {
                    no39tri.IsEnabled = false;
                }
            }
        }

        private void no40d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (no40d != null && no40tri != null)
            {
                if (no40d.SelectedIndex == 3)
                {
                    no40tri.IsEnabled = true;
                }
                else
                {
                    no40tri.IsEnabled = false;
                }
            }
        }
        #endregion

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
            result = result && checkRow(no02d, no02min, no02max, no02tri);
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
            result = result && checkRow(no25d, no25min, no25max, no25tri);
            result = result && checkRow(no26d, no26min, no26max, no26tri);
            result = result && checkRow(no27d, no27min, no27max, no27tri);
            result = result && checkRow(no28d, no28min, no28max, no28tri);
            result = result && checkRow(no29d, no29min, no29max, no29tri);
            result = result && checkRow(no30d, no30min, no30max, no30tri);
            result = result && checkRow(no31d, no31min, no31max, no31tri);
            result = result && checkRow(no32d, no32min, no32max, no32tri);
            result = result && checkRow(no33d, no33min, no33max, no33tri);
            result = result && checkRow(no34d, no34min, no34max, no34tri);
            result = result && checkRow(no35d, no35min, no35max, no35tri);
            result = result && checkRow(no36d, no36min, no36max, no36tri);
            result = result && checkRow(no37d, no37min, no37max, no37tri);
            result = result && checkRow(no38d, no38min, no38max, no38tri);
            result = result && checkRow(no39d, no39min, no39max, no39tri);
            result = result && checkRow(no40d, no40min, no40max, no40tri);
            result = result && checkOne(sim);
            return result;
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

        private void addOneRowData(ComboBox distribution, TextBox min, TextBox max, TextBox c)
        {
            string row = distribution.SelectedIndex.ToString() + "\t" + min.Text.ToString() + "\t" + max.Text.ToString() + "\t" + c.Text.ToString() + "\r\n";
            try
            {
                File.AppendAllText(Global.projectName + "\\InputFiles\\System\\14mon.bat", row);
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
                File.WriteAllText(Global.projectName + "\\InputFiles\\System\\14mon.bat", "");
                addOneRowData(no02d, no02min, no02max, no02tri);
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
                addOneRowData(no25d, no25min, no25max, no25tri);
                addOneRowData(no26d, no26min, no26max, no26tri);
                addOneRowData(no27d, no27min, no27max, no27tri);
                addOneRowData(no28d, no28min, no28max, no28tri);
                addOneRowData(no29d, no29min, no29max, no29tri);
                addOneRowData(no30d, no30min, no30max, no30tri);
                addOneRowData(no31d, no31min, no31max, no31tri);
                addOneRowData(no32d, no32min, no32max, no32tri);
                addOneRowData(no33d, no33min, no33max, no33tri);
                addOneRowData(no34d, no34min, no34max, no34tri);
                addOneRowData(no35d, no35min, no35max, no35tri);
                addOneRowData(no36d, no36min, no36max, no36tri);
                addOneRowData(no37d, no37min, no37max, no37tri);
                addOneRowData(no38d, no38min, no38max, no38tri);
                addOneRowData(no39d, no39min, no39max, no39tri);
                addOneRowData(no40d, no40min, no40max, no40tri);
                File.AppendAllText(Global.projectName + "\\InputFiles\\System\\14mon.bat", sim.Text);

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error!");
            }
        }

        private void readData()
        {
            if (File.Exists(Global.projectName + "\\InputFiles\\System\\14mon.bat"))
            {
                try
                {
                    string[] rows = File.ReadAllLines(Global.projectName + "\\InputFiles\\System\\14mon.bat");
                    if (rows.Length == 39)
                    {
                        readOneRowData(rows[0], no02d, no02min, no02max, no02tri);
                        readOneRowData(rows[1], no04d, no04min, no04max, no04tri);
                        readOneRowData(rows[2], no05d, no05min, no05max, no05tri);
                        readOneRowData(rows[3], no06d, no06min, no06max, no06tri);
                        readOneRowData(rows[4], no07d, no07min, no07max, no07tri);
                        readOneRowData(rows[5], no08d, no08min, no08max, no08tri);
                        readOneRowData(rows[6], no09d, no09min, no09max, no09tri);
                        readOneRowData(rows[7], no10d, no10min, no10max, no10tri);
                        readOneRowData(rows[8], no11d, no11min, no11max, no11tri);
                        readOneRowData(rows[9], no12d, no12min, no12max, no12tri);
                        readOneRowData(rows[10], no13d, no13min, no13max, no13tri);
                        readOneRowData(rows[11], no14d, no14min, no14max, no14tri);
                        readOneRowData(rows[12], no15d, no15min, no15max, no15tri);
                        readOneRowData(rows[13], no16d, no16min, no16max, no16tri);
                        readOneRowData(rows[14], no17d, no17min, no17max, no17tri);
                        readOneRowData(rows[15], no18d, no18min, no18max, no18tri);
                        readOneRowData(rows[16], no19d, no19min, no19max, no19tri);
                        readOneRowData(rows[17], no20d, no20min, no20max, no20tri);
                        readOneRowData(rows[18], no21d, no21min, no21max, no21tri);
                        readOneRowData(rows[19], no22d, no22min, no22max, no22tri);
                        readOneRowData(rows[20], no23d, no23min, no23max, no23tri);
                        readOneRowData(rows[21], no24d, no24min, no24max, no24tri);
                        readOneRowData(rows[22], no25d, no25min, no25max, no25tri);
                        readOneRowData(rows[23], no26d, no26min, no26max, no26tri);
                        readOneRowData(rows[24], no27d, no27min, no27max, no27tri);
                        readOneRowData(rows[25], no28d, no28min, no28max, no28tri);
                        readOneRowData(rows[26], no29d, no29min, no29max, no29tri);
                        readOneRowData(rows[27], no30d, no30min, no30max, no30tri);
                        readOneRowData(rows[28], no31d, no31min, no31max, no31tri);
                        readOneRowData(rows[29], no32d, no32min, no32max, no32tri);
                        readOneRowData(rows[30], no33d, no33min, no33max, no33tri);
                        readOneRowData(rows[31], no34d, no34min, no34max, no34tri);
                        readOneRowData(rows[32], no35d, no35min, no35max, no35tri);
                        readOneRowData(rows[33], no36d, no36min, no36max, no36tri);
                        readOneRowData(rows[34], no37d, no37min, no37max, no37tri);
                        readOneRowData(rows[35], no38d, no38min, no38max, no38tri);
                        readOneRowData(rows[36], no39d, no39min, no39max, no39tri);
                        readOneRowData(rows[37], no40d, no40min, no40max, no40tri);
                        //sim.Text = rows[38].Trim();
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
            if (File.Exists(Global.projectName + "\\InputFiles\\System\\14mon.bat"))
            {
                File.Delete(Global.projectName + "\\InputFiles\\System\\14mon.bat");
            }
            Nitrogen mon = new Nitrogen();
            mon.Show();
            this.Close();

        }
    }
}
