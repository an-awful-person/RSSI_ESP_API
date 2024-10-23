namespace RSSI_Triangulator.Models
{
    public class NetworkRelationship
    {
        public required string ScanningSSID { get; set; }
        public required string ScannedSSID { get; set; }
        public required int RSSI {  get; set; }
    }
}
