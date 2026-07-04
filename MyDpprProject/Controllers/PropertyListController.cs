using Microsoft.AspNetCore.Mvc;
using MyDpprProject.Models;

namespace MyDpprProject.Controllers
{
    public class PropertyListController : Controller
    {
        public IActionResult Index()
        {
            var values = Context.List<Property>("PropertyViewAll").ToList();
            return View(values);
        }
    }
}
