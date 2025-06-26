using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudServer.Common.Dto.UserDto
{
    public class UserInputDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }

        public UserInputDto()
        {
            Username = string.Empty;
            PhoneNumber = string.Empty;
            Address = string.Empty;
        }
    }
}
