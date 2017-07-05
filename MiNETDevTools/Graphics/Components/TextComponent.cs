using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiNETDevTools.Graphics.Internal;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;
using TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode;

namespace MiNETDevTools.Graphics.Components
{
    public class TextComponent : DisposeWrapper, IGraphicComponent
    {
        public int Size { get; set; }
        public string Text { get; set; }
        public Point Location { get; set; }

        TextFormat textFormat;
        Brush sceneColorBrush;
        protected string font;
        protected Color4 color;
        protected int lineLength;

        public TextComponent(string font, Color c, Point location, int size = 12, int lineLength = 500)
        {
            if (!String.IsNullOrEmpty(font))
                this.font = font;
            else
                this.font = "Calibri";

            this.color = new Color4(c.R, c.G, c.B, c.A);
            this.Location = location;
            this.Size = size;
            this.lineLength = lineLength;
        }

        public virtual void InitialiseGraphics(GraphicsDevice device)
        {
            RemoveAndDispose(ref sceneColorBrush);
            RemoveAndDispose(ref textFormat);

            sceneColorBrush = ToDispose(new SolidColorBrush(device.D2Context, color));
            textFormat = ToDispose(new TextFormat(device.DwFactory, font, Size)
            {
                TextAlignment = TextAlignment.Leading,
                ParagraphAlignment = ParagraphAlignment.Center
            });

            device.D2Context.TextAntialiasMode = TextAntialiasMode.Grayscale;
        }

        public virtual void CreateResources(GraphicsDevice device)
        {

        }

        public virtual void DrawFrame(GraphicsDevice device)
        {
            if (String.IsNullOrEmpty(Text))
                return;

            var context2D = device.D2Context;

            context2D.BeginDraw();
            context2D.Transform = Matrix3x2.Identity;
            context2D.DrawText(Text, textFormat, new RectangleF(Location.X, Location.Y, Location.X + lineLength, Location.Y + 16), sceneColorBrush);
            context2D.EndDraw();
        }
    }
}
