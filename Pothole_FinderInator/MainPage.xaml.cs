using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using Syncfusion.SfMaps.XForms;
using Xamarin.Forms.Maps;

namespace Pothole_FinderInator
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            StartAccelerationReading();
            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                GetCurrentLocation();
                return true;
            });
        }
        
        private async void GetCurrentLocation()
        {
            
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

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
        
        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;
            Console.WriteLine($"Reading: X: {data.Acceleration.X}, Y: {data.Acceleration.Y}, Z: {data.Acceleration.Z}");
            AkcelerometrY.Text = data.Acceleration.Y.ToString();
        }
    }
}