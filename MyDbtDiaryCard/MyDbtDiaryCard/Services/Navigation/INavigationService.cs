using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyDbtDiaryCard.Services.Navigation
{
    public interface INavigationService
    {
        string CurrentPageKey { get; }
        void Configure(string pageKey, Type pageType);
        Task NavigateAsync(string pageKey);
        Task NavigateAsync(string pageKey, object parameter);
        Task NavigateAsync(string pageKey, params object[] parameters);
        Task PushPopupAsync(string pageKey);
        Task GoBackAsync();

    }
}
