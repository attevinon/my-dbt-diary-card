using MyDbtDiaryCard.Events;
using MyDbtDiaryCard.ViewModels;
using System;
using System.Collections.Generic;

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

        public DbtSkillsPage(List<int> idList, Action<object, UsedDbtSkillsChangedEvent> action) : this()
        {

            this.BindingContext = new DbtSkillsViewModel(App.NavigationService, idList, action);
        }
    }
}