using Microsoft.AspNetCore.Identity;
using MovieStoreMvc.Models.Domain;
using MovieStoreMvc.Models.DTO;
using MovieStoreMvc.Repositories.Interface;
using System.Security.Claims;

namespace MovieStoreMvc.Repositories.Implement
{
    public class UserAuthenticationServices : IUserAuthenticationServices
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public UserAuthenticationServices(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }
        
        
        public async Task<Status> LoginAsync(Login model)
        {
            var status = new Status();
            var user = await userManager.FindByNameAsync(model.UserName);
            if(user == null)
            {
                status.StatusCode = 0;
                status.Message = "Tên sử dụng không hợp lệ";
                return status;
            }
            if(!await userManager.CheckPasswordAsync(user,model.Password))
            {
                status.StatusCode = 0;
                status.Message = "Mật khẩu không hợp lệ";
                return status;
            }
            var signInResult = await signInManager.PasswordSignInAsync(user, model.Password, true, true);
            if(signInResult.Succeeded)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.UserName),
                };
                foreach(var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                status.StatusCode = 1;
                status.Message = "Đăng nhập thành công";
            }    
            else if(signInResult.IsLockedOut)
            {
                status.StatusCode = 0;
                status.Message = "Người dùng đã đăng xuất";
            }
            else
            {
                status.StatusCode = 0;
                status.Message = "Lỗi đăng nhập";
            }
            return status;
        }

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<Status> RegisterAsync(Registration model)
        {
            var status = new Status();
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                status.StatusCode = 0;
                status.Message = "Người dùng đã tồn tại";
                return status;
            }
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Name = model.Name,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "Tạo người dùng thất bại";
                return status;
            }

            if (!await roleManager.RoleExistsAsync(model.Role))
                await roleManager.CreateAsync(new IdentityRole(model.Role));


            if (await roleManager.RoleExistsAsync(model.Role))
            {
                await userManager.AddToRoleAsync(user, model.Role);
            }

            status.StatusCode = 1;
            status.Message = "Bạn đã tạo thành công";
            return status;
        }
    }
}
