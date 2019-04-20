
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
        [HttpGet("StartNewTrip/{deviceID:int}")]
        public async Task<string> StartNewTrip(int deviceID){
            // Console.WriteLine("====> DeviceId:" + deviceID);
            // create a new running trip
            int id = await tripaccessor_.CreateTrip(deviceID);
            if(id == -1){
                HttpContext.Response.StatusCode = 503;
            }
            //int id = await tripaccessor_.NumberOfTrips();
            return id.ToString();
        }
        [HttpGet("EndTrip/{tripId}")]
        public async Task<string> EndTrip(int tripId){
            int id = await tripaccessor_.TerminateTrip(tripId);
            if(id == -1){
                HttpContext.Response.StatusCode = 400;
            }
            return id.ToString();
        }
        
        [HttpGet("TripStatus/{tripId:int=-1}")]
        public async Task<JsonResult> TripStatus(int tripId){

            if(tripId != -1){
                Trip t = await tripaccessor_.GetTripStatus(tripId);
                if(t == null){
                    HttpContext.Response.StatusCode = 404;
                    Console.WriteLine("Not found");
                    return Json(new ErrorJSONResult("Not found"));
                }
                return Json(t);
            }else{
                List<Trip> trips = await tripaccessor_.GetTripStatus();
                if(trips == null || trips.Count == 0){
                    HttpContext.Response.StatusCode = 404;
                    return Json(new ErrorJSONResult("Not found"));
                }
                return Json(trips);
            }
        }
        
    }
}