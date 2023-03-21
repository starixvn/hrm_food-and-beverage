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

                _isBusy = false;
            }
        }

        private void SetDefaultVegetarianManagement()
        {
            try
            {
                _LOG.Info("Set Default Vegetarian Management started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.SetDefaultVegetarianManagement();
                _LOG.Info("Result of Set Default Vegetarian Management: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
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
                    UpdateSectionHM();
                    UpdateDepartmentHM();
                    UpdateUnitHM();
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
                    UpdateSectionSMS();
                    UpdateDepartmentSMS();
                    UpdateUnitSMS();
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

        private void UpdateSectionHM()
        {
            try
            {
                _LOG.Info("Update Section Hong Ma started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateSectionForHM();
                _LOG.Info("Result of update Section Hong Ma: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

        private void UpdateDepartmentHM()
        {
            try
            {
                _LOG.Info("Update Department Hong Ma started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateDepartmentForHM();
                _LOG.Info("Result of update Department Hong Ma: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

        private void UpdateUnitHM()
        {
            try
            {
                _LOG.Info("Update Unit Hong Ma started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateUnitForHM();
                _LOG.Info("Result of update Unit Hong Ma: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

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

        private void UpdateSectionSMS()
        {
            try
            {
                _LOG.Info("Update Section SMS started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateSectionForSMS();
                _LOG.Info("Result of update Section SMS: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

        private void UpdateDepartmentSMS()
        {
            try
            {
                _LOG.Info("Update Department SMS started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateDepartmentForSMS();
                _LOG.Info("Result of update Department SMS: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

        private void UpdateUnitSMS()
        {
            try
            {
                _LOG.Info("Update Unit SMS started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateUnitForSMS();
                _LOG.Info("Result of update Unit SMS: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

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

        private void UpdateTeamLuminet()
        {
            try
            {
                _LOG.Info("Update Teams Luminet started at " + DateTime.Now.ToString());
                CommandResult result = _commonService.UpdateTeamForLuminet();
                _LOG.Info("Result of update team Luminet: " + (result == null ? "0" : result.Message));
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.InnerException.Message);
            }
        }

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
