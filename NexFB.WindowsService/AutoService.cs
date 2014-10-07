// --------------------------------------------------------------------------
// <copyright file="AutoService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.4">Created</change>
// </history>

namespace NexFB.WindowsService
{
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.ServiceProcess;
    using System.Timers;
    using log4net;
    using NexFB.Core.Models;

    /// <summary>
    /// Auto Service.
    /// </summary>
    public partial class AutoService : ServiceBase
    {
        /// <summary>
        /// Default interval for Ticker is 300 seconds.
        /// </summary>
        private const int _DEFAULT_TIMER_INTERVAL = 300;
        private static readonly ILog _LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private bool _isBusy = false;
        private Timer _updateCounterTimer = null;
        private CommonService _service = null;

        public AutoService()
        {
            InitializeComponent();
            _service = new CommonService();

            TimerInterval = int.Parse(ConfigurationManager.AppSettings["TimerInterval"]);
            _updateCounterTimer = new Timer(TimerInterval * 1000);
            _updateCounterTimer.Elapsed += new ElapsedEventHandler(ExecuteJob);
            _updateCounterTimer.Enabled = false;
        }

        public int TimerInterval { get; private set; }

        protected override void OnStart(string[] args)
        {
            _LOG.Info("Service started at " + DateTime.Now.ToString());
            _updateCounterTimer.Enabled = true;
        }

        protected override void OnStop()
        {
            _updateCounterTimer.Enabled = false;
            _LOG.Info("Service stopped at " + DateTime.Now.ToString());
        }

        /// <summary>
        /// Function to execute job.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">The ElapsedEventArgs.</param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        private void ExecuteJob(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!_isBusy)
                {
                    _isBusy = true;

                    UpdateInfoForNexcel();
                    UpdateInfoForHM();
                    UpdateInfoForSMS();

                    _isBusy = false;
                }
            }
            catch (Exception ex)
            {
                _isBusy = false;
                _LOG.Error("Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Function to update info for NEXCEL.
        /// </summary>
        private void UpdateInfoForNexcel()
        {
            CommandResult result = null;

            _LOG.Info("Update Teams Nexcel started at " + DateTime.Now.ToString());
            result = _service.UpdateTeamForNexcel();
            _LOG.Info("Result of update team Nexcel: " + (result == null ? "0" : result.Message));

            _LOG.Info("Update user Nexcel started at " + DateTime.Now.ToString());
            result = _service.UpdateUserForNexcel();
            _LOG.Info("Result of update user Nexcel: " + (result == null ? "0" : result.Message));

            _LOG.Info("Update Shifts Nexcel started at " + DateTime.Now.ToString());
            result = _service.UpdateShiftForNexcel();
            _LOG.Info("Result of update shift Nexcel: " + (result == null ? "0" : result.Message));

            _LOG.Info("Update Calendars Nexcel started at " + DateTime.Now.ToString());
            result = _service.UpdateEmployeeCalendarForNexcel();
            _LOG.Info("Result of update calendar Nexcel: " + (result == null ? "0" : result.Message));

            _LOG.Info("Update Photos Nexcel started at " + DateTime.Now.ToString());
            _service.UpdatePhotoForNexcel();
            _LOG.Info("Update photos Nexcel done.");
        }

        /// <summary>
        /// Function to update info for Hong Ma.
        /// </summary>
        private void UpdateInfoForHM()
        {
            CommandResult result = null;

            _LOG.Info("Update Teams Hong Ma started at " + DateTime.Now.ToString());
            result = _service.UpdateTeamForHM();
            _LOG.Info("Result of update team Hong Ma: " + (result == null ? "0" : result.Message));

            _LOG.Info("SUpdate user Hong Ma started at " + DateTime.Now.ToString());
            result = _service.UpdateUserForHM();
            _LOG.Info("Result of update user Hong Ma: " + (result == null ? "0" : result.Message));

            _LOG.Info("Update Shifts Hong Ma started at " + DateTime.Now.ToString());
            result = _service.UpdateShiftForHM();
            _LOG.Info("Result of update shift Hong Ma: " + (result == null ? "0" : result.Message));

            _LOG.Info("Update Calendars Hong Ma started at " + DateTime.Now.ToString());
            result = _service.UpdateEmployeeCalendarForHM();
            _LOG.Info("Result of update calendar Hong Ma: " + (result == null ? "0" : result.Message));

            _LOG.Info("Update Photos Hong Ma started at " + DateTime.Now.ToString());
            _service.UpdatePhotoForHM();
            _LOG.Info("Update photos Hong Ma done.");
        }

        /// <summary>
        /// Function to update info for SMS.
        /// </summary>
        private void UpdateInfoForSMS()
        {
            CommandResult result = null;

            _LOG.Info("Update Teams SMS started at " + DateTime.Now.ToString());
            result = _service.UpdateTeamForSms();
            _LOG.Info("Result of update team SMS: " + (result == null ? "0" : result.Message));

            _LOG.Info("SUpdate user SMS started at " + DateTime.Now.ToString());
            result = _service.UpdateUserForSms();
            _LOG.Info("Result of update user SMS: " + (result == null ? "0" : result.Message));

            _LOG.Info("Update Shifts SMS started at " + DateTime.Now.ToString());
            result = _service.UpdateShiftForSms();
            _LOG.Info("Result of update shift SMS: " + (result == null ? "0" : result.Message));

            _LOG.Info("Update Calendars SMS started at " + DateTime.Now.ToString());
            result = _service.UpdateEmployeeCalendarForSms();
            _LOG.Info("Result of update calendar SMS: " + (result == null ? "0" : result.Message));

            _LOG.Info("Update Photos SMS started at " + DateTime.Now.ToString());
            _service.UpdatePhotoForSms();
            _LOG.Info("Update photos SMS done.");
        }
    }
}
