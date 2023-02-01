using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MovieStoreMvc.Models.Domain
{
    public class DatabaseDbContext:IdentityDbContext<ApplicationUser>
    {
        public DatabaseDbContext(DbContextOptions<DatabaseDbContext> options):base(options)
        {

        }
    }
}
