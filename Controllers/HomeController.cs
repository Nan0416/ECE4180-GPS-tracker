
using Microsoft.AspNetCore.Mvc;
using ece4180.gpstracker.Models;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
namespace ece4180.gpstracker.controllers{
    [Route("Home")]
    public class HomeController: Controller{
        private readonly TripAccessor tripaccessor_;
        public HomeController(TripAccessor ta){
            tripaccessor_ = ta;
        }
        [Route("")]
        [Route("/")]
        [Route("Index")]
        public IActionResult Index(){
            return View();
        }
        [Route("StartNewTrip/{deviceID:int}")]
        public async Task<string> StartNewTrip(int deviceID){
            // Console.WriteLine("====> DeviceId:" + deviceID);
            // create a new running trip
            int id = await tripaccessor_.CreateTrip(deviceID);
            //int id = await tripaccessor_.NumberOfTrips();
            return id.ToString();
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
        [Route("GetStatus")]
        public string GetStatus(){
            //List<Trip> trip = tripcontext_.Trips.();
            //return ModelPrinter.Print()
            return "";
        }
    }
}