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

namespace WetLand.KSTest
{
    /// <summary>
    /// Interaction logic for KSTest.xaml
    /// </summary>
    using Accord.Statistics.Testing;
    using System.IO;
    using System.Collections.ObjectModel;
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using System.Threading;

    using Microsoft.Win32;
    using OxyPlot.Xps;
    using System.Diagnostics;
    public partial class KSTest : Window
    {
        public List<KResult> result;
        public Collection<Item> topTen { set; get; }
        private string obsFilePath = Global.projectName + @"\Observations\";
        private string resultFilePath = Global.projectName + @"\InputFiles\";
        private string[] obsFileName = { "102_obs_Onw.txt", "103_obs_Onss.txt", "104_obs_Onsf.txt", "105_obs_Nw.txt", "106_obs_Ns1.txt", "107_obs_Ns2.txt", "108_obs_NO3w.txt", "109_obs_NO3s1.txt",
                "110_obs_NO3s2.txt","111_obs_Ow.txt","112_obs_a.txt","113_obs_b.txt","114_obs_Pw.txt","115_obs_Ps1.txt","116_obs_Ps2.txt","118_obs_mw.txt","150_obs_DOCw.txt","151_obs_LPOCw.txt",
                "152_obs_RPOCw.txt","153_obs_DOCs1.txt","154_obs_LPOCs1.txt","155_obs_RPOCs1.txt","156_obs_DOCs2.txt","157_obs_LPOCs2.txt","158_obs_RPOCs2.txt","159_obs_TOCw.txt",
                "160_obs_CH4w.txt","161_obs_CH4s1.txt","162_obs_CH4s2.txt"
        };
        private static string[] resultFileName = { "102_Onw.txt", "103_Onss.txt", "104_Onsf.txt", "105_Nw.txt", "106_Ns1.txt", "107_Ns2.txt", "108_NO3w.txt", "109_NO3s1.txt",
                "110_NO3s2.txt","111_Ow.txt","112_a.txt","113_b.txt","114_Pw.txt","115_Ps1.txt","116_Ps2.txt","118_mw.txt","150_DOCw.txt","151_LPOCw.txt",
                "152_RPOCw.txt","153_DOCs1.txt","154_LPOCs1.txt","155_RPOCs1.txt","156_DOCs2.txt","157_LPOCs2.txt","158_RPOCs2.txt","159_TOCw.txt",
                "160_CH4w.txt","161_CH4s1.txt","162_CH4s2.txt"
        };
        public KSTest()
        {
            InitializeComponent();
            percentage.Text = (Global.percentage * 100).ToString();
            percentPanel.Visibility = Visibility.Hidden;

        }

