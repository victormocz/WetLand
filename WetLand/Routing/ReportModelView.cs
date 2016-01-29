using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WetLand.Routing
{
    using System.Collections.Generic;
    using System.IO;
    using OxyPlot;
    using OxyPlot.Series;
    using OxyPlot.Axes;
    using System.Collections;
    using System.Collections.ObjectModel;
    class ReportModelView
    {
        public class DateValue
        {
            public DateTime Date { get; set; }
            public double Value { get; set; }
        }
        public class HValue
        {
            public double H { get; set; }
            public double Value { get; set; }
        }

        public Collection<DateValue> routingData { get; set; }
        public Collection<HValue> HData { get; set; }



        public PlotModel CreateModel(int mode,int reportIndex,string title,string lengend)
        {
            if (mode == 0)
            {
                string filename = Global.projectName + @"\InputFiles\Routing\2_input_time_series.txt";
                var tmp = new PlotModel { Title = title };
                tmp.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "MMM/dd/yyyy", MajorGridlineStyle = LineStyle.Solid });
                tmp.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, MajorGridlineStyle = LineStyle.Solid });
                routingData = new Collection<DateValue>();
                var date = Global.startDate;

                string[] contents = File.ReadAllLines(filename);
                if (contents.Length < 2)
                {
                    throw new Exception(filename + " length is too short");
                }
                for (int i = 1; i < contents.Length; i++)
                {
                    string[] parameters = contents[i].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                    if (parameters.Length == 5)
                    {
                        double value = Convert.ToDouble(parameters[reportIndex + 1]);
                        routingData.Add(new DateValue { Date = date, Value = value });
                        date = date.AddDays(1);
                    }
                    else
                    {
                        throw new Exception("We need to have 5 parameters in line " + i.ToString());
                    }
                }
                // Create a line series
                var s1 = new LineSeries
                {
                    Title = lengend,
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    ItemsSource = this.routingData,
                    DataFieldX = "Date",
                    DataFieldY = "Value",
                    Color = OxyColors.Blue,
                    LineStyle = OxyPlot.LineStyle.Solid,
                    MarkerStroke = OxyColors.Red,
                    MarkerType = MarkerType.Circle
                };
                tmp.Series.Add(s1);
                return tmp;
            }
            else if(mode==1)
            {
                string filename = Global.projectName + @"\InputFiles\Routing\3_geometry_inputs.txt";
                var tmp = new PlotModel { Title = title };
                tmp.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, MajorGridlineStyle = LineStyle.Solid/*,Title="Test!",TitleFontSize=20*/ });
                tmp.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, MajorGridlineStyle = LineStyle.Solid });
                HData = new Collection<HValue>();
                string[] contents = File.ReadAllLines(filename);
                if (contents.Length < 2)
                {
                    throw new Exception(filename + " length is too short");
                }
                for (int i = 1; i < contents.Length; i++)
                {
                    string[] parameters = contents[i].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                    if (parameters.Length == 4)
                    {
                        double h = Convert.ToDouble(parameters[0]);
                        double value = Convert.ToDouble(parameters[reportIndex + 1]);
                        HData.Add(new HValue { H=h, Value = value });
                    }
                    else
                    {
                        throw new Exception("We need to have 4 parameters in line " + i.ToString());
                    }
                }

                // Create a line series
                var s1 = new LineSeries
                {
                    Title = lengend,
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    ItemsSource = this.HData,
                    DataFieldX = "H",
                    DataFieldY = "Value",
                    Color = OxyColors.Blue,
                    LineStyle = OxyPlot.LineStyle.Solid,
                    MarkerStroke = OxyColors.Red,
                    MarkerType = MarkerType.Circle
                };
                tmp.Series.Add(s1);
                return tmp;
            }
            else
            {
                string filename = Global.projectName + @"\InputFiles\Routing\5_output.txt";
                var tmp = new PlotModel { Title = title };
                tmp.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "MMM/dd/yyyy", MajorGridlineStyle = LineStyle.Solid });
                tmp.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, MajorGridlineStyle = LineStyle.Solid });
                routingData = new Collection<DateValue>();
                var date = Global.startDate;

                string[] contents = File.ReadAllLines(filename);
                if (contents.Length < 2)
                {
                    throw new Exception(filename + " length is too short");
                }
                for (int i = 1; i < contents.Length; i++)
                {
                    string[] parameters = contents[i].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                    if (parameters.Length == 5)
                    {
                        double value = Convert.ToDouble(parameters[reportIndex + 1]);
                        routingData.Add(new DateValue { Date = date, Value = value });
                        date = date.AddDays(1);
                    }
                    else
                    {
                        throw new Exception("We need to have 5 parameters in line " + i.ToString());
                    }
                }
                // Create a line series
                var s1 = new LineSeries
                {
                    Title = lengend,
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    ItemsSource = this.routingData,
                    DataFieldX = "Date",
                    DataFieldY = "Value",
                    Color = OxyColors.Blue,
                    LineStyle = OxyPlot.LineStyle.Solid,
                    MarkerStroke = OxyColors.Red,
                    MarkerType = MarkerType.Circle
                };
                tmp.Series.Add(s1);
                return tmp;
            }
        }
    }
}
