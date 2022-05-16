using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace MyDbtDiaryCard.Model
{
    [Table("DbtSkills")]
    public class DbtSkills
    {
        [PrimaryKey, Unique]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DbtModules Module { get; set; }

        //icon
        //isfavorite

    }

    public enum DbtModules
    {
        Mindfulness,
        DistressTolerance,
        EmotionRegulation,
        InterpersonalEffectiveness,
        Tools
    }

}
