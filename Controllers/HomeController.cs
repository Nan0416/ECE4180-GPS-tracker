
using Microsoft.AspNetCore.Mvc;
using ece4180.gpstracker.Models;
using System.Collections.Generic;
using System;
namespace ece4180.gpstracker.controllers{
    [Route("Home")]
    public class HomeController: Controller{
        private readonly TripContext tripcontext_;
        public HomeController(TripContext tc){
            tripcontext_ = tc;
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
            int id = 19;
            long startTime = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            Trip trip = new Trip{tripId = id, status = 1, startTime = startTime};
            tripcontext_.Trips.Add(trip);
            tripcontext_.SaveChanges();
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