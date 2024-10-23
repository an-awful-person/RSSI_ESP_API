namespace RSSI_Triangulator.Models
{
    public class NetworkModule
    {
        public required string MacAddress { get; set; }
        public List<ScannedNetwork> ScannedNetworks { get; set; } = new List<ScannedNetwork>();
    }
}
