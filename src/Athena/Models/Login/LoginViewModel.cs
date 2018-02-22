using System.ComponentModel.DataAnnotations;

namespace Athena.Models.Login
{
    public class LoginViewModel
    {
        public string ReturnUrl { get; set; }
        
        [Required]
        [EmailAddress]
        public string Provider { get; set; }
    }
}