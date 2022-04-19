using OxyPlot;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyDbtDiaryCard.Services.ChartsService
{
    internal interface ILineChartBuilder : IChartBuilder
    {
        void SetSeries(int[] dataArray, OxyColor color, string title = null);
        void SetLeftAxis(bool isBiggerScale);
    }
}
