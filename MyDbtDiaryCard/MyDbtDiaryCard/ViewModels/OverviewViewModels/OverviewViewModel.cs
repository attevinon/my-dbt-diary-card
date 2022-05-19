using MyDbtDiaryCard.Services.ChartsService;
using MyDbtDiaryCard.Services.Commands;
using MyDbtDiaryCard.Services.DataService;
using MyDbtDiaryCard.Services.Navigation;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyDbtDiaryCard.ViewModels
{
    //make common parent class for overviews?
    internal class OverviewViewModel : BaseOverviewViewModel
    {
        public override int DaysRange
        {
            get { return base.DaysRange; }
            set 
            {
                base.DaysRange = value;
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

        public ICommand ShowTableOverviewCommand { get; set; }

        public OverviewViewModel(INavigationService navigation) : base(navigation)
        {
            DataService.GetDataManager().DayEntryData.EntryDataUpdated += DataUpdated;

            ShowTableOverviewCommand = new ActionCommand(async () => await ShowTableView());
        }

        private void DataUpdated(object sender, EventArgs e)
        {
            RefreshCharts();
        }

        private async void RefreshCharts()
        {
            if (DataService.GetDataManager().DayEntryData.HasBeenInitialized == false)
                return;

            var entries = new Model.EntriesStats.DetailedEntriesStats();
            //change, not universal
            await entries.Initialize(DateTime.Today.AddDays(-DaysRange), DateTime.Today);

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

        private async Task ShowTableView()
        {
            await NavigationService.NavigateAsync("TableOverviewPage");
        }
    }
}
