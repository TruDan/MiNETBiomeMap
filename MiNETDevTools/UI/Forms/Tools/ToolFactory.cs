using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiNETDevTools.UI.Theme;

namespace MiNETDevTools.UI.Forms.Tools
{
    public static class ToolFactory
    {

        private static CustomTheme _theme { get; } = new CustomTheme();
        
        public static TToolType CreateTool<TToolType>(UiContext context) where TToolType : ToolWindow, new()
        {
            var tool = new TToolType();

            if(context != null)
                tool.LoadUiContext(context);
            
            _theme.PatchControl(tool);

            return tool;
        }
    }

    public interface IGraphicTool
    {
        void Draw();
    }
}
