namespace RSSI_Triangulator.Models
{
    public class NetworkScan
    {
        public required String MacAddress {  get; set; }

        public NetworkPing[] NetworkPings { get; set; } = new NetworkPing[0];

        public DateTime DateTime { get; } = DateTime.Now;
    }
}
