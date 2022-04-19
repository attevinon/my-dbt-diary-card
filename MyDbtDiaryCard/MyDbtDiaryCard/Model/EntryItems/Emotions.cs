using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyDbtDiaryCard.Model.EntryItems
{
    [Table("emotions")]
    internal class Emotions
    {
        //converter?
        [PrimaryKey, ForeignKey(typeof(DayEntry))]
        public DateTime Date { get; set; }
        public int Anger { get; set; } = -1;
        public int Sadness { get; set; } = -1;
        public int Fear { get; set; } = -1;
        public int Shame { get; set; } = -1;
        public int Pride { get; set; } = -1;
        public int Joy { get; set; } = -1;

        public Emotions(DateTime date)
        {
            Date = date;
        }

        public Emotions() { }
    }
}
