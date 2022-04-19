using MyDbtDiaryCard.Model;
using MyDbtDiaryCard.Model.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDbtDiaryCard.Services.DataService
{
    internal class MockDataService : IDayEntryRepository
    {
        public bool HasBeenInitialized { get; private set; }
        private List<DayEntry> DayEntries { get; set; }

        public async Task Init()
        {
            var date = new DateTime(2022, 04, 14);

            DayEntries = new List<DayEntry>();
            DayEntries.Add(new DayEntry(date)
            {
                DayFeelings = new Model.EntryItems.Feelings(date)
                {
                    EmotionMisery = 1,
                    PhysicalMisery = 2,
                    Excitation = 4
                },
                DayEmotions = new Model.EntryItems.Emotions(date)
                {
                    Anger = 3,
                    Sadness = 0,
                    Fear = 1,
                    Pride = 2,
                    Joy = 4
                }
            });

            DayEntries.Add(new DayEntry(date.AddDays(-1))
            {
                DayFeelings = new Model.EntryItems.Feelings(date.AddDays(-1))
                {
                    EmotionMisery = 2,
                    PhysicalMisery = 0,
                    Excitation = 4
                },
                DayEmotions = new Model.EntryItems.Emotions(date.AddDays(-1))
                {
                    Anger = 4,
                    Sadness = 1,
                    Fear = 2,
                    Pride = 0,
                    Joy = 3
                }
            });

            DayEntries.Add(new DayEntry(date.AddDays(-2))
            {
                DayFeelings = new Model.EntryItems.Feelings(date.AddDays(-2))
                {
                    EmotionMisery = 1,
                    PhysicalMisery = 2,
                    Excitation = 4
                },
                DayEmotions = new Model.EntryItems.Emotions(date.AddDays(-2))
                {
                    Anger = 3,
                    Sadness = 2,
                    Fear = 1,
                    Pride = 2,
                    Shame = 4,
                    Joy = 4
                }
            });

            DayEntries.Add(new DayEntry(date.AddDays(-3))
            {
                DayFeelings = new Model.EntryItems.Feelings(date.AddDays(-3))
                {
                    EmotionMisery = 3,
                    PhysicalMisery = 0,
                    Excitation = 1
                },
                DayEmotions = new Model.EntryItems.Emotions(date.AddDays(-3))
                {
                    Anger = 2,
                    Sadness = 3,
                    Fear = 1,
                    Pride = 0,
                    Shame = 4,
                    Joy = 1
                }
            });

            DayEntries.Add(new DayEntry(date.AddDays(-4))
            {
                DayFeelings = new Model.EntryItems.Feelings(date.AddDays(-4))
                {
                    EmotionMisery = 4,
                    PhysicalMisery = 1,
                    Excitation = 0
                },
                DayEmotions = new Model.EntryItems.Emotions(date.AddDays(-4))
                {
                    Anger = 1,
                    Sadness = 4,
                    Fear = 2,
                    Pride = 0,
                    Joy = 1
                }
            });

            DayEntries.Add(new DayEntry(date.AddDays(-5))
            {
                DayFeelings = new Model.EntryItems.Feelings(date.AddDays(-5))
                {
                    EmotionMisery = 5,
                    PhysicalMisery = 0,
                    Excitation = 1
                },
                DayEmotions = new Model.EntryItems.Emotions(date.AddDays(-5))
                {
                    Anger = 2,
                    Sadness = 5,
                    Fear = 1,
                    Shame = 4,
                    Joy = 0
                }
            });

            DayEntries.Add(new DayEntry(date.AddDays(-6))
            {
                DayFeelings = new Model.EntryItems.Feelings(date.AddDays(-6))
                {
                    EmotionMisery = 5,
                    PhysicalMisery = 0,
                    Excitation = 2
                },
                DayEmotions = new Model.EntryItems.Emotions(date.AddDays(-6))
                {
                    Anger = 1,
                    Sadness = 4,
                    Fear = 0,
                    Shame = 5,
                    Pride = 1,
                    Joy = 2
                }
            });

            DayEntries.Add(new DayEntry(date.AddDays(-8))
            {
                DayFeelings = new Model.EntryItems.Feelings(date.AddDays(-8))
                {
                    EmotionMisery = 5,
                    PhysicalMisery = 0,
                    Excitation = 1
                },
                DayEmotions = new Model.EntryItems.Emotions(date.AddDays(-8))
                {
                    Anger = 2,
                    Sadness = 5,
                    Fear = 1,
                    Shame = 4,
                    Joy = 0
                }
            });

            DayEntries.Add(new DayEntry(date.AddDays(-9))
            {
                DayFeelings = new Model.EntryItems.Feelings(date.AddDays(-9))
                {
                    EmotionMisery = 4,
     
                    Excitation = 2
                },
                DayEmotions = new Model.EntryItems.Emotions(date.AddDays(-9))
                {
                    Anger = 1,
                    Sadness = 4,
                    Fear = 0,
                    Shame = 3,
                    Joy = 1
                }
            });


            HasBeenInitialized = true;
        }

        public async Task<bool> AddDayEntryAsync(DayEntry newDayEntry)
        {
            if(GetDayEntryForDateAsync(newDayEntry.Date) != null)
            {
                DayEntries.Remove(await GetDayEntryForDateAsync(newDayEntry.Date));
                DayEntries.Add(newDayEntry);
                return true;
            }

            DayEntries.Add(newDayEntry);
            return true;
        }
        public async Task<DayEntry> GetDayEntryForDateAsync(DateTime date)
        {
            var day = DayEntries.Where(d => d.Date == date).FirstOrDefault();

            return day;
        }

        public async Task<List<DayEntry>> GetDayEntriesPerPeriodAsync(DateTime startDate, DateTime endDate)
        {
            var entries = DayEntries
                .Where(d => d.Date >= startDate && d.Date <= endDate)
                .OrderByDescending(d => d.Date)
                .ToList();

            return entries;
        }


        public async Task<List<DayEntry>> GetTheLatestDayEntriesAsync(int daysCount)
        {
            var entries = DayEntries
                .OrderByDescending(d => d.Date)
                .Take(daysCount)
                .ToList();

            return entries;
        }

        public async Task<List<DayEntry>> GetAllDayEntries()
        {
            return DayEntries;
        }

        public async Task<bool> DeleteDayEntryAsync(DayEntry dayEntryToRemove)
        {
            DayEntries.Remove(dayEntryToRemove);
            return true;
        }


    }
}
