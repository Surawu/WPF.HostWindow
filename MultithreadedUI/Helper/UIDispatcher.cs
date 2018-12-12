using System;
using System.Collections.Generic;
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
        public static async Task<T> CreateElementAsync<T>(HwndSource hwndSource)
            where T : Window, new()
        {
            parentHwnd = hwndSource;
            return await CreateElementAsync<T>(() => new T());
        }

        private static async Task<T> CreateElementAsync<T>(Func<T> func)
            where T : Window
        {
            if (func == null)
                throw new ArgumentException(nameof(func));

            var element = default(T);
            Exception exception = null;
            var are = new AutoResetEvent(false);
            var thread = new Thread(() =>
            {
                try
                {
                    SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(Dispatcher.CurrentDispatcher));
                    element = func();
                    element.SourceInitialized += OnSourceInitialized;
                    element.Show();
                    are.Set();
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

            await Task.Run(() =>
            {
                are.WaitOne();
                are.Dispose();
            });
            if (exception != null)
            {
                ExceptionDispatchInfo.Capture(exception).Throw();
            }
            return element;
        }

        private static void OnSourceInitialized(object sender, EventArgs e)
        {
            var childHandle = new WindowInteropHelper((SubWindow)sender).Handle;
            SetParent(childHandle, parentHwnd.Handle);
            long oldstyle = GetWindowLong(childHandle, GWL_STYLE);
            SetWindowLongA(childHandle, GWL_STYLE, (int)oldstyle & (~((int)WS_CAPTION | (int)WS_CAPTION_2)));
            MoveWindow(childHandle, 0, 0, 300, 300, true);

        }

        private const int GWL_STYLE = (-16);
        private const int WS_VISIBLE = 0x10000000;

        /// <summary> 
        /// 带有外边框和标题的windows的样式 
        /// </summary> 
        public const long WS_CAPTION = 0x00C00000L;
        public const long WS_CAPTION_2 = 0X00C0000L;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern long SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongA", SetLastError = true)]
        public static extern int SetWindowLongA([In()] System.IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern long GetWindowLong(IntPtr handle, int style);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint);
    }
}

