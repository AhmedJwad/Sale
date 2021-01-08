using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sale.Common.Entities
{
   public class City
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        [Display(Name = "City")]
        public string Name { get; set; }
        [JsonIgnore]
        [NotMapped]
        public int IdDepartment { get; set; }
        [JsonIgnore]
        public Department Department { get; set; }

    }
}
