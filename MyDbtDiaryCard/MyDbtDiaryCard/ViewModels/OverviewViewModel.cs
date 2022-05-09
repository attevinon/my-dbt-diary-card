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

        private int daysCount;
        public int DaysCount
        {
            get { return daysCount; }
            set 
            {
                SetProperty(ref daysCount, value);
                RefreshCharts();
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
        private bool isEnoughEntries;

        public bool IsEnoughEntries
        {
            get { return isEnoughEntries; }
            set { SetProperty(ref isEnoughEntries, value); }
        }


        public OverviewViewModel(INavigationService navigation) : base(navigation)
        {
            daysCount = 7;
            DataService.GetDataManager().DayEntryData.EntryDataUpdated += DataUpdated;
            DaysCountList = new List<int> { 7, 14, 30 };
        }

        private void DataUpdated(object sender, EventArgs e)
        {
            RefreshCharts();
        }

        private async void RefreshCharts()
        {
            if (DataService.GetDataManager().DayEntryData.HasBeenInitialized == false)
                return;

            var entries = new Model.EntriesStats();
            //change, not universal
            await entries.Initialize(DateTime.Today.AddDays(-DaysCount), DateTime.Today);

            if (entries == null || entries.DaysCount <= 1 )
            {
                IsEnoughEntries = false;
                return;
            }

            var chartsDirector = new ChartsDirector(entries);
            IsEnoughEntries = true;

            try
            {
                var builder = new LineChartBuilder();
                EmotionsChart = chartsDirector.BuildEmotionsChart(builder);
                UrgesChart = chartsDirector.BuildUrgesChart(builder);
                FeelingsChart = chartsDirector.BuildFeelingsChart(builder);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
    }
}
