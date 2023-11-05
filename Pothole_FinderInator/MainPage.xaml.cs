using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using Syncfusion.SfMaps.XForms;
using Xamarin.Forms.Maps;

namespace Pothole_FinderInator
{
    public partial class MainPage : ContentPage
    {
        private MapMarker _userMarker = new MapMarker();
        private ImageryLayer _imageryLayer = new ImageryLayer();
        
        public MainPage()
        {
            InitializeComponent();
            GetCurrentLocation();
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
                    
                    var centerPosition = new Syncfusion.SfMaps.XForms.Position(location.Latitude, location.Longitude);
                    var distance = Distance.FromKilometers(1.0); 
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception();
            }
        }
    }
}