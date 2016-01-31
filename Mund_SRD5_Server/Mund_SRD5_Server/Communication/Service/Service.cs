using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Description;

namespace Mundasia.Server.Communication
{
    public class Service
    {
        private static WebServiceHost host;
        private static string uri;
        private static ServiceHost basicHost;

        public static void Open()
        {
            uri = "http://192.168.1.2:6300/MundasiaServerService/";
            Uri baseAddress = new Uri(uri);
            host = new WebServiceHost(typeof(ServerService), baseAddress);
            WebHttpBinding binding = new WebHttpBinding();
            binding.MaxReceivedMessageSize = int.MaxValue;
            ServiceEndpoint endpoint = host.AddServiceEndpoint(typeof(IServerService), binding, "");
            host.Open();

            basicHost = new ServiceHost(typeof(ServerService), new Uri(uri + "basic/"));
            BasicHttpBinding basicBinding = new BasicHttpBinding();
            basicBinding.MaxReceivedMessageSize = int.MaxValue;
            ServiceEndpoint basicEndpoint = basicHost.AddServiceEndpoint(typeof(IServerService), basicBinding, "");
            basicHost.Open();
        }

        public static void Close()
        {
            basicHost.Close();
            host.Close();
        }
    }
}
