using MyDbtDiaryCard.Model;
using MyDbtDiaryCard.Model.Abstractions;
using MyDbtDiaryCard.Model.EntryItems;
using MyDbtDiaryCard.Services.Commands;
using MyDbtDiaryCard.Services.DataService;
using MyDbtDiaryCard.Services.Navigation;
using MyDbtDiaryCard.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;

namespace MyDbtDiaryCard.ViewModels
{
    internal class AddDayEntryViewModel : BaseViewModel
    {
        private static readonly IDayEntryRepository _dayEntryData;
        private readonly DateTime _date;

        private DayEntry day;

        public string StringDate => $"{_date:ddd}, {_date:D}";

        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand ChooseSkillsCommand { get; set; }

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
            CancelCommand = new ActionCommand(async () => await NavigationService.GoBackAsync());
            ChooseSkillsCommand = new ActionCommand(async () => await ShowSkillsList()); 

            FindDay();
        }

        //if true, here is ability to delete entry
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

        private Urges urges;
        public Urges DayUrges
        {
            get
            {
                if (urges == null)
                    urges = new Urges(_date);

                return urges;
            }
            set { SetProperty(ref urges, value); }
        }

        private ObservableCollection<DbtSkillUsed> dbtSkillsUsed;
        public ObservableCollection<DbtSkillUsed> DaysDbtSkillsUsed
        {
            get 
            {
                if (dbtSkillsUsed == null)
                    dbtSkillsUsed = new ObservableCollection<DbtSkillUsed>();

                return dbtSkillsUsed;
            }
            set { SetProperty(ref dbtSkillsUsed, value); }
        }


        private async void FindDay()
        {
            IsLoading = true;
            day = await _dayEntryData.GetDayEntryForDateAsync(_date);

            
            if (day == null)
            {
                IsEntryExistsInDb = false;
                day = new DayEntry(_date);
            }
            else
            {
                DayFeelings = day?.DayFeelings;
                DayEmotions = day?.DayEmotions;
                DayUrges = day?.DayUrges;
                DaysDbtSkillsUsed = new ObservableCollection<DbtSkillUsed>(day?.DaysDbtSkills);
                IsEntryExistsInDb = true;
            }

            IsLoading = false;
        }
        private async Task SaveEntryAsync()
        {
            IsLoading = true;
            day.DayFeelings = DayFeelings;
            day.DayEmotions = DayEmotions;
            day.DayUrges = DayUrges;
            day.DaysDbtSkills = new List<DbtSkillUsed>(DaysDbtSkillsUsed);

            await _dayEntryData.AddDayEntryAsync(day);

            IsLoading = false;

            await NavigationService.GoBackAsync();
        }

        private async Task DeleteEntryAsync()
        {
            if (!isEntryExistsInDb)
                return;

            await _dayEntryData.DeleteDayEntryAsync(day);

            await NavigationService.GoBackAsync();
        }

        private async Task ShowSkillsList()
        {
            List<int> dbtSkillsId;

            if(DaysDbtSkillsUsed == null)
            {
                dbtSkillsId = new List<int>(0);
            }
            else
            {
                dbtSkillsId = new List<int>();

                foreach (var skill in DaysDbtSkillsUsed)
                {
                    if(!dbtSkillsId.Contains(skill.SkillId))
                        dbtSkillsId.Add(skill.SkillId);
                }
            }

            await NavigationService.NavigateAsync("DbtSkillsPage", dbtSkillsId,
                new Action<object, UsedDbtSkillsChangedEvent>(DaysDbtSkillsChanged));

        }

        private async void DaysDbtSkillsChanged(object sender, UsedDbtSkillsChangedEvent args)
        {
            IsLoading = true;
            var oldSkillsList = DaysDbtSkillsUsed.ToList();
            foreach (var skill in oldSkillsList)
            {
                if (args.UsedSkillsId.Contains(skill.SkillId))
                    continue;

                if (!args.UsedSkillsId.Contains(skill.SkillId))
                {
                    DaysDbtSkillsUsed.Remove(skill);
                    continue;
                }
            }

            if (DaysDbtSkillsUsed.Count == args.UsedSkillsId.Count)
            {
                IsLoading = false;
                return;
            }

            foreach (var id in args.UsedSkillsId)
            {
                if(DaysDbtSkillsUsed.Where(s => s.SkillId == id).FirstOrDefault() == null)
                {
                    //WRONG!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    var skill = await DataService.GetDataManager().DbtSkillsData.GetDbtSkillForId(id);

                    DaysDbtSkillsUsed.Add(
                        new DbtSkillUsed { SkillId = id, SkillName = skill.Name,  Date = _date });
                    continue;
                }
            }

            if (DaysDbtSkillsUsed.Count != args.UsedSkillsId.Count)
                throw new Exception("dbt skills suck");

            IsLoading = false;
        }
    }
}
