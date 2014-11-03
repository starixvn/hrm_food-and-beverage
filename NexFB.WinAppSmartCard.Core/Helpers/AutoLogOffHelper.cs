// --------------------------------------------------------------------------
// <copyright file="AutoLogOffHelper.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.11.3">Created</change>
// </history>

namespace NexFB.WinAppSmartCard.Core.Helpers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Forms;

    /// <summary>
    /// Auto Log Off Helper.
    /// </summary>
    public static class AutoLogOffHelper
    {
        private static Timer _timer = null;

        private static int _logOffTime;

        [SuppressMessage("Microsoft.Design", "CA1003:UseGenericEventHandlerInstances", Justification = "Reviewed. Suppression is OK here.")]
        public delegate void MakeAutoLogOff(object sender, EventArgs e);

        public static event MakeAutoLogOff MakeAutoLogOffEvent;

        public static int LogOffTime
        {
            get
            {
                return _logOffTime;
            }

            set
            {
                _logOffTime = value;
            }
        }

        public static void StartAutoLogoffOption()
        {
            System.Windows.Interop.ComponentDispatcher.ThreadIdle += new EventHandler(DispatcherQueueEmptyHandler);
        }

        /// <summary>
        /// Timer tick.
        /// </summary>
        /// <param name="sender">Object Sender.</param>
        /// <param name="e">Event Args.</param>
        public static void TimerTick(object sender, EventArgs e)
        {
            if (_timer != null)
            {
                System.Windows.Interop.ComponentDispatcher.ThreadIdle -= new EventHandler(DispatcherQueueEmptyHandler);
                _timer.Stop();
                _timer = null;

                if (MakeAutoLogOffEvent != null)
                {
                    MakeAutoLogOffEvent(sender, e);
                }
            }
        }

        /// <summary>
        /// Dispatcher Queue Empty Handler.
        /// </summary>
        /// <param name="sender">Object Sender.</param>
        /// <param name="e">Event Args.</param>
        public static void DispatcherQueueEmptyHandler(object sender, EventArgs e)
        {
            if (_timer == null)
            {
                _timer = new Timer();
                _timer.Interval = LogOffTime * 1000;
                _timer.Tick += new EventHandler(TimerTick);
                _timer.Enabled = true;
            }
            else if (_timer.Enabled == false)
            {
                _timer.Enabled = true;
            }
        }

        public static void ResetLogoffTimer()
        {
            if (_timer != null)
            {
                _timer.Enabled = false;
                _timer.Enabled = true;
            }
        }
    }
}
