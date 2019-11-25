using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkUtil;

namespace Server
{
    public class Server
    {
        

        static void Main(string[] args)
        {
            Networking.StartServer(ReceivePlayerName, 11000);
        }


        private static void ReceivePlayerName(SocketState ss)
        {
            ss.OnNetworkAction = SendStartupInfo;
            Networking.GetData(ss);
        }


        private static void SendStartupInfo(SocketState ss)
        {
            ss.GetData()
            throw new NotImplementedException();
        }
    }
}
