using System;

namespace Pothole_FinderInator
{
    public class Pothole
    {
        private double  _latitude;
        private double  _longitude;
        private int _score;
        private String _size;
        
        public Pothole(double  latitude, double  longitude, String size)
        {
            this._latitude = latitude;
            this._longitude = longitude;
            this._size = size;
            _score = 0;
        }

        public double GetLatitude()
        {
            return _latitude;
        }

        public double GetLongitude()
        {
            return _longitude;
        }

        public String GetSize()
        {
            return _size;
        }
    }
}