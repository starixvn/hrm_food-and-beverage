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
        private static readonly ILog _LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private bool _isBusy = false;
        private Timer _updateCounterTimer = null;
        private CommonService _commonService = null;
        private SynchronizeEmployeeService _epaService = null;

        public AutoService()
        {
            InitializeComponent();
            _commonService = new CommonService();
            _epaService = new SynchronizeEmployeeService();

            TimerInterval = int.Parse(ConfigurationManager.AppSettings["TimerInterval"]);
            EnableNexcel = bool.Parse(ConfigurationManager.AppSettings["EnableNexcel"]);
            EnableHongMA = bool.Parse(ConfigurationManager.AppSettings["EnableHongMa"]);
            EnableSms = bool.Parse(ConfigurationManager.AppSettings["EnableSMS"]);
            EnableSyncDataForEpa = bool.Parse(ConfigurationManager.AppSettings["EnableSyncDataForEPA"]);
            IsTransferPhotoNexcel = bool.Parse(ConfigurationManager.AppSettings["IsTransferPhotoNexcel"]);
            IsTransferPhotoHongMA = bool.Parse(ConfigurationManager.AppSettings["IsTransferPhotoHongMa"]);
            IsTransferPhotoSms = bool.Parse(ConfigurationManager.AppSettings["IsTransferPhotoSMS"]);
            IsTransferPhotoSmsEpa = bool.Parse(ConfigurationManager.AppSettings["IsTransferPhotoForSMSEPA"]); 
            _updateCounterTimer = new Timer(TimerInterval * 1000);
            _updateCounterTimer.Elapsed += new ElapsedEventHandler(ExecuteJob);
            _updateCounterTimer.Enabled = false;
        }

        public int TimerInterval { get; private set; }

        public bool EnableNexcel { get; private set; }

        public bool EnableHongMA { get; private set; }

        public bool EnableSms { get; private set; }

        public bool EnableSyncDataForEpa { get; private set; }

        public bool IsTransferPhotoNexcel { get; private set; }

        public bool IsTransferPhotoHongMA { get; private set; }

        public bool IsTransferPhotoSms { get; private set; }

        public bool IsTransferPhotoSmsEpa { get; private set; }

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
                    UpdateInfoForEPA();

                    _isBusy = false;
                }
            }
            catch (Exception ex)
            {
                _isBusy = false;
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

        private void UpdateInfoForNexcel()
        {
            if (EnableNexcel)
            {
                UpdateTeamNexcel();
                UpdateUserNexcel();
                UpdateShiftNexcel();
                UpdateCalendarNexcel();
            }

            if (IsTransferPhotoNexcel)
            {
                UpdatePhotoNexcel();
            }
        }

        private void UpdateInfoForHM()
        {
            if (EnableHongMA)
            {
                UpdateTeamHM();
                UpdateUserHM();
                UpdateShiftHM();
                UpdateCalendarHM();
            }

            if (IsTransferPhotoHongMA)
            {
                UpdatePhotoHM();
            }
        }

        private void UpdateInfoForSMS()
        {
            if (EnableSms)
            {
                UpdateTeamSMS();
                UpdateUserSMS();
                UpdateShiftSMS();
                UpdateCalendarSMS();
            }

            if (IsTransferPhotoSms)
            {
                UpdatePhotoSMS();
            }
        }

        private void UpdateInfoForEPA()
        {
            if (EnableSyncDataForEpa)
            {
                UpdateDepartmentForEPA();
                UpdateTeamForEPA();
                UpdateJobTitleForEPA();
                UpdateUserForEPA();
                UpdateEmployeeSupervisorForEPA();
                UpdateGroupSupervisorForEPA();
                UpdateAttendanceViolationForEPA();
                //// UpdateShiftForEPA();
                //// UpdateCalendarForEPA();
                //// UpdateLeaveTypeForEPA();
                //// UpdateRequestForEPA();
                //// UpdateRequestHistoryForEPA();
                //// UpdateAttendanceForEPA();
            }

            if (IsTransferPhotoSmsEpa)
            {
                UpdatePhotoForEPA();
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        private void UpdateTeamNexcel()
        {
            try
            {
                _LOG.Info("Update Teams Nexcel started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateTeamForNexcel();
                _LOG.Info("Result of update team Nexcel: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        private void UpdateUserNexcel()
        {
            try
            {
                _LOG.Info("Update user Nexcel started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateUserForNexcel();
                _LOG.Info("Result of update user Nexcel: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        private void UpdateShiftNexcel()
        {
            try
            {
                _LOG.Info("Update Shifts Nexcel started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateShiftForNexcel();
                _LOG.Info("Result of update shift Nexcel: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        private void UpdateCalendarNexcel()
        {
            try
            {
                _LOG.Info("Update Calendars Nexcel started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateEmployeeCalendarForNexcel();
                _LOG.Info("Result of update calendar Nexcel: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        private void UpdatePhotoNexcel()
        {
            try
            {
                _LOG.Info("Update Photos Nexcel started at " + DateTime.Now.ToString());
                _commonService.UpdatePhotoForNexcel();
                _LOG.Info("Update photos Nexcel done.");
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        private void UpdateTeamHM()
        {
            try
            {
                _LOG.Info("Update Teams Hong Ma started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateTeamForHM();
                _LOG.Info("Result of update team Hong Ma: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        private void UpdateUserHM()
        {
            try
            {
                _LOG.Info("SUpdate user Hong Ma started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateUserForHM();
                _LOG.Info("Result of update user Hong Ma: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        private void UpdateShiftHM()
        {
            try
            {
                _LOG.Info("Update Shifts Hong Ma started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateShiftForHM();
                _LOG.Info("Result of update shift Hong Ma: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        private void UpdateCalendarHM()
        {
            try
            {
                _LOG.Info("Update Calendars Hong Ma started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateEmployeeCalendarForHM();
                _LOG.Info("Result of update calendar Hong Ma: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        private void UpdatePhotoHM()
        {
            try
            {
                _LOG.Info("Update Photos Hong Ma started at " + DateTime.Now.ToString());
                _commonService.UpdatePhotoForHM();
                _LOG.Info("Update photos Hong Ma done.");
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        private void UpdateTeamSMS()
        {
            try
            {
                _LOG.Info("Update Teams SMS started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateTeamForSms();
                _LOG.Info("Result of update team SMS: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        private void UpdateUserSMS()
        {
            try
            {
                _LOG.Info("SUpdate user SMS started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateUserForSms();
                _LOG.Info("Result of update user SMS: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        private void UpdateShiftSMS()
        {
            try
            {
                _LOG.Info("Update Shifts SMS started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateShiftForSms();
                _LOG.Info("Result of update shift SMS: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        private void UpdateCalendarSMS()
        {
            try
            {
                _LOG.Info("Update Calendars SMS started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateEmployeeCalendarForSms();
                _LOG.Info("Result of update calendar SMS: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        private void UpdatePhotoSMS()
        {
            try
            {
                _LOG.Info("Update Photos SMS started at " + DateTime.Now.ToString());
                _commonService.UpdatePhotoForSms();
                _LOG.Info("Update photos SMS done.");
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        private void UpdateDepartmentForEPA()
        {
            try
            {
                _LOG.Info("Update Departments (EPA) started at " + DateTime.Now.ToString());
                CommandResult result = _epaService.UpdateDepartment();
                _LOG.Info("Result of update Department (EPA): " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        private void UpdateTeamForEPA()
        {
            try
            {
                _LOG.Info("Update Teams (EPA) started at " + DateTime.Now.ToString());
                CommandResult result = _epaService.UpdateTeam();
                _LOG.Info("Result of update team (EPA): " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        private void UpdateUserForEPA()
        {
            try
            {
                _LOG.Info("SUpdate User (EPA) started at " + DateTime.Now.ToString());
                CommandResult result = _epaService.UpdateUser();
                _LOG.Info("Result of update user (EPA): " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        private void UpdatePhotoForEPA()
        {
            try
            {
                _LOG.Info("Update Photos (EPA) started at " + DateTime.Now.ToString());
                _epaService.UpdatePhoto();
                _LOG.Info("Update photos (EPA) done.");
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        private void UpdateJobTitleForEPA()
        {
            try
            {
                _LOG.Info("Update JobTitles (EPA) started at " + DateTime.Now.ToString());
                CommandResult result = _epaService.UpdateJobTitle();
                _LOG.Info("Result of update JobTitle (EPA): " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        private void UpdateAttendanceViolationForEPA()
        {
            try
            {
                _LOG.Info("Update Attendance Violation (EPA) started at " + DateTime.Now.ToString());
                CommandResult result = _epaService.UpdateEmployeeAttendanceViolation();
                _LOG.Info("Result of update Attendance Violation (EPA): " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        private void UpdateEmployeeSupervisorForEPA()
        {
            try
            {
                _LOG.Info("Update Employee Supervisor (EPA) started at " + DateTime.Now.ToString());
                CommandResult result = _epaService.UpdateEmployeeSupervisor();
                _LOG.Info("Result of update Employee Supervisor (EPA): " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        private void UpdateGroupSupervisorForEPA()
        {
            try
            {
                _LOG.Info("Update Group Supervisor (EPA) started at " + DateTime.Now.ToString());
                CommandResult result = _epaService.UpdateGroupSupervisor();
                _LOG.Info("Result of update Group Supervisor (EPA): " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

        ////[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        ////private void UpdateAttendanceForEPA()
        ////{
        ////    try
        ////    {
        ////        _LOG.Info("Update Attendance (EPA) started at " + DateTime.Now.ToString());
        ////        CommandResult result = _epaService.UpdateEmployeeAttendance();
        ////        _LOG.Info("Result of update Attendance (EPA): " + (result == null ? "0" : result.Message));
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        _LOG.Error("Error: " + ex.InnerException.Message);
        ////    }
        ////}

        ////[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        ////private void UpdateLeaveTypeForEPA()
        ////{
        ////    try
        ////    {
        ////        _LOG.Info("Update LeaveType (EPA) started at " + DateTime.Now.ToString());
        ////        CommandResult result = _epaService.UpdateLeaveType();
        ////        _LOG.Info("Result of update LeaveType (EPA): " + (result == null ? "0" : result.Message));
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        _LOG.Error("Error: " + ex.InnerException.Message);
        ////    }
        ////}

        ////[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        ////private void UpdateShiftForEPA()
        ////{
        ////    try
        ////    {
        ////        _LOG.Info("Update Shifts (EPA) started at " + DateTime.Now.ToString());
        ////        CommandResult result = _epaService.UpdateShift();
        ////        _LOG.Info("Result of update shift (EPA): " + (result == null ? "0" : result.Message));
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        _LOG.Error("Error: " + ex.InnerException.Message);
        ////    }
        ////}

        ////[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        ////private void UpdateCalendarForEPA()
        ////{
        ////    try
        ////    {
        ////        _LOG.Info("Update Calendars (EPA) started at " + DateTime.Now.ToString());
        ////        CommandResult result = _epaService.UpdateEmployeeCalendar();
        ////        _LOG.Info("Result of update calendar (EPA): " + (result == null ? "0" : result.Message));
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        _LOG.Error("Error: " + ex.InnerException.Message);
        ////    }
        ////}

        ////[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        ////private void UpdateRequestForEPA()
        ////{
        ////    try
        ////    {
        ////        _LOG.Info("Update Request (EPA) started at " + DateTime.Now.ToString());
        ////        CommandResult result = _epaService.UpdateEmployeeRequest();
        ////        _LOG.Info("Result of update Request (EPA): " + (result == null ? "0" : result.Message));
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        _LOG.Error("Error: " + ex.InnerException.Message);
        ////    }
        ////}

        ////[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        ////private void UpdateRequestHistoryForEPA()
        ////{
        ////    try
        ////    {
        ////        _LOG.Info("Update Request history (EPA) started at " + DateTime.Now.ToString());
        ////        CommandResult result = _epaService.UpdateEmployeeRequestHistory();
        ////        _LOG.Info("Result of update Request history (EPA): " + (result == null ? "0" : result.Message));
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        _LOG.Error("Error: " + ex.InnerException.Message);
        ////    }
        ////}
    }
}
