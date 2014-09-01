// --------------------------------------------------------------------------
// <copyright file="ControlBox.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.8.21">Created</change>
// </history>

namespace NexFB.WinApp.DependencyProperties
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Interop;

    /// <summary>
    /// Class ControlBox.
    /// </summary>
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

        private const int Style = -16;
        private const int ExtStyle = -20;

        private const int MaximizeBox = 0x10000;
        private const int MinimizeBox = 0x20000;
        private const int ContextHelp = 0x400;

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed. Suppression is OK here.")]
        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static bool GetHasHelpButton(Window element)
        {
            return (bool)element.GetValue(HasHelpButtonProperty);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed. Suppression is OK here.")]
        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static void SetHasHelpButton(Window element, bool value)
        {
            element.SetValue(HasHelpButtonProperty, value);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed. Suppression is OK here.")]
        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static bool GetHasMaximizeButton(Window element)
        {
            return (bool)element.GetValue(HasMaximizeButtonProperty);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed. Suppression is OK here.")]
        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static void SetHasMaximizeButton(Window element, bool value)
        {
            element.SetValue(HasMaximizeButtonProperty, value);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed. Suppression is OK here.")]
        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static bool GetHasMinimizeButton(Window element)
        {
            return (bool)element.GetValue(HasMinimizeButtonProperty);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed. Suppression is OK here.")]
        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static void SetHasMinimizeButton(Window element, bool value)
        {
            element.SetValue(HasMinimizeButtonProperty, value);
        }

        /// <summary>
        /// Change value of control box to set view style of Window.
        /// </summary>
        /// <param name="d">Dependency Object.</param>
        /// <param name="e">Event changed.</param>
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

        /// <summary>
        /// Update Style.
        /// </summary>
        /// <param name="window">Working window.</param>
        /// <param name="windowHandler">Data type represent to a pointer or handle.</param>
        private static void UpdateStyle(Window window, IntPtr windowHandler)
        {
            var style = NativeMethods.GetWindowLong(windowHandler, Style);

            if (GetHasMaximizeButton(window))
            {
                style |= MaximizeBox;
            }
            else
            {
                style &= ~MaximizeBox;
            }

            if (GetHasMinimizeButton(window))
            {
                style |= MinimizeBox;
            }
            else
            {
                style &= ~MinimizeBox;
            }
        }

        /// <summary>
        /// User external DLL.
        /// </summary>
        private static class NativeMethods
        {
            [DllImport("user32.dll")]
            internal static extern int GetWindowLong(IntPtr hWnd, int index);

            ////[DllImport("user32.dll")]
            ////internal static extern int SetWindowLong(IntPtr hWnd, int index, int newLong);
        }
    }
}
