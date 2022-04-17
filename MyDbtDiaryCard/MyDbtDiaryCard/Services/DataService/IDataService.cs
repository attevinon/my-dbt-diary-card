using MyDbtDiaryCard.Model.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyDbtDiaryCard.Services.DataService
{
    internal interface IDataService
    {
        IDayEntryRepository DayEntryData { get; }
        Task InitializeAsync(string dbPath);
        //Task DropAllDataAsync();
    }
}
