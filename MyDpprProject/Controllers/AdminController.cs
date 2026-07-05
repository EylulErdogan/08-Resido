using Microsoft.AspNetCore.Mvc;
using MyDpprProject.Filters;
using MyDpprProject.Models;

namespace MyDpprProject.Controllers
{
    [AdminAuthorize]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            var properties = Context.List<Property>("PropertyViewAll").ToList();
            var users = Context.List<User>("UserViewAll").ToList();

            ViewBag.PropertyCount = properties.Count;
            ViewBag.SaleCount = properties.Count(x => x.PropertyTypeId == 1);
            ViewBag.RentCount = properties.Count(x => x.PropertyTypeId == 2);
            ViewBag.UserCount = users.Count;

            ViewBag.LastProperties = properties
                .OrderByDescending(x => x.PropertyId)
                .Take(5)
                .ToList();

            return View();
        }
    }
}
