using CloudServer.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace CloudServer.Data.Connection
{
    public class IdentityAppDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public IdentityAppDbContext(DbContextOptions<IdentityAppDbContext> options) : base(options)
        {
        }
    }
}
