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
    using System.Windows.Documents;
    using NexFB.Criteria.Kitchen;
    using NexFB.Models.Kitchen;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.WinApp.Core.Constants;
    using NexFB.WinApp.Core.NotifyBase;
    using NexFB.WinApp.Core.PrintHelper;

    /// <summary>
    /// ViewModel Base.
    /// </summary>
    public class ViewModelBase : PropertyChangedBase
    {
        #region Fields

        private IKitchenService _kitchenService;
        private IPrintHelper _printingHelper;

        #endregion

        #region Constructors

        public ViewModelBase(IKitchenService kitchenService, IPrintHelper printingHelper)
        {
            _kitchenService = kitchenService ?? new KitchenService();
            _printingHelper = printingHelper ?? new PrintHelper();
        }

        public ViewModelBase()
            : this(null, null)
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

        #endregion

        #region Methods

        /// <summary>
        /// Method Print Invoice.
        /// </summary>
        /// <param name="orderId">Id of printing order.</param>
        /// <param name="invoiceNumber">First or Second invoice.</param>
        /// <returns>Check is printed.</returns>
        public bool ExecutePrintInvoice(int orderId, int invoiceNumber)
        {
            string releaseMode = ConfigurationManager.AppSettings[Constant.ReleaseMode];
            var template = (FixedDocument)_printingHelper.LoadTemplate(Constant.OrderFoodReceiptTemplate);
            var criteria = new GetOrderDetailsCriteria() { OrderId = orderId };
            Order model = _kitchenService.GetOrderDetails(criteria);
            model.InvoiceNumber = invoiceNumber;
            _printingHelper.InjectData(template, model);

            if (!string.IsNullOrEmpty(releaseMode))
            {
                switch (releaseMode.ToUpper())
                {
                    case "DEV":
                        string receiptDocument = System.IO.Path.GetTempFileName() + ".xps";
                        string[] paths = receiptDocument.Split('\\');
                        var fileName = paths[paths.Length - 1];
                        string receiptDocumentCustom = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\" + fileName;
                        _printingHelper.ConvertToXps(template, receiptDocumentCustom);
                        return true;

                    case "RELEASE":
                        bool printed = false;
                        printed = _printingHelper.Print(((IDocumentPaginatorSource)template).DocumentPaginator);
                        return printed;
                }
            }

            return false;
        }

        #endregion
    }
}
