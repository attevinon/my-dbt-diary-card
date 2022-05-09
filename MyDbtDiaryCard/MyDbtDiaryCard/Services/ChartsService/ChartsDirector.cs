using MyDbtDiaryCard.Model;
using MyDbtDiaryCard.Resx;
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
                chartBuilder.SetLegend(EntryResources.Feelings);
                chartBuilder.SetLeftAxis(false);
                chartBuilder.SetSeries(_stats.FeelingsStats.emMisery, OxyColor.Parse("#94e0c8"), EntryResources.F_EmotionMisery);
                chartBuilder.SetSeries(_stats.FeelingsStats.phMisery, OxyColor.Parse("#c098e3"), EntryResources.F_PhysicalMisery);
                chartBuilder.SetSeries(_stats.FeelingsStats.excitation, OxyColor.Parse("#ecd079"), EntryResources.F_Excitation);

                return chartBuilder.GetChart();
        }

        public PlotModel BuildEmotionsChart(ILineChartBuilder chartBuilder)
        {

            chartBuilder.Reset();
            chartBuilder.SetPlotModel(_stats.Dates);
            chartBuilder.SetLegend(EntryResources.Emotions);
            chartBuilder.SetLeftAxis(false);
            chartBuilder.SetSeries(_stats.EmotionsStats.anger, OxyColor.Parse("#dd5e4b"), EntryResources.E_Anger);
            chartBuilder.SetSeries(_stats.EmotionsStats.sadness, OxyColor.Parse("#27aeef"), EntryResources.E_Sadness);
            chartBuilder.SetSeries(_stats.EmotionsStats.fear, OxyColor.Parse("#b33dc6"), EntryResources.E_Fear);
            chartBuilder.SetSeries(_stats.EmotionsStats.shame, OxyColor.Parse("#87bc45"), EntryResources.E_Shame);
            chartBuilder.SetSeries(_stats.EmotionsStats.pride, OxyColor.Parse("#f477a4"), EntryResources.E_Pride);
            chartBuilder.SetSeries(_stats.EmotionsStats.joy, OxyColor.Parse("#edbf33"), EntryResources.E_Joy);

            return chartBuilder.GetChart();
        }

        public PlotModel BuildUrgesChart(ILineChartBuilder chartBuilder)
        {
            chartBuilder.Reset();
            chartBuilder.SetPlotModel(_stats.Dates);
            chartBuilder.SetLegend(EntryResources.Urges);
            chartBuilder.SetLeftAxis(true);
            chartBuilder.SetSeries(_stats.UrgesStats.selfharm, OxyColor.Parse("#df9f62"), EntryResources.U_SelfHarm);
            chartBuilder.SetSeries(_stats.UrgesStats.suicide, OxyColor.Parse("#e78ac3"), EntryResources.U_Suicide);
            chartBuilder.SetSeries(_stats.UrgesStats.drugs, OxyColor.Parse("#9c7cb0"), EntryResources.U_Drugs);
            chartBuilder.SetSeries(_stats.UrgesStats.alcohol, OxyColor.Parse("#6fadb9"), EntryResources.U_Alcohol);

            return chartBuilder.GetChart();
        }
    }
}
