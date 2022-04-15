using MyDbtDiaryCard.Model;
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
        private static readonly IDataService _dataService;
        readonly private DateTime _date;
        private DayEntry day;

        public string StringDate => _date.ToString("d");
        public List<string> GeneralScale { get; set; }
        public List<string> UrgesScale { get; set; }

        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        static AddDayEntryViewModel()
        {
            _dataService = SQLiteDataService.GetDataService();
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
            day = await _dataService.GetDayEntryForDateAsync(_date);

            
            if (day == null)
            {
                IsEntryExistsInDb = false;
                return;
            }

            DayFeelings = day?.DayFeelings;
            DayEmotions = day?.DayEmotions;
        }
        private async Task SaveEntryAsync()
        {
            if(!IsEntryExistsInDb)
                day = new DayEntry(_date);

            day.SetDayEmotions(DayEmotions);
            day.SetDayFeelings(DayFeelings);

            await _dataService.AddDayEntryAsync(day);

            await NavigationService.GoBack();
        }

        private async Task DeleteEntryAsync()
        {
            if (!isEntryExistsInDb)
                return;

            await _dataService.DeleteDayEntryAsync(day);

            NavigationService.GoBack();
        }

        //cancel command
    }
}
