using CloudServer.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace CloudServer.Data.Connection
{
    public class IdentityAppDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public IdentityAppDbContext(DbContextOptions<IdentityAppDbContext> options) : base(options) { }

        public DbSet<UserFile> UserFiles { get; set; }
        public DbSet<UserFolder> UserFolders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Ngăn cascade delete giữa UserFile và Folder
            builder.Entity<UserFile>()
                .HasOne(f => f.Folder)
                .WithMany(folder => folder.Files)
                .HasForeignKey(f => f.FolderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Nếu bạn muốn ngăn luôn cả quan hệ với User:
            builder.Entity<UserFile>()
                .HasOne(f => f.Users)
                .WithMany(user => user.Files)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Quan hệ tự tham chiếu trong Folder cũng nên cẩn thận
            builder.Entity<UserFolder>()
                .HasOne(f => f.ParentFolder)
                .WithMany(f => f.SubFolders)
                .HasForeignKey(f => f.ParentFolderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
