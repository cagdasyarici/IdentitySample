using IdentitySample.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentitySample.Models.Context
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        
        public AppDbContext(DbContextOptions<AppDbContext> dbContext) : base(dbContext)
        {

        }
    }
}

