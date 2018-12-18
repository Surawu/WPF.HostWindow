using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MultithreadedUI
{
    public class NativeMethods
    {
        public const int GWL_STYLE = (-16);

        /// <summary> 
        /// 带有外边框和标题的windows的样式 
        /// </summary> 
        public const long WS_CAPTION = 0x00C00000L;
        public const long WS_CAPTION_2 = 0X00C0000L;

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern long SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongA", SetLastError = true)]
        internal static extern int SetWindowLongA([In()] System.IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern long GetWindowLong(IntPtr handle, int style);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint);
    }
}
