using System.ComponentModel.DataAnnotations;

namespace Sale.Common.Entities
{
    public class Country
    {
        public int Id { get; set; }

        [MaxLength(50 , ErrorMessage ="the field {0} must be containe less tha {1}")]
        [Required]
        public string Name { get; set; }

    }
}
