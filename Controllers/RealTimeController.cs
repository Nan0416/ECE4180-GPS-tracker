
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
        [HttpGet("UploadPosition/{tripId:int}/{lat_v:double}/{long_v:double}")]
        [HttpPost("UploadPosition")]
        public async Task<string> UploadPosition(int tripId, double lat_v, double long_v){
            // Console.WriteLine($"time: {tripId}, lat: {lat_}, long: {long_}");
            int result = await tripaccessor_.AddLocation(tripId, lat_v, long_v);
            if(result == -1){
                HttpContext.Response.StatusCode = 400;
            }
            return result.ToString();
        }
        [HttpGet("Trip/{tripId:int}/{since:long=-1}")]
        public async Task<JsonResult> Trip(int tripId, long since){
            List<Location> locs = await tripaccessor_.GetTripLocations(tripId, TRIPSTATUS.RUNNING, since);
            if(locs == null) {
                HttpContext.Response.StatusCode = 404;
                return Json(new ErrorJSONResult("Not found")); 
            }
            return Json(locs);
        }
    }

}
