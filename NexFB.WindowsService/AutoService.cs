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
    using System.Reflection;
    using System.ServiceProcess;
    using System.Timers;
    using log4net;

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
        private void ExecuteJob(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!_isBusy)
                {
                    _isBusy = true;
                    _LOG.Info("Update Teams Nexcel started at " + DateTime.Now.ToString());
                    _service.UpdateTeamForNexcel();

                    _LOG.Info("Update user Nexcel started at " + DateTime.Now.ToString());
                    _service.UpdateUserForNexcel();

                    _LOG.Info("Update Teams Hong Ma started at " + DateTime.Now.ToString());
                    _service.UpdateTeamForHongMa();

                    _LOG.Info("SUpdate user Hong Ma started at " + DateTime.Now.ToString());
                    _service.UpdateUserForHongMa();
                    _isBusy = false;
                }
            }
            catch (Exception ex)
            {
                _LOG.Error("Error: " + ex.Message);
            }
        }
    }
}
