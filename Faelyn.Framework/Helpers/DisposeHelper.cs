using System;

namespace Faelyn.Framework.Helpers
{
    public sealed class DisposeHelper : IDisposable
    {
        private bool _isDisposed = false;
        private Action _disposeAction;

        public DisposeHelper(Action disposeAction)
        {
            _disposeAction = disposeAction ?? throw new ArgumentNullException(nameof(disposeAction));
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                Dispose(true);
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _disposeAction?.Invoke();
            }
            _disposeAction = null;
        }
    }
    
    public sealed class DisposeHelper<TObject> : IDisposable
    {
        private bool _isDisposed = false;
        private Action<TObject> _disposeAction;
        public TObject Reference { get; private set; }

        public DisposeHelper(TObject reference, Action<TObject> disposeAction)
        {
            if (typeof(TObject).IsClass && ((object)reference) == null)
            {
                throw new ArgumentNullException(nameof(reference));
            }

            _disposeAction = disposeAction ?? throw new ArgumentNullException(nameof(disposeAction));
            Reference = reference;
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                Dispose(true);
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _disposeAction?.Invoke(Reference);
            }
            _disposeAction = null;
            Reference = default(TObject);
        }
    }
}