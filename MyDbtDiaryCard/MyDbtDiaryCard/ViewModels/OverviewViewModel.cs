using MyDbtDiaryCard.Services.ChartsService;
using MyDbtDiaryCard.Services.Navigation;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyDbtDiaryCard.ViewModels
{
    internal class OverviewViewModel : BaseViewModel
    {
        private int daysCount = 14;
        public int DaysCount
        {
            get { return daysCount; }
            set { daysCount = value; }
        }

        private PlotModel feelingsChart;
        public PlotModel FeelingsChart
        {
            get { return feelingsChart; }
            set { SetProperty(ref feelingsChart, value); }
        }

        private PlotModel emotionsChart;
        public PlotModel EmotionsChart
        {
            get { return emotionsChart; }
            set { SetProperty(ref emotionsChart, value); }
        }


        public OverviewViewModel(INavigationService navigation) : base(navigation)
        {
            var entries = new Model.EntriesStats();
            entries.Initialize(DaysCount);

            var chartsDirector = new ChartsDirector(entries);

            var builder = new LineChartBuilder();
            EmotionsChart = chartsDirector.BuildEmotionsChart(builder);
            FeelingsChart = chartsDirector.BuildFeelingsChart(builder);
        }
    }
}
