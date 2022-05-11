using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyDbtDiaryCard.Model.Abstractions;
using MyDbtDiaryCard.Services.DataService.SQLiteRepositories;
using SQLite;

namespace MyDbtDiaryCard.Services.DataService
{
    internal class DataService : IDataService
    {
        private static readonly object _locker = new object();
        private static IDataService dataService;

        private Lazy<IDayEntryRepository> _lazyDayEntry;
        public IDayEntryRepository DayEntryData => _lazyDayEntry.Value;

        private Lazy<IDbtSkillsRepository> _lazyDbtSkills;
        public IDbtSkillsRepository DbtSkillsData => _lazyDbtSkills.Value;

        public bool IsLoaded { get; private set; }

        private DataService() { }
        public async Task InitializeAsync(string dbPath)
        {
            var connection = new SQLiteAsyncConnection(dbPath);

            _lazyDayEntry = new Lazy<IDayEntryRepository>(() => new DayEntryRepository(connection));
            _lazyDbtSkills = new Lazy<IDbtSkillsRepository>(() => new DbtSkillsRepository(connection));

            try
            {
                await _lazyDbtSkills.Value.Init();
                await _lazyDayEntry.Value.Init();

                IsLoaded = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex.StackTrace);
                IsLoaded = false;
            }
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
