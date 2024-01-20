namespace Pothole_FinderInator
{
    public class Pothole
    {
        private float _latitude;
        private float _longitude;
        private int _score;
        private size _size;
        
        public enum size
        {
            Small,
            Medium,
            Large
        }
        
        public Pothole(float latitude, float longitude, size size)
        {
            this._latitude = latitude;
            this._longitude = longitude;
            this._size = size;
            _score = 0;
        }
    }
}