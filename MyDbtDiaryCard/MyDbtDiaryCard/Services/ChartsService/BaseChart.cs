using System;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Collections.Generic;
using System.Text;
using MyDbtDiaryCard.Model;

namespace MyDbtDiaryCard.Services.ChartsService
{
    internal class BaseChart
    {
        public DateTime[] Dates { get; }
        private EntriesStats EntriesStatsOfPeriod { get; }
        private bool isInitialized;

        public BaseChart(int daysCount, EntriesStats entriesStats = null)
        {
            EntriesStatsOfPeriod = entriesStats;
            Dates = entriesStats.Dates;

            if(Dates == null)
            {
                Dates = new DateTime[daysCount];
                for (int i = 0; i < Dates.Length; i++)
                {
                    Dates[i] = DateTime.Today.AddDays(-i);
                }

                isInitialized = false;

                Array.Reverse(Dates);
            }
            else
            {
                isInitialized = true;
            }

        }

        private PlotModel CreateBaseLineSeriesModel(bool isBiggerScale = false)
        {
            var plotModel = new PlotModel()
            {
                LegendPosition = LegendPosition.BottomCenter,
                LegendPlacement = LegendPlacement.Outside,
                LegendItemAlignment = HorizontalAlignment.Center
            };

            try
            {
                plotModel.Axes.Add(new DateTimeAxis()
                {
                    Position = AxisPosition.Bottom,
                    IntervalType = DateTimeIntervalType.Days,
                    IsZoomEnabled = false,
                    StringFormat = "dd/MM"
                    //unit?
                }) ;


                if (isBiggerScale)
                {
                    plotModel.Axes.Add(
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
                    plotModel.Axes.Add(
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return plotModel;
        }

        public PlotModel CreateFeelingsChart()
        {
            //title adds in view
            var feelingsChart = CreateBaseLineSeriesModel();

            if (!isInitialized)
                return feelingsChart;

            var emMiseryLine = new LineSeries() { Color = OxyColors.Gold,
                Title = "Emotional misery"
            };
            var phMiseryLine = new LineSeries() { Color = OxyColors.CornflowerBlue, Title = "Physical misery" };
            var excitationLine = new LineSeries() { Color = OxyColors.IndianRed, Title = "Excitation" };

            foreach (var daysFeelings in EntriesStatsOfPeriod.FeelingsArray)
            {
                var date = daysFeelings.Date;

                emMiseryLine.Points.Add(new DataPoint(DateTimeAxis.ToDouble(date), daysFeelings?.EmotionMisery ?? double.NaN));
                phMiseryLine.Points.Add(new DataPoint(DateTimeAxis.ToDouble(date), daysFeelings?.PhysicalMisery ?? double.NaN));
                excitationLine.Points.Add(new DataPoint(DateTimeAxis.ToDouble(date), daysFeelings?.Excitation ?? double.NaN));
            }

            feelingsChart.Series.Add(emMiseryLine);
            feelingsChart.Series.Add(phMiseryLine);
            feelingsChart.Series.Add(excitationLine);

            return feelingsChart;
        }

    }
}
