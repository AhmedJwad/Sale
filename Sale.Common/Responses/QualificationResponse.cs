using System;
using System.Collections.Generic;
using System.Text;

namespace Sale.Common.Responses
{
   public class QualificationResponse
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public float Score { get; set; }
        public string Remarks { get; set; }
    }
}
