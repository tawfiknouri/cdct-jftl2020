namespace BusService.Model
{
    public class BusInfo
    {
        public int BusID { get; set; }
        public int Eta { get; set; }
        //public GpsLocation BusGpsLocation { get; set; }

        public BusInfo()
        {

        }
        public BusInfo(int busID, int eta)
        {
            Eta = eta;
            BusID = busID;
        }

        /*public BusInfo(int busID, int eta, GpsLocation gpsLocation = null)
        {
            BusID = busID;
            Eta = eta;
            BusGpsLocation = gpsLocation;
        }*/
    }
}