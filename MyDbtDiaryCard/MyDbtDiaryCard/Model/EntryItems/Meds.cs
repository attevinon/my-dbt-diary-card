using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyDbtDiaryCard.Model.EntryItems
{
    [Table("meds")]
    internal class Meds
    {
        [PrimaryKey, Unique, ForeignKey(typeof(DayEntry))]
        public string MedName { get; set; }
        public bool IsPrescribed { get; set; } = false;
        public bool TakenAsPrescribed { get; set; }
    }
}
