using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MyDbtDiaryCard.Model;
using MyDbtDiaryCard.Model.Abstractions;
using MyDbtDiaryCard.Model.EntryItems;
using SQLite;

namespace MyDbtDiaryCard.Services.DataService.Repositories
{
    internal class DayEntryRepository : BaseRepository, IDayEntryRepository
    {
        public DayEntryRepository(SQLiteAsyncConnection connection) : base(connection) 
        {
            hasBeenInitialized = false; 
        }

        public async Task Init()
        {
            try
            {
                var r1 = await connection.CreateTableAsync<Feelings>();
                await connection.CreateTableAsync<Emotions>();
                await connection.CreateTableAsync<Urges>();

                var r = await connection.CreateTableAsync<DayEntry>();

                hasBeenInitialized = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                hasBeenInitialized = false;
            }
        }

        public async Task<List<DayEntry>> GetTheLatestDayEntriesAsync(int daysCount)
        {
            var entries = await connection.Table<DayEntry>()
                .OrderByDescending(d => d.Date)
                .Take(daysCount)
                .ToListAsync();

            foreach (var day in entries)
            {
                await FindItemsForDayEntryAsync(day);
            }

            return entries;
        }

        public async Task<DayEntry> GetDayEntryForDateAsync(DateTime date)
        {
            var day = await FindByConditionAsync<DayEntry>(d => d.Date == date);

            await FindItemsForDayEntryAsync(day);

            return day;
        }

        public async Task<List<DayEntry>> GetDayEntriesPerPeriodAsync(DateTime startDate, DateTime endDate)
        {
            var entries = await FindManyByConditionAsync<DayEntry, DateTime>(
                d => d.Date >= startDate && d.Date <= endDate, d => d.Date, true);

            foreach (var day in entries)
            {
                await FindItemsForDayEntryAsync(day);
            }

            return entries;
        }

        public async Task<bool> AddDayEntryAsync(DayEntry newDayEntry)
        {
            try
            {
                bool result;

                if (await GetDayEntryForDateAsync(newDayEntry.Date) == null)
                {
                    result = await CreateAsync(newDayEntry)
                        && await CreateAsync(newDayEntry.DayFeelings)
                        && await CreateAsync(newDayEntry.DayEmotions)
                        && await CreateAsync(newDayEntry.DayUrges);

                }
                else
                {
                    result = await UpdateAsync(newDayEntry)
                        && await UpdateAsync(newDayEntry.DayFeelings)
                        && await UpdateAsync(newDayEntry.DayFeelings)
                        && await UpdateAsync(newDayEntry.DayUrges);
                }

                if (!result)
                {
                    Console.WriteLine("Exception while saving data!");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteDayEntryAsync(DayEntry dayEntryToRemove)
        {
            var result = await DeleteAsync(dayEntryToRemove);

            if (dayEntryToRemove.DayFeelings != null)
                result = result && await DeleteAsync(dayEntryToRemove.DayFeelings);

            if (dayEntryToRemove.DayEmotions != null)
                result = result && await DeleteAsync(dayEntryToRemove.DayEmotions);

            if (dayEntryToRemove.DayUrges != null)
                result = result && await DeleteAsync(dayEntryToRemove.DayUrges);

            return result;
        }

        private async Task FindItemsForDayEntryAsync(DayEntry day)
        {
            if (day == null)
                return;

            day.DayFeelings = await FindByConditionAsync<Feelings>(f => f.Date == day.Date);
            day.DayEmotions = await FindByConditionAsync<Emotions>(e => e.Date == day.Date);
            day.DayUrges = await FindByConditionAsync<Urges>(u => u.Date == day.Date);
        }
    }
}
