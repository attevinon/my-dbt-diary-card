using MyDbtDiaryCard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyDbtDiaryCard.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddDayEntryPage : ContentPage
    {
        public List<string> UsefulnessScale { get; }
        AddDayEntryViewModel addDayEntryViewModel;
        public AddDayEntryPage()
        {
            UsefulnessScale = new List<string>
            {
                "-",
                Resx.EntryResources.Usefulness_0,
                Resx.EntryResources.Usefulness_1,
                Resx.EntryResources.Usefulness_2,
                Resx.EntryResources.Usefulness_3,
                Resx.EntryResources.Usefulness_4,
                Resx.EntryResources.Usefulness_5,
                Resx.EntryResources.Usefulness_6,
                Resx.EntryResources.Usefulness_7,
            };

            InitializeComponent();
        }

        public AddDayEntryPage(DateTime date) : this()
        {
            BindingContext = addDayEntryViewModel = new AddDayEntryViewModel(App.NavigationService, date);
        }
    }
}