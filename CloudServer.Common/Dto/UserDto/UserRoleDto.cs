using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudServer.Common.Dto.UserDto
{
    public class UserRoleDto
    {
        public string RoleName { get; set; }
        public UserRoleDto()
        {
            RoleName = string.Empty;
        }
    }
}
