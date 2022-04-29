using MyDbtDiaryCard.Events;
using MyDbtDiaryCard.Model;
using MyDbtDiaryCard.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyDbtDiaryCard.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DbtSkillsPage : ContentPage
    {
        public DbtSkillsPage()
        {
            InitializeComponent();
        }

        //DELETE REFFFFFFFF!!!! NO MODEL HERE!!!!!!!!!!!!!!!!!!!!!
        public DbtSkillsPage(List<string> names, Action<object, UsedDbtSkillesChangedEvent> action) : this()
        {

            this.BindingContext = new DbtSkillsViewModel(App.NavigationService, names, action);
        }
    }
}