using Dapper;
using Microsoft.AspNetCore.Mvc;
using MyDpprProject.Models;

namespace MyDpprProject.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            var values = Context.List<User>("UserViewAll").ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddUser(User user)
        {
            DynamicParameters param = new DynamicParameters();
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            param.Add("@FullName", user.FullName);
            param.Add("@Email", user.Email);
            param.Add("@Password", user.Password);
            param.Add("@RoleId", user.RoleId);

            Context.ExecuteReturn("UserInsert", param);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult UpdateUser(int id)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@UserId", id);

            var value = Context.List<User>("UserViewById", param).FirstOrDefault();

            return View(value);
        }

        [HttpPost]
        public IActionResult UpdateUser(User user)
        {
            DynamicParameters param = new DynamicParameters();
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            param.Add("@UserId", user.UserId);
            param.Add("@FullName", user.FullName);
            param.Add("@Email", user.Email);
            param.Add("@Password", user.Password);
            param.Add("@RoleId", user.RoleId);

            Context.ExecuteReturn("UserUpdate", param);

            return RedirectToAction("Index");
        }

        public IActionResult DeleteUser(int id)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@UserId", id);

            Context.ExecuteReturn("UserDelete", param);

            return RedirectToAction("Index");
        }
    }
}