using Microsoft.AspNetCore.Mvc;
using MovieStoreMvc.Models.DTO;
using MovieStoreMvc.Repositories.Interface;

namespace MovieStoreMvc.Controllers
{
    public class UserAuthenticationController : Controller
    {
        private IUserAuthenticationServices authService;
        public UserAuthenticationController(IUserAuthenticationServices authService)
        {
            this.authService = authService;
        }

        public async Task<IActionResult> Register()
        {
            var model = new Registration
            {
                Email = "admin@gmail.com",
                Username = "admin",
                Name = "Manh",
                Password = "Admin@123",
                PasswordConfirm = "Admin@123",
                Role = "Admin"
            };
            var result = await authService.RegisterAsync(model);
            return Ok(result.Message);
        }
        public  IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await authService.LoginAsync(model);
            if(result.StatusCode == 1)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["msg"] = "Đăng nhập thất bại!";
                return RedirectToAction(nameof(Login));
            }
            
        }
        public async Task<IActionResult> LogOut()
        {
            await authService.LogoutAsync();
            return RedirectToAction("Login");
        }
    }
}
