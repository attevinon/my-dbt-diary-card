using System;
using System.Collections.Generic;
using MyDbtDiaryCard.Model.Abstractions;
using MyDbtDiaryCard.Services.DataService;
using System.Threading.Tasks;
using System.Text;

namespace MyDbtDiaryCard.Model.EntriesStats
{
    internal class BaseEntriesStats
    {
        private readonly IDayEntryRepository _dayEntryDataService;

        private DateTime start;
        private DateTime end;
        public List<DayEntry> DaysOfPeriod { get; private set; }
        public int DaysCount { get; private set; }
        public DateTime[] Dates { get; private set; }

        public BaseEntriesStats()
        {
            _dayEntryDataService = DataService.GetDataManager().DayEntryData;
        }

        public virtual async Task Initialize(DateTime startOfPeriod, DateTime endOfPeriod)
        {
            if (DaysOfPeriod != null)
                return;

            start = startOfPeriod;
            end = endOfPeriod;

            Dates = await GetDataStats();
            DaysOfPeriod = await GetDaysPeriod();
        }

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

        private async Task<List<DayEntry>> GetDaysPeriod()
        {
            var daysOfPeriodInit = await _dayEntryDataService.GetDayEntriesPerPeriodAsync(start, end);
            DaysCount = daysOfPeriodInit.Count;

            var daysOfPeriod = new List<DayEntry>(Dates.Length);

            foreach (var day in Dates)
            {
                daysOfPeriod.Add(daysOfPeriodInit.Find(d => d.Date == day) ?? new DayEntry(day));
            }

            return daysOfPeriod;
        }
    }
}
