using MyDbtDiaryCard.Model;
using MyDbtDiaryCard.Services.Commands;
using MyDbtDiaryCard.Services.DataService;
using MyDbtDiaryCard.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyDbtDiaryCard.ViewModels
{
    internal class DayEntryViewModel : BaseViewModel
    {
        DayEntry day;
        public DayEntry Day
        {
            get => day; 
            set
            {
                SetProperty(ref day, value);
            } 
        }

        private bool isDayEntryExists;
        public bool IsDayEntryExists
        {
            get { return isDayEntryExists; }
            set 
            {
                SetProperty(ref isDayEntryExists, value);
            }
        }


        public ICommand AddEntryCommand { get; set; }
        public ICommand DeleteEntryCommand { get; set; }
        public ICommand DropDbCommand { get; set; }

        public DayEntryViewModel(INavigationService navigation) : base(navigation)
        {
            AddEntryCommand = new ActionCommand(async () => await ShowAddDayEntryPage());
            DropDbCommand = new ActionCommand(async () => await DropDb());

            ChangeDay();
        }

        private DateTime pickedDate = DateTime.Today;
        public DateTime PickedDate
        {
            get { return pickedDate; }
            set 
            {
                if (value > DateTime.Today)
                    return;

                SetProperty(ref pickedDate, value);
                ChangeDay();
            }
        }

        private async Task ShowAddDayEntryPage()
        {
            await NavigationService.NavigateAsync("AddDayEntryPage", pickedDate);
            ChangeDay();
        }

        private async Task DropDb()
        {
            SQLiteDataService.DataService.DropAllDataAsync();
        }

        private async void ChangeDay()
        {
            Day = await SQLiteDataService.DataService.GetDayEntryForDateAsync(PickedDate);

            IsDayEntryExists = Day != null;
        }
    }
}
