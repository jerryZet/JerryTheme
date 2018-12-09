using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;


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
        /*     EXTERNAL IMPORTS AND ASSOCIATED STUFF       */
        /***************************************************/
        /// <summary>
        /// Import of SendMessage Win API method from user32.dll - for sending the Window Resize message to the system
        /// </summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);
        /// <summary>
        /// Window Hock for Win API (user32.dll)
        /// </summary>
        private HwndSource _hwndSource;
        /// <summary>
        /// ResizeDirection Enumeration for Window Resizing
        /// </summary>
        private enum ResizeDirection
        {
            Left = 1,
            Right = 2,
            Top = 3,
            TopLeft = 4,
            TopRight = 5,
            Bottom = 6,
            BottomLeft = 7,
            BottomRight = 8,
        }

        /*     WINDOW GENERAL SETUP        */
        /***********************************/
        /// <summary>
        /// Set DefaultStyleKeyProperty for all instances of JTWindow - for Style and Template purposes
        /// </summary>
        static JTWindow() => DefaultStyleKeyProperty.OverrideMetadata(typeof(JTWindow), new FrameworkPropertyMetadata(typeof(JTWindow)));
        /// <summary>
        /// JTWindow Constructor
        /// </summary>
        public JTWindow() : base()
        {
            PreviewMouseMove += JTWindow_OnPreviewMouseMove;
        }
        /// <summary>
        /// <para>OnApplyTemplate OVERRIDE</para>
        /// <para>for attaching click handlers to Header Buttons</para>
        /// </summary>
        public override void OnApplyTemplate()
        {
            // Header Click for Window DragMove and Maximize/Restore on DoubleClick Handler
            if (GetTemplateChild("brdWindowHeader") is Border brdWindowHeader)
            {
                brdWindowHeader.MouseDown += Header_MouseDown;
            }

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

            // Window resize with hocks handlers
            if (GetTemplateChild("grdResizeHocks") is Grid grdResizeHocks)
            {
                foreach (UIElement hock in grdResizeHocks.Children)
                {
                    if (hock is Rectangle resizeHock)
                    {
                        resizeHock.PreviewMouseDown += ResizeHock_PreviewMouseDown;
                        resizeHock.MouseMove += ResizeHock_MouseMove; ;
                    }
                }
            }

            // run base
            base.OnApplyTemplate();
        }
        /// <summary>
        /// OnInitialized OVERRIDE to get the SourceInitialized event hock
        /// </summary>
        protected override void OnInitialized(EventArgs e)
        {
            this.SourceInitialized += OnSourceInitialized;
            base.OnInitialized(e);
        }
        /// <summary>
        /// SourceInitialized Handler to set the Window Hock for resizing purposes
        /// </summary>
        private void OnSourceInitialized(object sender, EventArgs e)
        {
            _hwndSource = (HwndSource)PresentationSource.FromVisual(this);
        }


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


        /*     WINDOW HANDLING VIA MOUSE      */
        /**************************************/
        /// <summary>
        /// Window DragMove and Maximize/Restore actions when Header is Clicked/DoubleClicked
        /// </summary>
        private void Header_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1 && e.ChangedButton == System.Windows.Input.MouseButton.Left)
            {
                this.WindowState = (this.WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
            }
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left && e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
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
        /// <summary>
        /// Bring back the default arrow cursor when the mouse moves over the window and left mouse button is not pressed
        /// </summary>
        private void JTWindow_OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
            {
                Cursor = Cursors.Arrow;
            }
        }
        /// <summary>
        /// Change cursor to appropriate Sizing cursor direction depending on the resizeHock sending the event
        /// </summary>
        private void ResizeHock_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Rectangle rectangle = sender as Rectangle;
            switch (rectangle.Name)
            {
                case "top":
                    Cursor = Cursors.SizeNS;
                    break;
                case "bottom":
                    Cursor = Cursors.SizeNS;
                    break;
                case "left":
                    Cursor = Cursors.SizeWE;
                    break;
                case "right":
                    Cursor = Cursors.SizeWE;
                    break;
                case "topLeft":
                    Cursor = Cursors.SizeNWSE;
                    break;
                case "topRight":
                    Cursor = Cursors.SizeNESW;
                    break;
                case "bottomLeft":
                    Cursor = Cursors.SizeNESW;
                    break;
                case "bottomRight":
                    Cursor = Cursors.SizeNWSE;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Perform the Resize operation depending on the resizeHock sending the event
        /// </summary>
        private void ResizeHock_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Rectangle rectangle = sender as Rectangle;
            switch (rectangle.Name)
            {
                case "top":
                    Cursor = Cursors.SizeNS;
                    ResizeWindow(ResizeDirection.Top);
                    break;
                case "bottom":
                    Cursor = Cursors.SizeNS;
                    ResizeWindow(ResizeDirection.Bottom);
                    break;
                case "left":
                    Cursor = Cursors.SizeWE;
                    ResizeWindow(ResizeDirection.Left);
                    break;
                case "right":
                    Cursor = Cursors.SizeWE;
                    ResizeWindow(ResizeDirection.Right);
                    break;
                case "topLeft":
                    Cursor = Cursors.SizeNWSE;
                    ResizeWindow(ResizeDirection.TopLeft);
                    break;
                case "topRight":
                    Cursor = Cursors.SizeNESW;
                    ResizeWindow(ResizeDirection.TopRight);
                    break;
                case "bottomLeft":
                    Cursor = Cursors.SizeNESW;
                    ResizeWindow(ResizeDirection.BottomLeft);
                    break;
                case "bottomRight":
                    Cursor = Cursors.SizeNWSE;
                    ResizeWindow(ResizeDirection.BottomRight);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Sends the Resize Event Message to the system with the Window Hock
        /// </summary>
        /// <param name="direction">ResizeDirection in which the window should be resized</param>
        private void ResizeWindow(ResizeDirection direction)
        {
            SendMessage(_hwndSource.Handle, 0x112, (IntPtr)(61440 + direction), IntPtr.Zero);
        }
    }
}
