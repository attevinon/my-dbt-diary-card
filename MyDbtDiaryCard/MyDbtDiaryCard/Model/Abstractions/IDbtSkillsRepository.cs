using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyDbtDiaryCard.Model.Abstractions
{
    internal interface IDbtSkillsRepository
    {
        event EventHandler DbtSkillsLoaded;
        bool HasBeenInitialized { get; }
        Task Init();
        Task<DbtSkills> GetDbtSkillForName(string name);
        Task<IEnumerable<DbtSkills>> GetDbtSkillsForModule(DbtModules module);
    }
}
