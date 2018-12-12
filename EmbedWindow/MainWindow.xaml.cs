using System;
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

namespace EmbedWindow
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Windows.Forms.Panel formsPanel;
        private AutoResetEvent autoResetEvent = new AutoResetEvent(false);

        public MainWindow()
        {
            InitializeComponent();
            EmbedWindow();
        }

        private void EmbedWindow()
        {
            formsPanel = new System.Windows.Forms.Panel();
            formsPanel.Size = new System.Drawing.Size((int)Width, (int)Height);
            formsPanel.Visible = false;

            SubWindow subWindow = new SubWindow();
            var helper = new WindowInteropHelper(subWindow);
            subWindow.Show();
            while (true)
            {
                if (helper.Handle.ToInt32() != 0)
                {
                    autoResetEvent.Set();
                    break;
                }
            }
            autoResetEvent.WaitOne(); 

            var inPtr = helper.Handle;
            SetParent(inPtr, formsPanel.Handle);
            SetWindowLongA(inPtr, GWL_STYLE, WS_VISIBLE);
            MoveWindow(inPtr, 0, 0, formsPanel.Width, formsPanel.Height, true);
            host.Child = formsPanel;
        }

        private const int GWL_STYLE = (-16);
        private const int WS_VISIBLE = 0x10000000;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern long SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongA", SetLastError = true)]
        public static extern int SetWindowLongA([In()] System.IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint);
    }
}
