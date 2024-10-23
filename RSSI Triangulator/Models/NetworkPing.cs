namespace RSSI_Triangulator.Models
{
    public class NetworkPing
    {
        public required String SSID { get; set; }
        public required String BSSID { get; set; }
        public required int RSSI { get; set; }
    }
}
