using RSSI_Triangulator.Models;
using System.Collections.Generic;

namespace RSSI_Triangulator.Services
{
    public class TriangulationService : ITriangulationService
    {  
        private readonly List<NetworkModule> _networkModules = new List<NetworkModule>();
        private readonly HashSet<string> _knownModules = new HashSet<string>();
        //Need to know distance between known modules.
        //Need a way to find the average RSSI between a connection.
        public void AddNetworkScan(NetworkScan scan)
        {
            NetworkModule? foundModule = _networkModules.Find(network => network.MacAddress.Equals(scan.MacAddress));
            if (foundModule == null){
                foundModule = new NetworkModule() { MacAddress = scan.MacAddress };
                _networkModules.Add(foundModule);
            }

            scan.NetworkPings.ToList().ForEach(ping => {
                foundModule.ScannedNetworks.Add(new ScannedNetwork()
                {
                    scanningModuleSSID = scan.MacAddress,
                    scannedModuleName = ping.SSID,
                    scannedModuleSSID = ping.BSSID,
                    RSSI = ping.RSSI,
                });
            });

            foundModule.ScannedNetworks = foundModule.ScannedNetworks.OrderByDescending(n => n.DateTime).ToList();
            
            _knownModules.Add(scan.MacAddress);
        }

        public List<NetworkModule> GetNetworkModules()
        {
            return _networkModules;
        }

        public List<ScannedNetwork>? GetNetworkScansBetween(string module, DateTime start, DateTime end)
        {
            return _networkModules.Find(m => m.MacAddress.Equals(module))?.ScannedNetworks.FindAll(scan => scan.DateTime >= start && scan.DateTime <= end);
        }

        public string[] GetNetworkScanSources()
        {
            return _knownModules.ToArray();
        }

        public List<ScannedNetwork>? GetScansByFrom(string scanningSSID, string scannedSSID)
        {
            return _networkModules.Find(m => m.MacAddress.Equals(scanningSSID))?.ScannedNetworks.FindAll(scan => scan.scannedModuleSSID.Equals(scannedSSID));
        }

        public NetworkRelationship? GetAverageRSSI(string scanningSSID, string scannedSSID, int? seconds = null)
        {
            NetworkModule? foundModule = _networkModules.Find(m => m.MacAddress.Equals(scanningSSID));
            if (foundModule == null)
            {
                return null;
            }

            List<ScannedNetwork> scannedNetworks = foundModule.ScannedNetworks.FindAll(n => n.scannedModuleSSID.Equals(scannedSSID));
            if (!scannedNetworks.Any())
            {
                return null;
            } else if (seconds != null)
            {
                DateTime secondsAgo = DateTime.Now.AddSeconds((double)-seconds);
                scannedNetworks = scannedNetworks.FindAll(network => network.DateTime.CompareTo(secondsAgo) >= 0);
            }

            int totalRSSI = 0;
            scannedNetworks.ForEach(n => {totalRSSI += n.RSSI; });
            totalRSSI = totalRSSI / scannedNetworks.Count();

            return new NetworkRelationship() { 
                RSSI = totalRSSI, 
                ScannedSSID = scannedSSID,
                ScanningSSID = scanningSSID,
            };
        }

        public NetworkModule? GetNetworkModule(string ssid)
        {
            return _networkModules.Find(m => m.MacAddress.Equals(ssid));
        }

        public string[]? GetScannedSSIDs(string ssid)
        {
            NetworkModule? module = GetNetworkModule(ssid);
            if (module != null)
            {
                return GetScannedSSIDs(module);
            }
            else
            {
                return null;
            }
        }

        public string[] GetScannedSSIDs(NetworkModule module)
        {
            HashSet<string> ssidSet = new HashSet<string>();
            module.ScannedNetworks.ForEach(network =>
            {
                ssidSet.Add(network.scannedModuleSSID);
            });
            return ssidSet.ToArray();
        }

        public List<NetworkRelationship> GetAllAverageRSSIFrom(NetworkModule module, int? seconds = null)
        {
            List<NetworkRelationship> networkRelationships = new List<NetworkRelationship>();
            string[] scannedSSIDs = GetScannedSSIDs(module);
            scannedSSIDs.ToList().ForEach(ssid =>
            {
                NetworkRelationship? networkRelationship = GetAverageRSSI(module.MacAddress, ssid, seconds);
                if (networkRelationship != null)
                {
                    networkRelationships.Add(networkRelationship);
                }
            });
            return networkRelationships;
        }

        public List<NetworkRelationship> GetAllAverageRSSI(int? seconds = null) 
        {
            List<NetworkRelationship> networkRelationships = new List<NetworkRelationship>();

            _networkModules.ForEach(module =>
            {
                List<NetworkRelationship> moduleRelationsips = GetAllAverageRSSIFrom(module, seconds);
                networkRelationships.AddRange(moduleRelationsips);
            });

            return networkRelationships;
        }

        public List<ScannedNetwork> GetAllLatestScansFrom(NetworkModule module, int? seconds = null)
        {
            List<ScannedNetwork> latestScans = new List<ScannedNetwork>();
            string[] scannedSSIDs = GetScannedSSIDs(module);
            scannedSSIDs.ToList().ForEach(ssid =>
            {
                ScannedNetwork? scannedNetwork = module.ScannedNetworks.Find(network => network.scannedModuleSSID.Equals(ssid));
                if (scannedNetwork != null)
                {
                    latestScans.Add(scannedNetwork);
                }
            });
            if (seconds != null)
            {
                DateTime secondsAgo = DateTime.Now.AddSeconds((double)-seconds);
                latestScans = latestScans.FindAll(network => network.DateTime.CompareTo(secondsAgo) >= 0);
            }
            return latestScans;
        }

        public List<ScannedNetwork> GetAllLatestScans(int? seconds = null)
        {
            List<ScannedNetwork> latestScans = new List<ScannedNetwork>();
            _networkModules.ForEach(module =>
            {
                List<ScannedNetwork> moduleRelationsips = GetAllLatestScansFrom(module, seconds);
                latestScans.AddRange(moduleRelationsips);
            });

            return latestScans;
        }
    }
}
