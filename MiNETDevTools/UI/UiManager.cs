using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using MiNET;
using MiNETDevTools.Graphics;
using MiNETDevTools.Graphics.Biomes;

namespace MiNETDevTools.UI
{
    public class UiManager
    {
        public static UiManager Instance { get; private set; }

        private static readonly ILog Log = LogManager.GetLogger(typeof(UiManager));
        


        public UiContext Context { get; private set; }
        
        private readonly MiNetServer _server;
        private Thread _thread;

        public UiManager()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            Instance = this;
        }

        public void Start()
        {
            _thread = new Thread(StartInternal);
            _thread.SetApartmentState(ApartmentState.STA);
            _thread.Start();
        }

        
        public void StartInternal()
        {
            Application.CurrentCulture = Thread.CurrentThread.CurrentCulture;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += Application_ThreadException;


            Context = new UiContext();

            Application.Run(Context.MainForm);
        }

        public void Stop()
        {
            _thread?.Join();
            Context.MainForm.Close();
        }

        
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Log.Error("Unhandled exception during UI Thread", e.Exception);
        }
    }
}
