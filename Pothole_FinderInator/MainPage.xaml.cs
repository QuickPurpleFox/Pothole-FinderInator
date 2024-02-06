using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Essentials;
using Rg.Plugins.Popup.Services;
using Syncfusion.SfMaps.XForms;
using System.ComponentModel;
using System.Threading;

namespace Pothole_FinderInator
{
    public partial class MainPage : ContentPage
    {
        private HolePopUp _holePopUp;
        private CancellationTokenSource _cts;
        private bool _isDisplay = false;
        private SfMaps _map;
        private double latitude;
        private double longitude;

        public MainPage()
        {
            InitializeComponent();
            _map = this.SfMaps;
            StartAccelerationReading();
            DbConnectionHandler.GetPotholes();

            PopulateMap();

            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
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
                    latitude = location.Latitude;
                    longitude = location.Longitude;

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

            double shakeThreshold = 1.5;

            if (!_isDisplay && Math.Sqrt(data.Acceleration.X * data.Acceleration.X + data.Acceleration.Y * data.Acceleration.Y + data.Acceleration.Z * data.Acceleration.Z) > shakeThreshold)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    _isDisplay = true;
                    Pothole p = new Pothole(latitude, longitude, "");
                    HolePopUp popup = new HolePopUp(p);
                    popup.PopupClosed += () => { _isDisplay = false; };
                    await PopupNavigation.Instance.PushAsync(popup);

                });
            }
        }

        private void PopulateMap()
        { 
            foreach (var hole in DbConnectionHandler.PotholesList)
            {
                MapMarker potholeMarker = new MapMarker();
                potholeMarker.Label = "PotHole UwU";
                potholeMarker.Latitude = hole.GetLatitude().ToString();
                potholeMarker.Longitude = hole.GetLongitude().ToString();
                MapPossition.Markers.Add(potholeMarker);
            }
        }
    }
}
