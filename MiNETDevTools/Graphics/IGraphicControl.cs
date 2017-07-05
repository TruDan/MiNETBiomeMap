using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiNETDevTools.Graphics.Internal;
using SharpDX.Direct2D1;

namespace MiNETDevTools.Graphics
{
    public interface IGraphicControl : IGraphicComponent, IWin32Window
    {
        int Width { get; }
        int Height { get; }
    }
}
