using System.Collections.Generic;
using System.Net;
using Ninject.Modules;
using Services.Mail;

namespace Services.Modules
{
    public class MailingModule : NinjectModule
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _userName;
        private readonly string _password;
        private readonly NetworkCredential _networkCredential;

        public MailingModule(string host,int port,string userName, string password)
        {
            _host = host;
            _port = port;
            _userName = userName;
            _password = password;

            _networkCredential = new NetworkCredential(userName,password);
        }

        public override void Load()
        {
            Bind<IMailingService>().To<MailingService>().InSingletonScope()
                .WithConstructorArgument("host",_host)
                .WithConstructorArgument("port",_port)
                .WithConstructorArgument("networkCredential", _networkCredential);



            var modules = new List<INinjectModule>
            {
                new LogModule(),
            };

            Kernel.Load(modules);
        }
    }
}
