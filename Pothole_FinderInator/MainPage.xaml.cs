using System;
using System.Threading;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Essentials;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Pothole_FinderInator
{
    public partial class MainPage : ContentPage
    {
        private CancellationTokenSource _cts;
        private HolePopUp _popup;
        
        public MainPage()
        {
            InitializeComponent();
            StartAccelerationReading();

            _popup = new HolePopUp();
            
            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
            Device.StartTimer(new TimeSpan(0,0,1), () =>
            {
                GetCurrentLocation();
                return true;
            });
        }
        
        private async void GetCurrentLocation()
        {
            
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                _cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync();

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    
                    LatiBinding.Text = location.Latitude.ToString();
                    LongBinding.Text = location.Longitude.ToString();

                    MapPossition.GeoCoordinates = new Point(location.Latitude, location.Longitude);
                    
                    Marker.Latitude = location.Latitude.ToString();
                    Marker.Longitude = location.Longitude.ToString();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception();
            }
        }

        private static void StartAccelerationReading()
        {
            SensorSpeed speed = SensorSpeed.UI;
            Accelerometer.Start(speed);
        }
        
        private async void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;
            Console.WriteLine($"Reading: X: {data.Acceleration.X}, Y: {data.Acceleration.Y}, Z: {data.Acceleration.Z}");
            AkcelerometrX.Text = "X: " + data.Acceleration.X.ToString();
            AkcelerometrY.Text = "Y: " + data.Acceleration.Y.ToString();
            AkcelerometrZ.Text = "Z: " + data.Acceleration.Z.ToString();
            if (data.Acceleration.Y > 2 || data.Acceleration.X > 2 || data.Acceleration.Z > 2)
            {
                await Navigation.PushPopupAsync(_popup);
            }
        }
    }
}