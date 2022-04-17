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

            FindDay();
        }

        private DateTime pickedDate = DateTime.Today;
        public DateTime PickedDate
        {
            get { return pickedDate; }
            set 
            {
                if (value > DateTime.Today)
                    return;
                //notigication "you can't be from future" and nothing changes

                SetProperty(ref pickedDate, value);
                FindDay();
            }
        }

        private async Task ShowAddDayEntryPage()
        {
            await NavigationService.NavigateAsync("AddDayEntryPage", pickedDate);
        }

        private async Task DropDb()
        {

        }

        private async void FindDay()
        {
            Day = await DataService.GetDataManager().DayEntryData.GetDayEntryForDateAsync(PickedDate);

            IsDayEntryExists = Day != null;
        }

        //refresh page somehow after adding new entry 
    }
}
