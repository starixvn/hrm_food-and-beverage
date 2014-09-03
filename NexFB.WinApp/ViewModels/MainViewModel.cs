// --------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.8.22">Created</change>
// </history>

namespace NexFB.WinApp.ViewModels
{
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Input;
    using NexFB.Criteria.Kitchen;
    using NexFB.Models.Kitchen;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.WinApp.Core.Commands;
    using NexFB.WinApp.Core.PrintHelper;

    /// <summary>
    /// Main ViewModel.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Fields
        private const string OrderFoodReceiptTemplate = "PrintingTemplates\\OrderFoodReceipt.xaml";

        private static readonly WaitingMealViewModel WaitingMealViewModel = new WaitingMealViewModel();

        ////private static readonly DeliveredFoodViewModel DeliveredFoodViewModel = new DeliveredFoodViewModel();

        private static readonly OrderFoodViewModel OrderFoodViewModel = new OrderFoodViewModel();

        private static readonly PrintHelper DocumentPrintingHelper = new PrintHelper();

        private ViewModelBase _currentViewModel;
        
        private IKitchenService _kitchenService;

        #endregion

        #region Contructor

        public MainViewModel(IKitchenService kitchenService)
        {
            _kitchenService = kitchenService ?? new KitchenService();
        }

        public MainViewModel()
            : this(null)
        {
            CurrentViewModel = MainViewModel.WaitingMealViewModel;
            ViewPedingFoodCommand = new RelayCommand((x) => CanExecuteViewPendingFoood(), (y) => ExecuteViewPendingFood());
            ViewDeliveredFoodCommand = new RelayCommand((x) => CanExecuteViewDeliveredFood(), (y) => ExecuteViewDeliveredFood());
            ViewOrderFoodCommand = new RelayCommand((x) => CanExecuteViewOrderFood(), (y) => ExecuteViewOrderFood());
            CloseWindow = new RelayCommand((x) => ((Window)x).Close());

            TestPrint = new RelayCommand((x) => CanExecuteTestPrint(), (y) => ExecuteTestPrint());
        }

        #endregion

        #region Properties

        public ICommand CloseWindow { get; set; }

        public ICommand ViewPedingFoodCommand { get; set; }

        public ICommand ViewDeliveredFoodCommand { get; set; }

        public ICommand ViewOrderFoodCommand { get; set; }

        public ICommand TestPrint { get; set; }

        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }

            set
            {
                if (_currentViewModel == value)
                {
                    return;
                }

                _currentViewModel = value;
                OnPropertyChanged("CurrentViewModel");
            }
        }

        #endregion

        #region Command

        private bool CanExecuteViewPendingFoood()
        {
            return true;
        }

        private void ExecuteViewPendingFood()
        {
            CurrentViewModel = new WaitingMealViewModel();
        }

        private bool CanExecuteViewDeliveredFood()
        {
            return true;
        }

        private void ExecuteViewDeliveredFood()
        {
            CurrentViewModel = new DeliveredFoodViewModel();
        }

        private bool CanExecuteViewOrderFood()
        {
            return true;
        }

        private void ExecuteViewOrderFood()
        {
            CurrentViewModel = MainViewModel.OrderFoodViewModel;
        }

        #endregion

        private bool CanExecuteTestPrint()
        {
            return true;
        }

        /// <summary>
        /// Method Print Invoice.
        /// </summary>
        private void ExecuteTestPrint()
        {
            var template = (FixedDocument)DocumentPrintingHelper.LoadTemplate(OrderFoodReceiptTemplate);

            var criteria = new GetOrderDetailsCriteria() { OrderId = 6 };
            Order model = _kitchenService.GetOrderDetails(criteria);

            DocumentPrintingHelper.InjectData(template, model);

            string receiptDocument = System.IO.Path.GetTempFileName() + ".xps";
            DocumentPrintingHelper.ConvertToXps(template, receiptDocument);

            ////DocumentPrintingHelper.Print(((IDocumentPaginatorSource)template).DocumentPaginator);
        }
    }
}
