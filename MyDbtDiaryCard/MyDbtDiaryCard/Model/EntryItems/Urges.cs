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
        public int SelfHarm { get; set; }
        public int Suicide { get; set; }
        public int Alcohol { get; set; }
        public int Drugs { get; set; }

        public Urges(DateTime date)
        {
            Date = date;
        }

        public Urges() { }
    }
}
