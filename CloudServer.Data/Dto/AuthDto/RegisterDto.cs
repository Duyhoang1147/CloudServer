using System.ComponentModel.DataAnnotations;

namespace CloudServer.Data.Dto.AuthDto
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Username is require")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ComfirmPassword { get; set; }
        public RegisterDto()
        {
            Username = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            ComfirmPassword = string.Empty;
        }
    }
}
