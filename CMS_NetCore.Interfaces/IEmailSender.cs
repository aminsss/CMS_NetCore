namespace CMS_NetCore.Interfaces;

public interface IEmailSender
{
    string SendEmail(string to, string subject);
}