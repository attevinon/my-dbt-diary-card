using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace MyDbtDiaryCard.Model.EntryItems
{
    [Table("DbtSkillsUsed")]
    internal class DbtSkillUsed
    {
        [ForeignKey(typeof(DayEntry))]
        public DateTime Date { get; set; }

        [ForeignKey(typeof(DbtSkills))]
        public int SkillId { get; set; }
        public string SkillName { get; set; }
        public int Usefulness { get; set; } //change to int[] in future
        public string Additional { get; set; }
    }
}
