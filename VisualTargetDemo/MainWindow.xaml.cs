using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VisualTargetDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //Player1.Child = CreateMediaElementOnWorkerThread();
            //Player2.Child = CreateMediaElementOnWorkerThread();
            //Player3.Child = CreateMediaElementOnWorkerThread();
        }

        private HostVisual CreateMediaElementOnWorkerThread()
        {
            // Create the HostVisual that will "contain" the VisualTarget
            // on the worker thread.
            HostVisual hostVisual = new HostVisual();

            // Spin up a worker thread, and pass it the HostVisual that it
            // should be part of.
            Thread thread = new Thread(new ParameterizedThreadStart(MediaWorkerThread));
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start(hostVisual);

            // Wait for the worker thread to spin up and create the VisualTarget.
            s_event.WaitOne();

            return hostVisual;
        }

        private FrameworkElement CreateMediaElement()
        {
            // Create a MediaElement, and give it some content.

            TextBox textBox = new TextBox();
            textBox.Width = 200;
            textBox.Height = 100;
            textBox.Text = "Create a MediaElement, and give it some content";
            //return new SubWindow();
            return new FormsHost();
        }

        private void MediaWorkerThread(object arg)
        {
            // Create the VisualTargetPresentationSource and then signal the
            // calling thread, so that it can continue without waiting for us.
            HostVisual hostVisual = (HostVisual)arg;
            VisualTargetPresentationSource visualTargetPS = new VisualTargetPresentationSource(hostVisual);
            s_event.Set();

            // Create a MediaElement and use it as the root visual for the
            // VisualTarget.
            visualTargetPS.RootVisual = CreateMediaElement();

            // Run a dispatcher for this worker thread.  This is the central
            // processing loop for WPF.
            System.Windows.Threading.Dispatcher.Run();
        }

        private static AutoResetEvent s_event = new AutoResetEvent(false);
    }
}
