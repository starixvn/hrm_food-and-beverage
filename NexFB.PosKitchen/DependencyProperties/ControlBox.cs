namespace NexFB.PosKitchen.DependencyProperties
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Interop;

    public static class ControlBox
    {
        public static readonly DependencyProperty HasHelpButtonProperty = DependencyProperty.RegisterAttached(
            "HasHelpButton",
            typeof(bool),
            typeof(ControlBox),
            new UIPropertyMetadata(false, OnControlBoxChanged));

        public static readonly DependencyProperty HasMaximizeButtonProperty = DependencyProperty.RegisterAttached(
            "HasMaximizeButton",
            typeof(bool),
            typeof(ControlBox),
            new UIPropertyMetadata(true, OnControlBoxChanged));

        public static readonly DependencyProperty HasMinimizeButtonProperty = DependencyProperty.RegisterAttached(
            "HasMinimizeButton",
            typeof(bool),
            typeof(ControlBox),
            new UIPropertyMetadata(true, OnControlBoxChanged));

        private const int _Style = -16;

        private const int _MaximizeBox = 0x10000;
        private const int _MinimizeBox = 0x20000;

        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static bool GetHasHelpButton(Window element)
        {
            return (bool)element.GetValue(HasHelpButtonProperty);
        }

        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static void SetHasHelpButton(Window element, bool value)
        {
            element.SetValue(HasHelpButtonProperty, value);
        }

        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static bool GetHasMaximizeButton(Window element)
        {
            return (bool)element.GetValue(HasMaximizeButtonProperty);
        }

        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static void SetHasMaximizeButton(Window element, bool value)
        {
            element.SetValue(HasMaximizeButtonProperty, value);
        }

        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static bool GetHasMinimizeButton(Window element)
        {
            return (bool)element.GetValue(HasMinimizeButtonProperty);
        }

        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static void SetHasMinimizeButton(Window element, bool value)
        {
            element.SetValue(HasMinimizeButtonProperty, value);
        }

        private static void OnControlBoxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as Window;
            if (window != null)
            {
                var windowHandler = new WindowInteropHelper(window).Handle;
                if (windowHandler == IntPtr.Zero)
                {
                    window.SourceInitialized += OnWindowSourceInitialized;
                }
                else
                {
                    UpdateStyle(window, windowHandler);
                }
            }
        }

        private static void OnWindowSourceInitialized(object sender, EventArgs e)
        {
            var window = (Window)sender;

            var windowHandler = new WindowInteropHelper(window).Handle;
            UpdateStyle(window, windowHandler);

            window.SourceInitialized -= OnWindowSourceInitialized;
        }

        private static void UpdateStyle(Window window, IntPtr windowHandler)
        {
            var style = NativeMethods.GetWindowLong(windowHandler, _Style);

            if (GetHasMaximizeButton(window))
            {
                style |= _MaximizeBox;
            }
            else
            {
                style &= ~_MaximizeBox;
            }

            if (GetHasMinimizeButton(window))
            {
                style |= _MinimizeBox;
            }
            else
            {
                style &= ~_MinimizeBox;
            }
        }

        private static class NativeMethods
        {
            [DllImport("user32.dll")]
            internal static extern int GetWindowLong(IntPtr hWnd, int index);

            ////[DllImport("user32.dll")]
            ////internal static extern int SetWindowLong(IntPtr hWnd, int index, int newLong);
        }
    }
}