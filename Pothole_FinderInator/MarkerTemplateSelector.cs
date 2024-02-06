using Syncfusion.SfMaps.XForms;
using System;
using Xamarin.Forms;

namespace Pothole_FinderInator
{
    public class MarkerTemplateSelector : DataTemplateSelector
    {
        public DataTemplate GpsTemplate { get; set; }

        public DataTemplate SmallPotholeTemplate { get; set; }

        public DataTemplate MediumPotholeTemplate { get; set; }

        public DataTemplate BigPotholeTemplate { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {

            if (((MapMarker)item).Label == "gps")
            {
                return GpsTemplate;
            }
            else if (((MapMarker)item).Label == "big")
            {
                return BigPotholeTemplate;
            }
            if (((MapMarker)item).Label == "medium")
            {
                return MediumPotholeTemplate;
            }
            else if (((MapMarker)item).Label == "small")
            {
                return SmallPotholeTemplate;
            }
            return SmallPotholeTemplate;


        }
    }
}