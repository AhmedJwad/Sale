using Sale.Common.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sale.Common.Models
{
   public class OrderDetail
    {
        public int Id { get; set; }
        public ProductResponse Product { get; set; }
        public float Quantity { get; set; }
        public string Remarks { get; set; }
        public decimal? Value => (decimal)Quantity * Product?.Price;

    }
}
