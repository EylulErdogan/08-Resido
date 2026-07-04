using Dapper;
using Microsoft.AspNetCore.Mvc;
using MyDpprProject.Models;

namespace MyDpprProject.Controllers
{
    public class PropertyTypeController : Controller
    {
        public IActionResult Index()
        {
            var values = Context.List<PropertyType>("PropertyTypeViewAll").ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult AddPropertyType()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddPropertyType(PropertyType propertyType)
        {
            DynamicParameters param = new DynamicParameters();

            param.Add("@TypeName", propertyType.TypeName);

            Context.ExecuteReturn("PropertyTypeInsert", param);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult UpdatePropertyType(int id)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@PropertyTypeId", id);

            var value = Context.List<PropertyType>("PropertyTypeViewById", param).FirstOrDefault();

            return View(value);
        }

        [HttpPost]
        public IActionResult UpdatePropertyType(PropertyType propertyType)
        {
            DynamicParameters param = new DynamicParameters();

            param.Add("@PropertyTypeId", propertyType.PropertyTypeId);
            param.Add("@TypeName", propertyType.TypeName);

            Context.ExecuteReturn("PropertyTypeUpdate", param);

            return RedirectToAction("Index");
        }

        public IActionResult DeletePropertyType(int id)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@PropertyTypeId", id);

            Context.ExecuteReturn("PropertyTypeDelete", param);

            return RedirectToAction("Index");
        }
    }
}