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
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void LoginEvent(System.Object sender, System.EventArgs e)
        {
            var userValue = DbConnectionHandler.Login(Username.Text, Password.Text);
            if (userValue == 1)
            {
                DbConnectionHandler.UserName = Username.Text;
                Warning.Text = "Login succes!";
                Shell.Current.CurrentItem.IsVisible = false;
                await Shell.Current.GoToAsync("//Main Page");

            }
            else if (userValue > 1 || userValue == 0)
            {
                Warning.Text = "Wrong username or password!";
            }
            else
            {
                Warning.Text = "Connection to database failed!";
            }
        }
    }
}