using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudServer.Data.Entity
{
    public class UserFile
    {
        public Guid Id { get; set; }
        public string Filename { get; set; }
        public string StoredFileName { get; set; }
        public long Size { get; set; }
        public string ContentType { get; set; }
        public string StoragePath { get; set; }
        public DateTime UploadDate { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation properties
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? Users { get; set; }

        public Guid FolderId { get; set; }

        [ForeignKey("FolderId")]
        public UserFolder? Folder { get; set; }

        public UserFile()
        {
            Id = Guid.NewGuid();
            Filename = string.Empty;
            StoredFileName = string.Empty;
            Size = 0;
            ContentType = string.Empty;
            StoragePath = string.Empty;
            UploadDate = DateTime.UtcNow;
            IsDeleted = false;
        }
    }
}
