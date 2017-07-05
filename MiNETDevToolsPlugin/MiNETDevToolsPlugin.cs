using System;
using System.Net;
using log4net;
using log4net.Config;
using MiNET;
using MiNET.Plugins;
using MiNETDevToolsPlugin.Interfaces;
using MiNETDevToolsPlugin.Models;
using MiNETDevToolsPlugin.Services;
using ServiceWire;
using ServiceWire.TcpIp;
using ILog = log4net.ILog;

// Configure log4net using the .config file
[assembly: XmlConfigurator(Watch = true)]

namespace MiNETDevToolsPlugin
{
    [MiNET.Plugins.Attributes.Plugin(Author = "TruDan", Description = "Developer Tools for MiNET", PluginName = "DevTools", PluginVersion = "1.0")]
    public class MiNetDevToolsPlugin : Plugin, IStartup
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MiNetDevToolsPlugin));

        private ILevelService _levelService;

        private TcpHost _server;

        public MiNetDevToolsPlugin()
        {
        }

        protected override void OnEnable()
        {
           // _server.Start(IPAddress.Loopback, 19137);
            //_server.WaitForIsListening();

            //foreach (var m in _server.Methods)
            {
                //Log.InfoFormat("Exposed {0}", m.Key);
            }

            Log.InfoFormat("Dev Tools Started");
        }
        public override void OnDisable()
        {
            _server.Close();
            Log.InfoFormat("Dev Tools Stopped");
        }

        public void Configure(MiNetServer server)
        {
            var logger = new Logger(logLevel: LogLevel.Debug);
            var stats = new Stats();

            _levelService = new LevelService(server);

            _server = new TcpHost(new IPEndPoint(IPAddress.Any, 19137), logger, stats);
            _server.AddService<ILevelService>(_levelService);
            _server.Open();
            Log.InfoFormat("Dev Tools LevelService Started");
        }
    }
}
