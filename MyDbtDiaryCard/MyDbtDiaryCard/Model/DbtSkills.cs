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
    public enum DbtSkillsEnum
    {
        //Mindfulness
        WiseMind = 001,
        MiddlePath = 002,
        Observe = 003,
        Describe = 004,
        Participate =005,
        OneMindfully = 006,
        NonJudgmentally = 007,
        Effectively = 008,

        //DistressTolerance
        TIPP = 101,
        STOP = 102,
        ProsAndCons = 103,
        ImproveTheMoment = 104,
        Accept = 105,
        SelfSoothe = 106,
        RadicalAcceptance = 107,
        WillingnessAndHalfSmile = 108,

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
