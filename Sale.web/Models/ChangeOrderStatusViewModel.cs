﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sale.web.Models
{
    public class ChangeOrderStatusViewModel
    {
        public int Id { get; set; }
        public int OrderStatusId { get; set; }
        public IEnumerable<SelectListItem> OrderStatuses { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

    }
}
