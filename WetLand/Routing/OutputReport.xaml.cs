﻿using System;
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
    /// Interaction logic for OutputReport.xaml
    /// </summary>
    using OxyPlot;
    using OxyPlot.Series;
    using OxyPlot.Axes;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.IO;
    public partial class OutputReport : Window
    {
        public class DateValue
        {
            public DateTime Date { get; set; }
            public double Value { get; set; }
        }
        public string[] lengend = { "H",  "Outflow", "Volume", "Area" };
        public OutputReport()
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
                    if (reportIndex.SelectedIndex == 4) {
                        var tmp = new PlotModel { Title = "Qin and Outflow" };
                        tmp.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "MMM/dd/yyyy", MajorGridlineStyle = LineStyle.Solid });
                        tmp.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, MajorGridlineStyle = LineStyle.Solid });
                        Collection<DateValue>  qinData = new Collection<DateValue>();
                        Collection<DateValue> qoutData = new Collection<DateValue>();
                        string[] inputcontents = File.ReadAllLines(Global.projectName + @"\InputFiles\Routing\2_input_time_series.txt");
                        string[] outputcontents = File.ReadAllLines(Global.projectName + @"\InputFiles\Routing\5_output.txt");
                        int length = inputcontents.Length > outputcontents.Length ? outputcontents.Length : inputcontents.Length;
                        DateTime startDate = Global.startDate;
                        for (int i = 1; i < length; i++) {
                            string[] inputparameters = inputcontents[i].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                            string[] outputparameters = outputcontents[i].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                            double qin = Convert.ToDouble(inputparameters[1]);
                            double qout = Convert.ToDouble(outputparameters[2]);
                            qinData.Add(new DateValue { Date = startDate, Value = qin });
                            qoutData.Add(new DateValue { Date = startDate, Value = qout });
                            startDate = startDate.AddDays(1);
                        }
                        var s1 = new LineSeries
                        {
                            Title = "Qin",
                            StrokeThickness = 1,
                            MarkerSize = 3,
                            ItemsSource = qinData,
                            DataFieldX = "Date",
                            DataFieldY = "Value",
                            Color = OxyColors.Blue,
                            LineStyle = OxyPlot.LineStyle.Solid,
                            MarkerStroke = OxyColors.Red,
                            MarkerType = MarkerType.Circle
                        };
                        var s2 = new LineSeries
                        {
                            Title = "Qout",
                            StrokeThickness = 1,
                            MarkerSize = 3,
                            ItemsSource = qoutData,
                            DataFieldX = "Date",
                            DataFieldY = "Value",
                            Color = OxyColors.Green,
                            LineStyle = OxyPlot.LineStyle.Solid,
                            MarkerStroke = OxyColors.Black,
                            MarkerType = MarkerType.Star
                        };
                        tmp.Series.Add(s1);
                        tmp.Series.Add(s2);
                        report.Model = tmp;

                        return;
                    }
                    report.Model = model.CreateModel(2, reportIndex.SelectedIndex, reportIndex.Text, lengend[reportIndex.SelectedIndex]);
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
