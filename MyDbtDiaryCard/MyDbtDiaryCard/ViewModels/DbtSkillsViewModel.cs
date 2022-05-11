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
        private delegate void UsedDbtSkillsChanged(object sender, UsedDbtSkillsChangedEvent args);

        private event UsedDbtSkillsChanged SelectedDbtSkillsChanged;
        private readonly IDbtSkillsRepository _dbtSkillsData;

        public ActionCommand ConfirmChangesCommand { get; set; }

        public DbtSkillsViewModel(INavigationService navigation,
            List<int> selectedDbtSkillsId, Action<object, UsedDbtSkillsChangedEvent> action) : base(navigation)
        {
            _dbtSkillsData = DataService.GetDataManager().DbtSkillsData;
            SelectedDbtSkillsId = selectedDbtSkillsId;

            Initialize();

            SelectedDbtSkillsChanged += new UsedDbtSkillsChanged(action);
            ConfirmChangesCommand = new ActionCommand(async () => await ConfirmChanges());

            SelectedDbtSkillsList = new ObservableCollection<object>();
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

                await FindSkills();
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
        private async Task FindSkills()
        {
            try
            {
                if (SelectedDbtSkillsId == null || SelectedDbtSkillsId.Count == 0)
                    return;

                foreach (var id in SelectedDbtSkillsId)
                {
                    var skill = await _dbtSkillsData.GetDbtSkillForId(id);
                    foreach(List<DbtSkills> list in DbtSkillsList)
                    {
                        if (list[0].Module != skill.Module)
                            continue;

                        SelectedDbtSkillsList.Add(list.Where(s => s.Id == id).FirstOrDefault());
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public List<int> SelectedDbtSkillsId { get; set; }

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
            try
            {
                await ChangeSelectedSkills();
                SelectedDbtSkillsChanged(this, new UsedDbtSkillsChangedEvent(SelectedDbtSkillsId));
                await NavigationService.GoBackAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task ChangeSelectedSkills()
        {
            if(SelectedDbtSkillsList?.Count == 0 || SelectedDbtSkillsList == null)
            {
                SelectedDbtSkillsId?.Clear();
                return;
            }

            //typecasting
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
                if (!SelectedDbtSkillsId.Contains(skill.Id))
                {
                    SelectedDbtSkillsId.Add(skill.Id);
                }
            }

            if (SelectedDbtSkillsId.Count == selectedDbtSkills.Count)
                return;

            var oldLesectedList = SelectedDbtSkillsId.ToList();
            foreach (var id in oldLesectedList)
            {

                if(selectedDbtSkills.Where(s => s.Id == id).FirstOrDefault() == null)
                {
                    SelectedDbtSkillsId.Remove(id);
                }
            }

            if (SelectedDbtSkillsId.Count != SelectedDbtSkillsList.Count)
                throw new Exception("DbtSkills don't work correct but in skills page");
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
