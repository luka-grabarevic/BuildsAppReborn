using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace BuildsAppReborn.Infrastructure.Wpf
{
    public class WaitingIndicator : IDisposable
    {
        public WaitingIndicator()
        {
            Interlocked.Increment(ref count);
            Application.Current.Dispatcher.Invoke(() => Mouse.OverrideCursor = Cursors.Wait);
        }

        public void Dispose()
        {
            Interlocked.Decrement(ref count);

            if (count == 0)
            {
                Application.Current.Dispatcher.Invoke(() => Mouse.OverrideCursor = null);
            }
        }

        private static Int32 count;
    }
}