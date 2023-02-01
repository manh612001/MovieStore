using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MovieStoreMvc.Models.Domain
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
    }
}
