using MyDbtDiaryCard.Model.EntryItems;
using MyDbtDiaryCard.Services.DataService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyDbtDiaryCard.Model
{
    internal class EntriesStats
    {
        readonly IDataService _dataService;

        List<DayEntry> daysOfPeriod;
        public int DaysCount { get; private set; }
        public DateTime[] Dates { get; private set; }
        public Feelings[] FeelingsArray { get; private set; }
        public Emotions[] EmotionsArray { get; private set; }

        public EntriesStats()
        {
            _dataService = SQLiteDataService.GetDataService();
        }
        public async Task Initialize(DateTime startOfPeriod, DateTime endOfPeriod)
        {
            if (daysOfPeriod != null)
                return;

            daysOfPeriod = await _dataService.GetDayEntriesPerPeriod(startOfPeriod, endOfPeriod);
            SetProperties();
        }

        public async Task Initialize(int daysCount)
        {
            if (daysOfPeriod != null)
                return;

            daysOfPeriod = await _dataService.GetTheLatestDayEntries(daysCount);
            SetProperties();
        }

        private void SetProperties()
        {
            DaysCount = daysOfPeriod.Count;
            FeelingsArray = GetFeelingsStatistic();
            EmotionsArray = GetEmotionsStatistics();
        }

        //method to save days info

        private Feelings[] GetFeelingsStatistic()
        {
            var feelingsChart = new Feelings[daysOfPeriod.Count];

            for (int i = 0; i < daysOfPeriod.Count; i++)
            {
                feelingsChart[i] = daysOfPeriod[i].DayFeelings;
            }

            return feelingsChart;
        }

        private Emotions[] GetEmotionsStatistics()
        {
            var emotionsChart = new Emotions[daysOfPeriod.Count];

            for (int i = 0; i < daysOfPeriod.Count; i++)
            {
                emotionsChart[i] = daysOfPeriod[i].DayEmotions;
            }

            return emotionsChart;
        }
    }
}
