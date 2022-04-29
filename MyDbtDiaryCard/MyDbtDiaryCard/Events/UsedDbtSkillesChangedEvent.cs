using System;
using System.Collections.Generic;
using System.Text;

namespace MyDbtDiaryCard.Events
{
    public class UsedDbtSkillesChangedEvent : EventArgs
    {
        public UsedDbtSkillesChangedEvent(List<string> usedSkillsNames)
        {
            UsedSkillsNames = usedSkillsNames;
        }
        public List<string> UsedSkillsNames { get; set; }
    }
}
