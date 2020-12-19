using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Sale.prism.Helpers
{
    public class RegexHelper : IRegexHelper
    {
        public bool IsValidEmail(string emailaddress)
        {
            try
            {
                new MailAddress(emailaddress);
                return true;

            }
            catch (FormatException)
            {

                return false;
            }
        }
    }
}
