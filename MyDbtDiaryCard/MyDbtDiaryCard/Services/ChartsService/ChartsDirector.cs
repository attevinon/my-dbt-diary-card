using MyDbtDiaryCard.Model;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyDbtDiaryCard.Services.ChartsService
{
    internal class ChartsDirector
    {
        private readonly EntriesStats _stats;
        public ChartsDirector(EntriesStats entriesStats)
        {
            _stats = entriesStats;
        }

        public PlotModel BuildFeelingsChart(ILineChartBuilder chartBuilder)
        {
            chartBuilder.Reset();
            chartBuilder.SetPlotModel(_stats.Dates);
            chartBuilder.SetLeftAxis(false);
            chartBuilder.SetSeries(_stats.FeelingsStats.emMisery, OxyColors.Gold, "Emotional misery");
            chartBuilder.SetSeries(_stats.FeelingsStats.phMisery, OxyColors.Blue, "Physical misery");
            chartBuilder.SetSeries(_stats.FeelingsStats.excitation, OxyColors.OrangeRed, "Excitation");

            return chartBuilder.GetChart();
        }

        public PlotModel BuildEmotionsChart(ILineChartBuilder chartBuilder)
        {

            chartBuilder.Reset();
            chartBuilder.SetPlotModel(_stats.Dates);
            chartBuilder.SetLeftAxis(false);
            chartBuilder.SetSeries(_stats.EmotionsStats.anger, OxyColors.Red, "Anger");
            chartBuilder.SetSeries(_stats.EmotionsStats.sadness, OxyColors.Blue, "Sadness");
            chartBuilder.SetSeries(_stats.EmotionsStats.fear, OxyColors.Violet, "Fear");
            chartBuilder.SetSeries(_stats.EmotionsStats.shame, OxyColors.Pink, "Shame");
            chartBuilder.SetSeries(_stats.EmotionsStats.pride, OxyColors.Green, "Pride");
            chartBuilder.SetSeries(_stats.EmotionsStats.joy, OxyColors.Gold, "Joy");

            return chartBuilder.GetChart();
        }

    }
}
