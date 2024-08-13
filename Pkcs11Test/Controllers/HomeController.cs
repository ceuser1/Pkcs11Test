using Microsoft.AspNetCore.Mvc;
using Pkcs11Test.CustomClasses;
using Pkcs11Test.Models;
using System.Diagnostics;

namespace Pkcs11Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        

        public IActionResult Index()
        {
            //using var hsm = new HsmAdapter();
            //hsm.Login();
            //var keys = hsm.GenerateKeyPair();
            //Debug.WriteLine("Private key object handle: " + keys.Item2);

            return View();
        }

        public IActionResult Privacy()
        {
            //using var hsm = new HsmAdapter();
            //hsm.Login();
            //var keys = hsm.GenerateKeyPair();
            //Debug.WriteLine("Private key object handle: " + keys.Item2);

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
