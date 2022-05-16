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
    public partial class OverviewPage : ContentPage
    {
        public List<int> DaysRangeList { get; }
        public OverviewPage()
        {
            DaysRangeList = new List<int> { 7, 14, 30 };

            InitializeComponent();
            var vm = new OverviewViewModel(App.NavigationService);
            BindingContext = vm;

        }
    }
}