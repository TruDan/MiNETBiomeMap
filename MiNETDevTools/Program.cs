using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using log4net.Config;
using MiNET;
using MiNETDevTools.UI;
using MiNETDevTools.UI.Forms;

// Configure log4net using the .config file
[assembly: XmlConfigurator(Watch = true)]

namespace MiNETDevTools
{
    public class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        
        [STAThread]
        public static void Main(string[] args)
        {
            Log.InfoFormat("Starting...");
            
            //var s = new MiNetServer(new IPEndPoint(IPAddress.Any, 19132));
            var uiManager = new UiManager();
            uiManager.Start();

            Console.ReadLine();

            uiManager.Stop();
        }

    }
}
