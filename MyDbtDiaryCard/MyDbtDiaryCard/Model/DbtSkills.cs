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
        [PrimaryKey, Unique, AutoIncrement]
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

    internal enum DbtSkillsEnum
    {

        //EmotionRegulation
        Abc = 201,
        Please = 202,
        OppositeToEmotionAction = 203,
        LabelingEmotions = 204,

        //InterpersonalEffectiveness
        Dear = 301,
        Man = 302,
        Give = 303,
        Fast = 304,
        Validation = 305,

        //Tools
        ChainAnalysis = 401
    }
}
