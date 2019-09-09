using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_NetCore.Interfaces
{
    public interface IEmailSender
    {
        string SendEmail(string to, string subject);
    }
}
