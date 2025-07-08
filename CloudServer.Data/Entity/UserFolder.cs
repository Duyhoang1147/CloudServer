using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudServer.Data.Entity
{
    public class UserFolder
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(1024)]
        public string Path { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }


        // Navigation properties
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        public Guid? ParentFolderId { get; set; }

        [ForeignKey("ParentFolderId")]
        public UserFolder? ParentFolder { get; set; }

        public ICollection<UserFolder> SubFolders { get; set; } = new List<UserFolder>();
        public ICollection<UserFile> Files { get; set; } = new List<UserFile>();

        public UserFolder()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Path = string.Empty;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
