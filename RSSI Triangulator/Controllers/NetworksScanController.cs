using Microsoft.AspNetCore.Mvc;
using RSSI_Triangulator.Models;
using RSSI_Triangulator.Services;
using System.Net;

namespace RSSI_Triangulator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NetworksScanController : ControllerBase
    {

        private readonly ITriangulationService _triangulationService;
        private readonly ILogger<NetworksScanController> _logger;

        public NetworksScanController(
            ILogger<NetworksScanController> logger,
            ITriangulationService triangulationService
            )
        {
            _triangulationService = triangulationService;
            _logger = logger;
        }

        [HttpPost("network_scan", Name = "PostNetworkScan")]
        public IActionResult PostNetworkScan([FromBody] NetworkScan scan)
        {
            if (scan == null)
            {
                return BadRequest("No data acquired");
            }
            _triangulationService.AddNetworkScan(scan);
            return Ok(_triangulationService.GetNetworkModules());
        }

        [HttpPost("set_bluetooth_network_scan", Name = "PostBluetoothNetworkScan")]
        public IActionResult PostNetworkScan([FromBody] NetworkScan scan)
        {
            if (scan == null)
            {
                return BadRequest("No data acquired");
            }
            _triangulationService.AddNetworkScan(scan, true);
            return Ok(_triangulationService.GetNetworkModules(true));
        }

        [HttpGet("network_modules", Name = "GetNetworkScans")]
        public List<NetworkModule> GetNetworkScans()
        {
            return _triangulationService.GetNetworkModules();
        }

        [HttpGet("sources", Name = "GetNetworkSources")]
        public string[] FoundSources()
        {
            return _triangulationService.GetNetworkScanSources();
        }

        [HttpGet("between", Name = "GetNetworkScansBetween")]
        public List<ScannedNetwork>? GetNetworkScansBetween(string module, DateTime start, DateTime end)
        {
            return _triangulationService.GetNetworkScansBetween(module,start, end);
        }

        [HttpGet("average", Name = "GetAverageRSSI")]
        public NetworkRelationship? GetAverageRSSI(string scanningSSID, string scannedSSID, int? seconds = null)
        {
            return _triangulationService.GetAverageRSSI(scanningSSID, scannedSSID, seconds);
        }

        [HttpGet("scans_from_by", Name = "GetScansFromBy")]
        public List<ScannedNetwork>? GetScansByFrom(string scanningSSID, string scannedSSID)
        {
            return _triangulationService.GetScansByFrom(scanningSSID, scannedSSID);
        }

        [HttpGet("scanned_ssids_by", Name = "GetScannedSSIDsBy")]
        public string[]? GetScannedSSIDs(string scanningSSID)
        {
            return _triangulationService.GetScannedSSIDs(scanningSSID);
        }

        [HttpGet("all_average_RSSI_from", Name = "GetAllAverageRSSIFrom")]
        public List<NetworkRelationship>? GetAllAverageRSSIFrom(string scanningSSID, int? seconds = null) {
            NetworkModule? networkModule = _triangulationService.GetNetworkModule(scanningSSID);
            if(networkModule == null)
            {
                return null;
            }

            return _triangulationService.GetAllAverageRSSIFrom(networkModule, seconds);
        }

        [HttpGet("all_average_RSSI", Name = "GetAllAverageRSSI")]
        public List<NetworkRelationship> GetAllAverageRSSI(int? seconds = null)
        {
            return _triangulationService.GetAllAverageRSSI(seconds);
        }

        [HttpGet("latest_scans_from", Name = "GetLatestScansFrom")]
        public List<ScannedNetwork>? GetLatestScansFrom(string scanningSSID, int? seconds = null)
        {
            NetworkModule? networkModule = _triangulationService.GetNetworkModule(scanningSSID);
            if (networkModule == null)
            {
                return null;
            }

            return _triangulationService.GetAllLatestScansFrom(networkModule, seconds);
        }

        [HttpGet("latest_scans", Name = "GetLatestScans")]
        public List<ScannedNetwork>? GetLatestScans(int? seconds = null)
        {
            return _triangulationService.GetAllLatestScans(seconds);
        }
    }
}
