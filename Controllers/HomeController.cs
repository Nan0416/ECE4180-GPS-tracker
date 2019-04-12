
using Microsoft.AspNetCore.Mvc;
namespace ece4180.gpstracker.controllers{
    public class HomeController: Controller{
        public HomeController(){

        }
        public IActionResult Index(){
            return View();
        }
        public void StartNewTrip(int deviceID){
            // create a new running trip
        }
        public void EndTrip(int tripId){
            
        }
        public void ChangeStatus(int tripId, int status){
            // status =1 terminated
        }
        public void GetStatus(int tripId){
            
        }
    }
}