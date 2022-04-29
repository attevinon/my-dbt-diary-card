using MyDbtDiaryCard.Model.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyDbtDiaryCard.Services.DataService
{
    internal interface IDataService
    {
        bool IsLoaded { get; }
        IDayEntryRepository DayEntryData { get; }
        IDbtSkillsRepository DbtSkillsData { get; }
        Task InitializeAsync(string dbPath);
        //Task DropAllDataAsync();
    }
}
