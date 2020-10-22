
using System.ComponentModel.DataAnnotations;
namespace Sale.Common.Request
{
    public class emailrequest
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
