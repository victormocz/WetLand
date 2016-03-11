using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WetLand.Analysis
{
    using System.Collections.Generic;
    using OxyPlot;
    using OxyPlot.Series;
    using OxyPlot.Axes;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Text.RegularExpressions;

    class ReportModelView
    {
        public class DateValue
        {
            public DateTime Date { get; set; }
            public double Value { get; set; }
        }
        public PlotModel MyModel { get; private set; }
        public Collection<DateValue> Data { get; set; }
        public int index { get; set; }

        public PlotModel CreateModel(string filename, String Title, String Lengend, int simulationNum, string ytitle)
        {
            var tmp = new PlotModel { Title = Title };
            tmp.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "MMM/dd/yyyy", MajorGridlineStyle = LineStyle.Solid, Title = "Date (day)", TitleFontSize = 15 });
            tmp.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, MajorGridlineStyle = LineStyle.Solid, Title = ytitle, TitleFontSize = 15 });
            this.Data = new Collection<DateValue>();
            var date = Global.startDate;

            bool startAddLine = false;
            StringBuilder simStr = new StringBuilder();
            foreach (var line in File.ReadLines(filename))
            {
                string[] para = line.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                if (para.Length == 1 && Regex.IsMatch(para[0], @"^\d+$") && Convert.ToInt32(para[0]) > simulationNum)
                {
                    startAddLine = false;
                    break;
                }
                if (startAddLine)
                {
                    simStr.Append(line + "\t");
                }
                if (para.Length == 1 && Regex.IsMatch(para[0], @"^\d+$") && Convert.ToInt32(para[0]) == simulationNum)
                {
                    startAddLine = true;
                }
            }
            string[] parameters = simStr.ToString().Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
            simStr.Clear();
            foreach (var parameter in parameters)
            {
                this.Data.Add(new DateValue { Date = date, Value = Convert.ToDouble(parameter) });
                date = date.AddDays(1);
            }


            // Create a line series
            var s1 = new LineSeries
            {
                Title = Lengend,
                StrokeThickness = 1,
                MarkerSize = 3,
                ItemsSource = this.Data,
                DataFieldX = "Date",
                DataFieldY = "Value",
                Color = OxyColors.Blue,
                MarkerStroke = OxyColors.Red,
                MarkerType = MarkerType.None //may need to change
            };

            tmp.Series.Add(s1);
            MyModel = tmp;
            return tmp;
        }
    }

}
