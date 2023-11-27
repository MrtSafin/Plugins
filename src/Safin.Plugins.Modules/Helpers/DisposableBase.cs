using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safin.Plugins
{
    public class DisposableBase : IDisposable
    {
        private bool _disposedValue = false;
        ~DisposableBase() => Dispose(false);
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    DisposeManaged();
                }

                DisposeUnmanaged();
                _disposedValue = true;
            }
        }
        protected virtual void DisposeManaged()
        {

        }
        protected virtual void DisposeUnmanaged()
        {

        }
    }
}
