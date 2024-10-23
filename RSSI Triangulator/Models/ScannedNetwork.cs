namespace RSSI_Triangulator.Models
{
    public class ScannedNetwork
    {
        public required string scanningModuleSSID { get; set; }
        public required string scannedModuleName { get; set; }
        public required string scannedModuleSSID { get; set; }
        public int RSSI { get; set; }
        public DateTime DateTime { get; } = DateTime.Now;
    }
}
