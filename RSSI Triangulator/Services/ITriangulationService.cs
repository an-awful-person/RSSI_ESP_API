using RSSI_Triangulator.Models;

namespace RSSI_Triangulator.Services
{
    public interface ITriangulationService
    {
        void AddNetworkScan(NetworkScan scan);
        List<NetworkModule> GetNetworkModules();
        List<ScannedNetwork>? GetNetworkScansBetween(string module, DateTime start, DateTime end);
        string[] GetNetworkScanSources();
        NetworkRelationship? GetAverageRSSI(string scanningSSID, string scannedSSID, int? seconds = null);
        List<ScannedNetwork>? GetScansByFrom(string scanningSSID, string scannedSSID);
        NetworkModule? GetNetworkModule(string ssid);
        string[]? GetScannedSSIDs(string ssid);
        string[] GetScannedSSIDs(NetworkModule module);
        List<NetworkRelationship> GetAllAverageRSSIFrom(NetworkModule module, int? seconds = null);
        List<NetworkRelationship> GetAllAverageRSSI(int? seconds = null);
        List<ScannedNetwork> GetAllLatestScansFrom(NetworkModule module, int? seconds = null);
        List<ScannedNetwork> GetAllLatestScans(int? seconds = null);
    }
}
