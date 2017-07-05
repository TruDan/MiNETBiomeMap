using System.Diagnostics;
using System.Drawing;
using MiNETDevTools.Graphics.Internal;

namespace MiNETDevTools.Graphics.Components
{
    public class FpsCounter : TextComponent
    {
        private const int MaxSamples = 100;

        private Stopwatch _stopwatch;
        private long _frameCount;

        private int _tickIndex = 0;
        private long _tickSum = 0;
        private long[] _tickList = new long[MaxSamples];

        public FpsCounter() : base("Segoe UI", Color.Yellow, new Point(16, 16), 10)
        {
            
        }

        private double CalculateAverageTick(long newTick)
        {
            _tickSum -= _tickList[_tickIndex];
            _tickSum += newTick;
            _tickList[_tickIndex] = newTick;
            if (++_tickIndex == MaxSamples)
                _tickIndex = 0;

            if (_frameCount < MaxSamples)
                return (double) _tickSum / _frameCount;
            else
                return (double) _tickSum / MaxSamples;
        }

        public override void InitialiseGraphics(GraphicsDevice device)
        {
            base.InitialiseGraphics(device);

            _stopwatch = Stopwatch.StartNew();
        }

        public override void DrawFrame(GraphicsDevice device)
        {
            _frameCount++;

            var averageTick = CalculateAverageTick(_stopwatch.ElapsedTicks) / Stopwatch.Frequency;
            Text = $"{1.0 / averageTick:F2} FPS ({averageTick * 1000.0:F1} ms)";

            base.DrawFrame(device);

            _stopwatch.Restart();
        }
    }
}
