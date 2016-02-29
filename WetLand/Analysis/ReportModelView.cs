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


            int count = 0;
            string simStr = "";
            foreach (var line in File.ReadLines(filename))
            {
                count++;
                if (count > simulationNum + 3 * (simulationNum - 1) && count < simulationNum + 3 * (simulationNum - 1) + 2)
                {
                    simStr += line;
                }
                if (count >= simulationNum + 3 * (simulationNum - 1) + 2)
                {
                    count = 0;
                    break;
                }
            }
            string[] parameters = simStr.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
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
