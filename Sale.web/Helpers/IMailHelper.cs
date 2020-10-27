using Sale.Common.Responses;

namespace Sale.web.Helpers
{
    public interface IMailHelper
    {
        Response SendMail(string to, string subject, string body);
    }

}

