using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WetLand.PostProcess
{
    using System.Collections.Generic;
    using OxyPlot;
    using OxyPlot.Series;
    using OxyPlot.Axes;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Threading;
    class ReportModelView
    {
        public PostProcessData ppdata;
        public class DateValue
        {
            public DateTime Date { get; set; }
            public double Value { get; set; }
        }
        public class AreaValue
        {
            public DateTime Date { get; set; }
            public double Minimum { get; set; }
            public double Maximum { get; set; }
        }
        public PlotModel MyModel { get; set; }
        public Collection<DateValue> ObsData { get; set; }
        public Collection<DateValue> MedianData { get; set; }
        public Collection<AreaValue> BUBData { get; set; }
        public Collection<AreaValue> BData { get; set; }
        public int index { get; set; }

        private  string obsFilePath = Global.projectName + @"\Observations\";
        private  string resultFilePath = Global.projectName + @"\InputFiles\";
        private  string[] obsFileName = { "102_obs_Onw.txt", "103_obs_Onss.txt", "104_obs_Onsf.txt", "105_obs_Nw.txt", "106_obs_Ns1.txt", "107_obs_Ns2.txt", "108_obs_NO3w.txt", "109_obs_NO3s1.txt",
                "110_obs_NO3s2.txt","111_obs_Ow.txt","112_obs_a.txt","113_obs_b.txt","114_obs_Pw.txt","115_obs_Ps1.txt","116_obs_Ps2.txt","118_obs_mw.txt","150_obs_DOCw.txt","151_obs_LPOCw.txt",
                "152_obs_RPOCw.txt","153_obs_DOCs1.txt","154_obs_LPOCs1.txt","155_obs_RPOCs1.txt","156_obs_DOCs2.txt","157_obs_LPOCs2.txt","158_obs_RPOCs2.txt","159_obs_TOCw.txt",
                "160_obs_CH4w.txt","161_obs_CH4s1.txt","162_obs_CH4s2.txt"
        };
        private static string[] resultFileName = { "102_Onw.txt", "103_Onss.txt", "104_Onsf.txt", "105_Nw.txt", "106_Ns1.txt", "107_Ns2.txt", "108_NO3w.txt", "109_NO3s1.txt",
                "110_NO3s2.txt","111_Ow.txt","112_a.txt","113_b.txt","114_Pw.txt","115_Ps1.txt","116_Ps2.txt","118_mw.txt","150_DOCw.txt","151_LPOCw.txt",
                "152_RPOCw.txt","153_DOCs1.txt","154_LPOCs1.txt","155_RPOCs1.txt","156_DOCs2.txt","157_LPOCs2.txt","158_RPOCs2.txt","159_TOCw.txt",
                "160_CH4w.txt","161_CH4s1.txt","162_CH4s2.txt"
        };


        public PlotModel CreateModel(int reportIndex,string title,string ytitle)
        {
            var tmp = new PlotModel { Title = title };

            tmp.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "MMM/dd/yyyy", MajorGridlineStyle = LineStyle.Solid,Title="Date (day)",TitleFontSize=15 });
            tmp.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, MajorGridlineStyle = LineStyle.Solid, Title = ytitle, TitleFontSize = 15 });

            this.ObsData = new Collection<DateValue>();
            this.BUBData = new Collection<AreaValue>();
            this.BData = new Collection<AreaValue>();
            this.MedianData = new Collection<DateValue>();

            ppdata = new PostProcessData();
            int obsCount = ppdata.getObservedValue(obsFilePath+obsFileName[reportIndex]);
            var date = Global.startDate.AddDays(-1.0);
            for (int i = 0; i < ppdata.observedValue.Length; i++) {
                if (ppdata.observedValue[i].HasValue)
                {
                    this.ObsData.Add(new DateValue { Date = date, Value = ppdata.observedValue[i].Value });
                }
                date = date.AddDays(1.0);
            }
            ppdata.getSimulationValue(resultFilePath + resultFileName[reportIndex]);
            ppdata.sort();
            date = Global.startDate.AddDays(-1);
            double percent = Global.percentage;
            int Bbound = (int)Math.Floor(ppdata.simulationSeries.Count * percent);
            int bound95 = (int)Math.Floor(ppdata.simulationSeries.Count * 0.95);
            for (int day = 0; day < Global.nofDays; day++)
            {
                double max = Double.MinValue;
                double min = Double.MaxValue;
                double bmax = Double.MinValue;
                double bmin = Double.MaxValue;
                for (int simnum = 0; simnum < bound95; simnum++)
                {
                    if (ppdata.simulationSeries[simnum].daysData[day] > max)
                    {
                        max = ppdata.simulationSeries[simnum].daysData[day];
                    } 
                    if (ppdata.simulationSeries[simnum].daysData[day] < min)
                    {
                        min = ppdata.simulationSeries[simnum].daysData[day];
                    }
                    if (simnum == Bbound)
                    {
                        bmax = max;
                        bmin = min;
                    }
                }
                this.BData.Add(new AreaValue { Maximum = bmax, Minimum = bmin, Date = date });
                this.BUBData.Add(new AreaValue { Maximum = max, Minimum = min, Date = date });
                date = date.AddDays(1);
            }

            int medianIndex;
            if (ppdata.simulationSeries.Count % 2 == 0)
            {
                medianIndex = (ppdata.simulationSeries.Count / 2 + ppdata.simulationSeries.Count / 2 - 1)/2;
            }
            else
            {
                medianIndex = ppdata.simulationSeries.Count / 2;
            }
            date = Global.startDate.AddDays(-1);
            for (int day = 0; day < Global.nofDays; day++)
            {
                this.MedianData.Add(new DateValue { Date = date, Value = ppdata.simulationSeries[medianIndex].daysData[day] });
                date = date.AddDays(1);
            }
            // Create a line series
            var s1 = new LineSeries
            {
                Title = "Observed",
                StrokeThickness = 1,
                MarkerSize = 3,
                ItemsSource = this.ObsData,
                DataFieldX = "Date",
                DataFieldY = "Value",
                Color = OxyColors.Transparent,
                LineStyle= OxyPlot.LineStyle.None,
                MarkerStroke = OxyColors.Red,
                MarkerType = MarkerType.Circle
            };

            var s2 = new LineSeries
            {
                Title = "Median",
                StrokeThickness = 2,
                MarkerSize = 3,
                ItemsSource = this.MedianData,
                DataFieldX = "Date",
                DataFieldY = "Value",
                Color = OxyColors.Black,
                LineStyle = OxyPlot.LineStyle.Dash,
            };

            var s3 = new LineSeries
            {
                Title = "MBE(%)="+ppdata.simulationSeries[0].MBE.ToString("f2"),
                LineStyle = OxyPlot.LineStyle.None,
                Color = OxyColors.Transparent,
                MarkerType = MarkerType.None,
                FontSize = 12
            };
            var bestModel = new LineSeries
            {
                Title = "Best Model: Ens = " + ppdata.simulationSeries[0].Ens.ToString("f2")+ "; MBE (%)=" + ppdata.simulationSeries[0].MBE.ToString("f2"),
                LineStyle = OxyPlot.LineStyle.None,
                Color = OxyColors.Transparent,
                MarkerType = MarkerType.None,
            };
            var s4 = new LineSeries
            {
                Title = "Ens=" + ppdata.simulationSeries[0].Ens.ToString("f2"),
                LineStyle = OxyPlot.LineStyle.None,
                Color = OxyColors.Transparent,
                MarkerType = MarkerType.None,
            };
            var s5 = new LineSeries
            {
                Title = "PBIAS (%)=" + ppdata.simulationSeries[0].PBIAS.ToString("f2"),
                LineStyle = OxyPlot.LineStyle.None,
                Color = OxyColors.Transparent,
                MarkerType = MarkerType.None,
            };

            //create a area series
            var a1 = new AreaSeries
            {
                Title = "BUB",
                StrokeThickness = 0,
                MarkerSize = 1,
                ItemsSource = this.BUBData,
                DataFieldX="Date",
                DataFieldY= "Minimum",
                DataFieldX2="Date",
                DataFieldY2= "Maximum",
                Fill=OxyColors.LightGray,
            };
            var a2 = new AreaSeries
            {
                Title = "B",
                StrokeThickness = 0,
                MarkerSize = 1,
                ItemsSource = this.BData,
                DataFieldX = "Date",
                DataFieldY = "Minimum",
                DataFieldX2 = "Date",
                DataFieldY2 = "Maximum",
                Fill = OxyColors.DarkGray,
            };

            
            if (Global.DeterminMode == false)
            {
                tmp.Series.Add(a1);
                if (obsCount != 0)
                {
                    tmp.Series.Add(a2);
                    tmp.Series.Add(s1);
                }
                tmp.Series.Add(s2);
                if (obsCount != 0)
                {
                    tmp.Series.Add(bestModel);

                }
            }
            else {
                s2.Title = "Simulation";
                s2.Color = OxyColors.Blue;
                //s2.MarkerType = MarkerType.Circle;
                //s2.MarkerStroke = OxyColors.Red;
                tmp.Series.Add(s2);
                tmp.Series.Add(s1);
                tmp.Series.Add(s4);
                tmp.Series.Add(s5);
            }
            this.MyModel = tmp;
            return tmp;
        }
    }
}