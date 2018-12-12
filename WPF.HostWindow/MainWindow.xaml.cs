﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF.HostWindow
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private HwndSource parentHwnd;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            var hwnd = (HwndSource)PresentationSource.FromVisual(this);
            parentHwnd = hwnd;

            var dispatcher = await UIDispatcher.RunNewAsync("Background UI");
            await dispatcher.InvokeAsync(() =>
            {
                var window = new Window1();
                window.SourceInitialized += OnSourceInitialized;
                window.Show();
            });
        }

        private void OnSourceInitialized(object sender, EventArgs e)
        {
            var childHandle = new WindowInteropHelper((Window1)sender).Handle;
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
        public static extern long GetWindowLong(IntPtr handle, int style);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern long SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongA", SetLastError = true)]
        public static extern int SetWindowLongA([System.Runtime.InteropServices.InAttribute()] System.IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint);

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
    }
}
