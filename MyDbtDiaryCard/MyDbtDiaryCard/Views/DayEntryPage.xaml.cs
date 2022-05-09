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
            dayEntryVM.WrongDataPicked += ShowAlert;
            this.BindingContext = dayEntryVM;
        }

        private async void ShowAlert(object sender, EventArgs e)
        {
            await DisplayAlert("Alert", "You can't be from future!", "ok :(");
        }
    }
}