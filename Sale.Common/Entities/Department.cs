﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sale.Common.Entities
{
   public class Department
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        [Display(Name = "Department")]
        public string Name { get; set; }

        public ICollection<City> Cities { get; set; }

        [DisplayName("Cities Number")]
        public int CitiesNumber => Cities == null ? 0 : Cities.Count;

        [JsonIgnore]
        [NotMapped]
        public int IdCountry { get; set; }

        [JsonIgnore]
        public Country Country { get; set; }


    }
}
