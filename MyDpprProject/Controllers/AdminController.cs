using Microsoft.AspNetCore.Mvc;
using MyDpprProject.Filters;

namespace MyDpprProject.Controllers
{
    [AdminAuthorize]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
