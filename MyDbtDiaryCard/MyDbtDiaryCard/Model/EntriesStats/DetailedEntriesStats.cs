using MyDbtDiaryCard.Model.EntryItems;
using MyDbtDiaryCard.Services.DataService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyDbtDiaryCard.Model.EntriesStats
{
    internal class DetailedEntriesStats : BaseEntriesStats
    {
        public (int[] anger, int[] sadness, int[] fear, int[] shame, int[] pride, int[] joy) EmotionsStats { get; private set; }
        public (int[] emMisery, int[] phMisery, int[] excitation) FeelingsStats { get; private set; }
        public (int[] selfharm, int[] suicide, int[] drugs, int[] alcohol) UrgesStats { get; private set; }

        public DetailedEntriesStats() : base()
        { }
        public override async Task Initialize(DateTime startOfPeriod, DateTime endOfPeriod)
        {
            await base.Initialize(startOfPeriod, endOfPeriod);
            await SetProperties();
        }

        private async Task SetProperties()
        {
            EmotionsStats = await GetEmotionsStats();
            FeelingsStats = await GetFeelingsStats();
            UrgesStats = await GetUrgesStats();
        }

        private async Task<(int[], int[], int[], int[], int[], int[])> GetEmotionsStats()
        {
            var anger = new int[DaysOfPeriod.Count];
            var sadness = new int[DaysOfPeriod.Count];
            var fear = new int[DaysOfPeriod.Count];
            var shame = new int[DaysOfPeriod.Count];
            var pride = new int[DaysOfPeriod.Count];
            var joy = new int[DaysOfPeriod.Count];

            await Task.Run(() =>
            {
                for (int i = 0; i < Dates.Length; i++)
                {
                    var emotions = DaysOfPeriod[i]?.DayEmotions ?? new Emotions();

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
            var emMisery = new int[DaysOfPeriod.Count];
            var phMisery = new int[DaysOfPeriod.Count];
            var excitation = new int[DaysOfPeriod.Count];

            await Task.Run(() =>
            {
                for (int i = 0; i < Dates.Length; i++)
                {
                    var feelings = DaysOfPeriod[i]?.DayFeelings ?? new Feelings();

                    emMisery[i] = feelings.EmotionMisery;
                    phMisery[i] = feelings.PhysicalMisery;
                    excitation[i] = feelings.Excitation;
                }
            });

            return (emMisery, phMisery, excitation);
        }

        private async Task<(int[], int[], int[], int[])> GetUrgesStats()
        {
            var selfharm = new int[DaysOfPeriod.Count];
            var suicide = new int[DaysOfPeriod.Count];
            var drugs = new int[DaysOfPeriod.Count];
            var alcohol = new int[DaysOfPeriod.Count];

            await Task.Run(() =>
            {
                for (int i = 0; i < Dates.Length; i++)
                {
                    var urges = DaysOfPeriod[i]?.DayUrges ?? new Urges();

                    selfharm[i] = urges.SelfHarm;
                    suicide[i] = urges.Suicide;
                    drugs[i] = urges.Drugs;
                    alcohol[i] = urges.Alcohol;
                }
            });

            return (selfharm, suicide, drugs, alcohol);
        }

        //method to save days info
    }
}
