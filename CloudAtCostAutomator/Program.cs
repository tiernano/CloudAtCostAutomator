using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudAtCostAutomator
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Count() != 2)
            {
                Console.WriteLine("Need 2 arguments: email address and key");
                return;
            }

            var cacClient = new CloudAtCostAPI.CloudClient();
            cacClient.Key = args[1];
            cacClient.Login = args[0];

            var servers = cacClient.ListServers().Result;

            foreach(var x in servers)
            {
                try
                {
                    Console.WriteLine("IP: {0} - HostName: {1} - RootPass: {2}", x.ip, x.hostname, x.rootpass);

                    var passwordAuth = new PasswordAuthenticationMethod("root", x.rootpass);
                    var machine = new SshClient(new ConnectionInfo(x.ip, "root", passwordAuth));

                    machine.Connect();

                    Console.WriteLine("{0} - {1}", x.ip, machine.RunCommand("uptime"));

                    machine.Disconnect();
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Error connecting: {0}", ex.Message);
                }

            }

            Console.ReadLine();
        }
    }
}
