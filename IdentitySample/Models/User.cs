using System.ComponentModel.DataAnnotations;

namespace IdentitySample.Models
{
    public class User
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
