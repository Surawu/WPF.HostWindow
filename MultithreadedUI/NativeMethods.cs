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
        /// <summary> 
        /// 带有外边框和标题的windows的样式 
        /// </summary> 
        public const long WS_CAPTION = 0x00C00000L;
        public const long WS_CAPTION_2 = 0X00C0000L;
        // public const long WS_BORDER = 0X0080000L; 

        /// <summary> 
        /// window 扩展样式 分层显示 
        /// </summary> 
        public const long WS_EX_LAYERED = 0x00080000L;
        public const long WS_CHILD = 0x40000000L;

        /// <summary> 
        /// 带有alpha的样式 
        /// </summary> 
        public const long LWA_ALPHA = 0x00000002L;

        /// <summary> 
        /// 颜色设置 
        /// </summary> 
        public const long LWA_COLORKEY = 0x00000001L;

        /// <summary> 
        /// window的基本样式 
        /// </summary> 
        public const int GWL_STYLE = -16;

        /// <summary> 
        /// window的扩展样式 
        /// </summary> 
        public const int GWL_EXSTYLE = -20;

        /// <summary> 
        /// 设置窗体的样式 
        /// </summary> 
        /// <param name="handle">操作窗体的句柄</param> 
        /// <param name="oldStyle">进行设置窗体的样式类型.</param> 
        /// <param name="newStyle">新样式</param> 
        [DllImport("user32.dll", EntryPoint = "SetWindowLongA", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetWindowLongA(IntPtr handle, int oldStyle, long newStyle);

        /// <summary> 
        /// 获取窗体指定的样式. 
        /// </summary> 
        /// <param name="handle">操作窗体的句柄</param> 
        /// <param name="style">要进行返回的样式</param> 
        /// <returns>当前window的样式</returns> 
        [DllImport("user32.dll", SetLastError = true)]
        public static extern long GetWindowLong(IntPtr handle, int style);

        /// <summary> 
        /// 设置窗体的工作区域. 
        /// </summary> 
        /// <param name="handle">操作窗体的句柄.</param> 
        /// <param name="handleRegion">操作窗体区域的句柄.</param> 
        /// <param name="regraw">if set to <c>true</c> [regraw].</param> 
        /// <returns>返回值</returns> 
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern int SetWindowRgn(IntPtr handle, IntPtr handleRegion, bool regraw);


        //=================================================================================
        /// <summary>
        /// 设置窗体为无边框风格
        /// </summary>
        /// <param name="hWnd"></param>
        public static void SetWindowNoBorder(IntPtr hWnd)
        {
            long oldstyle = GetWindowLong(hWnd, NativeMethods.GWL_STYLE);

            SetWindowLongA(hWnd, GWL_STYLE, oldstyle & (~(WS_CAPTION | WS_CAPTION_2)));
            //SetWindowLong(hWnd, GWL_EXSTYLE, WS_CHILD);
        }
    }

}
