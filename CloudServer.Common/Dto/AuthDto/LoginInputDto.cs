using System.ComponentModel.DataAnnotations;

namespace CloudServer.Common.Dto.AuthDto
{
    public class LoginInputDto
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        public LoginInputDto()
        {
            Username = string.Empty;
            Password = string.Empty;
            RememberMe = false;
        }
    }
}
