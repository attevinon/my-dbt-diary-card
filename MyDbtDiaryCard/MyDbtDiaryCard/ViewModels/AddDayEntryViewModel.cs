using MyDbtDiaryCard.Model;
using MyDbtDiaryCard.Model.Abstractions;
using MyDbtDiaryCard.Model.EntryItems;
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
    internal class AddDayEntryViewModel : BaseViewModel
    {
        private static readonly IDayEntryRepository _dayEntryData;

        private readonly DateTime _date;

        private DayEntry day;

        public string StringDate => _date.ToString("d");
        public List<string> GeneralScale { get; set; }
        public List<string> UrgesScale { get; set; }

        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        static AddDayEntryViewModel()
        {
            _dayEntryData = DataService.GetDataManager().DayEntryData;
        }

        public AddDayEntryViewModel(INavigationService navigation, DateTime date) : base(navigation)
        {
            if (date > DateTime.Today)
                throw new Exception();

            _date = date;

            SaveCommand = new ActionCommand(async () => await SaveEntryAsync());
            DeleteCommand = new ActionCommand(async () => await DeleteEntryAsync());
            CancelCommand = new ActionCommand(async () => await NavigationService.GoBack());

            GeneralScale = new List<string> { "-", "0", "1", "2", "3", "4", "5"};
            UrgesScale = new List<string> { "-", "0", "1", "2", "3", "4", "5", "X"};

            FindDay();
        }

        private bool isEntryExistsInDb;
        public bool IsEntryExistsInDb
        {
            get { return isEntryExistsInDb; }
            set { SetProperty(ref isEntryExistsInDb, value); }
        }

        private Feelings feelings;
        public Feelings DayFeelings
        {
            get 
            {
                if (feelings == null)
                    feelings = new Feelings(_date);

                return feelings;
            }
            set
            {
                SetProperty(ref feelings, value);
            }
        }

        private Emotions emotions;
        public Emotions DayEmotions
        {
            get 
            {
                if(emotions == null)
                    emotions = new Emotions(_date);

                return emotions; 
            }
            set
            {
                SetProperty(ref emotions, value); 
            }
        }


        private async void FindDay()
        {
            day = await _dayEntryData.GetDayEntryForDateAsync(_date);

            
            if (day == null)
            {
                IsEntryExistsInDb = false;
                day = new DayEntry(_date);
            }

            IsEntryExistsInDb = true;
            DayFeelings = day?.DayFeelings;
            DayEmotions = day?.DayEmotions;
        }
        private async Task SaveEntryAsync()
        {
            day.SetDayEmotions(DayEmotions);
            day.SetDayFeelings(DayFeelings);

            await _dayEntryData.AddDayEntryAsync(day);

            await NavigationService.GoBack();
        }

        private async Task DeleteEntryAsync()
        {
            if (!isEntryExistsInDb)
                return;

            await _dayEntryData.DeleteDayEntryAsync(day);

            await NavigationService.GoBack();
        }

        //cancel command
    }
}
