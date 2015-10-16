using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace FitbitProxyASPNetCoreFxTest.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var httpClient = new HttpClient();
            try
            {
                //the line below will run fine the first time and all subsequent times until the hour rolls over

                //clear the SSL cache so we don't choke the next minute
                if (DateTime.Now.Minute == 0)
                {
                    Console.WriteLine("Cleared SSL Cache");
                }
                var result = await httpClient.GetStringAsync("https://www.fitbit.com"); //fitbit.com behind CloudFlare Proxy
                Console.WriteLine(DateTime.UtcNow.ToString() + " Fitbit.com Content-Length:" + result.Length);

                ViewBag.Result = result;

            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex;
                //at xx:00 when the hour rolls over the TLS exception will occur here. Like clockwork.
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
