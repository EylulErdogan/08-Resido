using Dapper;
using Microsoft.AspNetCore.Mvc;
using MyDpprProject.Filters;
using MyDpprProject.Models;

namespace MyDpprProject.Controllers
{
    [LoginAuthorize]
    public class PropertyController : Controller
    {
        public IActionResult Index()
        {
            var values = Context.List<Property>("PropertyViewAll").ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult AddProperty()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddProperty(Property property)
        {
            DynamicParameters param = new DynamicParameters();

            param.Add("@Title", property.Title);
            param.Add("@Description", property.Description);
            param.Add("@Price", property.Price);
            param.Add("@City", property.City);
            param.Add("@District", property.District);
            param.Add("@Address", property.Address);
            param.Add("@BedCount", property.BedCount);
            param.Add("@BathCount", property.BathCount);
            param.Add("@SquareMeter", property.SquareMeter);
            param.Add("@ImageUrl", property.ImageUrl);
            param.Add("@PropertyTypeId", property.PropertyTypeId);

            Context.ExecuteReturn("PropertyInsert", param);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult UpdateProperty(int id)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@PropertyId", id);

            var value = Context.List<Property>("PropertyViewById", param).FirstOrDefault();

            return View(value);
        }

        [HttpPost]
        public IActionResult UpdateProperty(Property property)
        {
            DynamicParameters param = new DynamicParameters();

            param.Add("@PropertyId", property.PropertyId);
            param.Add("@Title", property.Title);
            param.Add("@Description", property.Description);
            param.Add("@Price", property.Price);
            param.Add("@City", property.City);
            param.Add("@District", property.District);
            param.Add("@Address", property.Address);
            param.Add("@BedCount", property.BedCount);
            param.Add("@BathCount", property.BathCount);
            param.Add("@SquareMeter", property.SquareMeter);
            param.Add("@ImageUrl", property.ImageUrl);
            param.Add("@PropertyTypeId", property.PropertyTypeId);

            Context.ExecuteReturn("PropertyUpdate", param);

            return RedirectToAction("Index");
        }
        public IActionResult PropertyViewById(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@PropertyId", id);

            var value = Context.List<Property>("PropertyViewById", parameters)
                               .FirstOrDefault();

            if (value == null)
            {
                return NotFound();
            }

            return View(value);
        }

        public IActionResult DeleteProperty(int id)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@PropertyId", id);

            Context.ExecuteReturn("PropertyDelete", param);

            return RedirectToAction("Index");
        }
    }
}