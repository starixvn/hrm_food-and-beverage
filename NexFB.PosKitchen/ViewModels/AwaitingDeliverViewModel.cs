namespace NexFB.PosKitchen.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using NexFB.Core.Enums;
    using NexFB.Criteria.Kitchen;
    using NexFB.Models.Kitchen;

    public class AwaitingDeliverViewModel : ViewModelBase
    {
        #region Fields

        private static ObservableCollection<Order> _awaitingDeliverMeals = new ObservableCollection<Order>();
        private string _searchValue;

        #endregion

        #region Constructors
        public AwaitingDeliverViewModel()
            : this(null)
        {
        }

        public AwaitingDeliverViewModel(string searchValue)
        {
            SearchValue = searchValue;
            AwaitingDeliverMeals = GetAwaitingDeliverMealOrder;
        }

        #endregion

        #region Properties

        public ObservableCollection<Order> AwaitingDeliverMeals
        {
            get
            {
                return _awaitingDeliverMeals;
            }

            set
            {
                if (_awaitingDeliverMeals == value)
                {
                    return;
                }

                _awaitingDeliverMeals = value;
                OnPropertyChanged("AwaitingDeliverMeals");
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

        public ObservableCollection<Order> GetAwaitingDeliverMealOrder
        {
            get
            {
                IList<Order> awaitingDeliverMeals;
                if (!string.IsNullOrWhiteSpace(SearchValue))
                {
                    const byte MaxLengthOrderNo = 3;
                    awaitingDeliverMeals = SearchValue.Length <= MaxLengthOrderNo
                        ? GetAwaitingDeliverMealsByOrderNo(int.Parse(SearchValue))
                        : GetAwaitingDeliverMealsByOrderId(int.Parse(SearchValue));
                }
                else
                {
                    var criteria = new GetAwaitingDeliverMealOrderCriteria();
                    awaitingDeliverMeals = KitchenService.GetAwaitingDeliverMealOrder(criteria);
                }

                return new ObservableCollection<Order>(awaitingDeliverMeals);
            }
        }

        public void EditDataInAwaitingDeliverMeals(Order order)
        {
            if (AwaitingDeliverMeals.Any(x => x.OrderId == order.OrderId))
            {
                Order oldOrder = AwaitingDeliverMeals.FirstOrDefault(x => x.OrderId == order.OrderId);
                AwaitingDeliverMeals.Remove(oldOrder);
                AwaitingDeliverMeals.Add(order);
            }
        }

        public void DeleteDataInAwaitingDeliverMeals(int orderId)
        {
            if (AwaitingDeliverMeals.Any(x => x.OrderId == orderId))
            {
                var oldOrder = AwaitingDeliverMeals.FirstOrDefault(x => x.OrderId == orderId);
                AwaitingDeliverMeals.Remove(oldOrder);
            }
        }

        public void NewOrderDataInAwaitingDeliverMeals(Order order)
        {
            if (!AwaitingDeliverMeals.Any(x => x.OrderId == order.OrderId))
            {
                AwaitingDeliverMeals.Add(order);
            }
        }

        public void GetNewDataForApp()
        {
            AwaitingDeliverMeals = this.GetAwaitingDeliverMealOrder;
        }

        private List<Order> GetAwaitingDeliverMealsByOrderNo(int orderNo)
        {
            var criteria = new GetOrderDetailsByOrderNoCriteria { OrderNo = orderNo };
            IList<Order> orders = KitchenService.GetOrdersByOrderNo(criteria);
            return orders.Where(item => item.CurrentStatusId == (int)OrderStatus.ReceivedOrder
                || item.CurrentStatusId == (int)OrderStatus.ReducedOrder).ToList();
        }

        private List<Order> GetAwaitingDeliverMealsByOrderId(int orderId)
        {
            var meals = new List<Order>();
            var criteria = new GetMealOrderByIdCriteria { OrderId = orderId };
            Order order = KitchenService.GetMealOrderById(criteria);
            if (order != null &&
                (order.CurrentStatusId == (int)OrderStatus.ReceivedOrder
                || order.CurrentStatusId == (int)OrderStatus.ReducedOrder))
            {
                meals.Add(order);
            }

            return meals;
        }

        #endregion
    }
}