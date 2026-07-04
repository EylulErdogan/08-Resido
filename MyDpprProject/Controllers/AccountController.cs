using Dapper;
using Microsoft.AspNetCore.Mvc;
using MyDpprProject.Models;

namespace MyDpprProject.Controllers
{
    public class AccountController : Controller
    {
        [HttpPost]
        public IActionResult Login(User user)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Email", user.Email);
            parameters.Add("@Password", user.Password);

            var result = Context.List<User>("UserLogin", parameters).FirstOrDefault();

            if (result != null)
            {
                HttpContext.Session.SetInt32("UserId", result.UserId);
                HttpContext.Session.SetString("FullName", result.FullName);
                HttpContext.Session.SetString("Email", result.Email);
                HttpContext.Session.SetInt32("RoleId", result.RoleId);

                if (result.RoleId == 1)
                {
                    return Json(new { success = true, redirectUrl = "/Admin" });
                }

                return Json(new { success = true, redirectUrl = "/" });
            }

            return Json(new { success = false, message = "Email veya şifre hatalı." });
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@FullName", user.FullName);
            parameters.Add("@Email", user.Email);
            parameters.Add("@Password", user.Password);
            parameters.Add("@RoleId", 2);

            var result = Context.List<int>("UserRegister", parameters).FirstOrDefault();

            if (result == 1)
            {
                TempData["Success"] = "Kayıt başarılı. Giriş yapabilirsiniz.";
            }
            else
            {
                TempData["RegisterError"] = "Bu email zaten kayıtlı.";
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult ForgotPassword(string email, string newPassword)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Email", email);
            parameters.Add("@NewPassword", newPassword);

            var result = Context.List<int>("UserForgotPassword", parameters).FirstOrDefault();

            if (result == 1)
            {
                TempData["Success"] = "Şifreniz başarıyla güncellendi.";
            }
            else
            {
                TempData["ForgotPasswordError"] = "Bu email adresi bulunamadı.";
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}