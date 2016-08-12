using System;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using Services.Log;

namespace Services.Mail
{
    public class MailingService : IMailingService
    {
        private static MailingService _instance;
        private readonly SmtpClient _client;
        private readonly ILogService _logService;

        public MailingService Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new Exception("Instance was not created yet. Must call CreateInstance method first.");
                }

                return _instance;
            }
        }

        private MailingService(string host, int port, System.Net.NetworkCredential networkCredential)
        {
            _client = new SmtpClient(host, port) {Credentials = networkCredential};
        }

        /// <summary>
        /// Creating the instance of the class. if instance was already created, it will be overridden.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="networkCredential"></param>
        /// <param name="logService"></param>
        public MailingService(string host, int port, System.Net.NetworkCredential networkCredential,ILogService logService)
        {
            _instance = new MailingService(host, port, networkCredential);
            //_client = new SmtpClient(host, port);
            //_client.Credentials = networkCredential;
            _logService = logService;
        }

        public void SendMail(MailMessage msg)
        {
            try
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(SendAsync), msg);
            }
            catch (Exception ex)
            {
                _logService.Logger.Error("Failed to start async mail job", ex);
            }
        }

        private void SendAsync(object obj)
        {
            MailMessage msg = obj as MailMessage;
            if (msg != null)
            {
                try
                {
                    _client.Send((MailMessage)msg);
                }
                catch (Exception ex)
                {
                    var to = msg.To.Select(m => m.Address);
                    _logService.Logger.Error(string.Format("Failed to send mail, subject: \"{0}\", recipients: {1}", msg.Subject, string.Join(", ", to)), ex);
                    throw;
                }
            }
        }

        
    }

    
}
