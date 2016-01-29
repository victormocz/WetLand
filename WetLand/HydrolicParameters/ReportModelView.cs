using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WetLand.HydrolicParameters
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

        public PlotModel CreateModel(String Title,String Lengend, int viewCol,String fileName)
        {
            var tmp = new PlotModel { Title = Title };
            tmp.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "MMM/dd/yyyy", MajorGridlineStyle = LineStyle.Solid });
            tmp.Axes.Add(new LinearAxis { Position = AxisPosition.Left,Minimum=0,MajorGridlineStyle=LineStyle.Solid });
            this.Data = new Collection<DateValue>();
            var date = Global.startDate;

            string[] content = File.ReadAllLines(fileName);
            List<Double> col = new List<double>();
            for (int i = 2; i < content.Length; i++)
            {
                string[] parameters = content[i].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                this.Data.Add(new DateValue { Date = date, Value = Convert.ToDouble(parameters[viewCol]) });
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
                MarkerType = MarkerType.Circle
            };

            tmp.Series.Add(s1);
            return tmp;
        }
    }

}
