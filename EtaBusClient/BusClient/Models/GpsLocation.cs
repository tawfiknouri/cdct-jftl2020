namespace BusClient.Model
{
    public class GpsLocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public GpsLocation(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

    }
}