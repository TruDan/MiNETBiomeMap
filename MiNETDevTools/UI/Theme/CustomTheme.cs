using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using WeifenLuo.WinFormsUI.Docking;
using WeifenLuo.WinFormsUI.ThemeVS2012;
using WeifenLuo.WinFormsUI.ThemeVS2013;
using WeifenLuo.WinFormsUI.ThemeVS2015;

namespace MiNETDevTools.UI.Theme
{
    [System.ComponentModel.DesignerCategory("")]
    public class CustomTheme : VS2015ThemeBase
    {
        private delegate void ControlPatchDelegate<in T>(T control);
        private delegate void ControlPatchDelegate(Control control);

        private static readonly ILog Log = LogManager.GetLogger(typeof(CustomTheme));

        private IDictionary<Type, ControlPatchDelegate> _patches = new Dictionary<Type, ControlPatchDelegate>();

        public CustomTheme() : base(Decompress(Resources.vs2015dark_vstheme))
        {
            Extender.DockPaneFactory = new CustomDockPaneFactory();
            
            AddPatcher<ListView>((c) => c.BackColor = Color.FromArgb(0x7F252526));
        }

        private void AddPatcher<T>(ControlPatchDelegate<T> patcher) where T : Control
        {
            _patches.Add(typeof(T), c =>
            {
                c.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(c, BindingFlags.Instance | BindingFlags.NonPublic, null, new object[]{ControlStyles.SupportsTransparentBackColor, true}, null);
                patcher.Invoke((T) c);
            });
        }

        public void PatchControl(Control control)
        {
            ControlPatchDelegate patch;
            if (_patches.TryGetValue(control.GetType(), out patch))
            {
                patch.Invoke(control);
            }

            if (control.HasChildren)
            {
                foreach (var child in control.Controls)
                {
                    if (child is Control)
                    {
                        PatchControl((Control) child);
                    }
                }
            }
        }

        public class CustomDockPaneFactory : DockPanelExtender.IDockPaneFactory
        {
            public DockPane CreateDockPane(IDockContent content, DockState visibleState, bool show)
            {
                return new CustomDockPane(content, visibleState, show);
            }

            public DockPane CreateDockPane(IDockContent content, FloatWindow floatWindow, bool show)
            {
                return new CustomDockPane(content, floatWindow, show);
            }

            public DockPane CreateDockPane(IDockContent content, DockPane previousPane, DockAlignment alignment, double proportion, bool show)
            {
                return new CustomDockPane(content, previousPane, alignment, proportion, show);
            }

            public DockPane CreateDockPane(IDockContent content, Rectangle floatWindowBounds, bool show)
            {
                return new CustomDockPane(content, floatWindowBounds, show);
            }
        }

        public class CustomDockPane : VS2013DockPane
        {
            public CustomDockPane(IDockContent content, DockState visibleState, bool show) : base(content, visibleState, show)
            {
            }

            public CustomDockPane(IDockContent content, FloatWindow floatWindow, bool show) : base(content, floatWindow, show)
            {
            }

            public CustomDockPane(IDockContent content, DockPane previousPane, DockAlignment alignment, double proportion, bool show) : base(content, previousPane, alignment, proportion, show)
            {
            }

            public CustomDockPane(IDockContent content, Rectangle floatWindowBounds, bool show) : base(content, floatWindowBounds, show)
            {
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.Clear(DockPanel.Theme.ColorPalette.DockTarget.Background);
                var color = DockPanel.Theme.ColorPalette.ToolWindowBorder;
                e.Graphics.FillRectangle(DockPanel.Theme.PaintingService.GetBrush(color), e.ClipRectangle);
            }
        }
    }
}
