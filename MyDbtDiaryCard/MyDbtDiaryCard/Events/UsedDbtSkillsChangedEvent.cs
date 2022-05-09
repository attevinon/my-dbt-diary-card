using System;
using System.Collections.Generic;
using System.Text;

namespace MyDbtDiaryCard.Events
{
    public class UsedDbtSkillsChangedEvent : EventArgs
    {
        public UsedDbtSkillsChangedEvent(List<int> usedSkillsId)
        {
            UsedSkillsId = usedSkillsId;
        }
        public List<int> UsedSkillsId { get; set; }
    }
}
