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
            chart.LegendPosition = LegendPosition.BottomCenter;
            chart.LegendPlacement = LegendPlacement.Outside;
            chart.LegendItemAlignment = HorizontalAlignment.Center;

            try
            {
                chart.Axes.Add(new DateTimeAxis()
                {
                    Position = AxisPosition.Bottom,
                    IntervalType = DateTimeIntervalType.Days,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    StringFormat = "dd/MM"
                    //unit?
                });
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            this.dates = dates;
        }

        public void SetSeries(int[] dataArray, OxyColor color, string titile = null)
        {
            DataPoint[] points = new DataPoint[dates.Length];

            for (int i = 0; i < points.Length; i++)
            {
                points[i] = new DataPoint(DateTimeAxis.ToDouble(dates[i]),
                    dataArray[i] == -1 ? double.NaN : dataArray[i]);
            }

            chart.Series.Add( new LineSeries() { ItemsSource = points, Title = titile, Color = color } );
        }

        public PlotModel GetChart()
        {
            return chart;
        }

        public void SetLeftAxis(bool isBiggerScale)
        {

            if (isBiggerScale)
            {
                chart.Axes.Add(
                    new LinearAxis()
                    {
                        Position = AxisPosition.Left,
                        Minimum = 0,
                        Maximum = 7,
                        IsZoomEnabled = false,
                        IsPanEnabled = false,
                        LabelFormatter = new Func<double, string>(
                             (double d) => d == 6 ? "X" : d.ToString())

                    });
            }
            else
            {
                chart.Axes.Add(
                    new LinearAxis()
                    {
                        Position = AxisPosition.Left,
                        Minimum = -0.1,
                        Maximum = 6,
                        IsPanEnabled = false,
                        IsZoomEnabled = false
                    });
            }
        }

        public void SetLegend()
        {
            throw new NotImplementedException();
        }
    }

}
