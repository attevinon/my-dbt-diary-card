using OxyPlot;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyDbtDiaryCard.Services.ChartsService
{
    internal interface IChartBuilder
    {
        void Reset();
        void SetPlotModel(DateTime[] dates);
        void SetLegend(string title);
        PlotModel GetChart();
    }
}
