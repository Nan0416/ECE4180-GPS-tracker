
using Microsoft.AspNetCore.Mvc;
namespace ece4180.gpstracker.controllers{
    [Route("Home")]
    public class HomeController: Controller{
        public HomeController(){

        }
        [Route("")]
        [Route("/")]
        [Route("Index")]
        public IActionResult Index(){
            return View();
        }
        [Route("StartNewTrip/{deviceId}")]
        public string StartNewTrip(int deviceID){
            // create a new running trip
            return "19";
        }
        [Route("EndTrip/{tripId}")]
        public string EndTrip(int tripId){
            return "OK";
        }
        private bool ChangeStatus(int tripId, int status){
            return false;
            // status =1 terminated
        }
        [Route("GetStauts/{tripId}")]
        public string GetStatus(int tripId){
            return "inprogress";
        }
    }
}