        private void reportIndex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (report == null || reportIndex == null)
            {
                return;
            }
            menu_print.IsEnabled = true;
            menu_pdf.IsEnabled = true;
            if (reportIndex.SelectedIndex < 0) {
                return;
            }
            if (reportIndex.SelectedIndex == 0)
            {
                var graph = new PlotModel { Title = "DMax", LegendPlacement = LegendPlacement.Outside, LegendPosition = LegendPosition.RightTop, LegendOrientation = LegendOrientation.Vertical };
                graph.Axes.Add(new CategoryAxis { ItemsSource = this.topTen, LabelField = "name" });
                graph.Axes.Add(new LinearAxis { Position = AxisPosition.Left, MinimumPadding = 0, AbsoluteMinimum = 0 });
                graph.Series.Add(new ColumnSeries { Title = "parameters", ItemsSource = this.topTen, ValueField = "Dmax" });
                report.Model = graph;
                return;
            }
            string title = result[reportIndex.SelectedIndex - 1].name;
            if (result[reportIndex.SelectedIndex - 1].p < 0.05)
            {//P value changed
                title = " Model is sensitive to " + title + " parameter.";
            }
            else
            {
                title = " Model is not sensitive to " + title + " parameter.";
            }
            var tmp = new PlotModel { Title = title };
            tmp.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, MajorGridlineStyle = LineStyle.Solid, Title = "Parameter range", TitleFontSize = 15 });
            tmp.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, MajorGridlineStyle = LineStyle.Solid, Title = "Percent", TitleFontSize = 15 });
            Collection<Values> DataB = new Collection<Values>();
            Collection<Values> DataNB = new Collection<Values>();
            List<Double> col = new List<double>();
            for (int i = 0; i < result[reportIndex.SelectedIndex - 1].NBData.Count; i++)
            {

                DataB.Add(new Values { xvalue = result[reportIndex.SelectedIndex - 1].Interp_BData[i], yvalue = (i + 2) * 100 / (result[reportIndex.SelectedIndex - 1].Interp_BData.Count + 1) });
                DataNB.Add(new Values { xvalue = result[reportIndex.SelectedIndex - 1].NBData[i], yvalue = (i + 2) * 100 / (result[reportIndex.SelectedIndex - 1].Interp_BData.Count + 1) });
            }
            // Create a line series
            var Bseries = new LineSeries
            {
                Title = "B",
                StrokeThickness = 1,
                MarkerSize = 3,
                ItemsSource = DataB,
                DataFieldX = "xvalue",
                DataFieldY = "yvalue",
                Color = OxyColors.Blue,
                MarkerStroke = OxyColors.Green,
                MarkerType = MarkerType.None
            };
            var NBseries = new LineSeries
            {
                Title = "NB",
                StrokeThickness = 1,
                MarkerSize = 3,
                ItemsSource = DataNB,
                DataFieldX = "xvalue",
                DataFieldY = "yvalue",
                Color = OxyColors.Red,
                MarkerStroke = OxyColors.Red,
                MarkerType = MarkerType.None
            };

            tmp.Series.Add(Bseries);
            tmp.Series.Add(NBseries);
            report.Model = tmp;


        }
        private void getSimulation(List<int> simulationNum)
        {
            PostProcess.PostProcessData test = new PostProcess.PostProcessData();
            test.getObservedValue(obsFilePath + obsFileName[0]);
            test.getSimulationValue(resultFilePath + resultFileName[0]);
            test.sort();
            for (int i = 0; i < test.simulationSeries.Count; i++)
            {
                simulationNum.Add(test.simulationSeries[i].simulationNumber);
            }
        }

        private void calculation_Click(object sender, RoutedEventArgs e)
        {
            Thread cal = new Thread(calculationThread);
            calculation.Visibility = Visibility.Hidden;
            percentPanel.Visibility = Visibility.Hidden;
            reportIndex.Visibility = Visibility.Hidden;
            progress.Visibility = Visibility.Visible;
            status.Visibility = Visibility.Visible;
            report.Model = null;
            cal.Start();
            
        }
        private void calculationThread()
        {
            try
            {

                List<int> simulationNum = new List<int>();
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 5
                ));
                getSimulation(simulationNum);
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 5
                ));
                double percent = Global.percentage;
                string[] contents = File.ReadAllLines(Global.projectName + @"\InputFiles\14_generated_parameters.txt");
                int bound = (int)Math.Floor((contents.Length - 1) * percent);

                Data14 Bdata14 = new Data14();
                Data14 OriginalBdata14 = new Data14();
                Data14 NBdata14 = new Data14();
                Data14 NBData14Sorted = new Data14();
                Data15 Bdata15 = new Data15();
                Data15 OriginalBdata15 = new Data15();
                Data15 NBdata15 = new Data15();
                Data15 NBData15Sorted = new Data15();
                for (int i = 0; i < bound; i++)
                {
                    Bdata14.addOneRowData(contents[simulationNum[i]]);
                    OriginalBdata14.addOneRowData(contents[simulationNum[i]]);
                }
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 6
                ));
                //NB dataset
                for (int i = bound; i < contents.Length - 1; i++)
                {
                    NBdata14.addOneRowData(contents[simulationNum[i]]);
                    NBData14Sorted.addOneRowData(contents[simulationNum[i]]);
                }
                NBdata14.sort();
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 7
                ));
                contents = File.ReadAllLines(Global.projectName + @"\InputFiles\15_generated_parameters_carbon.txt");
                bound = (int)Math.Floor((contents.Length - 2) * percent);

                for (int i = 0; i < bound; i++)
                {
                    Bdata15.addOneRowData(contents[simulationNum[i] + 1]);
                    OriginalBdata15.addOneRowData(contents[simulationNum[i] + 1]);
                }
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 8
                ));
                for (int i = bound; i < contents.Length - 2; i++)
                {
                    NBdata15.addOneRowData(contents[simulationNum[i] + 1]);
                    NBData15Sorted.addOneRowData(contents[simulationNum[i] + 1]);
                }

                Dispatcher.Invoke(new Action(() =>
                progress.Value = 9
                ));
                Bdata14.sort();
                Bdata15.sort();
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 12
                ));
                //calculate x
                double[] x = new double[Bdata15.aca.Count];
                for (int i = 0; i < x.Length; i++)
                {
                    x[i] = (i + 1.0) / (x.Length + 1.0);
                }
                //calculate m
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 15
                ));
                double[] m = new double[NBdata15.aca.Count];
                for (int i = 0; i < m.Length; i++)
                {
                    m[i] = (i + 1.0) / (m.Length + 1.0);
                }

                Data14 Interp_B14 = new Data14();
                Data15 Interp_B15 = new Data15();

                Data14 Interp_B14Sorted = new Data14();
                Data15 Interp_B15Sorted = new Data15();
                for (int y = 0; y < m.Length; y++)
                {
                    double[] value14 = new double[38];
                    double[] value15 = new double[24];
                    for (int i = 0; i < x.Length - 1; i++)
                    {
                        value14[0] = Bdata14.l2[i] + (Bdata14.l2[i + 1] - Bdata14.l2[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[1] = Bdata14.theta[i] + (Bdata14.theta[i + 1] - Bdata14.theta[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[2] = Bdata14.Is[i] + (Bdata14.Is[i + 1] - Bdata14.Is[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[3] = Bdata14.fnup[i] + (Bdata14.fnup[i + 1] - Bdata14.fnup[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[4] = Bdata14.kd[i] + (Bdata14.kd[i + 1] - Bdata14.kd[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[5] = Bdata14.kep[i] + (Bdata14.kep[i + 1] - Bdata14.kep[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[6] = Bdata14.kga0[i] + (Bdata14.kga0[i + 1] - Bdata14.kga0[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[7] = Bdata14.kgb0[i] + (Bdata14.kgb0[i + 1] - Bdata14.kgb0[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[8] = Bdata14.kmin1s[i] + (Bdata14.kmin1s[i + 1] - Bdata14.kmin1s[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[9] = Bdata14.knw[i] + (Bdata14.knw[i + 1] - Bdata14.knw[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[10] = Bdata14.kminw[i] + (Bdata14.kminw[i + 1] - Bdata14.kminw[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[11] = Bdata14.kns[i] + (Bdata14.kns[i + 1] - Bdata14.kns[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[12] = Bdata14.kden[i] + (Bdata14.kden[i + 1] - Bdata14.kden[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[13] = Bdata14.rowp[i] + (Bdata14.rowp[i + 1] - Bdata14.rowp[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[14] = Bdata14.vels_o[i] + (Bdata14.vels_o[i + 1] - Bdata14.vels_o[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[15] = Bdata14.vels_s[i] + (Bdata14.vels_s[i + 1] - Bdata14.vels_s[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[16] = Bdata14.velb[i] + (Bdata14.velb[i + 1] - Bdata14.velb[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[17] = Bdata14.ana[i] + (Bdata14.ana[i + 1] - Bdata14.ana[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[18] = Bdata14.rchl[i] + (Bdata14.rchl[i + 1] - Bdata14.rchl[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[19] = Bdata14.ss[i] + (Bdata14.ss[i + 1] - Bdata14.ss[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[20] = Bdata14.sw[i] + (Bdata14.sw[i + 1] - Bdata14.sw[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[21] = Bdata14.c_uw[i] + (Bdata14.c_uw[i + 1] - Bdata14.c_uw[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[22] = Bdata14.frap[i] + (Bdata14.frap[i + 1] - Bdata14.frap[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[23] = Bdata14.c1[i] + (Bdata14.c1[i + 1] - Bdata14.c1[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[24] = Bdata14.c2[i] + (Bdata14.c2[i + 1] - Bdata14.c2[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[25] = Bdata14.ph[i] + (Bdata14.ph[i + 1] - Bdata14.ph[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[26] = Bdata14.s[i] + (Bdata14.s[i + 1] - Bdata14.s[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[27] = Bdata14.kw[i] + (Bdata14.kw[i + 1] - Bdata14.kw[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[28] = Bdata14.apa[i] + (Bdata14.apa[i + 1] - Bdata14.apa[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[29] = Bdata14.dnp[i] + (Bdata14.dnp[i + 1] - Bdata14.dnp[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[30] = Bdata14.ksa[i] + (Bdata14.ksa[i + 1] - Bdata14.ksa[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[31] = Bdata14.ksb[i] + (Bdata14.ksb[i + 1] - Bdata14.ksb[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[32] = Bdata14.rann1[i] + (Bdata14.rann1[i + 1] - Bdata14.rann1[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[33] = Bdata14.fw[i] + (Bdata14.fw[i + 1] - Bdata14.fw[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[34] = Bdata14.fact[i] + (Bdata14.fact[i + 1] - Bdata14.fact[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[35] = Bdata14.a_vr_o[i] + (Bdata14.a_vr_o[i + 1] - Bdata14.a_vr_o[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[36] = Bdata14.a_vr_s[i] + (Bdata14.a_vr_s[i + 1] - Bdata14.a_vr_s[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value14[37] = Bdata14.porw[i] + (Bdata14.porw[i + 1] - Bdata14.porw[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);


                        value15[0] = Bdata15.aca[i] + (Bdata15.aca[i + 1] - Bdata15.aca[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value15[1] = Bdata15.fadoc[i] + (Bdata15.fadoc[i + 1] - Bdata15.fadoc[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value15[2] = Bdata15.falpoc[i] + (Bdata15.falpoc[i + 1] - Bdata15.falpoc[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value15[3] = Bdata15.farpoc[i] + (Bdata15.farpoc[i + 1] - Bdata15.farpoc[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value15[4] = Bdata15.fbdoc[i] + (Bdata15.fbdoc[i + 1] - Bdata15.fbdoc[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value15[5] = Bdata15.fblpoc[i] + (Bdata15.fblpoc[i + 1] - Bdata15.fblpoc[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value15[6] = Bdata15.fbrpoc[i] + (Bdata15.fbrpoc[i + 1] - Bdata15.fbrpoc[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value15[7] = Bdata15.klpoc[i] + (Bdata15.klpoc[i + 1] - Bdata15.klpoc[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value15[8] = Bdata15.krpoc[i] + (Bdata15.krpoc[i + 1] - Bdata15.krpoc[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value15[9] = Bdata15.ksato[i] + (Bdata15.ksato[i + 1] - Bdata15.ksato[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value15[10] = Bdata15.kino[i] + (Bdata15.kino[i + 1] - Bdata15.kino[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value15[11] = Bdata15.kn[i] + (Bdata15.kn[i + 1] - Bdata15.kn[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value15[12] = Bdata15.kinn[i] + (Bdata15.kinn[i + 1] - Bdata15.kinn[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value15[13] = Bdata15.k1doc[i] + (Bdata15.k1doc[i + 1] - Bdata15.k1doc[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value15[14] = Bdata15.k2doc[i] + (Bdata15.k2doc[i + 1] - Bdata15.k2doc[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value15[15] = Bdata15.k3doc[i] + (Bdata15.k3doc[i + 1] - Bdata15.k3doc[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value15[16] = Bdata15.k4doc[i] + (Bdata15.k4doc[i + 1] - Bdata15.k4doc[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value15[17] = Bdata15.cp1[i] + (Bdata15.cp1[i + 1] - Bdata15.cp1[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value15[18] = Bdata15.cp2[i] + (Bdata15.cp2[i + 1] - Bdata15.cp2[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value15[19] = Bdata15.cp3[i] + (Bdata15.cp3[i + 1] - Bdata15.cp3[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value15[20] = Bdata15.fbw[i] + (Bdata15.fbw[i + 1] - Bdata15.fbw[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value15[21] = Bdata15.k1ch4[i] + (Bdata15.k1ch4[i + 1] - Bdata15.k1ch4[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value15[22] = Bdata15.k2ch4[i] + (Bdata15.k2ch4[i + 1] - Bdata15.k2ch4[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        value15[23] = Bdata15.rveg[i] + (Bdata15.rveg[i + 1] - Bdata15.rveg[i]) / (x[i + 1] - x[i]) * (m[y] - x[i]);
                        if (x[i + 1] > m[y])
                        {
                            break;
                        }
                    }

                    Interp_B14.insertOneRowData(value14);
                    Interp_B14Sorted.insertOneRowData(value14);
                    Interp_B15.insertOneRowData(value15);
                    Interp_B15Sorted.insertOneRowData(value15);
                }
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 20
                ));


                Interp_B14Sorted.cdf(Global.Nitrogen[0].type, Interp_B14Sorted.l2, Global.Nitrogen[0].min, Global.Nitrogen[0].max, Global.Nitrogen[0].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[1].type, Interp_B14Sorted.theta, Global.Nitrogen[1].min, Global.Nitrogen[1].max, Global.Nitrogen[1].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[2].type, Interp_B14Sorted.Is, Global.Nitrogen[2].min, Global.Nitrogen[2].max, Global.Nitrogen[2].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[3].type, Interp_B14Sorted.fnup, Global.Nitrogen[3].min, Global.Nitrogen[3].max, Global.Nitrogen[3].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[4].type, Interp_B14Sorted.kd, Global.Nitrogen[4].min, Global.Nitrogen[4].max, Global.Nitrogen[4].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[5].type, Interp_B14Sorted.kep, Global.Nitrogen[5].min, Global.Nitrogen[5].max, Global.Nitrogen[5].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[6].type, Interp_B14Sorted.kga0, Global.Nitrogen[6].min, Global.Nitrogen[6].max, Global.Nitrogen[6].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[7].type, Interp_B14Sorted.kgb0, Global.Nitrogen[7].min, Global.Nitrogen[7].max, Global.Nitrogen[7].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[8].type, Interp_B14Sorted.kmin1s, Global.Nitrogen[8].min, Global.Nitrogen[8].max, Global.Nitrogen[8].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[9].type, Interp_B14Sorted.knw, Global.Nitrogen[9].min, Global.Nitrogen[9].max, Global.Nitrogen[9].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[10].type, Interp_B14Sorted.kminw, Global.Nitrogen[10].min, Global.Nitrogen[10].max, Global.Nitrogen[10].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[11].type, Interp_B14Sorted.kns, Global.Nitrogen[11].min, Global.Nitrogen[11].max, Global.Nitrogen[11].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[12].type, Interp_B14Sorted.kden, Global.Nitrogen[12].min, Global.Nitrogen[12].max, Global.Nitrogen[12].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[13].type, Interp_B14Sorted.rowp, Global.Nitrogen[13].min, Global.Nitrogen[13].max, Global.Nitrogen[13].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[14].type, Interp_B14Sorted.vels_o, Global.Nitrogen[14].min, Global.Nitrogen[14].max, Global.Nitrogen[14].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[15].type, Interp_B14Sorted.vels_s, Global.Nitrogen[15].min, Global.Nitrogen[15].max, Global.Nitrogen[15].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[16].type, Interp_B14Sorted.velb, Global.Nitrogen[16].min, Global.Nitrogen[16].max, Global.Nitrogen[16].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[17].type, Interp_B14Sorted.ana, Global.Nitrogen[17].min, Global.Nitrogen[17].max, Global.Nitrogen[17].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[18].type, Interp_B14Sorted.rchl, Global.Nitrogen[18].min, Global.Nitrogen[18].max, Global.Nitrogen[18].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[19].type, Interp_B14Sorted.ss, Global.Nitrogen[19].min, Global.Nitrogen[19].max, Global.Nitrogen[19].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[20].type, Interp_B14Sorted.sw, Global.Nitrogen[20].min, Global.Nitrogen[20].max, Global.Nitrogen[20].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[21].type, Interp_B14Sorted.c_uw, Global.Nitrogen[21].min, Global.Nitrogen[21].max, Global.Nitrogen[21].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[22].type, Interp_B14Sorted.frap, Global.Nitrogen[22].min, Global.Nitrogen[22].max, Global.Nitrogen[22].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[23].type, Interp_B14Sorted.c1, Global.Nitrogen[23].min, Global.Nitrogen[23].max, Global.Nitrogen[23].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[24].type, Interp_B14Sorted.c2, Global.Nitrogen[24].min, Global.Nitrogen[24].max, Global.Nitrogen[24].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[25].type, Interp_B14Sorted.ph, Global.Nitrogen[25].min, Global.Nitrogen[25].max, Global.Nitrogen[25].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[26].type, Interp_B14Sorted.s, Global.Nitrogen[26].min, Global.Nitrogen[26].max, Global.Nitrogen[26].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[27].type, Interp_B14Sorted.kw, Global.Nitrogen[27].min, Global.Nitrogen[27].max, Global.Nitrogen[27].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[28].type, Interp_B14Sorted.apa, Global.Nitrogen[28].min, Global.Nitrogen[28].max, Global.Nitrogen[28].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[29].type, Interp_B14Sorted.dnp, Global.Nitrogen[29].min, Global.Nitrogen[29].max, Global.Nitrogen[29].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[30].type, Interp_B14Sorted.ksa, Global.Nitrogen[30].min, Global.Nitrogen[30].max, Global.Nitrogen[30].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[31].type, Interp_B14Sorted.ksb, Global.Nitrogen[31].min, Global.Nitrogen[31].max, Global.Nitrogen[31].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[32].type, Interp_B14Sorted.rann1, Global.Nitrogen[32].min, Global.Nitrogen[32].max, Global.Nitrogen[32].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[33].type, Interp_B14Sorted.fw, Global.Nitrogen[33].min, Global.Nitrogen[33].max, Global.Nitrogen[33].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[34].type, Interp_B14Sorted.fact, Global.Nitrogen[34].min, Global.Nitrogen[34].max, Global.Nitrogen[34].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[35].type, Interp_B14Sorted.a_vr_o, Global.Nitrogen[35].min, Global.Nitrogen[35].max, Global.Nitrogen[35].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[36].type, Interp_B14Sorted.a_vr_s, Global.Nitrogen[36].min, Global.Nitrogen[36].max, Global.Nitrogen[36].c);
                Interp_B14Sorted.cdf(Global.Nitrogen[37].type, Interp_B14Sorted.porw, Global.Nitrogen[37].min, Global.Nitrogen[37].max, Global.Nitrogen[37].c);
                Interp_B14Sorted.sort();
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 25
                ));
                //aca	FaDOC	FaLPOC	FaRPOC	FbDOC	FbLPOC	FbRPOC	kLPOC	kRPOC	KsatO	KinO	KN	KinN	K1DOC	k2DOC	k3DOC	k4DOC	cp1	cp2	cp3	fbw	k1CH4	k2CH4	Rveg
                Interp_B15Sorted.cdf(Global.Carbon[0].type, Interp_B15Sorted.aca, Global.Carbon[0].min, Global.Carbon[0].max, Global.Carbon[0].c);
                Interp_B15Sorted.cdf(Global.Carbon[1].type, Interp_B15Sorted.fadoc, Global.Carbon[1].min, Global.Carbon[1].max, Global.Carbon[1].c);
                Interp_B15Sorted.cdf(Global.Carbon[2].type, Interp_B15Sorted.falpoc, Global.Carbon[2].min, Global.Carbon[2].max, Global.Carbon[2].c);
                Interp_B15Sorted.cdf(Global.Carbon[3].type, Interp_B15Sorted.farpoc, Global.Carbon[3].min, Global.Carbon[3].max, Global.Carbon[3].c);
                Interp_B15Sorted.cdf(Global.Carbon[4].type, Interp_B15Sorted.fbdoc, Global.Carbon[4].min, Global.Carbon[4].max, Global.Carbon[4].c);
                Interp_B15Sorted.cdf(Global.Carbon[5].type, Interp_B15Sorted.fblpoc, Global.Carbon[5].min, Global.Carbon[5].max, Global.Carbon[5].c);
                Interp_B15Sorted.cdf(Global.Carbon[6].type, Interp_B15Sorted.fbrpoc, Global.Carbon[6].min, Global.Carbon[6].max, Global.Carbon[6].c);
                Interp_B15Sorted.cdf(Global.Carbon[7].type, Interp_B15Sorted.klpoc, Global.Carbon[7].min, Global.Carbon[7].max, Global.Carbon[7].c);
                Interp_B15Sorted.cdf(Global.Carbon[8].type, Interp_B15Sorted.krpoc, Global.Carbon[8].min, Global.Carbon[8].max, Global.Carbon[8].c);
                Interp_B15Sorted.cdf(Global.Carbon[9].type, Interp_B15Sorted.ksato, Global.Carbon[9].min, Global.Carbon[9].max, Global.Carbon[9].c);
                Interp_B15Sorted.cdf(Global.Carbon[10].type, Interp_B15Sorted.kino, Global.Carbon[10].min, Global.Carbon[10].max, Global.Carbon[10].c);
                Interp_B15Sorted.cdf(Global.Carbon[11].type, Interp_B15Sorted.kn, Global.Carbon[11].min, Global.Carbon[11].max, Global.Carbon[11].c);
                Interp_B15Sorted.cdf(Global.Carbon[12].type, Interp_B15Sorted.kinn, Global.Carbon[12].min, Global.Carbon[12].max, Global.Carbon[12].c);
                Interp_B15Sorted.cdf(Global.Carbon[13].type, Interp_B15Sorted.k1doc, Global.Carbon[13].min, Global.Carbon[13].max, Global.Carbon[13].c);
                Interp_B15Sorted.cdf(Global.Carbon[14].type, Interp_B15Sorted.k2doc, Global.Carbon[14].min, Global.Carbon[14].max, Global.Carbon[14].c);
                Interp_B15Sorted.cdf(Global.Carbon[15].type, Interp_B15Sorted.k3doc, Global.Carbon[15].min, Global.Carbon[15].max, Global.Carbon[15].c);
                Interp_B15Sorted.cdf(Global.Carbon[16].type, Interp_B15Sorted.k4doc, Global.Carbon[16].min, Global.Carbon[16].max, Global.Carbon[16].c);
                Interp_B15Sorted.cdf(Global.Carbon[17].type, Interp_B15Sorted.cp1, Global.Carbon[17].min, Global.Carbon[17].max, Global.Carbon[17].c);
                Interp_B15Sorted.cdf(Global.Carbon[18].type, Interp_B15Sorted.cp2, Global.Carbon[18].min, Global.Carbon[18].max, Global.Carbon[18].c);
                Interp_B15Sorted.cdf(Global.Carbon[19].type, Interp_B15Sorted.cp3, Global.Carbon[19].min, Global.Carbon[19].max, Global.Carbon[19].c);
                Interp_B15Sorted.cdf(Global.Carbon[20].type, Interp_B15Sorted.fbw, Global.Carbon[20].min, Global.Carbon[20].max, Global.Carbon[20].c);
                Interp_B15Sorted.cdf(Global.Carbon[21].type, Interp_B15Sorted.k1ch4, Global.Carbon[21].min, Global.Carbon[21].max, Global.Carbon[21].c);
                Interp_B15Sorted.cdf(Global.Carbon[22].type, Interp_B15Sorted.k2ch4, Global.Carbon[22].min, Global.Carbon[22].max, Global.Carbon[22].c);
                Interp_B15Sorted.cdf(Global.Carbon[23].type, Interp_B15Sorted.rveg, Global.Carbon[23].min, Global.Carbon[23].max, Global.Carbon[23].c);
                Interp_B15Sorted.sort();

                //NBData14Sorted.cdf(0, NBData14Sorted.l2, 5, 50);
                NBData14Sorted.cdf(Global.Nitrogen[0].type, NBData14Sorted.l2, Global.Nitrogen[0].min, Global.Nitrogen[0].max, Global.Nitrogen[0].c);
                NBData14Sorted.cdf(Global.Nitrogen[1].type, NBData14Sorted.theta, Global.Nitrogen[1].min, Global.Nitrogen[1].max, Global.Nitrogen[1].c);
                NBData14Sorted.cdf(Global.Nitrogen[2].type, NBData14Sorted.Is, Global.Nitrogen[2].min, Global.Nitrogen[2].max, Global.Nitrogen[2].c);
                NBData14Sorted.cdf(Global.Nitrogen[3].type, NBData14Sorted.fnup, Global.Nitrogen[3].min, Global.Nitrogen[3].max, Global.Nitrogen[3].c);
                NBData14Sorted.cdf(Global.Nitrogen[4].type, NBData14Sorted.kd, Global.Nitrogen[4].min, Global.Nitrogen[4].max, Global.Nitrogen[4].c);
                NBData14Sorted.cdf(Global.Nitrogen[5].type, NBData14Sorted.kep, Global.Nitrogen[5].min, Global.Nitrogen[5].max, Global.Nitrogen[5].c);
                NBData14Sorted.cdf(Global.Nitrogen[6].type, NBData14Sorted.kga0, Global.Nitrogen[6].min, Global.Nitrogen[6].max, Global.Nitrogen[6].c);
                NBData14Sorted.cdf(Global.Nitrogen[7].type, NBData14Sorted.kgb0, Global.Nitrogen[7].min, Global.Nitrogen[7].max, Global.Nitrogen[7].c);
                NBData14Sorted.cdf(Global.Nitrogen[8].type, NBData14Sorted.kmin1s, Global.Nitrogen[8].min, Global.Nitrogen[8].max, Global.Nitrogen[8].c);
                NBData14Sorted.cdf(Global.Nitrogen[9].type, NBData14Sorted.knw, Global.Nitrogen[9].min, Global.Nitrogen[9].max, Global.Nitrogen[9].c);
                NBData14Sorted.cdf(Global.Nitrogen[10].type, NBData14Sorted.kminw, Global.Nitrogen[10].min, Global.Nitrogen[10].max, Global.Nitrogen[10].c);
                NBData14Sorted.cdf(Global.Nitrogen[11].type, NBData14Sorted.kns, Global.Nitrogen[11].min, Global.Nitrogen[11].max, Global.Nitrogen[11].c);
                NBData14Sorted.cdf(Global.Nitrogen[12].type, NBData14Sorted.kden, Global.Nitrogen[12].min, Global.Nitrogen[12].max, Global.Nitrogen[12].c);
                NBData14Sorted.cdf(Global.Nitrogen[13].type, NBData14Sorted.rowp, Global.Nitrogen[13].min, Global.Nitrogen[13].max, Global.Nitrogen[13].c);
                NBData14Sorted.cdf(Global.Nitrogen[14].type, NBData14Sorted.vels_o, Global.Nitrogen[14].min, Global.Nitrogen[14].max, Global.Nitrogen[14].c);
                NBData14Sorted.cdf(Global.Nitrogen[15].type, NBData14Sorted.vels_s, Global.Nitrogen[15].min, Global.Nitrogen[15].max, Global.Nitrogen[15].c);
                NBData14Sorted.cdf(Global.Nitrogen[16].type, NBData14Sorted.velb, Global.Nitrogen[16].min, Global.Nitrogen[16].max, Global.Nitrogen[16].c);
                NBData14Sorted.cdf(Global.Nitrogen[17].type, NBData14Sorted.ana, Global.Nitrogen[17].min, Global.Nitrogen[17].max, Global.Nitrogen[17].c);
                NBData14Sorted.cdf(Global.Nitrogen[18].type, NBData14Sorted.rchl, Global.Nitrogen[18].min, Global.Nitrogen[18].max, Global.Nitrogen[18].c);
                NBData14Sorted.cdf(Global.Nitrogen[19].type, NBData14Sorted.ss, Global.Nitrogen[19].min, Global.Nitrogen[19].max, Global.Nitrogen[19].c);
                NBData14Sorted.cdf(Global.Nitrogen[20].type, NBData14Sorted.sw, Global.Nitrogen[20].min, Global.Nitrogen[20].max, Global.Nitrogen[20].c);
                NBData14Sorted.cdf(Global.Nitrogen[21].type, NBData14Sorted.c_uw, Global.Nitrogen[21].min, Global.Nitrogen[21].max, Global.Nitrogen[21].c);
                NBData14Sorted.cdf(Global.Nitrogen[22].type, NBData14Sorted.frap, Global.Nitrogen[22].min, Global.Nitrogen[22].max, Global.Nitrogen[22].c);
                NBData14Sorted.cdf(Global.Nitrogen[23].type, NBData14Sorted.c1, Global.Nitrogen[23].min, Global.Nitrogen[23].max, Global.Nitrogen[23].c);
                NBData14Sorted.cdf(Global.Nitrogen[24].type, NBData14Sorted.c2, Global.Nitrogen[24].min, Global.Nitrogen[24].max, Global.Nitrogen[24].c);
                NBData14Sorted.cdf(Global.Nitrogen[25].type, NBData14Sorted.ph, Global.Nitrogen[25].min, Global.Nitrogen[25].max, Global.Nitrogen[25].c);
                NBData14Sorted.cdf(Global.Nitrogen[26].type, NBData14Sorted.s, Global.Nitrogen[26].min, Global.Nitrogen[26].max, Global.Nitrogen[26].c);
                NBData14Sorted.cdf(Global.Nitrogen[27].type, NBData14Sorted.kw, Global.Nitrogen[27].min, Global.Nitrogen[27].max, Global.Nitrogen[27].c);
                NBData14Sorted.cdf(Global.Nitrogen[28].type, NBData14Sorted.apa, Global.Nitrogen[28].min, Global.Nitrogen[28].max, Global.Nitrogen[28].c);
                NBData14Sorted.cdf(Global.Nitrogen[29].type, NBData14Sorted.dnp, Global.Nitrogen[29].min, Global.Nitrogen[29].max, Global.Nitrogen[29].c);
                NBData14Sorted.cdf(Global.Nitrogen[30].type, NBData14Sorted.ksa, Global.Nitrogen[30].min, Global.Nitrogen[30].max, Global.Nitrogen[30].c);
                NBData14Sorted.cdf(Global.Nitrogen[31].type, NBData14Sorted.ksb, Global.Nitrogen[31].min, Global.Nitrogen[31].max, Global.Nitrogen[31].c);
                NBData14Sorted.cdf(Global.Nitrogen[32].type, NBData14Sorted.rann1, Global.Nitrogen[32].min, Global.Nitrogen[32].max, Global.Nitrogen[32].c);
                NBData14Sorted.cdf(Global.Nitrogen[33].type, NBData14Sorted.fw, Global.Nitrogen[33].min, Global.Nitrogen[33].max, Global.Nitrogen[33].c);
                NBData14Sorted.cdf(Global.Nitrogen[34].type, NBData14Sorted.fact, Global.Nitrogen[34].min, Global.Nitrogen[34].max, Global.Nitrogen[34].c);
                NBData14Sorted.cdf(Global.Nitrogen[35].type, NBData14Sorted.a_vr_o, Global.Nitrogen[35].min, Global.Nitrogen[35].max, Global.Nitrogen[35].c);
                NBData14Sorted.cdf(Global.Nitrogen[36].type, NBData14Sorted.a_vr_s, Global.Nitrogen[36].min, Global.Nitrogen[36].max, Global.Nitrogen[36].c);
                NBData14Sorted.cdf(Global.Nitrogen[37].type, NBData14Sorted.porw, Global.Nitrogen[37].min, Global.Nitrogen[37].max, Global.Nitrogen[37].c);
                NBData14Sorted.sort();
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 28
                ));
                //NB15sorted
                NBData15Sorted.cdf(Global.Carbon[0].type, NBData15Sorted.aca, Global.Carbon[0].min, Global.Carbon[0].max, Global.Carbon[0].c);
                NBData15Sorted.cdf(Global.Carbon[1].type, NBData15Sorted.fadoc, Global.Carbon[1].min, Global.Carbon[1].max, Global.Carbon[1].c);
                NBData15Sorted.cdf(Global.Carbon[2].type, NBData15Sorted.falpoc, Global.Carbon[2].min, Global.Carbon[2].max, Global.Carbon[2].c);
                NBData15Sorted.cdf(Global.Carbon[3].type, NBData15Sorted.farpoc, Global.Carbon[3].min, Global.Carbon[3].max, Global.Carbon[3].c);
                NBData15Sorted.cdf(Global.Carbon[4].type, NBData15Sorted.fbdoc, Global.Carbon[4].min, Global.Carbon[4].max, Global.Carbon[4].c);
                NBData15Sorted.cdf(Global.Carbon[5].type, NBData15Sorted.fblpoc, Global.Carbon[5].min, Global.Carbon[5].max, Global.Carbon[5].c);
                NBData15Sorted.cdf(Global.Carbon[6].type, NBData15Sorted.fbrpoc, Global.Carbon[6].min, Global.Carbon[6].max, Global.Carbon[6].c);
                NBData15Sorted.cdf(Global.Carbon[7].type, NBData15Sorted.klpoc, Global.Carbon[7].min, Global.Carbon[7].max, Global.Carbon[7].c);
                NBData15Sorted.cdf(Global.Carbon[8].type, NBData15Sorted.krpoc, Global.Carbon[8].min, Global.Carbon[8].max, Global.Carbon[8].c);
                NBData15Sorted.cdf(Global.Carbon[9].type, NBData15Sorted.ksato, Global.Carbon[9].min, Global.Carbon[9].max, Global.Carbon[9].c);
                NBData15Sorted.cdf(Global.Carbon[10].type, NBData15Sorted.kino, Global.Carbon[10].min, Global.Carbon[10].max, Global.Carbon[10].c);
                NBData15Sorted.cdf(Global.Carbon[11].type, NBData15Sorted.kn, Global.Carbon[11].min, Global.Carbon[11].max, Global.Carbon[11].c);
                NBData15Sorted.cdf(Global.Carbon[12].type, NBData15Sorted.kinn, Global.Carbon[12].min, Global.Carbon[12].max, Global.Carbon[12].c);
                NBData15Sorted.cdf(Global.Carbon[13].type, NBData15Sorted.k1doc, Global.Carbon[13].min, Global.Carbon[13].max, Global.Carbon[13].c);
                NBData15Sorted.cdf(Global.Carbon[14].type, NBData15Sorted.k2doc, Global.Carbon[14].min, Global.Carbon[14].max, Global.Carbon[14].c);
                NBData15Sorted.cdf(Global.Carbon[15].type, NBData15Sorted.k3doc, Global.Carbon[15].min, Global.Carbon[15].max, Global.Carbon[15].c);
                NBData15Sorted.cdf(Global.Carbon[16].type, NBData15Sorted.k4doc, Global.Carbon[16].min, Global.Carbon[16].max, Global.Carbon[16].c);
                NBData15Sorted.cdf(Global.Carbon[17].type, NBData15Sorted.cp1, Global.Carbon[17].min, Global.Carbon[17].max, Global.Carbon[17].c);
                NBData15Sorted.cdf(Global.Carbon[18].type, NBData15Sorted.cp2, Global.Carbon[18].min, Global.Carbon[18].max, Global.Carbon[18].c);
                NBData15Sorted.cdf(Global.Carbon[19].type, NBData15Sorted.cp3, Global.Carbon[19].min, Global.Carbon[19].max, Global.Carbon[19].c);
                NBData15Sorted.cdf(Global.Carbon[20].type, NBData15Sorted.fbw, Global.Carbon[20].min, Global.Carbon[20].max, Global.Carbon[20].c);
                NBData15Sorted.cdf(Global.Carbon[21].type, NBData15Sorted.k1ch4, Global.Carbon[21].min, Global.Carbon[21].max, Global.Carbon[21].c);
                NBData15Sorted.cdf(Global.Carbon[22].type, NBData15Sorted.k2ch4, Global.Carbon[22].min, Global.Carbon[22].max, Global.Carbon[22].c);
                NBData15Sorted.cdf(Global.Carbon[23].type, NBData15Sorted.rveg, Global.Carbon[23].min, Global.Carbon[23].max, Global.Carbon[23].c);
                NBData15Sorted.sort();
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 30
                ));
                result = new List<KResult>();
                //L2	theta	Is	fNup	kd	kep	kga0	kgb0	kmin1s	knw	kminw	kns	kden	
                //rowp	vels_o	vels_s  	velb	ana	rChl	Ss	Sw	c_Uw	frap	
                //c1	c2	PH	S	Kw	apa	Dnp	Ksa	Ksb	RanN1	fW	fact  	a_vr_o	a_vr_s	porw
                result.Add(new KResult("l2", Interp_B14.l2, OriginalBdata14.l2, NBdata14.l2, Interp_B14Sorted.l2, NBData14Sorted.l2));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 31
                ));
                result.Add(new KResult("theta", Interp_B14.theta, OriginalBdata14.theta, NBdata14.theta, Interp_B14Sorted.theta, NBData14Sorted.theta));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 32
                ));
                result.Add(new KResult("Is", Interp_B14.Is, OriginalBdata14.Is, NBdata14.Is, Interp_B14Sorted.Is, NBData14Sorted.Is));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 33
                ));
                result.Add(new KResult("fnup", Interp_B14.fnup, OriginalBdata14.fnup, NBdata14.fnup, Interp_B14Sorted.fnup, NBData14Sorted.fnup));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 34
                ));
                result.Add(new KResult("kd", Interp_B14.kd, OriginalBdata14.kd, NBdata14.kd, Interp_B14Sorted.kd, NBData14Sorted.kd));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 35
                ));
                result.Add(new KResult("kep", Interp_B14.kep, OriginalBdata14.kep, NBdata14.kep, Interp_B14Sorted.kep, NBData14Sorted.kep));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 36
                ));
                result.Add(new KResult("kga0", Interp_B14.kga0, OriginalBdata14.kga0, NBdata14.kga0, Interp_B14Sorted.kga0, NBData14Sorted.kga0));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 37
                ));
                result.Add(new KResult("kgb0", Interp_B14.kgb0, OriginalBdata14.kgb0, NBdata14.kgb0, Interp_B14Sorted.kgb0, NBData14Sorted.kgb0));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 38
                ));
                result.Add(new KResult("kmin1s", Interp_B14.kmin1s, OriginalBdata14.kmin1s, NBdata14.kmin1s, Interp_B14Sorted.kmin1s, NBData14Sorted.kmin1s));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 39
                ));
                result.Add(new KResult("kns", Interp_B14.kns, OriginalBdata14.kns, NBdata14.kns, Interp_B14Sorted.kns, NBData14Sorted.kns));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 40
                ));
                result.Add(new KResult("kminw", Interp_B14.kminw, OriginalBdata14.kminw, NBdata14.kminw, Interp_B14Sorted.kminw, NBData14Sorted.kminw));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 41
                ));
                result.Add(new KResult("kns", Interp_B14.kns, OriginalBdata14.kns, NBdata14.kns, Interp_B14Sorted.kns, NBData14Sorted.kns));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 42
                ));
                result.Add(new KResult("kden", Interp_B14.kden, OriginalBdata14.kden, NBdata14.kden, Interp_B14Sorted.kden, NBData14Sorted.kden));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 43
                ));
                result.Add(new KResult("rowp", Interp_B14.rowp, OriginalBdata14.rowp, NBdata14.rowp, Interp_B14Sorted.rowp, NBData14Sorted.rowp));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 44
                ));
                result.Add(new KResult("vels_o", Interp_B14.vels_o, OriginalBdata14.vels_o, NBdata14.vels_o, Interp_B14Sorted.vels_o, NBData14Sorted.vels_o));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 45
                ));
                result.Add(new KResult("vels_s", Interp_B14.vels_s, OriginalBdata14.vels_s, NBdata14.vels_s, Interp_B14Sorted.vels_s, NBData14Sorted.vels_s));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 46
                ));
                result.Add(new KResult("velb", Interp_B14.velb, OriginalBdata14.velb, NBdata14.velb, Interp_B14Sorted.velb, NBData14Sorted.velb));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 47
                ));
                result.Add(new KResult("ana", Interp_B14.ana, OriginalBdata14.ana, NBdata14.ana, Interp_B14Sorted.ana, NBData14Sorted.ana));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 48
                ));
                result.Add(new KResult("rchl", Interp_B14.rchl, OriginalBdata14.rchl, NBdata14.rchl, Interp_B14Sorted.rchl, NBData14Sorted.rchl));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 49
                ));
                result.Add(new KResult("ss", Interp_B14.ss, OriginalBdata14.ss, NBdata14.ss, Interp_B14Sorted.ss, NBData14Sorted.ss));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 50
                ));
                result.Add(new KResult("sw", Interp_B14.sw, OriginalBdata14.sw, NBdata14.sw, Interp_B14Sorted.sw, NBData14Sorted.sw));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 51
                ));
                result.Add(new KResult("c_uw", Interp_B14.c_uw, OriginalBdata14.c_uw, NBdata14.c_uw, Interp_B14Sorted.c_uw, NBData14Sorted.c_uw));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 52
                ));
                result.Add(new KResult("frap", Interp_B14.frap, OriginalBdata14.frap, NBdata14.frap, Interp_B14Sorted.frap, NBData14Sorted.frap));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 53
                ));
                result.Add(new KResult("c1", Interp_B14.c1, OriginalBdata14.c1, NBdata14.c1, Interp_B14Sorted.c1, NBData14Sorted.c1));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 54
                ));
                result.Add(new KResult("c2", Interp_B14.c2, OriginalBdata14.c2, NBdata14.c2, Interp_B14Sorted.c2, NBData14Sorted.c2));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 55
                ));
                result.Add(new KResult("ph", Interp_B14.ph, OriginalBdata14.ph, NBdata14.ph, Interp_B14Sorted.ph, NBData14Sorted.ph));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 56
                ));
                result.Add(new KResult("s", Interp_B14.s, OriginalBdata14.s, NBdata14.s, Interp_B14Sorted.s, NBData14Sorted.s));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 57
                ));
                result.Add(new KResult("kw", Interp_B14.kw, OriginalBdata14.kw, NBdata14.kw, Interp_B14Sorted.kw, NBData14Sorted.kw));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 58
                ));
                result.Add(new KResult("apa", Interp_B14.apa, OriginalBdata14.apa, NBdata14.apa, Interp_B14Sorted.apa, NBData14Sorted.apa));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 59
                ));
                result.Add(new KResult("dnp", Interp_B14.dnp, OriginalBdata14.dnp, NBdata14.dnp, Interp_B14Sorted.dnp, NBData14Sorted.dnp));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 60
                ));
                result.Add(new KResult("ksa", Interp_B14.ksa, OriginalBdata14.ksa, NBdata14.ksa, Interp_B14Sorted.ksa, NBData14Sorted.ksa));
                result.Add(new KResult("ksb", Interp_B14.ksb, OriginalBdata14.ksb, NBdata14.ksb, Interp_B14Sorted.ksb, NBData14Sorted.ksb));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 62
                ));
                result.Add(new KResult("rann1", Interp_B14.rann1, OriginalBdata14.rann1, NBdata14.rann1, Interp_B14Sorted.rann1, NBData14Sorted.rann1));
                result.Add(new KResult("fw", Interp_B14.fw, OriginalBdata14.fw, NBdata14.fw, Interp_B14Sorted.fw, NBData14Sorted.fw));
                result.Add(new KResult("fact", Interp_B14.fact, OriginalBdata14.fact, NBdata14.fact, Interp_B14Sorted.fact, NBData14Sorted.fact));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 65
                ));
                result.Add(new KResult("a_vr_o", Interp_B14.a_vr_o, OriginalBdata14.a_vr_o, NBdata14.a_vr_o, Interp_B14Sorted.a_vr_o, NBData14Sorted.a_vr_o));
                result.Add(new KResult("a_vr_s", Interp_B14.a_vr_s, OriginalBdata14.a_vr_s, NBdata14.a_vr_s, Interp_B14Sorted.a_vr_s, NBData14Sorted.a_vr_s));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 67
                ));
                result.Add(new KResult("porw", Interp_B14.porw, OriginalBdata14.porw, NBdata14.porw, Interp_B14Sorted.porw, NBData14Sorted.porw));
                //result for carbon
                //aca	FaDOC	FaLPOC	FaRPOC	FbDOC	FbLPOC	FbRPOC	kLPOC	kRPOC	KsatO	KinO	KN	KinN	K1DOC	k2DOC	k3DOC	k4DOC	cp1	cp2	cp3	fbw	k1CH4	k2CH4	Rveg
                result.Add(new KResult("aca", Interp_B15.aca, OriginalBdata15.aca, NBdata15.aca, Interp_B15Sorted.aca, NBData15Sorted.aca));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 70
                ));
                result.Add(new KResult("fadoc", Interp_B15.fadoc, OriginalBdata15.fadoc, NBdata15.fadoc, Interp_B15Sorted.fadoc, NBData15Sorted.fadoc));
                result.Add(new KResult("falpoc", Interp_B15.falpoc, OriginalBdata15.falpoc, NBdata15.falpoc, Interp_B15Sorted.falpoc, NBData15Sorted.falpoc));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 72
                ));
                result.Add(new KResult("farpoc", Interp_B15.farpoc, OriginalBdata15.farpoc, NBdata15.farpoc, Interp_B15Sorted.farpoc, NBData15Sorted.farpoc));
                result.Add(new KResult("fbdoc", Interp_B15.fbdoc, OriginalBdata15.fbdoc, NBdata15.fbdoc, Interp_B15Sorted.fbdoc, NBData15Sorted.fbdoc));
                result.Add(new KResult("fblpoc", Interp_B15.fblpoc, OriginalBdata15.fblpoc, NBdata15.fblpoc, Interp_B15Sorted.fblpoc, NBData15Sorted.fblpoc));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 75
                ));
                result.Add(new KResult("fbrpoc", Interp_B15.fbrpoc, OriginalBdata15.fbrpoc, NBdata15.fbrpoc, Interp_B15Sorted.fbrpoc, NBData15Sorted.fbrpoc));
                result.Add(new KResult("klpoc", Interp_B15.klpoc, OriginalBdata15.klpoc, NBdata15.klpoc, Interp_B15Sorted.klpoc, NBData15Sorted.klpoc));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 77
                ));
                result.Add(new KResult("krpoc", Interp_B15.krpoc, OriginalBdata15.krpoc, NBdata15.krpoc, Interp_B15Sorted.krpoc, NBData15Sorted.krpoc));
                result.Add(new KResult("ksato", Interp_B15.ksato, OriginalBdata15.ksato, NBdata15.ksato, Interp_B15Sorted.ksato, NBData15Sorted.ksato));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 79
                ));
                result.Add(new KResult("kino", Interp_B15.kino, OriginalBdata15.kino, NBdata15.kino, Interp_B15Sorted.kino, NBData15Sorted.kino));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 80
                ));
                result.Add(new KResult("kn", Interp_B15.kn, OriginalBdata15.kn, NBdata15.kn, Interp_B15Sorted.kn, NBData15Sorted.kn));
                result.Add(new KResult("kinn", Interp_B15.kinn, OriginalBdata15.kinn, NBdata15.kinn, Interp_B15Sorted.kinn, NBData15Sorted.kinn));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 82
                ));
                result.Add(new KResult("k1doc", Interp_B15.k1doc, OriginalBdata15.k1doc, NBdata15.k1doc, Interp_B15Sorted.k1doc, NBData15Sorted.k1doc));
                result.Add(new KResult("k2doc", Interp_B15.k2doc, OriginalBdata15.k2doc, NBdata15.k2doc, Interp_B15Sorted.k2doc, NBData15Sorted.k2doc));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 84
                ));
                result.Add(new KResult("k3doc", Interp_B15.k3doc, OriginalBdata15.k3doc, NBdata15.k3doc, Interp_B15Sorted.k3doc, NBData15Sorted.k3doc));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 85
                ));
                result.Add(new KResult("k4doc", Interp_B15.k4doc, OriginalBdata15.k4doc, NBdata15.k4doc, Interp_B15Sorted.k4doc, NBData15Sorted.k4doc));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 86
                ));
                result.Add(new KResult("cp1", Interp_B15.cp1, OriginalBdata15.cp1, NBdata15.cp1, Interp_B15Sorted.cp1, NBData15Sorted.cp1));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 87
                ));
                result.Add(new KResult("cp2", Interp_B15.cp2, OriginalBdata15.cp2, NBdata15.cp2, Interp_B15Sorted.cp2, NBData15Sorted.cp2));
                result.Add(new KResult("cp3", Interp_B15.cp3, OriginalBdata15.cp3, NBdata15.cp3, Interp_B15Sorted.cp3, NBData15Sorted.cp3));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 90
                ));
                result.Add(new KResult("fbw", Interp_B15.fbw, OriginalBdata15.fbw, NBdata15.fbw, Interp_B15Sorted.fbw, NBData15Sorted.fbw));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 91
                ));
                result.Add(new KResult("k1ch4", Interp_B15.k1ch4, OriginalBdata15.k1ch4, NBdata15.k1ch4, Interp_B15Sorted.k1ch4, NBData15Sorted.k1ch4));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 92
                ));
                result.Add(new KResult("k2ch4", Interp_B15.k2ch4, OriginalBdata15.k2ch4, NBdata15.k2ch4, Interp_B15Sorted.k2ch4, NBData15Sorted.k2ch4));
                result.Add(new KResult("rveg", Interp_B15.rveg, OriginalBdata15.rveg, NBdata15.rveg, Interp_B15Sorted.rveg, NBData15Sorted.rveg));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 95
                ));
                result.Sort();
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 97
                ));
                
                Dispatcher.Invoke(new Action(() =>
                reportIndex.Items.Clear()));
                Dispatcher.Invoke(new Action(() =>
                reportIndex.Items.Add("Overview of Sensitivity Analysis (DMax).")));

                this.topTen = new Collection<Item>();
                for (int i = 0; i < 20; i++)
                {//TODO may need change to 10
                    topTen.Add(new Item() { name = result[i].name, Dmax = result[i].Dmax });
                    Dispatcher.Invoke(new Action(() =>
                reportIndex.Items.Add("CDFs of Parameter " + result[i].name + " ")));
                }

                var graph = new PlotModel { Title = "DMax", LegendPlacement = LegendPlacement.Outside, LegendPosition = LegendPosition.RightTop, LegendOrientation = LegendOrientation.Vertical };
                graph.Axes.Add(new CategoryAxis { ItemsSource = this.topTen, LabelField = "name" });
                graph.Axes.Add(new LinearAxis { Position = AxisPosition.Left, MinimumPadding = 0, AbsoluteMinimum = 0 });
                graph.Series.Add(new ColumnSeries { Title = "parameters", ItemsSource = this.topTen, ValueField = "Dmax", FillColor = OxyColors.Green });
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 100
                ));
                Dispatcher.Invoke(new Action(() =>
                report.Model = graph
                ));
                Dispatcher.Invoke(new Action(() =>
                reportIndex.Visibility = Visibility.Visible
                ));
                //Dispatcher.Invoke(new Action(() =>
                //calculation.Visibility = Visibility.Hidden
                //));
                Dispatcher.Invoke(new Action(() =>
                percentPanel.Visibility = Visibility.Visible
                ));
                Dispatcher.Invoke(new Action(() =>
                progress.Visibility = Visibility.Hidden
                ));
                Dispatcher.Invoke(new Action(() =>
                status.Visibility = Visibility.Hidden
                ));
                Dispatcher.Invoke(new Action(() =>
                progress.Value = 0
                ));
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double percent = Convert.ToDouble(percentage.Text);
                if (percent > 10 || percent < 1)
                {
                    throw new Exception("Percentage should between 1% to 10%");
                }
                percent = percent * 0.01;
                Global.percentage = percent;
                calculation_Click(sender, e);
            }
            catch (FormatException)
            {
                MessageBox.Show("Please input the correct percentage", "Error");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private string GetFilename(string filter, string defaultExt)
        {
            var dlg = new SaveFileDialog { Filter = filter, DefaultExt = defaultExt };
            return dlg.ShowDialog(this.Owner).Value ? dlg.FileName : null;
        }
        private static void OpenContainingFolder(string fileName)
        {
            // var folder = Path.GetDirectoryName(fileName);
            var psi = new ProcessStartInfo("Explorer.exe", "/select," + fileName);
            Process.Start(psi);
        }

        private void menu_pdf_Click(object sender, RoutedEventArgs e)
        {
            var path = this.GetFilename(".pdf files|*.pdf", ".pdf");
            if (path != null)
            {
                using (var stream = File.Create(path))
                {
                    OxyPlot.PdfExporter.Export(report.Model, stream, report.ActualWidth, report.ActualHeight);
                }

                OpenContainingFolder(path);
            }
        }

        private void menu_print_Click(object sender, RoutedEventArgs e)
        {
            XpsExporter.Print(report.Model, report.ActualWidth, report.ActualHeight);
        }
    }
    public class Item
    {
        public string name { get; set; }
        public double Dmax { get; set; }
    }
    public class Values
    {
        public double xvalue { get; set; }
        public double yvalue { get; set; }
    }
}
