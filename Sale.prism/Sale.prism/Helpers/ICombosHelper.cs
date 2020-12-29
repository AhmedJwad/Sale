using Sale.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sale.prism.Helpers
{
   public  interface ICombosHelper
    {
        IEnumerable<PaymentMethod> GetPaymentMethods();

    }
}
