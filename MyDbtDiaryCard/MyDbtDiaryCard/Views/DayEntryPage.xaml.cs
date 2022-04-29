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
    public partial class DayEntryPage : ContentPage
    {
        public DayEntryPage()
        {

                InitializeComponent();
                var dayEntryVM = new DayEntryViewModel(App.NavigationService);
                this.BindingContext = dayEntryVM;
        }
    }
}