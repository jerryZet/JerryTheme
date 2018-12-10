using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace JerryTheme
{
    public class JTWindowFrame : Window
    {
        /*     WINDOW GENERAL SETUP        */
        /***********************************/
        /// <summary>
        /// Set DefaultStyleKeyProperty for all instances of JTWindow - for Style and Template purposes
        /// </summary>
        static JTWindowFrame()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(JTWindowFrame), new FrameworkPropertyMetadata(typeof(JTWindowFrame)));
        }
        /// <summary>
        /// <para>OnApplyTemplate OVERRIDE</para>
        /// <para>for attaching click handlers to Header Buttons</para>
        /// </summary>
        public override void OnApplyTemplate()
        {
            // Header Button Handlers for Window Minimize, Maximize/Restore and Close actions
            if (GetTemplateChild("btnMinimize") is Button btnMinimize)
            {
                btnMinimize.Click += MinimizeButton_Click;
            }
            if (GetTemplateChild("btnMaximizeRestore") is Button btnMaximizeRestore)
            {
                btnMaximizeRestore.Click += MaximizeRestoreButton_Click;
            }
            if (GetTemplateChild("btnClose") is Button btnClose)
            {
                btnClose.Click += CloseButton_Click;
            }

            // run base
            base.OnApplyTemplate();
        }

        /*     HEADER BACKGROUND BRUSH        */
        /**************************************/
        /// <summary>
        /// Dependency property to allow setting HeaderBackground from XAML
        /// </summary>
        public static readonly DependencyProperty HeaderBarBackgroundProperty = DependencyProperty.Register("HeaderBarBackground", typeof(Brush), typeof(JTWindowFrame));
        /// <summary>
        /// HeaderBackground accessor
        /// </summary>
        public Brush HeaderBarBackground
        {
            get { return (Brush)GetValue(HeaderBarBackgroundProperty); }
            set { SetValue(HeaderBarBackgroundProperty, value); }
        }



        /// <summary>
        /// Minimize Window action
        /// </summary>
        private void MinimizeButton_Click(object sender, EventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        /// <summary>
        /// Maximize/Restore Window action
        /// </summary>
        private void MaximizeRestoreButton_Click(object sender, EventArgs e)
        {
            this.WindowState = (this.WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }
        /// <summary>
        /// Close Window action
        /// </summary>
        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
