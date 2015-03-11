// --------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.14">Created</change>
// </history>

namespace NexFB.WinAppSmartCard
{
    using System;
    using System.Configuration;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interop;
    using AutoLogOffHelper;
    using NexFB.WinAppSmartCard.Core.Constants;
    using NexFB.WinAppSmartCard.Core.Resources;
    using NexFB.WinAppSmartCard.ViewModels;

    /// <summary>
    /// Interaction logic for MainWindow.
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string _sessionTimeout = ConfigurationManager.AppSettings[ConstantValue.SessionTimeout];
        private static double _screenWidth = SystemParameters.PrimaryScreenWidth;
        private double _screeHeight = SystemParameters.PrimaryScreenHeight;
        private int _logOffTime = 0;

        public MainWindow()
        {
            InitializeComponent();
            this.Height = _screeHeight;
            this.Width = _screenWidth;
            this.Loaded += (sender, e) => MainWindow_Loaded();
        }

        public void InitializeAutoLogoffFeature()
        {
            HwndSource windowSpecificOSMessageListener = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            windowSpecificOSMessageListener.AddHook(new HwndSourceHook(CallBackMethod));
            AutoLogOff.LogOffTime = _logOffTime;
            AutoLogOff.MakeAutoLogOffEvent += (sender, e) => AutoLogOffHelper_MakeAutoLogOffEvent();
            AutoLogOff.StartAutoLogoffOption();
        }

        /// <summary>
        /// Method Window Loaded.
        /// </summary>
        private void MainWindow_Loaded()
        {
            bool isNumeric = false;
            if (_sessionTimeout != null)
            {
                isNumeric = int.TryParse(_sessionTimeout, out _logOffTime);
                if (!isNumeric)
                {
                    _logOffTime = 60;
                }
            }

            this.KeyDown += new KeyEventHandler(KeyboardEventHandler);

            ////InitializeAutoLogoffFeature();
        }

        private void LanguageButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var contextMenu = languageButton.ContextMenu;
            contextMenu.PlacementTarget = this;
            contextMenu.IsOpen = true;
        }

        /// <summary>
        /// Call Back Method to detect user interact with screen.
        /// </summary>
        /// <param name="hwnd">The window handle.</param>
        /// <param name="msg">The message ID.</param>
        /// <param name="widthParam">The message's width Parameter value.</param>
        /// <param name="longParam">The message's long Parameter value.</param>
        /// <param name="handled">A value that indicates whether the message was handled. Set the value to true if the message was handled; otherwise, false.</param>
        /// <returns>Represents the method that handles Win32 window messages.</returns>
        private IntPtr CallBackMethod(IntPtr hwnd, int msg, IntPtr widthParam, IntPtr longParam, ref bool handled)
        {
            ////  Listening OS message to test whether it is a user activity
            if ((msg >= 0x0200 && msg <= 0x020A) || (msg <= 0x0106 && msg >= 0x00A0) || msg == 0x0021)
            {
                AutoLogOff.ResetLogoffTimer();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(msg.ToString());
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// Auto Log Off Event.
        /// </summary>
        private void AutoLogOffHelper_MakeAutoLogOffEvent()
        {
            var currentViewModel = (MainViewModel)this.DataContext;
            if (currentViewModel.MainScreenVisible == CommonResources.Visible)
            {
                if (Application.Current.Windows.Count > 1)
                {
                    WindowCollection windows = Application.Current.Windows;
                    foreach (Window item in windows)
                    {
                        if (item.GetType().Name != "MainWindow")
                        {
                            item.Close();
                        }
                    }
                }

                currentViewModel.ExecuteReturnToStandByScreen();
            }
        }

        private void MenuItem_PreviewMouseDown_Exit(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void MenuItem_PreviewMouseDown_Restart(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
            System.Windows.Forms.Application.Restart();
        }

        /// <summary>
        /// Keyboard event.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The key event.</param>
        private void KeyboardEventHandler(object sender, KeyEventArgs e)
        {
            var currentViewModel = (MainViewModel)this.DataContext;

            if (e.Key == Key.Enter 
                && ((MainDishesViewModel)currentViewModel.CurrentViewModel).ObservableListEmployee.Count > 0)
            {
                ((MainDishesViewModel)currentViewModel.CurrentViewModel).ExecuteConfirmCommand();
            }
            else if (e.Key == Key.NumLock)
            {
                currentViewModel.ExecuteReturnToStandByScreen();
            }
        }
    }
}
