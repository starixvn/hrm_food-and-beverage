namespace NexFB.PosKitchen.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using NexFB.Core.Enums;
    using NexFB.Criteria.Kitchen;
    using NexFB.Models.Kitchen;

    public class DeliveredFoodViewModel : ViewModelBase
    {
        #region Fields

        private ObservableCollection<Order> _deliveredMeals;
        private string _searchValue;
        private MainViewModel _mainViewModel;

        #endregion

        #region Constructors

        public DeliveredFoodViewModel()
            : this(null, null)
        {
        }

        public DeliveredFoodViewModel(string searchValue, MainViewModel mainViewModel)
        {
            SearchValue = searchValue;
            DeliveredMeals = this.GetDeliveredMealOrder;
            MainViewModel = mainViewModel;
        }

        #endregion

        #region Properties

        public ObservableCollection<Order> DeliveredMeals
        {
            get
            {
                return _deliveredMeals;
            }

            set
            {
                if (_deliveredMeals == value)
                {
                    return;
                }

                _deliveredMeals = value;
                OnPropertyChanged("DeliveredMeals");
            }
        }

        public MainViewModel MainViewModel
        {
            get
            {
                return _mainViewModel;
            }

            set
            {
                if (_mainViewModel == value)
                {
                    return;
                }

                _mainViewModel = value;
                OnPropertyChanged("MainViewModel");
            }
        }

        public ObservableCollection<Order> GetDeliveredMealOrder
        {
            get
            {
                List<Order> deliveredMeals;
                if (!string.IsNullOrEmpty(SearchValue))
                {
                    const byte MaxLengthOrderNo = 3;
                    deliveredMeals = SearchValue.Length <= MaxLengthOrderNo
                        ? GetDeliveredMealsByOrderNo(int.Parse(SearchValue))
                        : GetDeliveredMealsByOrderId(int.Parse(SearchValue));
                }
                else
                {
                    var criteria = new GetDeliveredMealOrderCriteria();
                    deliveredMeals = KitchenService.GetDeliveredMealOrder(criteria);
                }

                return new ObservableCollection<Order>(deliveredMeals);
            }
        }

        public string SearchValue
        {
            get
            {
                return _searchValue;
            }

            set
            {
                if (_searchValue == value)
                {
                    return;
                }

                _searchValue = value;
                OnPropertyChanged("SearchValue");
            }
        }

        #endregion

        #region Methods

        public void NewOrderDataInDeliveredMeals(Order order)
        {
            if (!DeliveredMeals.Any(x => x.OrderId == order.OrderId))
            {
                DeliveredMeals.Add(order);
            }
        }

        public void DeleteDataInDeliveredMeals(int orderId)
        {
            if (DeliveredMeals.Any(x => x.OrderId == orderId))
            {
                Order currentOrder = DeliveredMeals.FirstOrDefault(x => x.OrderId == orderId);
                DeliveredMeals.Remove(currentOrder);
            }
        }

        public void GetNewDataForApp()
        {
            DeliveredMeals = this.GetDeliveredMealOrder;
        }

        private List<Order> GetDeliveredMealsByOrderNo(int orderNo)
        {
            var criteria = new GetOrderDetailsByOrderNoCriteria { OrderNo = orderNo };
            IList<Order> orders = KitchenService.GetOrdersByOrderNo(criteria);
            return orders.Where(item => item.CurrentStatusId == (int)OrderStatus.DeliveredOrder
                || item.CurrentStatusId == (int)OrderStatus.EditedOrder).ToList();
        }

        private List<Order> GetDeliveredMealsByOrderId(int orderId)
        {
            var deliveredMeals = new List<Order>();
            var criteria = new GetMealOrderByIdCriteria { OrderId = orderId };
            Order order = KitchenService.GetMealOrderById(criteria);
            if (order != null
                && (order.CurrentStatusId == (int)OrderStatus.DeliveredOrder
                    || order.CurrentStatusId == (int)OrderStatus.EditedOrder))
            {
                deliveredMeals.Add(order);
            }

            return deliveredMeals;
        }

        #endregion
    }
}