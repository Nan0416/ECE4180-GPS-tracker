using Microsoft.AspNetCore.Mvc;
using ece4180.gpstracker.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
namespace ece4180.gpstracker.controllers{
    
    [Route("Archive")]
    public class ArchiveController: Controller{
        private readonly TripAccessor tripaccessor_;
        public ArchiveController(TripAccessor ta){
            tripaccessor_ = ta;
        }
        [HttpGet("DownloadTrip/{tripId}")]
        public async Task<JsonResult> DownloadTrip(int tripId){
            Console.WriteLine("Archive");
            List<Location> locs = await tripaccessor_.GetTripLocations(tripId, TRIPSTATUS.TERMINATED);
            if(locs == null){
                HttpContext.Response.StatusCode = 404;
                return Json(null);
            }
            string result = "";
            foreach(Location loc in locs){
                result += $"{loc.timeStamp} {loc.lat_} {loc.long_}\n";
            }
            return Json(locs);
        }
    } 
}