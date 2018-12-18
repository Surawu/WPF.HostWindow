using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

namespace MultithreadedUI
{
    public static class UIDispatcher
    {
        
        public static HwndSource parentHwnd;
        public static T CreateElement<T>(HwndSource hwndSource)
            where T : Window, new()
        {
            parentHwnd = hwndSource;

            Func<T> func = () => new T();
            var element = default(T);
            Exception exception = null;
            var thread = new Thread(() =>
            {
                try
                {
                    Thread.Sleep(3300);

                    SynchronizationContext.SetSynchronizationContext(
                      new DispatcherSynchronizationContext(Dispatcher.CurrentDispatcher));
                    element = func();
                    element.SourceInitialized += OnSourceInitialized;
                    element.Show();
                    Trace.WriteLine("PLC Thread");
                    Dispatcher.Run();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            });
            thread.Name = "Back ground UI";
            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            if (exception != null)
            {
                ExceptionDispatchInfo.Capture(exception).Throw();
            }
            return element;
        }

        private static void OnSourceInitialized(object sender, EventArgs e)
        {
            var childHandle = new WindowInteropHelper((SubWindow)sender).Handle;
            NativeMethods.SetParent(childHandle, parentHwnd.Handle);
            long oldstyle = NativeMethods.GetWindowLong(childHandle, NativeMethods.GWL_STYLE);
            NativeMethods.SetWindowLongA(childHandle, NativeMethods.GWL_STYLE,
                (int)oldstyle & (~((int)NativeMethods.WS_CAPTION | (int)NativeMethods.WS_CAPTION_2)));
            NativeMethods.MoveWindow(childHandle, 0, 0, 300, 300, true);
        }
    }
}

