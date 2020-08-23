using BusClient.Model;

namespace BusClient.Models
{
    public class BusInfo
    {
        public int BusID { get; set; }
        public int Eta { get; set; }
        public GpsLocation BusGpsLocation { get; set; }


        public BusInfo(int busID, int eta = 0, GpsLocation gpsLocation = null)
        {
            BusID = busID;
            Eta = eta;
            BusGpsLocation = gpsLocation;
        }
    }
}