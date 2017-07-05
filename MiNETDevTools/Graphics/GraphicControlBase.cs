using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiNETDevTools.Graphics.Internal;
using SharpDX;

namespace MiNETDevTools.Graphics
{
    [System.ComponentModel.DesignerCategory("")]
    public abstract class GraphicControlBase
    {
        protected GraphicsDevice Graphics { get; }

        private readonly object _threadSync = new object();
        private Thread _thread;

        protected GraphicControlBase(IGraphicControl control)
        {
            //Graphics = new GraphicsDevice(control);
        }

        #region Thread

        public void Start()
        {

        }

        private void RunInternal()
        {

        }

        public void Stop()
        {

        }

        #endregion
    }
}
