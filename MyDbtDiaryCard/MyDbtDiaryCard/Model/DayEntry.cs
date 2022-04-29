using MyDbtDiaryCard.Model.EntryItems;
using MyDbtDiaryCard.Services.DataService;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyDbtDiaryCard.Model
{
    [Table("Entries")]
    internal class DayEntry
    {

        [PrimaryKey, Unique, NotNull]
        public DateTime Date { get; set; }

        [OneToOne]
        public Feelings DayFeelings { get; set; }

        [OneToOne]
        public Emotions DayEmotions { get; set; }

        [OneToOne]
        public Urges DayUrges { get; set; }

        [OneToMany]
        public List<DbtSkillUsed> DaysDbtSkills { get; set; }

        /* [OneToMany]
         public List<Meds> DayMeds { get; set; }*/

        //public Lazy<Treatment> DayTreatment { get; set; }

        public DayEntry(DateTime date)
        {
            Date = date;
        }
        public DayEntry() { }

        public void SetDayFeelings(Feelings feelings) //v presenter peredaetsya
        {
            //maybe add if() => ...; (smth like checking)
            DayFeelings = feelings;
        }

        public void SetDayEmotions(Emotions em)
        {
            DayEmotions = em;
        }
    }
}
