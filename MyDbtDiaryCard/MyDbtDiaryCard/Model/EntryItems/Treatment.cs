using System;
using System.Collections.Generic;
using System.Text;

namespace MyDbtDiaryCard.Model.EntryItems
{
    internal struct Treatment
    {
        DateTime Date { get; set; }
        TreatmentType TrType { get; set; }
        int Helpness { get; set; }
        string Notes { get; set; }
    }
}
