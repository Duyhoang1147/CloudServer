using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudServer.Common.Dto.UserDto
{
    public class UserClaimsDto
    {
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        public UserClaimsDto()
        {
            ClaimType = string.Empty;
            ClaimValue = string.Empty;
        }
    }
}
