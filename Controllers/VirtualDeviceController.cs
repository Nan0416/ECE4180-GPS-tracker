using Microsoft.AspNetCore.Mvc;
namespace ece4180.gpstracker.controllers{

    [Route("VirtualDevice")]
    public class VirtualDeviceController: Controller{
        public VirtualDeviceController(){

        }
        [Route("")]
        [Route("/Index")]
        public IActionResult Index(){
            return View();
        }
    }
}