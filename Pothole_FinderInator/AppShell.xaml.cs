using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pothole_FinderInator
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            if (DbConnectionHandler.UserName == null)
            {
                MainPageSideViewCommand.IsEnabled = false;
                MainPageSideViewCommand.IsVisible = false;
            }
            else
            {
                UserIsLogged();
            }
        }

        private void UserIsLogged()
        {
            LoginPageViewCommand.IsEnabled = false;
            LoginPageViewCommand.IsVisible = false;
        }
    }
}