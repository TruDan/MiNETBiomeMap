using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MiNETDevTools.UI.Util
{
    public static class Native
    {

        [DllImport("user32.dll")]
        internal static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        ///The SetWindowLongPtr function changes an attribute of the specified window
        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        internal static extern int SetWindowLong32
            (HandleRef hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        internal static extern int SetWindowLong32
            (IntPtr windowHandle, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        internal static extern IntPtr SetWindowLongPtr64
            (IntPtr windowHandle, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        internal static extern IntPtr SetWindowLongPtr64
            (HandleRef hWnd, int nIndex, IntPtr dwNewLong);


        // Get a handle to an application window.
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        // Find window by Caption only. Note you must pass IntPtr.Zero as the first parameter.
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        internal static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        public static int SetWindowLong(IntPtr windowHandle, int nIndex, int dwNewLong)
        {
            if (IntPtr.Size == 8) //Check if this window is 64bit
            {
                return (int)SetWindowLongPtr64(windowHandle, nIndex, new IntPtr(dwNewLong));
            }
            return SetWindowLong32(windowHandle, nIndex, dwNewLong);
        }

        public static IntPtr Find(string moduleName, string mainWindowTitle)
        {
            //Search the window using Module and Title
            IntPtr WndToFind = FindWindow(moduleName, mainWindowTitle);
            if (WndToFind.Equals(IntPtr.Zero))
            {
                if (!string.IsNullOrEmpty(mainWindowTitle))
                {
                    //Search window using TItle only.
                    WndToFind = FindWindowByCaption(WndToFind, mainWindowTitle);
                    if (WndToFind.Equals(IntPtr.Zero))
                        return new IntPtr(0);
                }
            }
            return WndToFind;
        }

        public const int GWL_EXSTYLE = -20; //Sets a new extended window style
        public const int GWL_HINSTANCE = -6; //Sets a new application instance handle.
        public const int GWL_HWNDPARENT = -8; //Set window handle as parent
        public const int GWL_ID = -12; //Sets a new identifier of the window.
        public const int GWL_STYLE = -16; // Set new window style

        public const int GWL_USERDATA = -21; //Sets the user data associated with the window. 
        //This data is intended for use by the application 
        //that created the window. Its value is initially zero.
        public const int GWL_WNDPROC = -4; //Sets a new address for the window procedure.
    }

    [Flags]
    internal enum PositioningFlags
    {
        SWP_ASYNCWINDOWPOS = 0x4000,
        SWP_DEFERERASE = 0x2000,
        SWP_DRAWFRAME = 0x20,
        SWP_FRAMECHANGED = 0x20,
        SWP_HIDEWINDOW = 0x80,
        SWP_NOACTIVATE = 0x10,
        SWP_NOCOPYBITS = 0x100,
        SWP_NOMOVE = 2,
        SWP_NOOWNERZORDER = 0x200,
        SWP_NOREDRAW = 8,
        SWP_NOREPOSITION = 0x200,
        SWP_NOSENDCHANGING = 0x400,
        SWP_NOSIZE = 1,
        SWP_NOZORDER = 4,
        SWP_SHOWWINDOW = 0x40
    }
}
