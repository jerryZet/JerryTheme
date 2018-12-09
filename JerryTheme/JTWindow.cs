using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace JerryTheme
{
    public class JTWindow : Window
    {
        static JTWindow() => DefaultStyleKeyProperty.OverrideMetadata(typeof(JTWindow), new FrameworkPropertyMetadata(typeof(JTWindow)));

        protected void Window_Minimize(object sender, EventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        protected void Window_MaximizeRestore(object sender, EventArgs e)
        {
            this.WindowState = (this.WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }

        protected void Window_Close(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
