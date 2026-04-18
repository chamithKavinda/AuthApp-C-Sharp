using System.ComponentModel.DataAnnotations;

namespace AuthApp.Models
{
    public class SignInViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}