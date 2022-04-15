using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MyDbtDiaryCard.Model;
using MyDbtDiaryCard.Model.EntryItems;
using SQLite;

namespace MyDbtDiaryCard.Services.DataService
{
    internal class SQLiteDataService : IDataService
    {
        private static SQLiteDataService dataService;
        public static IDataService DataService { get; }

        private SQLiteAsyncConnection connection;
        private static readonly object _locker = new object();

        public bool HasBeenInitialized { get; private set; } = false;

        static SQLiteDataService()
        {
            DataService = GetDataService();
        }

        private SQLiteDataService()
        {
            HasBeenInitialized = false;
        }

        public static IDataService GetDataService()
        {
            if(dataService == null)
            {
                lock (_locker)
                {
                    if(dataService == null)
                    {
                        dataService = new SQLiteDataService();
                    }
                }
            }

            return dataService;
        }
        public async Task Initialize(string dbPath)
        {
            try
            {
                connection = new SQLiteAsyncConnection(dbPath);

                await connection.EnableWriteAheadLoggingAsync();

                var r1 = await connection.CreateTableAsync<Feelings>();
                await connection.CreateTableAsync<Emotions>();

                    Console.WriteLine(r1);

                var r = await connection.CreateTableAsync<DayEntry>();

                    Console.WriteLine(r);

                HasBeenInitialized = true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                HasBeenInitialized = false;
            }

        }

        public async Task<bool> AddDayEntryAsync(DayEntry newDayEntry)
        {
            try
            {
                int res;

                if (await GetDayEntryForDateAsync(newDayEntry.Date) == null)
                {
                    res = await connection.InsertAsync(newDayEntry);
                    int resFeelings = await connection.InsertAsync(newDayEntry.DayFeelings);
                    int resEmotions = await connection.InsertAsync(newDayEntry.DayEmotions);

                    if (resFeelings != 1 || resEmotions != 1)
                    {
                        Console.WriteLine("Exception while saving data Feelings/Emo!!!!!!!!!");
                        return false;
                    }
                }
                else
                {
                    res = await connection.UpdateAsync(newDayEntry);
                }

                if (res != 1)
                {
                    Console.WriteLine("Exception while saving data!");
                    return false;
                }

                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex + ex.Message);
                return false;
            }
        }

        public async Task<DayEntry> GetDayEntryForDateAsync(DateTime date)
        {
            var day = await connection.Table<DayEntry>().Where(d => d.Date == date).FirstOrDefaultAsync();

            if(day != null)
            {
                day.DayFeelings = await connection.Table<Feelings>().Where(f => f.Date == date).FirstOrDefaultAsync();
                day.DayEmotions = await connection.Table<Emotions>().Where(e => e.Date == date).FirstOrDefaultAsync();
            }

            return day;
        }

        public async Task<List<DayEntry>> GetDayEntriesPerPeriod(DateTime startDate, DateTime endDate)
        {
            var entries = await connection.Table<DayEntry>()
                .Where(d => d.Date >= startDate && d.Date <= endDate)
                .OrderByDescending(d => d.Date)
                .ToListAsync();

            return entries;
        }

        public async Task<List<DayEntry>> GetTheLatestDayEntries(int daysCount)
        {
            var entries = await connection.Table<DayEntry>()
                .OrderByDescending(d => d.Date)
                .Take(daysCount)
                .ToListAsync();

            return entries;
        }

        public async Task<List<DayEntry>> GetAllDayEntries()
        {
            var entries = await connection.Table<DayEntry>()
                .OrderByDescending(d => d.Date)
                .ToListAsync();

            return entries;
        }

        public async Task DropAllDataAsync()
        {
            await connection.DeleteAllAsync<DayEntry>();
            await connection.DeleteAllAsync<Feelings>();
            await connection.DeleteAllAsync<Emotions>();
        }

        public async Task<bool> DeleteDayEntryAsync(DayEntry dayEntryToRemove)
        {
            try
            {
                await connection.DeleteAsync(dayEntryToRemove.DayFeelings);
                await connection.DeleteAsync(dayEntryToRemove.DayEmotions);
                var res = await connection.DeleteAsync(dayEntryToRemove);

                return res == 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
