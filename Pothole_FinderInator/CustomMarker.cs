using Syncfusion.SfMaps.XForms;
using Xamarin.Forms;

namespace Pothole_FinderInator
{
    public class CustomMarker : MapMarker
    {
        public CustomMarker()
        {
            Image = ImageSource.FromResource("gps.png");
        }
        
        public ImageSource Image { get; set; }
    }
}