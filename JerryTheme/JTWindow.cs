using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace JerryTheme
{
    /// <summary>
    /// <para>JTWindow a BorderLess Window with:</para>
    /// <para> - Minimize, Maximize/Restore, Close buttons</para>
    /// <para> - Double Click Header (Maximize/Restore)</para>
    /// <para> - Window DragMove by click-drag Header</para>
    /// <para> - Resize by click-drag edges</para>
    /// </summary>
    public class JTWindow : Window
    {
        /// <summary>
        /// Set DefaultStyleKeyProperty for all instances of JTWindow - for Style and Template purposes
        /// </summary>
        static JTWindow() => DefaultStyleKeyProperty.OverrideMetadata(typeof(JTWindow), new FrameworkPropertyMetadata(typeof(JTWindow)));


        /*     HEADER BACKGROUND BRUSH        */
        /**************************************/
        /// <summary>
        /// Dependency property to allow setting HeaderBackground from XAML
        /// </summary>
        public static readonly DependencyProperty HeaderBarBackgroundProperty = DependencyProperty.Register("HeaderBarBackground", typeof(Brush), typeof(JTWindow), new FrameworkPropertyMetadata(null));
        /// <summary>
        /// HeaderBackground accessor
        /// </summary>
        public Brush HeaderBarBackground
        {
            get { return (Brush)GetValue(HeaderBarBackgroundProperty); }
            set { SetValue(HeaderBarBackgroundProperty, value); }
        }

        /// <summary>
        /// <para>OnApplyTemplate OVERRIDE</para>
        /// <para>For attaching click handlers to Header Buttons</para>
        /// </summary>
        public override void OnApplyTemplate()
        {
            if (GetTemplateChild("btnMinimize") is Button btnMinimize)
            {
                btnMinimize.Click += Window_Minimize;
            }

            if (GetTemplateChild("btnMaximizeRestore") is Button btnMaximizeRestore)
            {
                btnMaximizeRestore.Click += Window_MaximizeRestore;
            }

            if (GetTemplateChild("btnClose") is Button btnClose)
            {
                btnClose.Click += Window_Close;
            }

            base.OnApplyTemplate();
        }

        /// <summary>
        /// Minimize Window action
        /// </summary>
        protected void Window_Minimize(object sender, EventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        /// <summary>
        /// Maximize/Restore Window action
        /// </summary>
        protected void Window_MaximizeRestore(object sender, EventArgs e)
        {
            this.WindowState = (this.WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }
        /// <summary>
        /// Close Window action
        /// </summary>
        protected void Window_Close(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
