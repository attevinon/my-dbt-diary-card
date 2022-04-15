using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using System.Reflection;

namespace MyDbtDiaryCard.Services.Navigation
{
    public class PageNavigationService : INavigationService
    {
        private readonly object _locker = new object();
        private readonly Dictionary<string, Type> _pagesByKey = new Dictionary<string, Type>();
        private readonly Stack<NavigationPage> _navigationPagesStack = new Stack<NavigationPage>();
        private NavigationPage CurrentNavigationPage => _navigationPagesStack.Peek();
        public string CurrentPageKey
        {
            get
            {
                lock (_locker)
                {
                    if (CurrentNavigationPage.CurrentPage == null)
                        return null;

                    var pageType = CurrentNavigationPage.CurrentPage.GetType();

                    return (_pagesByKey.ContainsValue(pageType)) ?
                        _pagesByKey.First(p => p.Value == pageType).Key : null;
                }
            }
        }

        public Page SetRootPage(string rootPageKey)
        {
            var rootPage = GetPage(rootPageKey);
            _navigationPagesStack.Clear();
            var mainPage = new NavigationPage(rootPage);
            _navigationPagesStack.Push(mainPage);
            return mainPage;
        }

        public void Configure(string pageKey, Type pageType)
        {
            lock (_locker)
            {
                if (_pagesByKey.ContainsKey(pageKey))
                {
                    _pagesByKey[pageKey] = pageType;
                }
                else
                {
                    _pagesByKey.Add(pageKey, pageType);
                }
            }
        }


        public async Task NavigateAsync(string pageKey)
        {
            var page = GetPage(pageKey);
            await CurrentNavigationPage.Navigation.PushAsync(page);

        }
        public async Task NavigateAsync(string pageKey, object parameter)
        {
            var page = GetPage(pageKey, parameter);
            await CurrentNavigationPage.Navigation.PushAsync(page);
        }
        public async Task GoBack()
        {
            var navigationStack = CurrentNavigationPage.Navigation;

            if (navigationStack.NavigationStack.Count > 1)
            { 
                await CurrentNavigationPage.PopAsync();
                return;
            }

            if (_navigationPagesStack.Count > 1) //?????????????????????????????????????????
            {
                _navigationPagesStack.Pop();
                await CurrentNavigationPage.Navigation.PopModalAsync();
                return;
            }

            await CurrentNavigationPage.PopAsync();

        }

        private Page GetPage(string pageKey, object parameter = null)
        {
            lock(_locker)
            {
                if (!_pagesByKey.ContainsKey(pageKey))
                    throw new Exception();

                var pageType = _pagesByKey[pageKey];
                object[] parameters;
                ConstructorInfo constructor; //???????????????????????????????????

                if (parameter == null)
                {
                    constructor = pageType.GetTypeInfo()
                        .DeclaredConstructors
                        .FirstOrDefault(c => !c.GetParameters().Any());
                    parameters = new object[] { };
                }
                else
                {
                    constructor = pageType.GetTypeInfo()
                        .DeclaredConstructors
                        .FirstOrDefault(c =>
                        {
                            var p = c.GetParameters();
                            return p.Length == 1 && p[0].ParameterType == parameter.GetType();
                        });

                    parameters = new[] 
                    {
                        parameter
                    };
                }

                if (constructor == null)
                    throw new Exception("No suitable constructor :(");

                var page = constructor.Invoke(parameters) as Page;
                return page;
            }
        }


    }
}
