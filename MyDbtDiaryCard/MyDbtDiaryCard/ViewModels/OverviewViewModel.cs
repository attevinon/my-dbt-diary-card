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


        public OverviewViewModel(INavigationService navigation) : base(navigation)
        {
            var entries = new Model.EntriesStats();
            entries.Initialize(DaysCount);

            var charts = new BaseChart(daysCount, entries);

            FeelingsChart = charts.CreateFeelingsChart();
        }
    }
}
