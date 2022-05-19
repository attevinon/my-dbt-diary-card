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
    public partial class TableOverviewPage : ContentPage
    {
        public List<int> DaysRangeList { get; }
        public double CellHeight
        {
            get
            {
                if (vm == null)
                    return 50;

                return stckEntries.Height / vm.DaysRange;
            }
        }

        TableViewModel vm;
        public TableOverviewPage()
        {
            DaysRangeList = new List<int> { 7, 14 };

            InitializeComponent();

            vm = new TableViewModel(App.NavigationService);
            this.BindingContext = vm;

            startDatePicker.MaximumDate = DateTime.Today.AddDays(-1);
            endDatePicker.MaximumDate = DateTime.Today;

            stckEntries.LayoutChanged += (object sender, EventArgs e) => stckEntries.ForceLayout();
        }
    }
}