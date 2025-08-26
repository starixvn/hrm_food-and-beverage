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
        private readonly Timer _updateCounterTimer;
        private readonly CommonService _commonService;

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
            _logfile.Info($"Service started at {DateTime.Now}");
            _updateCounterTimer.Enabled = true;
        }

        protected override void OnStop()
        {
            _updateCounterTimer.Enabled = false;
            _logfile.Info($"Service stopped at {DateTime.Now}");
        }

        private void ExecuteJob(object sender, ElapsedEventArgs e)
        {
            if (!_isBusy)
            {
                _isBusy = true;

                UpdateInfoForNexcel();
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