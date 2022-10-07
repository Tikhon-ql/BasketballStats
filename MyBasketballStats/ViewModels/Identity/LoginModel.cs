using System.ComponentModel.DataAnnotations;

namespace MyBasketballStats.ViewModels.Identity
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;
        
        public string ReturnUrl { get; set; }
    }
}
