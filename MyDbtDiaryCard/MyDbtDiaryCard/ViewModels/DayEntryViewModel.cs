using MyDbtDiaryCard.Model;
using MyDbtDiaryCard.Model.EntryItems;
using MyDbtDiaryCard.Services.Commands;
using MyDbtDiaryCard.Services.DataService;
using MyDbtDiaryCard.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyDbtDiaryCard.ViewModels
{
    internal class DayEntryViewModel : BaseViewModel
    {
        public event EventHandler WrongDataPicked;

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

        private IEnumerable<DbtSkillUsed> skillsUsed;
        public IEnumerable<DbtSkillUsed> SkillsUsed
        {
            get { return skillsUsed; }
            set { SetProperty(ref skillsUsed, value); }
        }

        public ICommand AddEntryCommand { get; }
        public ICommand GetNextDayCommand { get; }
        public ICommand GetPreviousDayCommand { get; }
        public ICommand ShowUsefulnessHelpCommand { get; }

        public DayEntryViewModel(INavigationService navigation) : base(navigation)
        {
            AddEntryCommand = new ActionCommand(async () => await ShowAddDayEntryPage());

            GetNextDayCommand = new DateCommand(() => PickedDate = PickedDate.AddDays(1));
            GetPreviousDayCommand = new ActionCommand(GetPreviousDay);

            ShowUsefulnessHelpCommand = new ActionCommand(
                async () => await NavigationService.PushPopupAsync("UsefulnessHelpPage"));

            DataService.GetDataManager().DayEntryData.EntryDataUpdated += DataUpdated;
        }

        private DateTime pickedDate = DateTime.Today;
        public DateTime PickedDate
        {
            get { return pickedDate; }
            set 
            {
                if (value > DateTime.Today)
                {
                    WrongDataPicked?.Invoke(this, null);
                    return;
                }


                SetProperty(ref pickedDate, value);
                FindDay();
            }
        }

        private void DataUpdated(object sender, EventArgs e)
        {
            FindDay();
        }

        private async void FindDay()
        {
            try
            {
                IsLoading = true;

                Day = await DataService.GetDataManager().DayEntryData?.GetDayEntryForDateAsync(PickedDate);
                SkillsUsed = Day?.DaysDbtSkills ?? null;

                IsDayEntryExists = Day != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void GetPreviousDay()
        {
            if (PickedDate > DateTime.MinValue)
                PickedDate = PickedDate.AddDays(-1);
        }

        private async Task ShowAddDayEntryPage()
        {
            await NavigationService.NavigateAsync("AddDayEntryPage", pickedDate);
        }
    }
}
