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
        public int Anger { get; set; }
        public int Sadness { get; set; }
        public int Fear { get; set; }
        public int Shame { get; set; }
        public int Joy { get; set; }
        public int Love { get; set; }

        public Emotions(DateTime date)
        {
            Date = date;
        }

        public Emotions() { }
    }
}
