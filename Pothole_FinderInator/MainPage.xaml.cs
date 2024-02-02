using System;
using System.Collections.Generic;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Essentials;
using Rg.Plugins.Popup.Services;
using Syncfusion.SfMaps.XForms;
using Xamarin.Forms.Maps;

namespace Pothole_FinderInator
{
    public partial class MainPage : ContentPage
    {
        private HolePopUp _holePopUp;
        private CancellationTokenSource _cts;
        private bool _isDisplay = false;
        private SfMaps _map;
        
        public MainPage()
        {
            InitializeComponent();
            _map = SfMaps;
            StartAccelerationReading();
            DbConnectionHandler.GetPotholes();

            PopulateMap();
            
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
            
            double shakeThreshold = 2;
            
            if (!_isDisplay && Math.Sqrt(data.Acceleration.X * data.Acceleration.X + data.Acceleration.Y * data.Acceleration.Y + data.Acceleration.Z * data.Acceleration.Z) > shakeThreshold)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    _isDisplay = true;
                    HolePopUp popup = new HolePopUp();
                    popup.PopupClosed += () => { _isDisplay = false; };
                    await PopupNavigation.Instance.PushAsync(popup);
                });
            }
        }

        private void PopulateMap()
        {
            //Szczecin: long=14.5530200 lati=53.4289400
            var markers = new List<Pin>();
            
            ImageryLayer layer = new ImageryLayer();
            
            MapMarker marker = new MapMarker();
            marker.Label = "PotHole UwU";
            marker.Latitude = "53";
            marker.Longitude = "14";
            layer.Markers.Add(marker);
            this.Content = _map;
            
            _map.Layers.Add(layer);
            
            /*foreach (var hole in DbConnectionHandler.PotholesList)
            {
                
            }*/
        }
    }
}