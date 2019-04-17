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
        [Route("DownloadTrip/{tripId}")]
        public async Task<string> DownloadTrip(int tripId){
            List<Location> locs = await tripaccessor_.GetTripLocations(tripId);
            string result = "";
            if(locs == null) {return "Invalid"; }
            foreach(Location loc in locs){
                result += $"{loc.timeStamp} {loc.lat_} {loc.long_}\n";
            }
            return result;
        }
    } 
}