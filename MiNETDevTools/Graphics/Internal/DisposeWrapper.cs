using System;
using SharpDX;

namespace MiNETDevTools.Graphics.Internal
{
    public class DisposeWrapper : IDisposable
    {
        protected DisposeCollector DisposeCollector { get; set; }

        internal bool IsAttached { get; set; }

        protected internal bool IsDisposed { get; private set; }

        protected internal bool IsDisposing { get; private set; }

        public event EventHandler<EventArgs> Disposing;

        protected internal DisposeWrapper()
        {
        }

        protected internal void RemoveToDispose<T>(T toDisposeArg)
        {
            if (!ReferenceEquals(toDisposeArg, null) && DisposeCollector != null)
                DisposeCollector.Remove(toDisposeArg);
        }

        protected internal void RemoveAndDispose<T>(ref T objectToDispose)
        {
            if (!ReferenceEquals(objectToDispose, null) && DisposeCollector != null)
                DisposeCollector.RemoveAndDispose(ref objectToDispose);
        }

        protected internal T ToDispose<T>(T toDisposeArg)
        {
            if (!ReferenceEquals(toDisposeArg, null))
            {
                if (DisposeCollector == null)
                    DisposeCollector = new DisposeCollector();
                return DisposeCollector.Collect(toDisposeArg);
            }
            return default(T);
        }

        protected virtual void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources)
            {
                // Dispose all ComObjects
                if (DisposeCollector != null)
                    DisposeCollector.Dispose();
                DisposeCollector = null;
            }
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposing = true;

                // Call the disposing event.
                var handler = Disposing;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }

                Dispose(true);
                IsDisposed = true;
            }
        }
    }
    
}
