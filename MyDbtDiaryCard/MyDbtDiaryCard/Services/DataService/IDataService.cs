using MyDbtDiaryCard.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyDbtDiaryCard.Services.DataService
{
    internal interface IDataService
    {
        bool HasBeenInitialized { get; }
        Task Initialize(string dbPath);
        Task DropAllDataAsync();
        Task<DayEntry> GetDayEntryForDateAsync(DateTime date);
        Task<bool> AddDayEntryAsync(DayEntry newDayEntry);
        Task<bool> DeleteDayEntryAsync(DayEntry dayEntryToRemove);
        Task<List<DayEntry>> GetDayEntriesPerPeriod(DateTime startDate, DateTime endDate);
        Task<List<DayEntry>> GetTheLatestDayEntries(int daysCount);
        Task<List<DayEntry>> GetAllDayEntries();

    }
}
