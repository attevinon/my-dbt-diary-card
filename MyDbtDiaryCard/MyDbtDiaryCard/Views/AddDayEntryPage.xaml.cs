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
        AddDayEntryViewModel addDayEntryViewModel;
        public AddDayEntryPage()
        {
            InitializeComponent();
        }

        public AddDayEntryPage(DateTime date) : this()
        {
            BindingContext = addDayEntryViewModel = new AddDayEntryViewModel(App.NavigationService, date);
        }
    }
}