
using Microsoft.AspNetCore.Mvc;
namespace ece4180.gpstracker.controllers{
    public class HomeController: Controller{
        public HomeController(){

        }
        public IActionResult Index(){
            return View();
        }
    }
}