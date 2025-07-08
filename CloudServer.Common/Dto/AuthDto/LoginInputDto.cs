using System.ComponentModel.DataAnnotations;

namespace CloudServer.Common.Dto.AuthDto
{
    public class LoginInputDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        public LoginInputDto()
        {
            Email = string.Empty;
            Password = string.Empty;
            RememberMe = false;
        }
    }
}
