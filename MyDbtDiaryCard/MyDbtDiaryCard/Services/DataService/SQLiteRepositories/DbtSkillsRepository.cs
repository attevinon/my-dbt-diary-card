using MyDbtDiaryCard.Model;
using MyDbtDiaryCard.Model.Abstractions;
using MyDbtDiaryCard.Resx;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyDbtDiaryCard.Services.DataService.SQLiteRepositories
{
    internal class DbtSkillsRepository : BaseRepository, IDbtSkillsRepository
    {
        public event EventHandler DbtSkillsLoaded;
        public DbtSkillsRepository(SQLiteAsyncConnection connection) : base(connection)
        {
            hasBeenInitialized = false;
        }

        public async Task Init()
        {
            if (hasBeenInitialized == true)
                return;

            await connection.CreateTableAsync<DbtSkills>();

            try
            {
                /*var initMinfulness = connection.InsertAllAsync(InitMindfulnessSkills());
                var initDistressTolerance = connection.InsertAllAsync(InitDistressToleranceSkills());
                var initEmotionRegulation = connection.InsertAllAsync(InitEmotionRegulationSkills());

                await Task.WhenAll(initMinfulness, initDistressTolerance, initEmotionRegulation);*/

                await connection.InsertAllAsync(InitMindfulnessSkills());
                await connection.InsertAllAsync(InitDistressToleranceSkills());
                await connection.InsertAllAsync(InitEmotionRegulationSkills());

                HasBeenInitialized = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                HasBeenInitialized = false;
            }

        }

        public async Task<DbtSkills> GetDbtSkillForName(string name)
        {
            var skill = await FindByConditionAsync<DbtSkills>(s => s.Name == name);
            return skill;
        }

        public async Task<IEnumerable<DbtSkills>> GetDbtSkillsForModule(DbtModules module)
        {
            try
            {
                var skills = await FindManyByConditionAsync<DbtSkills, string>(s => s.Module == module, s => s.Name);
                return skills;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        private IEnumerable<DbtSkills> InitMindfulnessSkills()
        {
            IEnumerable<DbtSkills> skills = new List<DbtSkills>()
            { 
                new DbtSkills(){ Name = DbtSkillsResources.M_WiseMind},
                new DbtSkills(){ Name = DbtSkillsResources.M_MiddlePath},
                new DbtSkills(){ Name = DbtSkillsResources.M_Observe},
                new DbtSkills(){ Name = DbtSkillsResources.M_Describe,},
                new DbtSkills(){ Name = DbtSkillsResources.M_Participate},
                new DbtSkills(){ Name = DbtSkillsResources.M_NonJudgmentally},
                new DbtSkills(){ Name = DbtSkillsResources.M_OneMindfully},
                new DbtSkills(){ Name = DbtSkillsResources.M_Effectively},
            };

            foreach (var skill in skills)
            {
                skill.Module = DbtModules.Mindfulness;
            }

            return skills;
        }

        private IEnumerable<DbtSkills> InitEmotionRegulationSkills()
        {
            IEnumerable<DbtSkills> skills = new List<DbtSkills>()
            {
                new DbtSkills() { Name = DbtSkillsResources.E_CheckTheFacts},
                new DbtSkills() { Name = DbtSkillsResources.E_LabelingEmotions},
                new DbtSkills() { Name = DbtSkillsResources.E_Opposite_Action},
                new DbtSkills() { Name = DbtSkillsResources.E_Please},
                new DbtSkills() { Name = DbtSkillsResources.E_ProblemSloving}
            };

            foreach (var skill in skills)
            {
                skill.Module = DbtModules.EmotionRegulation;
            }

            return skills;
        }
        private IEnumerable<DbtSkills> InitDistressToleranceSkills()
        {
            IEnumerable<DbtSkills> skills = new List<DbtSkills>()
            {
                new DbtSkills(){ Name = DbtSkillsResources.D_STOP},
                new DbtSkills(){ Name = DbtSkillsResources.D_TIPP},
                new DbtSkills(){ Name = DbtSkillsResources.D_ProsAndCons},
                new DbtSkills(){ Name = DbtSkillsResources.D_Accept},
                new DbtSkills(){ Name = DbtSkillsResources.D_SelfSoothe},
                new DbtSkills(){ Name = DbtSkillsResources.D_ImproveTheMoment},
                new DbtSkills(){ Name = DbtSkillsResources.D_RadicalAcceptance},
                new DbtSkills(){ Name = DbtSkillsResources.D_WillingnessAndHalfSmile},

            };

            foreach (var skill in skills)
            {
                skill.Module = DbtModules.DistressTolerance;
            }

            return skills;
        }

    }
}
