
using Microsoft.AspNetCore.Mvc;
namespace ece4180.gpstracker.controllers{
    [Route("RealTime")]
    public class RealTimeController: Controller{
        // a restful api, how to return json
        [HttpGet("UploadPosition/{tripId}/{lat_}/{long_}")]
        public string UploadPosition(int id, double lat_, double long_){
            return "OK";
        }
        [HttpGet("DownloadTrip/{tripId}")]
        public string DownloadTrip(int tripId){
            return "OK";
        }
    }

}
