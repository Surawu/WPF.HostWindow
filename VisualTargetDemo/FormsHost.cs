using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms.Integration;

namespace VisualTargetDemo
{
    public class FormsHost : Grid
    {
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            WindowsFormsHost formsHost = new WindowsFormsHost();
            var btn = new System.Windows.Forms.Button();
            btn.Text = "System.Windows.Forms.Button";
            formsHost.Child = btn;

            this.Children.Add(formsHost);
        }
    }
}
