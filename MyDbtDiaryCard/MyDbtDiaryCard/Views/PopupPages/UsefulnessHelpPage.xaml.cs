using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyDbtDiaryCard.Views.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UsefulnessHelpPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        public UsefulnessHelpPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PopPopupAsync();
        }

        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            return true;
        }
    }
}