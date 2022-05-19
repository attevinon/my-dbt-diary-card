using System;
using MyDbtDiaryCard.Views;
using MyDbtDiaryCard.Services.Navigation;
using MyDbtDiaryCard.Services.DataService;
using System.IO;
using System.Globalization;
using MyDbtDiaryCard.Views.PopupPages;

namespace MyDbtDiaryCard
{
    public partial class App 
    {
        public static INavigationService NavigationService { get; } = new PageNavigationService();
        public App()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("ru");

            //File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MyDbtDiaryDb.db3"));

            InitializeComponent();

            DataService.GetDataManager().InitializeAsync(
                Path.Combine(Environment.GetFolderPath(
                        Environment.SpecialFolder.LocalApplicationData), "MyDbtDiaryDb.db3"));

            //popup pages
            NavigationService.Configure(nameof(UsefulnessHelpPage), typeof(UsefulnessHelpPage));

            //general pages
            NavigationService.Configure(nameof(DayEntryPage), typeof(DayEntryPage));
            NavigationService.Configure(nameof(AddDayEntryPage), typeof(AddDayEntryPage));
            NavigationService.Configure(nameof(OverviewPage), typeof(OverviewPage));
            NavigationService.Configure(nameof(TableOverviewPage), typeof(TableOverviewPage));
            NavigationService.Configure(nameof(DbtSkillsPage), typeof(DbtSkillsPage));

            //main page
            NavigationService.Configure(nameof(Views.MainPage), typeof(Views.MainPage));

            try
            {
                MainPage = (NavigationService as PageNavigationService)?.SetRootPage(nameof(Views.MainPage));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message, ex.StackTrace);
                this.Quit();
            }
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
