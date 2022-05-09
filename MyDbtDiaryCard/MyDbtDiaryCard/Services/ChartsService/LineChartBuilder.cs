using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyDbtDiaryCard.Services.ChartsService
{
    internal class LineChartBuilder : ILineChartBuilder
    {
        private PlotModel chart;
        private DateTime[] dates;

        public void Reset()
        {
            chart = new PlotModel();
        }

        public void SetPlotModel(DateTime[] dates)
        {
            if (dates == null)
                return;

            this.dates = dates;

            try
            {
                var axis = new DateTimeAxis()
                {
                    Position = AxisPosition.Bottom,
                    IntervalType = DateTimeIntervalType.Days,
                    StringFormat = "dd/MM",
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                };


                axis.Reset();

                if(dates != null && dates.Length != 0)
                {
                    axis.MinimumRange = dates.Length;
                    axis.MaximumRange = dates.Length + 1.0;
                    axis.Minimum = DateTimeAxis.ToDouble(dates[0].AddDays(-1));
                    axis.Maximum = DateTimeAxis.ToDouble(dates[dates.Length - 1].AddDays(1));

                    if (dates.Length <= 7)
                    {
                        axis.MajorStep = 1;
                        axis.MinorTickSize = 0;
                    }
                    else if (dates.Length <= 14)
                    {
                        axis.MajorStep = 2;
                    }
                    else
                    {
                        axis.MajorStep = 4;
                    }
                }

                chart.Axes.Add(axis);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public void SetSeries(int[] dataArray, OxyColor color, string titile = null)
        {
            bool isSmooth = false;
            DataPoint[] points = new DataPoint[dates.Length];

            for (int i = 0; i < points.Length; i++)
            {
                points[i] = dataArray[i] == -1 
                    ? new DataPoint(double.NaN, double.NaN) 
                    : new DataPoint(DateTimeAxis.ToDouble(dates[i]), dataArray[i]);
            }

            try
            {
                var line = new LineSeries()
                {
                    Title = titile,
                    Color = color,
                    LineJoin = LineJoin.Round,
                    MarkerType = MarkerType.Circle,
                    MarkerFill = color,
                    MarkerSize = 3,
                };

                line.ItemsSource = points;
                line.Smooth = isSmooth;

                chart.Series.Add(line);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

        }

        public PlotModel GetChart()
        {
            return chart;
        }

        public void SetLeftAxis(bool isBiggerScale)
        {
            LinearAxis axis = new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = -0.1,
                IsZoomEnabled = false,
                IsPanEnabled = false,
                MajorStep = 1,
                MinorStep = 10
            };

            if (isBiggerScale)
            {
                axis.Maximum = 6.5;
                axis.LabelFormatter = new Func<double, string>(
                             (double d) => d == 6 ? "X" : d.ToString());

            }
            else
            {
                axis.Maximum = 5.5;
            }

            chart.Axes.Add(axis);
        }

        public void SetLegend(string title)
        {
            chart.LegendPosition = LegendPosition.BottomCenter;
            chart.LegendPlacement = LegendPlacement.Outside;
            chart.LegendItemAlignment = HorizontalAlignment.Center;
            chart.LegendOrientation = LegendOrientation.Horizontal;
            chart.LegendFontSize = 13;
            chart.Title = title;
        }
    }

}
