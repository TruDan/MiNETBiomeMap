using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiNETDevTools.Graphics
{
    public class GraphicsWindow
    {
        public IntPtr Handle { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public GraphicsWindow(IWin32Window window, int width, int height) : this(window.Handle, 0, 0, width, height)
        {
        }

        public GraphicsWindow(Control form) : this(form.Handle, form.Left, form.Top, form.Width, form.Height)
        {
        }

        public GraphicsWindow(IntPtr handle, int x, int y, int width, int height)
        {
            Handle = handle;
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

    }
}
