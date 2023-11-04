using System;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Pothole_FinderInator
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            GetCurrentLocation();
        }

        private async void GetCurrentLocation()
        {
            try
            {

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception();
            }
        }
    }
}