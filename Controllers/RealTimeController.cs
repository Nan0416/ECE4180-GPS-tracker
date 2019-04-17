
using Microsoft.AspNetCore.Mvc;
using System;
namespace ece4180.gpstracker.controllers{
    [Route("RealTime")]
    public class RealTimeController: Controller{
        
        public RealTimeController(){

        }
        // a restful api, how to return json
        [HttpGet("UploadPosition/{tripId}/{lat_}/{long_}")]
        public string UploadPosition(int id, double lat_, double long_){
            Console.WriteLine("time: {0}, lat: {1}, long: {2}", id, lat_, long_);
            return "OK";
        }
        [HttpGet("DownloadTrip/{tripId}")]
        public string DownloadTrip(int tripId){
            return "OK";
        }
    }

}
