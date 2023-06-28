namespace NexFB.WindowsService
{
    using System;
    using System.Reflection;
    using System.ServiceProcess;
    using System.Timers;
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using log4net;
    using NexFB.Core.Models;

    public partial class AutoService : ServiceBase
    {
        private static readonly ILog _logfile = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private bool _isBusy = false;
        private Timer _updateCounterTimer = null;
        private CommonService _commonService = null;

        public AutoService()
        {
            InitializeComponent();
            _commonService = new CommonService();

            const short Milliseconds = 1000;
            _updateCounterTimer = new Timer(AppSettings.TimerInterval * Milliseconds);
            _updateCounterTimer.Elapsed += new ElapsedEventHandler(ExecuteJob);
            _updateCounterTimer.Enabled = false;
        }

        protected override void OnStart(string[] args)
        {
            _logfile.Info("Service started at " + DateTime.Now.ToString());
            _updateCounterTimer.Enabled = true;
        }

        protected override void OnStop()
        {
            _updateCounterTimer.Enabled = false;
            _logfile.Info("Service stopped at " + DateTime.Now.ToString());
        }

        private void ExecuteJob(object sender, ElapsedEventArgs e)
        {
            if (!_isBusy)
            {
                _isBusy = true;

                UpdateInfoForNexcel();
                UpdateInfoForHM();
                UpdateInfoForSMS();
                UpdateInfoForLuminet();
                SetDefaultVegetarianManagement();
                UpdateBreakRuleOrder();

                _isBusy = false;
            }
        }

        private void UpdateBreakRuleOrder()
        {
            try
            {
                _logfile.Info("Set Default Vegetarian Management started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateBreakRuleOrder();
                _logfile.Info("Result of Set Default Vegetarian Management: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void SetDefaultVegetarianManagement()
        {
            try
            {
                _logfile.Info("Set Default Vegetarian Management started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.SetDefaultVegetarianManagement();
                _logfile.Info("Result of Set Default Vegetarian Management: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void UpdateInfoForNexcel()
        {
            try
            {
                if (AppSettings.EnableNexcel)
                {
                    UpdateTeamNexcel();
                    UpdateUserNexcel();
                    UpdateShiftNexcel();
                    UpdateCalendarNexcel();
                }

                if (AppSettings.IsTransferPhotoNexcel)
                {
                    UpdatePhotoNexcel();
                }
            }
            catch (Exception ex)
            {
                _isBusy = false;
                HandleError(ex);
            }
        }

        private void UpdateInfoForHM()
        {
            try
            {
                if (AppSettings.EnableHongMA)
                {
                    UpdateTeamHM();
                    UpdateSectionHM();
                    UpdateDepartmentHM();
                    UpdateUnitHM();
                    UpdateUserHM();
                    UpdateShiftHM();
                    UpdateCalendarHM();
                }

                if (AppSettings.IsTransferPhotoHongMA)
                {
                    UpdatePhotoHM();
                }
            }
            catch (Exception ex)
            {
                _isBusy = false;
                HandleError(ex);
            }
        }

        private void UpdateInfoForSMS()
        {
            try
            {
                if (AppSettings.EnableSms)
                {
                    UpdateTeamSMS();
                    UpdateSectionSMS();
                    UpdateDepartmentSMS();
                    UpdateUnitSMS();
                    UpdateUserSMS();
                    UpdateShiftSMS();
                    UpdateCalendarSMS();
                }

                if (AppSettings.IsTransferPhotoSms)
                {
                    UpdatePhotoSMS();
                }
            }
            catch (Exception ex)
            {
                _isBusy = false;
                HandleError(ex);
            }
        }

        private void UpdateInfoForLuminet()
        {
            try
            {
                if (AppSettings.EnableLuminet)
                {
                    UpdateTeamLuminet();
                    UpdateUserLuminet();
                    UpdateShiftLuminet();
                    UpdateCalendarLuminet();
                }

                if (AppSettings.IsTransferPhotoLuminet)
                {
                    UpdatePhotoLuminet();
                }
            }
            catch (Exception ex)
            {
                _isBusy = false;
                HandleError(ex);
            }
        }

        private void UpdateTeamNexcel()
        {
            try
            {
                _logfile.Info("Update Teams Nexcel started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateTeamForNexcel();
                _logfile.Info("Result of update team Nexcel: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void UpdateUserNexcel()
        {
            try
            {
                _logfile.Info("Update user Nexcel started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateUserForNexcel();
                _logfile.Info("Result of update user Nexcel: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void UpdateShiftNexcel()
        {
            try
            {
                _logfile.Info("Update Shifts Nexcel started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateShiftForNexcel();
                _logfile.Info("Result of update shift Nexcel: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void UpdateCalendarNexcel()
        {
            try
            {
                _logfile.Info("Update Calendars Nexcel started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateEmployeeCalendarForNexcel();
                _logfile.Info("Result of update calendar Nexcel: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void UpdatePhotoNexcel()
        {
            try
            {
                _logfile.Info("Update Photos Nexcel started at " + DateTime.Now.ToString());
                _commonService.UpdatePhotoForNexcel();
                _logfile.Info("Update photos Nexcel done.");
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void UpdateTeamHM()
        {
            try
            {
                _logfile.Info("Update Teams Hong Ma started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateTeamForHM();
                _logfile.Info("Result of update team Hong Ma: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void UpdateSectionHM()
        {
            try
            {
                _logfile.Info("Update Section Hong Ma started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateSectionForHM();
                _logfile.Info("Result of update Section Hong Ma: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void UpdateDepartmentHM()
        {
            try
            {
                _logfile.Info("Update Department Hong Ma started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateDepartmentForHM();
                _logfile.Info("Result of update Department Hong Ma: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void UpdateUnitHM()
        {
            try
            {
                _logfile.Info("Update Unit Hong Ma started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateUnitForHM();
                _logfile.Info("Result of update Unit Hong Ma: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void UpdateUserHM()
        {
            try
            {
                _logfile.Info("SUpdate user Hong Ma started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateUserForHM();
                _logfile.Info("Result of update user Hong Ma: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void UpdateShiftHM()
        {
            try
            {
                _logfile.Info("Update Shifts Hong Ma started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateShiftForHM();
                _logfile.Info("Result of update shift Hong Ma: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void UpdateCalendarHM()
        {
            try
            {
                _logfile.Info("Update Calendars Hong Ma started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateEmployeeCalendarForHM();
                _logfile.Info("Result of update calendar Hong Ma: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void UpdatePhotoHM()
        {
            try
            {
                _logfile.Info("Update Photos Hong Ma started at " + DateTime.Now.ToString());
                _commonService.UpdatePhotoForHM();
                _logfile.Info("Update photos Hong Ma done.");
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void UpdateTeamSMS()
        {
            try
            {
                _logfile.Info("Update Teams SMS started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateTeamForSms();
                _logfile.Info("Result of update team SMS: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void UpdateSectionSMS()
        {
            try
            {
                _logfile.Info("Update Section SMS started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateSectionForSMS();
                _logfile.Info("Result of update Section SMS: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void UpdateDepartmentSMS()
        {
            try
            {
                _logfile.Info("Update Department SMS started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateDepartmentForSMS();
                _logfile.Info("Result of update Department SMS: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void UpdateUnitSMS()
        {
            try
            {
                _logfile.Info("Update Unit SMS started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateUnitForSMS();
                _logfile.Info("Result of update Unit SMS: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void UpdateUserSMS()
        {
            try
            {
                _logfile.Info("SUpdate user SMS started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateUserForSms();
                _logfile.Info("Result of update user SMS: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void UpdateShiftSMS()
        {
            try
            {
                _logfile.Info("Update Shifts SMS started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateShiftForSms();
                _logfile.Info("Result of update shift SMS: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void UpdateCalendarSMS()
        {
            try
            {
                _logfile.Info("Update Calendars SMS started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateEmployeeCalendarForSms();
                _logfile.Info("Result of update calendar SMS: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void UpdatePhotoSMS()
        {
            try
            {
                _logfile.Info("Update Photos SMS started at " + DateTime.Now.ToString());
                _commonService.UpdatePhotoForSms();
                _logfile.Info("Update photos SMS done.");
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void UpdateTeamLuminet()
        {
            try
            {
                _logfile.Info("Update Teams Luminet started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateTeamForLuminet();
                _logfile.Info("Result of update team Luminet: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void UpdateUserLuminet()
        {
            try
            {
                _logfile.Info("Update user Luminet started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateUserForLuminet();
                _logfile.Info("Result of update user Luminet: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void UpdateShiftLuminet()
        {
            try
            {
                _logfile.Info("Update Shifts Luminet started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateShiftForLuminet();
                _logfile.Info("Result of update shift Luminet: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void UpdateCalendarLuminet()
        {
            try
            {
                _logfile.Info("Update Calendars Luminet started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateEmployeeCalendarForLuminet();
                _logfile.Info("Result of update calendar Luminet: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void UpdatePhotoLuminet()
        {
            try
            {
                _logfile.Info("Update Photos Luminet started at " + DateTime.Now.ToString());
                _commonService.UpdatePhotoForLuminet();
                _logfile.Info("Update photos Luminet done.");
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private static void HandleError(Exception exception)
        {
            var logMessage = new LogMessage(exception.Message)
                .AddSection("Database Info", DatabaseInfoGetters.GetDbExceptionInfo(exception))
                .AddSection("Server Info", CommonInfoGetters.GetServerInfo())
                .AddSection("Exception", CommonInfoGetters.GetExceptionInfo(exception))
                .ToString();

            LogService.Error(logMessage);
        }
    }
}