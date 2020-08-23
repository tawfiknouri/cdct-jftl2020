namespace BusService.Model
{
    public class GpsLocation
    {

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public GpsLocation()
        {

        }
        public GpsLocation(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

    }
}