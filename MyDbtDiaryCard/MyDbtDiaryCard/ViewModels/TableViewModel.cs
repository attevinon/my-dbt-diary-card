using MyDbtDiaryCard.Services.Navigation;
using MyDbtDiaryCard.Services.DataService;
using System;
using System.Collections.Generic;
using MyDbtDiaryCard.Model;
using System.Windows.Input;
using MyDbtDiaryCard.Services.Commands;

namespace MyDbtDiaryCard.ViewModels
{
    internal class TableViewModel : BaseViewModel
    {
        private int daysRange = 7;
        public int DaysRange
        {
            get { return daysRange; }
            set { SetProperty(ref daysRange, value);
                Console.WriteLine(" public int DaysRange SET");
            }
        }

        private DateTime startDate ;
        public DateTime StartDate 
        { 
            get => startDate;
            set
            {
                if (value > DateTime.Today)
                    throw new Exception("Start date must not be from future");

                SetProperty(ref startDate, value);
                Console.WriteLine(" public DateTime StartDate SET");
                EndDate = value.AddDays(DaysRange);

            } 
        }

        private DateTime endDate;
        public DateTime EndDate 
        {
            get => endDate;
            set
            {
                if (value > DateTime.Today)
                    value = DateTime.Today;

                SetProperty(ref endDate, value);
                Console.WriteLine(" public DateTime EndDate SET");
                RefreshTable();
            } 
        }

        private IEnumerable<DayEntry> entries;
        public IEnumerable<DayEntry> Entries
        {
            get => entries;
            set
            {
                SetProperty(ref entries, value);
                Console.WriteLine(" IEnumerable<DayEntry> Entries SET");
            } 
        }

        public ICommand GoBackCommand { get; }
        //public ICommand GetPreviousRangeCommand { get; }
        //public ICommand GetNextRangeCommand { get; }

        public TableViewModel(INavigationService navigation) : base(navigation)
        {
            StartDate = DateTime.Today.AddDays(-DaysRange);

            GoBackCommand = new ActionCommand(async () => await NavigationService.GoBackAsync());
        }

        private async void RefreshTable()
        {
            try
            {
                IsLoading = true;

                if (DataService.GetDataManager().DayEntryData.HasBeenInitialized == false)
                    return;

                var entries = new EntriesStats();
                await entries.Initialize(StartDate, EndDate);

                if (entries == null)
                {
                    return;
                }

                Entries = new List<DayEntry>(entries.DaysOfPeriod);

                IsLoading = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex.StackTrace);
            }

        }

    }
}
