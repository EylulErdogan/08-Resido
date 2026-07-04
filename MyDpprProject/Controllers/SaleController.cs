using Dapper;
using Microsoft.AspNetCore.Mvc;
using MyDpprProject.Filters;
using MyDpprProject.Models;

namespace MyDpprProject.Controllers
{
    [LoginAuthorize]
    public class SaleController : Controller
    {
        public IActionResult Index()
        {
            var values = Context.List<Sale>("SaleViewAll").ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult AddSale()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddSale(Sale sale)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                TempData["OpenLoginModal"] = true;
                TempData["LoginMessage"] = "Rezervasyon yapmak için giriş yapmalısınız.";

                return RedirectToAction("PropertyViewById", "Property", new { id = sale.PropertyId });
            }

            sale.UserId = userId.Value;
            sale.SaleDate = DateTime.Now;

            DynamicParameters param = new DynamicParameters();

            param.Add("@PropertyId", sale.PropertyId);
            param.Add("@UserId", sale.UserId);
            param.Add("@SaleDate", sale.SaleDate);
            param.Add("@TotalPrice", sale.TotalPrice);

            Context.ExecuteReturn("SaleInsert", param);

            TempData["Success"] = "Rezerve edildi.";

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult UpdateSale(int id)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@SaleId", id);

            var value = Context.List<Sale>("SaleViewById", param).FirstOrDefault();

            return View(value);
        }

        [HttpPost]
        public IActionResult UpdateSale(Sale sale)
        {
            DynamicParameters param = new DynamicParameters();

            param.Add("@SaleId", sale.SaleId);
            param.Add("@PropertyId", sale.PropertyId);
            param.Add("@UserId", sale.UserId);
            param.Add("@SaleDate", sale.SaleDate);
            param.Add("@TotalPrice", sale.TotalPrice);

            Context.ExecuteReturn("SaleUpdate", param);

            return RedirectToAction("Index");
        }

        public IActionResult DeleteSale(int id)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@SaleId", id);

            Context.ExecuteReturn("SaleDelete", param);

            return RedirectToAction("Index");
        }
    }
}