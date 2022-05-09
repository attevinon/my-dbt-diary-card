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
        private bool isLoading;
        public bool IsLoading
        {
            get { return isLoading; }
            protected set { SetProperty(ref isLoading, value); }
        }
        protected INavigationService NavigationService { get; }

        public BaseViewModel(INavigationService navigation)
        {
            NavigationService = navigation;
            
        }
    }
}
