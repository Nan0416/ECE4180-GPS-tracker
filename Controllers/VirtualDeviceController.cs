using Microsoft.AspNetCore.Mvc;
namespace ece4180.gpstracker.controllers{
    public class VirtualDeviceController: Controller{
        public VirtualDeviceController(){

        }
        public IActionResult Index(){
            return View();
        }
    }
}