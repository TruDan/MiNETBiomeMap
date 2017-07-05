using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using log4net.Config;
using MiNET;

// Configure log4net using the .config file
[assembly: XmlConfigurator(Watch = true)]
namespace DevToolsDebug
{
    static class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            Log.InfoFormat("Starting UI");
        }
    }
}
