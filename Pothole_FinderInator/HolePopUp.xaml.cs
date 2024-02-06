using Rg.Plugins.Popup.Services;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;

namespace Pothole_FinderInator
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HolePopUp : Rg.Plugins.Popup.Pages.PopupPage
    {
        public delegate void PopupClosedHandler();
        public event PopupClosedHandler PopupClosed;
        public static double PopUpWidth;
        public static double PopUpHeight;
        public Pothole potHole;
        string SelectedSize;

        public HolePopUp(Pothole p)
        {
            potHole = p;
            PopUpWidth = (DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density) * 0.9;
            PopUpHeight = (DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density) * 0.5;
            InitializeComponent();
        }

        private void ClosePopupCommand(System.Object sender, System.EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            PopupClosed?.Invoke();
        }

        public void Accepted(System.Object sender, System.EventArgs e)
        {
            SelectedSize = HoleSizePicker.Items[HoleSizePicker.SelectedIndex].ToString();
            if (!DbConnectionHandler.InsertPotHole(potHole.GetLatitude().ToString().Replace(",", "."), potHole.GetLongitude().ToString().Replace(",", "."), SelectedSize))
            {
                ocs.Text = "ERROR";
            }
            
            PopupNavigation.Instance.PopAsync();
            
        }
    }
}