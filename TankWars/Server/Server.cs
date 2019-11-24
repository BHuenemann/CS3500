using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkUtil;

namespace Server
{
    class Server
    {
        static void Main(string[] args)
        {
            Networking.StartServer(SendStartupInfo, 11000);
        }


        private static void SendStartupInfo(SocketState obj)
        {
            throw new NotImplementedException();
        }
    }
}
