using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AllInOne.Common.Smtp
{
    public interface ISmtpService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}