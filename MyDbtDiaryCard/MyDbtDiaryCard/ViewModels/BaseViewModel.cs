using MyDbtDiaryCard.Services.DataService;
using MyDbtDiaryCard.Services.Navigation;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyDbtDiaryCard.ViewModels
{
    internal class BaseViewModel : BindableBase
    {
        protected INavigationService NavigationService { get; }

        public BaseViewModel(INavigationService navigation)
        {
            NavigationService = navigation;
            
        }
    }
}
