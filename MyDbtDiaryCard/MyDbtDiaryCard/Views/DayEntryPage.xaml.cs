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

           //remove?
            dayEntryVM.WrongDataPicked += ShowAlert;

            this.BindingContext = dayEntryVM;

            datePicker.MaximumDate = DateTime.Today;
        }

        private async void ShowAlert(object sender, EventArgs e)
        {
            await DisplayAlert(Resx.ViewResources.Alert, Resx.ViewResources.Alert_DayFromFuture, Resx.ViewResources.Ok);
        }
    }
}