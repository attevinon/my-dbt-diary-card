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

            var result = await connection.CreateTableAsync<DbtSkills>();

            try
            {
                if (result == CreateTableResult.Migrated)
                    return;

                await connection.InsertAllAsync(InitMindfulnessSkills());
                await connection.InsertAllAsync(InitDistressToleranceSkills());
                await connection.InsertAllAsync(InitEmotionRegulationSkills());
                await connection.InsertAllAsync(InitInterpersonalEffectivenessSkills());

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

        public async Task<DbtSkills> GetDbtSkillForId(int id)
        {
            var skill = await FindByConditionAsync<DbtSkills>(s => s.Id == id);
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
                new DbtSkills(){ Id = 101, Name = DbtSkillsResources.M_WiseMind},
                new DbtSkills(){ Id = 102, Name = DbtSkillsResources.M_MiddlePath},
                new DbtSkills(){ Id = 103, Name = DbtSkillsResources.M_Observe},
                new DbtSkills(){ Id = 104, Name = DbtSkillsResources.M_Describe,},
                new DbtSkills(){ Id = 105, Name = DbtSkillsResources.M_Participate},
                new DbtSkills(){ Id = 106, Name = DbtSkillsResources.M_NonJudgmentally},
                new DbtSkills(){ Id = 107, Name = DbtSkillsResources.M_OneMindfully},
                new DbtSkills(){ Id = 108, Name = DbtSkillsResources.M_Effectively},
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
                new DbtSkills() { Id = 301, Name = DbtSkillsResources.E_CheckTheFacts},
                new DbtSkills() { Id = 302, Name = DbtSkillsResources.E_LabelingEmotions},
                new DbtSkills() { Id = 303, Name = DbtSkillsResources.E_Opposite_Action},
                new DbtSkills() { Id = 304, Name = DbtSkillsResources.E_Please},
                new DbtSkills() { Id = 305, Name = DbtSkillsResources.E_ProblemSloving},
                new DbtSkills() { Id = 306, Name = DbtSkillsResources.E_AccumulatePositiveEmotions},
                new DbtSkills() { Id = 307, Name = DbtSkillsResources.E_BuildMastery},
                new DbtSkills() { Id = 308, Name = DbtSkillsResources.E_CopeAhead},
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
                new DbtSkills(){ Id = 201, Name = DbtSkillsResources.D_STOP},
                new DbtSkills(){ Id = 202, Name = DbtSkillsResources.D_TIPP},
                new DbtSkills(){ Id = 203, Name = DbtSkillsResources.D_ProsAndCons},
                new DbtSkills(){ Id = 204, Name = DbtSkillsResources.D_Accept},
                new DbtSkills(){ Id = 205, Name = DbtSkillsResources.D_SelfSoothe},
                new DbtSkills(){ Id = 206, Name = DbtSkillsResources.D_ImproveTheMoment},
                new DbtSkills(){ Id = 207, Name = DbtSkillsResources.D_RadicalAcceptance},
                new DbtSkills(){ Id = 208, Name = DbtSkillsResources.D_WillingnessAndHalfSmile},

            };

            foreach (var skill in skills)
            {
                skill.Module = DbtModules.DistressTolerance;
            }

            return skills;
        }

        private IEnumerable<DbtSkills> InitInterpersonalEffectivenessSkills()
        {
            IEnumerable<DbtSkills> skills = new List<DbtSkills>()
            {
                new DbtSkills() { Id = 401, Name = DbtSkillsResources.I_Dear},
                new DbtSkills() { Id = 402, Name = DbtSkillsResources.I_Fast},
                new DbtSkills() { Id = 403, Name = DbtSkillsResources.I_Give},
                new DbtSkills() { Id = 404, Name = DbtSkillsResources.I_Man},
                new DbtSkills() { Id = 405, Name = DbtSkillsResources.I_Validation},
            };

            foreach (var skill in skills)
            {
                skill.Module = DbtModules.InterpersonalEffectiveness;
            }

            return skills;
        }
    }
}
