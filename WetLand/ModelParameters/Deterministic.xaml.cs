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
    /// Interaction logic for Deterministic.xaml
    /// </summary>
    using System.IO;
    public partial class Deterministic : Window
    {
        public Deterministic()
        {
            InitializeComponent();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {

            if (checkAllData())
            {
                string writeline14 = box02.Text + "\t" + box04.Text + "\t" + box05.Text + "\t" + box06.Text + "\t" + box07.Text + "\t" + box08.Text + "\t" + box09.Text + "\t" + box10.Text + "\t" + box11.Text + "\t" + box12.Text + "\t" + box13.Text + "\t" + box14.Text + "\t" + box15.Text + "\t" + box16.Text + "\t" + box17.Text + "\t" + box18.Text + "\t" + box19.Text + "\t" + box20.Text + "\t" + box21.Text + "\t" + box22.Text + "\t" + box23.Text + "\t" + box24.Text + "\t" + box25.Text + "\t" + box26.Text + "\t" + box27.Text + "\t" + box28.Text + "\t" + box29.Text + "\t" + box30.Text + "\t" + box31.Text + "\t" + box32.Text + "\t" + box33.Text + "\t" + box34.Text + "\t" + box35.Text + "\t" + box36.Text + "\t" + box37.Text + "\t" + box38.Text + "\t" + box39.Text + "\t" + box40.Text;
                string writeline15 = box_01.Text + "\t" + box_02.Text + "\t" + box_03.Text + "\t" + box_04.Text + "\t" + box_05.Text + "\t" + box_06.Text + "\t" + box_07.Text + "\t" + box_08.Text + "\t" + box_09.Text + "\t" + box_10.Text + "\t" + box_11.Text + "\t" + box_12.Text + "\t" + box_13.Text + "\t" + box_14.Text + "\t" + box_15.Text + "\t" + box_16.Text + "\t" + box_17.Text + "\t" + box_18.Text + "\t" + box_19.Text + "\t" + box_20.Text + "\t" + box_21.Text + "\t" + box_22.Text + "\t" + box_23.Text + "\t" + box_24.Text;
                string[] content;
                try
                {
                    content = File.ReadAllLines(Global.projectName + "\\InputFiles\\10_fixed_parameters.txt");
                }
                catch (Exception exc)
                {
                    Status.Visibility = Visibility.Visible;
                    Status.Text = exc.Message;
                    return;
                }
                string[] words = content[1].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                words[5] = "1";
                string simChanged = words[0] + "\t" + words[1] + "\t" + words[2] + "\t" + words[3] + "\t" + words[4] + "\t" + words[5] + "\t";
                content[1] = simChanged;
                File.WriteAllLines(Global.projectName + "\\InputFiles\\10_fixed_parameters.txt", content);
                string title14 = "L2\ttheta\tIs\tfNup\tkd\tkep\tkga0\tkgb0\tkmin1s\tknw\tkminw\tkns\tkden\trowp\tvels_o\tvels_s  \tvelb\tana\trChl\tSs\tSw\tc_Uw\tfrap\tc1\tc2\tPH\tS\tKw\tapa\tDnp\tKsa\tKsb\tRanN1\tfW\tfact  \ta_vr_o\ta_vr_s\tporw";
                string title15 = "aca\tFaDOC\tFaLPOC\tFaRPOC\tFbDOC\tFbLPOC\tFbRPOC\tkLPOC\tkRPOC\tKsatO\tKinO\tKN\tKinN\tK1DOC\tk2DOC\tk3DOC\tk4DOC\tcp1\tcp2\tcp3\tfbw\tk1CH4\tk2CH4\tRveg\r\nN.A.\tN.A.\tN.A.\tN.A.\tN.A.\tN.A.\tN.A.\t1/day\t1/day\tmgO/lit\tmg/lit\tmg/lit\tmg/lit\t1/day\t1/day\t1/day\t1/day\tN.A.\tN.A.\tN.A.\tN.A\t1/day\t1/day\tcm root/ gr chla";
                try
                {
                    File.WriteAllText(Global.projectName + "\\InputFiles\\14_generated_parameters.txt", title14 + "\r\n" + writeline14);
                    File.WriteAllText(Global.projectName + "\\InputFiles\\System\\14determin.bat", writeline14);
                    File.WriteAllText(Global.projectName + "\\InputFiles\\15_generated_parameters_carbon.txt", title15 + "\r\n" + writeline15);
                    File.WriteAllText(Global.projectName + "\\InputFiles\\System\\15determin.bat", writeline15);
                }
                catch (Exception) { }
                Status.Visibility = Visibility.Visible;
                Status.Text = "Data Saved Successfully";
                //Global.DeterminMode = true;
                Global.mainWin.menu_run.IsEnabled = true;

                Global.mainWin.KSTest.IsEnabled = false;
                //show Simulation stackpannel
            }
        }

        private void box01_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private bool checkAllData()
        {
            bool result = true;
            result = result && checkOne(box02) && checkOne(box04) && checkOne(box05) && checkOne(box06) && checkOne(box07) && checkOne(box08);
            result = result && checkOne(box09) && checkOne(box10) && checkOne(box11) && checkOne(box12) && checkOne(box13) && checkOne(box14);
            result = result && checkOne(box15) && checkOne(box16) && checkOne(box17) && checkOne(box18) && checkOne(box19) && checkOne(box20);
            result = result && checkOne(box21) && checkOne(box22) && checkOne(box23) && checkOne(box24) && checkOne(box25) && checkOne(box26);
            result = result && checkOne(box27) && checkOne(box28) && checkOne(box29) && checkOne(box30) && checkOne(box31) && checkOne(box32);
            result = result && checkOne(box33) && checkOne(box34) && checkOne(box35) && checkOne(box36) && checkOne(box37) && checkOne(box38);
            result = result && checkOne(box39) && checkOne(box40);

            result = result && checkOne(box_02) && checkOne(box_04) && checkOne(box_05) && checkOne(box_06) && checkOne(box_07) && checkOne(box_08);
            result = result && checkOne(box_09) && checkOne(box_10) && checkOne(box_11) && checkOne(box_12) && checkOne(box_13) && checkOne(box_14);
            result = result && checkOne(box_15) && checkOne(box_16) && checkOne(box_17) && checkOne(box_18) && checkOne(box_19) && checkOne(box_20);
            result = result && checkOne(box_21) && checkOne(box_22) && checkOne(box_23) && checkOne(box_24) && checkOne(box_01) && checkOne(box_03);

            return result;
        }

        private bool checkOne(TextBox boxData)
        {
            if (boxData.Text.Length == 0)
            {
                boxData.BorderBrush = Brushes.Red;
                Status.Visibility = Visibility.Visible;
                Status.Text = "Field Cannot Be Empty";
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
                Status.Visibility = Visibility.Visible;
                Status.Text = "This Field Can have only Decimal Values";
                return false;
            }
        }
        private void default_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Global.projectName + "\\InputFiles\\System\\14determin.bat"))
            {
                File.Delete(Global.projectName + "\\InputFiles\\System\\14determin.bat");
            }
            if (File.Exists(Global.projectName + "\\InputFiles\\System\\15determin.bat"))
            {
                File.Delete(Global.projectName + "\\InputFiles\\System\\15determin.bat");
            }
            Deterministic fp = new Deterministic();
            fp.Show();
            this.Close();
        }
        private void readData()
        {
            if (File.Exists(Global.projectName + "\\InputFiles\\System\\14determin.bat") && File.Exists(Global.projectName + "\\InputFiles\\System\\15determin.bat"))
            {
                string content14 = File.ReadAllText(Global.projectName + "\\InputFiles\\System\\14determin.bat");
                string content15 = File.ReadAllText(Global.projectName + "\\InputFiles\\System\\15determin.bat");
                string[] parameter14 = content14.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                string[] parameter15 = content15.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                if (parameter14.Length == 38)
                {
                    box02.Text = parameter14[0];
                    box04.Text = parameter14[1];
                    box05.Text = parameter14[2];
                    box06.Text = parameter14[3];
                    box07.Text = parameter14[4];
                    box08.Text = parameter14[5];
                    box09.Text = parameter14[6];
                    box10.Text = parameter14[7];
                    box11.Text = parameter14[8];
                    box12.Text = parameter14[9];
                    box13.Text = parameter14[10];
                    box14.Text = parameter14[11];
                    box15.Text = parameter14[12];
                    box16.Text = parameter14[13];
                    box17.Text = parameter14[14];
                    box18.Text = parameter14[15];
                    box19.Text = parameter14[16];
                    box20.Text = parameter14[17];
                    box21.Text = parameter14[18];
                    box22.Text = parameter14[19];
                    box23.Text = parameter14[20];
                    box24.Text = parameter14[21];
                    box25.Text = parameter14[22];
                    box26.Text = parameter14[23];
                    box27.Text = parameter14[24];
                    box28.Text = parameter14[25];
                    box29.Text = parameter14[26];
                    box30.Text = parameter14[27];
                    box31.Text = parameter14[28];
                    box32.Text = parameter14[29];
                    box33.Text = parameter14[30];
                    box34.Text = parameter14[31];
                    box35.Text = parameter14[32];
                    box36.Text = parameter14[33];
                    box37.Text = parameter14[34];
                    box38.Text = parameter14[35];
                    box39.Text = parameter14[36];
                    box40.Text = parameter14[37];
                }
                if (parameter15.Length == 24)
                {
                    box_01.Text = parameter15[0];
                    box_02.Text = parameter15[1];
                    box_03.Text = parameter15[2];
                    box_04.Text = parameter15[3];
                    box_05.Text = parameter15[4];
                    box_06.Text = parameter15[5];
                    box_07.Text = parameter15[6];
                    box_08.Text = parameter15[7];
                    box_09.Text = parameter15[8];
                    box_10.Text = parameter15[9];
                    box_11.Text = parameter15[10];
                    box_12.Text = parameter15[11];
                    box_13.Text = parameter15[12];
                    box_14.Text = parameter15[13];
                    box_15.Text = parameter15[14];
                    box_16.Text = parameter15[15];
                    box_17.Text = parameter15[16];
                    box_18.Text = parameter15[17];
                    box_19.Text = parameter15[18];
                    box_20.Text = parameter15[19];
                    box_21.Text = parameter15[20];
                    box_22.Text = parameter15[21];
                    box_23.Text = parameter15[22];
                    box_24.Text = parameter15[23];
                }
            }


        }
    }
}
