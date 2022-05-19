using MyDbtDiaryCard.Services.Navigation;
using MyDbtDiaryCard.Services.DataService;
using System;
using System.Collections.Generic;
using MyDbtDiaryCard.Model;
using System.Windows.Input;
using MyDbtDiaryCard.Services.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace MyDbtDiaryCard.ViewModels
{
    internal class TableViewModel : BaseOverviewViewModel
    {
        public override int DaysRange
        {
            get { return base.DaysRange; }
            set 
            {
                base.DaysRange = value;
            }
        }

        public override DateTime EndDate
        {   
            get => base.EndDate;
            set
            {
                base.EndDate = value;
                RefreshTable();
            }
        }

        private IEnumerable<DayEntry> entries;
        public IEnumerable<DayEntry> Entries
        {
            get => entries;
            set {SetProperty(ref entries, value); } 
        }

        public ICommand GoBackCommand { get; }
        public ICommand ShowUsefulnessHelpCommand { get; }

        public TableViewModel(INavigationService navigation) : base(navigation)
        {
            GoBackCommand = new ActionCommand(async () => await NavigationService.GoBackAsync());
            ShowUsefulnessHelpCommand = new ActionCommand(
                async () => await NavigationService.PushPopupAsync("UsefulnessHelpPage"));
        }

        private async void RefreshTable()
        {
            IsLoading = true;

            if (DataService.GetDataManager().DayEntryData.HasBeenInitialized == false)
                return;

            try
            {
                var entries = new Model.EntriesStats.BaseEntriesStats();
                await entries.Initialize(StartDate, EndDate);

                if (entries.DaysCount < 1)
                {
                    IsEnoughEntries = false;
                    IsLoading = false;
                    return;
                }

                IsEnoughEntries = true;
                Entries = new List<DayEntry>(entries.DaysOfPeriod);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex.StackTrace);
            }

            IsLoading = false;
        }
    }
}
