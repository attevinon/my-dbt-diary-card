using MyDbtDiaryCard.Model.Abstractions;
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
        private readonly IDayEntryRepository _dayEntryDataService;

        private List<DayEntry> daysOfPeriod;
        public int DaysCount { get; private set; }
        public DateTime[] Dates { get; private set; }
        public (int[] anger, int[] sadness, int[] fear, int[] shame, int[] pride, int[] joy) EmotionsStats { get; set; }
        public (int[] emMisery, int[] phMisery, int[] excitation) FeelingsStats { get; set; }

        public EntriesStats()
        {
            _dayEntryDataService = DataService.GetDataManager().DayEntryData;
        }
        public async Task Initialize(DateTime startOfPeriod, DateTime endOfPeriod)
        {
            if (daysOfPeriod != null)
                return;

            daysOfPeriod = await _dayEntryDataService.GetDayEntriesPerPeriodAsync(startOfPeriod, endOfPeriod);
            SetProperties();
        }

        public async Task Initialize(int daysCount)
        {
            if (daysOfPeriod != null)
                return;

            daysOfPeriod = await _dayEntryDataService.GetTheLatestDayEntriesAsync(daysCount);
            SetProperties();
        }

        private void SetProperties()
        {
            DaysCount = daysOfPeriod.Count;
            Dates = GetDataStats();
            EmotionsStats = GetEmotionsStats();
            FeelingsStats = GetFeelingsStats();
        }

        private (int[], int[], int[], int[], int[], int[]) GetEmotionsStats()
        {
            var anger = new int[daysOfPeriod.Count];
            var sadness = new int[daysOfPeriod.Count];
            var fear = new int[daysOfPeriod.Count];
            var shame = new int[daysOfPeriod.Count];
            var pride = new int[daysOfPeriod.Count];
            var joy = new int[daysOfPeriod.Count];

            for (int i = 0; i < DaysCount; i++)
            {
                var emotions = daysOfPeriod[i]?.DayEmotions ?? new Emotions();

                anger[i] = emotions.Anger;
                sadness[i] = emotions.Sadness;
                fear[i] = emotions.Fear;
                shame[i] = emotions.Shame;
                pride[i] = emotions.Pride;
                joy[i] = emotions.Joy;
            }

            return (anger, sadness, fear, shame, pride, joy);
        }

        private (int[], int[], int[]) GetFeelingsStats()
        {
            var emMisery = new int[daysOfPeriod.Count];
            var phMisery = new int[daysOfPeriod.Count];
            var excitation = new int[daysOfPeriod.Count];

            for (int i = 0; i < DaysCount; i++)
            {
                var feelings = daysOfPeriod[i]?.DayFeelings ?? new Feelings();

                emMisery[i] = feelings.EmotionMisery;
                phMisery[i] = feelings.PhysicalMisery;
                excitation[i] = feelings.Excitation;
            }

            return (emMisery, phMisery, excitation);
        }
        //method to save days info

        private DateTime[] GetDataStats()
        {
            var dates = new DateTime[daysOfPeriod.Count];

            for (int i = 0; i < daysOfPeriod.Count; i++)
            {
                dates[i] = daysOfPeriod[i].Date;
            }

            return dates;
        }
    }
}
