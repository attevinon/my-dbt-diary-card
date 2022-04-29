using MyDbtDiaryCard.Services.Navigation;
using MyDbtDiaryCard.Model;
using MyDbtDiaryCard.Model.Abstractions;
using MyDbtDiaryCard.Events;
using MyDbtDiaryCard.Services.Commands;
using MyDbtDiaryCard.Services.DataService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using MyDbtDiaryCard.Resx;
using System.Threading;

namespace MyDbtDiaryCard.ViewModels
{
    internal class DbtSkillsViewModel : BaseViewModel
    {
        private delegate void UsedDbtSkillsChanged(object sender, UsedDbtSkillesChangedEvent args);

        private event UsedDbtSkillsChanged selectedDbtSkillsChanged;
        private readonly IDbtSkillsRepository _dbtSkillsData;

        public ActionCommand ConfirmChangesCommand { get; set; }

        public DbtSkillsViewModel(INavigationService navigation,
            List<string> selectedDbtSkillsNames, Action<object, UsedDbtSkillesChangedEvent> action) : base(navigation)
        {
            _dbtSkillsData = DataService.GetDataManager().DbtSkillsData;

            Initialize();

            selectedDbtSkillsChanged += new UsedDbtSkillsChanged(action);
            ConfirmChangesCommand = new ActionCommand(async () => await ConfirmChanges());

            SelectedDbtSkillsList = new ObservableCollection<object>();

            SelectedDbtSkillsNames = selectedDbtSkillsNames;
        }
        private async Task Initialize()
        {
            IsLoading = true;

            DbtSkillsList = new ObservableCollection<DbtSkillsModules>();

            try 
            {
                DbtSkillsList.Add(new DbtSkillsModules(
                DbtSkillsResources.Module_Mindfulness, new List<DbtSkills>(
                 await _dbtSkillsData.GetDbtSkillsForModule(DbtModules.Mindfulness))));

                DbtSkillsList.Add(new DbtSkillsModules(
                DbtSkillsResources.Module_DistressTolerance, new List<DbtSkills>(
                        await _dbtSkillsData.GetDbtSkillsForModule(DbtModules.DistressTolerance))));

                DbtSkillsList.Add(new DbtSkillsModules(
                    DbtSkillsResources.Module_EmotionRegulation, new List<DbtSkills>(
                        await _dbtSkillsData.GetDbtSkillsForModule(DbtModules.EmotionRegulation))));

                FindSkills();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
            finally
            {
                IsLoading = false;
            }
        }
        private async void FindSkills()
        {
            if (SelectedDbtSkillsNames == null || SelectedDbtSkillsNames.Count == 0)
                return;

            try
            {
                foreach (var name in SelectedDbtSkillsNames)
                {
                    SelectedDbtSkillsList.Add(await _dbtSkillsData.GetDbtSkillForName(name));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private bool isLoading;
        public bool IsLoading
        {
            get { return isLoading; }
            set { SetProperty(ref isLoading, value); }
        }


        public List<string> SelectedDbtSkillsNames { get; set; }

        private ObservableCollection<object> selectedDbtSkills;
        public ObservableCollection<object> SelectedDbtSkillsList
        {
            get { return selectedDbtSkills; }
            set 
            {
                SetProperty(ref selectedDbtSkills, value);
            }
        }


        private ObservableCollection<DbtSkillsModules> dbtSkills;
        public ObservableCollection<DbtSkillsModules> DbtSkillsList
        {
            get { return dbtSkills; }
            set { SetProperty(ref dbtSkills, value); }
        }

        private async Task ConfirmChanges()
        {
            await ChangeSelectedSkills();
            selectedDbtSkillsChanged(this, new UsedDbtSkillesChangedEvent(SelectedDbtSkillsNames));
            await NavigationService.GoBackAsync();
        }

        private async Task ChangeSelectedSkills()
        {
            if(SelectedDbtSkillsList?.Count == 0 || SelectedDbtSkillsNames == null)
            {
                SelectedDbtSkillsNames?.Clear();
                return;
            }

            //какая-то херь с преобразованием
            if (!(SelectedDbtSkillsList is IList<DbtSkills> selectedDbtSkills))
            {
                selectedDbtSkills = new List<DbtSkills>();
                foreach (var obj in SelectedDbtSkillsList)
                {
                    selectedDbtSkills.Add(obj as DbtSkills);
                }
            }

            foreach (var skill in selectedDbtSkills)
            {
                if (!SelectedDbtSkillsNames.Contains(skill.Name))
                {
                    SelectedDbtSkillsNames.Add(skill.Name);
                }
            }

            if (SelectedDbtSkillsNames.Count == selectedDbtSkills.Count)
                return;

            foreach (var skillName in SelectedDbtSkillsNames)
            {

                if(selectedDbtSkills.Where(s => s.Name == skillName).FirstOrDefault() == null)
                {
                    SelectedDbtSkillsNames.Remove(skillName);
                }
            }

            if (SelectedDbtSkillsNames.Count != SelectedDbtSkillsList.Count)
                throw new Exception("dbtSkills SUCKSSSSSSS but in dbtSkills page");
        }
    }

    public class DbtSkillsModules : List<DbtSkills>
    {
        public string ModuleName { get; private set; }
        public DbtSkillsModules(string moduleName, List<DbtSkills> dbtSkills) : base(dbtSkills)
        {
            ModuleName = moduleName;
        }
    }
}
