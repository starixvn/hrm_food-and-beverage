// --------------------------------------------------------------------------
// <copyright file="ViewModelBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.8.22">Created</change>
// </history>

namespace NexFB.WinApp.ViewModels
{
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Documents;
    using System.Windows.Threading;
    using BlogsPrajeesh.BlogSpot.WPFControls;
    using NexFB.Criteria.Kitchen;
    using NexFB.Models.Kitchen;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.WinApp.Core.Constants;
    using NexFB.WinApp.Core.NotifyBase;
    using NexFB.WinApp.Core.PrintHelper;
    using NexFB.WinApp.Views;

    /// <summary>
    /// ViewModel Base.
    /// </summary>
    public class ViewModelBase : PropertyChangedBase
    {
        #region Fields

        private IKitchenService _kitchenService;
        private ICommonService _commonService;
        private IPrintHelper _printingHelper;
        private double _screenWidth;
        private double _screeHeight;

        #endregion

        #region Constructors

        public ViewModelBase(IKitchenService kitchenService, ICommonService commomService, IPrintHelper printingHelper)
        {
            _kitchenService = kitchenService ?? new KitchenService();
            _commonService = commomService ?? new CommonService();
            _printingHelper = printingHelper ?? new PrintHelper();
        }

        public ViewModelBase()
            : this(null, null, null)
        {
        }

        #endregion

        #region Properties

        public IKitchenService KitchenService
        {
            get
            {
                return _kitchenService;
            }

            set
            {
                if (_kitchenService == value)
                {
                    return;
                }

                _kitchenService = value;
                OnPropertyChanged("KitchenService");
            }
        }

        public ICommonService CommonService
        {
            get
            {
                return _commonService;
            }

            set
            {
                if (_commonService == value)
                {
                    return;
                }

                _commonService = value;
                OnPropertyChanged("CommonService");
            }
        }

        public double ScreenWidth
        {
            get
            {
                return _screenWidth;
            }

            set
            {
                _screenWidth = value;
                this.OnPropertyChanged("ScreenWidth");
            }
        }

        public double ScreenHeight
        {
            get
            {
                return _screeHeight;
            }

            set
            {
                _screeHeight = value;
                this.OnPropertyChanged("ScreenHeight");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Method Print Invoice.
        /// </summary>
        /// <param name="orderId">Id of printing order.</param>
        /// <param name="invoiceNumber">First or Second invoice.</param>
        /// <param name="receiveFromKitchenOrder">Is request received from kitchen.</param>
        /// <returns>Check is printed.</returns>
        public bool ExecutePrintInvoice(int orderId, int invoiceNumber, bool receiveFromKitchenOrder)
        {
            string releaseMode = ConfigurationManager.AppSettings[Constant.ReleaseMode];
            var template = (FixedDocument)_printingHelper.LoadTemplate(Constant.OrderFoodReceiptTemplate);
            var criteria = new GetOrderDetailsCriteria() { OrderId = orderId };
            Order model = _kitchenService.GetOrderDetails(criteria);
            if (receiveFromKitchenOrder)
            {
                model.CurrentStatusId = 1;
            }

            model.InvoiceNumber = invoiceNumber;
            _printingHelper.InjectData(template, model);

            if (!string.IsNullOrEmpty(releaseMode))
            {
                NotificationWindow notifyWindows = new NotificationWindow();
                notifyWindows.Show();

                if (releaseMode.ToUpper().ToString() == "RELEASE")
                {
                    bool printed = false;
                    printed = _printingHelper.Print(((IDocumentPaginatorSource)template).DocumentPaginator);
                    StartCloseTimer(notifyWindows);

                    return printed;
                }
                else
                {
                    string receiptDocument = System.IO.Path.GetTempFileName() + ".xps";
                    string[] paths = receiptDocument.Split('\\');
                    var fileName = paths[paths.Length - 1];
                    string receiptDocumentCustom = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\" + fileName;
                    _printingHelper.ConvertToXps(template, receiptDocumentCustom);
                    StartCloseTimer(notifyWindows);
                    return true;
                }
            }

            return false;
        }

        public void ShowErrorMessage(string message)
        {
            WPFMessageBox.Show(message);
        }

        private void StartCloseTimer(NotificationWindow windows)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1.5d);
            timer.Tick += (sender, e) => TimerTick(sender, windows);
            timer.Start();
        }

        private void TimerTick(object sender, NotificationWindow windows)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            timer.Stop();
            windows.Close();
        }

        #endregion
    }
}
