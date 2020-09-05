using Congregation.Common.Responses;

namespace Congregation.Web.Helpers
{
    public interface IMailHelper
    {
        Response SendMail(string to, string subject, string body);
    }

}
