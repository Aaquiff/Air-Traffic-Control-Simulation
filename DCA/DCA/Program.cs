using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATCDatabase;
using System.ServiceModel;

namespace DCA
{
    
    class Program
    {
        static void Main(string[] args)
        {
            MasterControllerImpl masterController = new MasterControllerImpl();
            ServiceHost host = CreateHost(masterController);

            host.Open();
            Console.WriteLine("Press Enter to Exit");
            Console.ReadLine();
            host.Close();
        }

        //Creates the server host
        private static ServiceHost CreateHost(MasterControllerImpl masterController)
        {
            ServiceHost host;
            NetTcpBinding tcpBinding = new NetTcpBinding();

            tcpBinding.MaxReceivedMessageSize = System.Int32.MaxValue;
            tcpBinding.ReaderQuotas.MaxArrayLength = System.Int32.MaxValue;

            host = new ServiceHost(masterController);
            host.AddServiceEndpoint(typeof(IMasterController), tcpBinding, "net.tcp://localhost:50002/DCAMaster");
            return host;
        }
    }
}
