using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyDbtDiaryCard.Model.EntryItems
{
    [Table("feelings")]
    internal class Feelings
    {
        [PrimaryKey, ForeignKey(typeof(DayEntry))]
        public DateTime Date { get; set; }
        public int EmotionMisery { get; set; }
        public int PhysicalMisery { get; set; }
        public int Excitation { get; set; }
        public string Additional { get; set; }

        public Feelings(DateTime date)
        {
            Date = date;
        }

        public Feelings() { }
    }
}
