using System.ComponentModel.DataAnnotations;

namespace Sale.web.Models
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
