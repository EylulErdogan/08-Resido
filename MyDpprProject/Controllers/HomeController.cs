using Microsoft.AspNetCore.Mvc;
using MyDpprProject.Models;
using System.Diagnostics;

namespace MyDpprProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var values = Context.List<Property>("PropertyViewAll").ToList();
            return View(values);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
