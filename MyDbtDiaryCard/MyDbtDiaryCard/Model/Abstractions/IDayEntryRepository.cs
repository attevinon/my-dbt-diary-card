using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyDbtDiaryCard.Model.Abstractions
{
    internal interface IDayEntryRepository
    {
        Task Init();
        Task<DayEntry> GetDayEntryForDateAsync(DateTime date);
        Task<List<DayEntry>> GetDayEntriesPerPeriodAsync(DateTime startDate, DateTime endDate);
        Task<List<DayEntry>> GetTheLatestDayEntriesAsync(int daysCount);
        Task<bool> AddDayEntryAsync(DayEntry newDayEntry);
        Task<bool> DeleteDayEntryAsync(DayEntry dayEntryToRemove);
    }
}
