// --------------------------------------------------------------------------
// <copyright file="ViewModelBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.8.22">Created</change>
// </history>

namespace NexFB.PosKitchen.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Windows.Documents;
    using System.Windows.Threading;
    using NexFB.Core.Enums;
    using NexFB.Criteria.Kitchen;
    using NexFB.Models.Kitchen;
    using NexFB.Pos.Core.Constants;
    using NexFB.Pos.Core.NotifyBase;
    using NexFB.Pos.Core.PrintHelper;
    using NexFB.PosKitchen.Views;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using WPFMessageBoxCustomControl;

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
            model.MenuCount = PrepareOrderDetailsCount(model);
            if (receiveFromKitchenOrder)
            {
                model.CurrentStatusId = (int)OrderStatus.NewOrder;
            }

            if (model.CurrentStatusId == (int)OrderStatus.ReducedOrder || model.CurrentStatusId == (int)OrderStatus.NewOrder)
            {
                model.InvoiceNumber = invoiceNumber;
                _printingHelper.InjectData(template, model);

                if (!string.IsNullOrEmpty(releaseMode))
                {
                    NotificationWindow notifyWindows = new NotificationWindow();
                    notifyWindows.Show();

                    if (releaseMode.ToUpper().ToString() == Constant.FinalMode)
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
            else
            {
                return true;
            }
        }

        public void ShowErrorMessage(int currentCulture, string message)
        {
            WPFMessageBox.Show(currentCulture, message);
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

        /// <summary>
        /// Group data menu for print.
        /// </summary>
        /// <param name="order">Current printed order.</param>
        /// <returns>List MenuCount object.</returns>
        private List<MenuCount> PrepareOrderDetailsCount(Order order)
        {
            List<MenuCount> menuDetails = new List<MenuCount>();
            if (order != null)
            {
                List<OrderMenu> orderMenus = order.ListMenu;
                var groupItems = orderMenus.GroupBy(
                      x => new
                      {
                          MenuId = x.MenuId,
                          MainDish = x.IsMainMenu,
                          SideDish = x.IsSideDish
                      },
                      x => new
                      {
                          MainDish = x.IsMainMenu,
                          SideDish = x.IsSideDish,
                          MenuVnmName = x.MenuVnmName,
                          MenuEngName = x.MenuEngName,
                          Quantity = x.QuantityOrder
                      }).Select(
                      x => new
                      {
                          MenuId = x.Key.MenuId,
                          ListItem = x.ToList()
                      }).ToList();

                groupItems.ForEach(x => menuDetails.Add(new MenuCount()
                {
                    MenuId = x.MenuId,
                    MenuVnmName = x.ListItem[0].MenuVnmName,
                    MenuEngName = x.ListItem[0].MenuEngName,
                    Quantity = x.ListItem.Sum(y => y.Quantity)
                }));
            }

            return menuDetails;
        }

        #endregion
    }
}
