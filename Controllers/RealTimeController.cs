
using Microsoft.AspNetCore.Mvc;
using ece4180.gpstracker.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
namespace ece4180.gpstracker.controllers{
    [Route("RealTime")]
    public class RealTimeController: Controller{
        
        private readonly TripAccessor tripaccessor_;
        public RealTimeController(TripAccessor ta){
            tripaccessor_ = ta;
        }
        // a restful api, how to return json
        [HttpGet("UploadPosition/{tripId:int}/{lat_:double}/{long_:double}")]
        public async Task<string> UploadPosition(int tripId, double lat_, double long_){
            // Console.WriteLine($"time: {tripId}, lat: {lat_}, long: {long_}");
            int result = await tripaccessor_.AddLocation(tripId, lat_, long_);
            return result.ToString();
        }
        /* [Route("DownloadTrip/{tripId}")]
        public async Task<string> DownloadTrip(int tripId, long since){
            List<Location> locs = await tripaccessor_.GetTripLocations(tripId);
            string result = "";
            if(locs == null) {return "Invalid"; }
            foreach(Location loc in locs){
                result += $"{loc.timeStamp} {loc.lat_} {loc.long_}\n";
            }
            return result;
        }*/
    }

}
