using System.Net.Mail;

namespace Services.Mail
{
    public interface IMailingService
    {
       // void SendMail(MailMessage msg);
        //void CreateInstance(string host, int port, System.Net.NetworkCredential networkCredential, ILogService logService);
        MailingService Instance { get; }
    }
}