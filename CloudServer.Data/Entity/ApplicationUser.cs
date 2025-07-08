using Microsoft.AspNetCore.Identity;

namespace CloudServer.Data.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<UserFile> Files { get; set; } = new List<UserFile>();
    }
}
