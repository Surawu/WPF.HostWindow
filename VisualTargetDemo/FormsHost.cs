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
            var txt = new System.Windows.Forms.TextBox
            {
                Text = "System.Windows.Forms.TextBox"
            };
            formsHost.Child = txt;

            this.Children.Add(formsHost);
        }
    }
}
