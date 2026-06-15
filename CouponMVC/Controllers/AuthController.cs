using CouponMVC.Models;
using CouponMVC.Service.IService;
using CouponMVC.Utility;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
//using static System.Net.Mime.MediaTypeNames;

namespace CouponMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();

            return View(loginRequestDto);
        }
        [HttpGet]
        public IActionResult Register()
        {
            var rolelist = new List<SelectListItem>()
            {
                new SelectListItem{Text=SD.RoleAdmin,Value=SD.RoleAdmin},
                new SelectListItem{Text=SD.RoleCustomer,Value=SD.RoleCustomer},

            };
            ViewBag.RoleList = rolelist;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto obj)
        {
            ResponseDto result = await _authService.RegisterAsync(obj);
            ResponseDto assignrole;
            if (result != null && result.IsSuccess)
            {
                if (string.IsNullOrEmpty(obj.RoleName))
                {
                    obj.RoleName = SD.RoleCustomer;
                }
                assignrole = await _authService.AssignRoleAsync(obj);
                if (assignrole != null && assignrole.IsSuccess)
                {
                    TempData["success"] = "Registration Successfully!!";
                    return RedirectToAction(nameof(Login));
                }
            }

            var rolelist = new List<SelectListItem>()
            {
                new SelectListItem{Text=SD.RoleAdmin,Value=SD.RoleAdmin},
                new SelectListItem{Text=SD.RoleCustomer,Value=SD.RoleCustomer},

            };
            ViewBag.RoleList = rolelist;
            return View();
        }
        public IActionResult Logout()
        {
            return View();
        }
    }
}
