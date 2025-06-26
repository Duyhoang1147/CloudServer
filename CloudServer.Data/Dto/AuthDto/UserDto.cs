using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudServer.Data.Dto.AuthDto
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public ICollection<string> Roles { get; set; } = new List<string>();
        public ICollection<string> Permissions { get; set; } = new List<string>();
        public UserDto()
        {
            Id = string.Empty;
            Username = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            Address = string.Empty;
        }
    }
}
