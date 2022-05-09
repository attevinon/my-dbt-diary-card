﻿using MyDbtDiaryCard.Model.Abstractions;
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
        private DateTime start;
        private DateTime end;
        public int DaysCount { get; private set; }
        public DateTime[] Dates { get; private set; }
        public (int[] anger, int[] sadness, int[] fear, int[] shame, int[] pride, int[] joy) EmotionsStats { get; set; }
        public (int[] emMisery, int[] phMisery, int[] excitation) FeelingsStats { get; set; }
        public (int[] selfharm, int[] suicide, int[] drugs, int[] alcohol) UrgesStats { get; set; }

        public EntriesStats()
        {
            _dayEntryDataService = DataService.GetDataManager().DayEntryData;
        }
        public async Task Initialize(DateTime startOfPeriod, DateTime endOfPeriod)
        {
            if (daysOfPeriod != null)
                return;

            start = startOfPeriod;
            end = endOfPeriod;

            Dates = await GetDataStats();
            await GetDaysPeriod();

            await SetProperties();
        }

        private async Task SetProperties()
        {
            EmotionsStats = await GetEmotionsStats();
            FeelingsStats = await GetFeelingsStats();
            UrgesStats = await GetUrgesStats();
        }

        private async Task GetDaysPeriod()
        {
            var daysOfPeriod = await _dayEntryDataService.GetDayEntriesPerPeriodAsync(start, end);
            DaysCount = daysOfPeriod.Count;

            this.daysOfPeriod = new List<DayEntry>(Dates.Length);

            foreach (var day in Dates)
            {
                this.daysOfPeriod.Add(daysOfPeriod.Find(d => d.Date == day) ?? null);
            }
        }

        private async Task<(int[], int[], int[], int[], int[], int[])> GetEmotionsStats()
        {
            var anger = new int[daysOfPeriod.Count];
            var sadness = new int[daysOfPeriod.Count];
            var fear = new int[daysOfPeriod.Count];
            var shame = new int[daysOfPeriod.Count];
            var pride = new int[daysOfPeriod.Count];
            var joy = new int[daysOfPeriod.Count];

            await Task.Run(() =>
            {
                for (int i = 0; i < Dates.Length; i++)
                {
                    var emotions = daysOfPeriod[i]?.DayEmotions ?? new Emotions();

                    anger[i] = emotions.Anger;
                    sadness[i] = emotions.Sadness;
                    fear[i] = emotions.Fear;
                    shame[i] = emotions.Shame;
                    pride[i] = emotions.Pride;
                    joy[i] = emotions.Joy;
                }
            });

            return (anger, sadness, fear, shame, pride, joy);
        }

        private async Task<(int[], int[], int[])> GetFeelingsStats()
        {
            var emMisery = new int[daysOfPeriod.Count];
            var phMisery = new int[daysOfPeriod.Count];
            var excitation = new int[daysOfPeriod.Count];

            await Task.Run(() =>
            {
                for (int i = 0; i < Dates.Length; i++)
                {
                    var feelings = daysOfPeriod[i]?.DayFeelings ?? new Feelings();

                    emMisery[i] = feelings.EmotionMisery;
                    phMisery[i] = feelings.PhysicalMisery;
                    excitation[i] = feelings.Excitation;
                }
            });

            return (emMisery, phMisery, excitation);
        }

        private async Task<(int[], int[], int[], int[])> GetUrgesStats()
        {
            var selfharm = new int[daysOfPeriod.Count];
            var suicide = new int[daysOfPeriod.Count];
            var drugs = new int[daysOfPeriod.Count];
            var alcohol = new int[daysOfPeriod.Count];

            await Task.Run(() =>
            {
                for (int i = 0; i < Dates.Length; i++)
                {
                    var urges = daysOfPeriod[i]?.DayUrges ?? new Urges();

                    selfharm[i] = urges.SelfHarm;
                    suicide[i] = urges.Suicide;
                    drugs[i] = urges.Drugs;
                    alcohol[i] = urges.Alcohol;
                }
            });

            return (selfharm, suicide, drugs, alcohol);
        }

        //method to save days info

        private async Task<DateTime[]> GetDataStats()
        {
            var dates = new DateTime[(end - start).Days];

            await Task.Run(() => 
            {
                for (int i = 0; i < dates.Length; i++)
                {
                    dates[i] = start.AddDays(i);
                }
            });

            return dates;
        }
    }
}
