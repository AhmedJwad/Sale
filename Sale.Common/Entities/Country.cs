using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sale.Common.Entities
{
    public class Country
    {
        public int Id { get; set; }

        [MaxLength(50 , ErrorMessage ="the field {0} must be containe less tha {1}")]
        [Required]
        [Display(Name = "Country")]
        public string Name { get; set; }

        public ICollection<Department>Departments { get; set; }

        [DisplayName("Departments Number")]
        public int DepartmentsNumber => Departments == null ? 0 : Departments.Count;

    }
}
