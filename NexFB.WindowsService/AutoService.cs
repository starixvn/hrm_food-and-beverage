namespace NexFB.WindowsService
{
    using log4net;
    using NexFB.Core.Enums;
    using NexFB.Core.Models;
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.ServiceProcess;
    using System.Timers;

    public partial class AutoService : ServiceBase
    {
        private static readonly ILog _LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private bool _isBusy = false;
        private Timer _updateCounterTimer = null;
        private CommonService _commonService = null;

        public AutoService()
        {
            InitializeComponent();
            _commonService = new CommonService();

            TimerInterval = int.Parse(ConfigurationManager.AppSettings["TimerInterval"]);
            EnableNexcel = bool.Parse(ConfigurationManager.AppSettings["EnableNexcel"]);
            EnableHongMA = bool.Parse(ConfigurationManager.AppSettings["EnableHongMa"]);
            EnableSms = bool.Parse(ConfigurationManager.AppSettings["EnableSMS"]);
            EnableLuminet = bool.Parse(ConfigurationManager.AppSettings["EnableLuminet"]);
            IsTransferPhotoNexcel = bool.Parse(ConfigurationManager.AppSettings["IsTransferPhotoNexcel"]);
            IsTransferPhotoHongMA = bool.Parse(ConfigurationManager.AppSettings["IsTransferPhotoHongMa"]);
            IsTransferPhotoSms = bool.Parse(ConfigurationManager.AppSettings["IsTransferPhotoSMS"]);
            IsTransferPhotoLuminet = bool.Parse(ConfigurationManager.AppSettings["IsTransferPhotoLuminet"]);
            _updateCounterTimer = new Timer(TimerInterval * 1000);
            _updateCounterTimer.Elapsed += new ElapsedEventHandler(ExecuteJob);
            _updateCounterTimer.Enabled = false;
        }

        public int TimerInterval { get; private set; }

        public bool EnableNexcel { get; private set; }

        public bool EnableHongMA { get; private set; }

        public bool EnableSms { get; private set; }

        public bool EnableLuminet { get; private set; }

        public bool IsTransferPhotoNexcel { get; private set; }

        public bool IsTransferPhotoHongMA { get; private set; }

        public bool IsTransferPhotoSms { get; private set; }

        public bool IsTransferPhotoLuminet { get; private set; }

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

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        private void ExecuteJob(object sender, ElapsedEventArgs e)
        {
            if (!_isBusy)
            {
                _isBusy = true;

                UpdateInfoForNexcel();
                UpdateInfoForHM();
                UpdateInfoForSMS();
                UpdateInfoForLuminet();

                _isBusy = false;
            }
        }

        private void UpdateInfoForNexcel()
        {
            try
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
            catch (Exception ex)
            {
                _isBusy = false;
                _LOG.Error("Error Nexcel: " + ex.InnerException.Message);
            }
        }

        private void UpdateInfoForHM()
        {
            try
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
            catch (Exception ex)
            {
                _isBusy = false;
                _LOG.Error("Error Nexcel: " + ex.InnerException.Message);
            }
        }

        private void UpdateInfoForSMS()
        {
            try
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
            catch (Exception ex)
            {
                _isBusy = false;
                _LOG.Error("Error Nexcel: " + ex.InnerException.Message);
            }
        }

        private void UpdateInfoForLuminet()
        {
            try
            {
                if (EnableLuminet)
                {
                    UpdateTeamLuminet();
                    UpdateUserLuminet();
                    UpdateShiftLuminet();
                    UpdateCalendarLuminet();
                }

                if (IsTransferPhotoLuminet)
                {
                    UpdatePhotoLuminet();
                }
            }
            catch (Exception ex)
            {
                _isBusy = false;
                _LOG.Error("Error Luminet: " + ex.InnerException.Message);
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
        private void UpdateTeamLuminet()
        {
            try
            {
                _LOG.Info("Update Teams Luminet started at " + DateTime.Now.ToString());
                //CommandResult result = UpdateTeamForLuminet();
                CommandResult result = _commonService.UpdateTeamForLuminet();
                _LOG.Info("Result of update team Luminet: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

        //private CommandResult UpdateTeamForLuminet()
        //{
        //    CommandResult result = null;
        //    DateTime fromDate = _commonService.GetDateFromBeforeUpdateTeam((int)DepartmentIdentity.Luminet);
        //    IList<LuminetService.TeamModel> teams = LuminetService.GetTeam(fromDate);
        //    if (teams != null && teams.Any())
        //    {
        //        var insertCriteria = new InsertAndUpdateTeamLuminetCriteria()
        //        {
        //            DepartmentId = (int)DepartmentIdentity.Luminet,
        //            Teams = teams
        //        };

        //        result = Repository.Update(insertCriteria);
        //    }

        //    return result;
        //}

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        private void UpdateUserLuminet()
        {
            try
            {
                _LOG.Info("Update user Luminet started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateUserForLuminet();
                _LOG.Info("Result of update user Luminet: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        private void UpdateShiftLuminet()
        {
            try
            {
                _LOG.Info("Update Shifts Luminet started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateShiftForLuminet();
                _LOG.Info("Result of update shift Luminet: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        private void UpdateCalendarLuminet()
        {
            try
            {
                _LOG.Info("Update Calendars Luminet started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateEmployeeCalendarForLuminet();
                _LOG.Info("Result of update calendar Luminet: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        private void UpdatePhotoLuminet()
        {
            try
            {
                _LOG.Info("Update Photos Luminet started at " + DateTime.Now.ToString());
                _commonService.UpdatePhotoForLuminet();
                _LOG.Info("Update photos Luminet done.");
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }
    }
}
