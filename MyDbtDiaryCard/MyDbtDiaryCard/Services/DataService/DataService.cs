using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyDbtDiaryCard.Model.Abstractions;
using MyDbtDiaryCard.Services.DataService.Repositories;
using SQLite;

namespace MyDbtDiaryCard.Services.DataService
{
    internal class DataService : IDataService
    {
        private static readonly object _locker = new object();
        private static IDataService dataService;

        private Lazy<IDayEntryRepository> _lazyDayEntry;
        public IDayEntryRepository DayEntryData => _lazyDayEntry.Value;

        private DataService() { }
        public async Task InitializeAsync(string dbPath)
        {
            var connection = new SQLiteAsyncConnection(dbPath);

            //_lazyDayEntry = new Lazy<IDayEntryRepository>(() => new DayEntryRepository(connection));

            _lazyDayEntry = new Lazy<IDayEntryRepository>(() => new MockDataService());
            await _lazyDayEntry.Value.Init();

        }
        public static IDataService GetDataManager()
        {
            if (dataService == null)
            {
                lock (_locker)
                {
                    if (dataService == null)
                    {
                        dataService = new DataService();
                    }
                }
            }

            return dataService;
        }

    }
}
