using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MiNETDevToolsPlugin.Interfaces;
using ServiceWire.TcpIp;

namespace MiNETDevTools.Plugin
{
    public static class PluginClient
    {

        public static TcpClient<ILevelService> LevelService => new TcpClient<ILevelService>(new IPEndPoint(IPAddress.Loopback, 19137));

    }
}
