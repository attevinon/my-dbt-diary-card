using System;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Xamarin;
using System.Collections.Generic;
using System.Text;
using MyDbtDiaryCard.Model;

namespace MyDbtDiaryCard.Services.ChartsService
{
    internal class BaseChart
    {
        public DateTime[] Dates { get; }
        private EntriesStats EntriesStatsOfPeriod { get; }

        public BaseChart(EntriesStats entriesStats)
        {
            EntriesStatsOfPeriod = entriesStats;
            Dates = entriesStats.Dates;
        }

        private PlotModel CreateBaseLineSeriesModel(bool isBiggerScale = false)
        {
            var plotModel = new PlotModel()
            {
                PlotType = PlotType.XY,
                IsLegendVisible = true
            };

            plotModel.Axes.Add(
                new DateTimeAxis() 
                {
                    Position = AxisPosition.Bottom,
                    Minimum = DateTimeAxis.ToDouble(Dates[0]) * 0.9,
                    Maximum = DateTimeAxis.ToDouble(Dates[Dates.Length]) * 1.1,
                    IntervalType = DateTimeIntervalType.Days,
                    StringFormat = "dd/MM",
                    TitleFormatString = "dd/MMM"
                    //unit?
                });

            if (isBiggerScale)
            {
                plotModel.Axes.Add(
                    new LinearAxis()
                    {
                        Position = AxisPosition.Left,
                        Minimum = 0,
                        Maximum = 6 * 1.1,
                        IntervalLength = 1,
                        LabelFormatter = new Func<double, string>(
                             (double d) => d == 6 ?  "X" : d.ToString())
                    });
            }
            else
            {
                plotModel.Axes.Add(
                    new LinearAxis()
                    {
                        Position = AxisPosition.Left,
                        Minimum = 0,
                        Maximum = 5 * 1.1,
                        IntervalLength = 1
                    });
            }

            return plotModel;
        }

        public PlotModel CreateFeelingsChart()
        {
            //title adds in view
            var feelingsChart = CreateBaseLineSeriesModel();

            var emMiseryLine = new LineSeries() { Color = OxyColors.Gold };
            var phMiseryLine = new LineSeries() { Color = OxyColors.CornflowerBlue };
            var excitationLine = new LineSeries() { Color = OxyColors.IndianRed };

            foreach (var daysFeelings in EntriesStatsOfPeriod.FeelingsArray)
            {
                var date = daysFeelings.Date;
                emMiseryLine.Points.Add(DateTimeAxis.CreateDataPoint(date, daysFeelings?.EmotionMisery ?? double.NaN));
                phMiseryLine.Points.Add(DateTimeAxis.CreateDataPoint(date, daysFeelings?.PhysicalMisery ?? double.NaN));
                excitationLine.Points.Add(DateTimeAxis.CreateDataPoint(date, daysFeelings?.Excitation ?? double.NaN));
            }

            return feelingsChart;
        }

    }
}
