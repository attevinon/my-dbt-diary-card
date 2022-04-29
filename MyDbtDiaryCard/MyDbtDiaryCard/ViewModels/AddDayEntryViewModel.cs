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
using System.Text;
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

        public string StringDate => _date.ToString("d");
        public List<string> GeneralScale { get; set; }
        public List<string> UrgesScale { get; set; }
        public List<string> UsefulnessScale { get; set; }

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

            GeneralScale = new List<string> { "-", "0", "1", "2", "3", "4", "5"};
            UrgesScale = new List<string> { "-", "0", "1", "2", "3", "4", "5", "X" };
            UsefulnessScale = new List<string> { "-", "0", "1", "2", "3", "4", "5", "6", "7" };

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
            day = await _dayEntryData.GetDayEntryForDateAsync(_date);

            
            if (day == null)
            {
                IsEntryExistsInDb = false;
                day = new DayEntry(_date);
            }
            else
            {
                IsEntryExistsInDb = true;
            }

            DayFeelings = day?.DayFeelings;
            DayEmotions = day?.DayEmotions;
            DayUrges = day?.DayUrges;
        }
        private async Task SaveEntryAsync()
        {
            day.SetDayEmotions(DayEmotions);
            day.SetDayFeelings(DayFeelings);
            day.DayUrges = DayUrges;
            day.DaysDbtSkills = new List<DbtSkillUsed>(DaysDbtSkillsUsed);

            await _dayEntryData.AddDayEntryAsync(day);

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
            List<string> dbtSkillsNames;

            if(DaysDbtSkillsUsed == null)
            {
                dbtSkillsNames = new List<string>(0);
            }
            else
            {
                dbtSkillsNames = new List<string>();

                foreach (var skill in DaysDbtSkillsUsed)
                {
                    if(!dbtSkillsNames.Contains(skill.Skill))
                        dbtSkillsNames.Add(skill.Skill);
                }
            }

            await NavigationService.NavigateAsync("DbtSkillsPage", dbtSkillsNames,
                new Action<object, UsedDbtSkillesChangedEvent>(DaysDbtSkillsChanged));

        }

        private void DaysDbtSkillsChanged(object sender, UsedDbtSkillesChangedEvent args)
        {
            foreach (var skill in DaysDbtSkillsUsed)
            {
                if (args.UsedSkillsNames.Contains(skill.Skill))
                    continue;

                if (!args.UsedSkillsNames.Contains(skill.Skill))
                {
                    DaysDbtSkillsUsed.Remove(skill);
                    continue;
                }
            }

            if (DaysDbtSkillsUsed.Count == args.UsedSkillsNames.Count)
                return;

            foreach (var skillName in args.UsedSkillsNames)
            {
                if(DaysDbtSkillsUsed.Where(s => s.Skill == skillName).FirstOrDefault() == null)
                {
                    DaysDbtSkillsUsed.Add(
                        new DbtSkillUsed { Skill = skillName, Date = _date });
                    continue;
                }
            }

            if (DaysDbtSkillsUsed.Count != args.UsedSkillsNames.Count)
                throw new Exception("dbt skills suck");
        }

        //cancel command
    }
}
