using System;
using MyDbtDiaryCard.Views;
using Xamarin.Forms;
using Prism.Mvvm;
using Xamarin.Forms.Xaml;
using MyDbtDiaryCard.Services.Navigation;
using MyDbtDiaryCard.Services.DataService;
using System.IO;

namespace MyDbtDiaryCard
{
    public partial class App 
    {
        public static INavigationService NavigationService { get; } = new PageNavigationService();
        public App()
        {
            InitializeComponent();

            /*File.Delete(Path.Combine(Environment.GetFolderPath(
                        Environment.SpecialFolder.LocalApplicationData), "MyDbtDiaryDatabase.db3"));*/

            DataService.GetDataManager().InitializeAsync(
                Path.Combine(Environment.GetFolderPath(
                        Environment.SpecialFolder.LocalApplicationData),"MyDbtDiaryDb.db3"));

            NavigationService.Configure(nameof(DayEntryPage), typeof(DayEntryPage));
            NavigationService.Configure(nameof(AddDayEntryPage), typeof(AddDayEntryPage));
            NavigationService.Configure(nameof(OverviewPage), typeof(OverviewPage));
            NavigationService.Configure(nameof(Views.MainPage), typeof(Views.MainPage));

            MainPage = (NavigationService as PageNavigationService)?.SetRootPage(nameof(Views.MainPage));
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
