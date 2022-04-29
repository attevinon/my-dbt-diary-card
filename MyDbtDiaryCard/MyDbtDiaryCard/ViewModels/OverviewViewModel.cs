using MyDbtDiaryCard.Services.ChartsService;
using MyDbtDiaryCard.Services.DataService;
using MyDbtDiaryCard.Services.Navigation;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyDbtDiaryCard.ViewModels
{
    internal class OverviewViewModel : BaseViewModel
    {
        public List<int> DaysCountList { get; set; }

        private int daysCount = 14;
        public int DaysCount
        {
            get { return daysCount; }
            set 
            {
                SetProperty(ref daysCount, value);
            }
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

        private PlotModel urgesChart;
        public PlotModel UrgesChart
        {
            get { return urgesChart; }
            set { SetProperty(ref urgesChart, value); }
        }



        public OverviewViewModel(INavigationService navigation) : base(navigation)
        {
            DataService.GetDataManager().DayEntryData.EntryDataUpdated += DataUpdated;
            DaysCountList = new List<int> { 7, 14 };
        }

        private async void DataUpdated(object sender, EventArgs e)
        {
            await RefreshCharts();
        }

        private async Task RefreshCharts()
        {
            var entries = new Model.EntriesStats();
            await entries.Initialize(DaysCount);

            var chartsDirector = new ChartsDirector(entries);

            var builder = new LineChartBuilder();
            EmotionsChart = chartsDirector.BuildEmotionsChart(builder);
            FeelingsChart = chartsDirector.BuildFeelingsChart(builder);
            UrgesChart = chartsDirector.BuildUrgesChart(builder);
        }
    }
}
