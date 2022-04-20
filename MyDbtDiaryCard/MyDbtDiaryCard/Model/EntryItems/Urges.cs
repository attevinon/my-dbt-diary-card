using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyDbtDiaryCard.Model.EntryItems
{
    [Table("urges")]
    internal class Urges
    {
        [PrimaryKey, ForeignKey(typeof(DayEntry))]
        public DateTime Date { get; set; }
        public int SelfHarm { get; set; } = -1;
        public int Suicide { get; set; } = -1;
        public int Alcohol { get; set; } = -1;
        public int Drugs { get; set; } = -1;

        public Urges(DateTime date)
        {
            Date = date;
        }

        public Urges() { }
    }
}
