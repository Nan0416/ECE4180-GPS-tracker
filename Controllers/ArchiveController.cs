using Microsoft.AspNetCore.Mvc;

namespace ece4180.gpstracker.controllers{
    [Route("Archive")]
    public class ArchiveController: Controller{
        public ArchiveController(){

        }
        [Route("DownloadTrip/{tripId}")]
        public string DownloadTrip(int tripId){
            return "OK";
        }
    } 
}