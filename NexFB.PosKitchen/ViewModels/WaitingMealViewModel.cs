namespace NexFB.PosKitchen.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using NexFB.Core.Enums;
    using NexFB.Criteria.Kitchen;
    using NexFB.Models.Kitchen;

    public class WaitingMealViewModel : ViewModelBase
    {
        #region Fields
        private ObservableCollection<Order> _waitingMeals = new ObservableCollection<Order>();
        private string _searchValue;

        #endregion

        #region Constructors
        public WaitingMealViewModel()
            : this(null)
        {
        }

        public WaitingMealViewModel(string searchValue)
        {
            SearchValue = searchValue;
            WaitingMeals = this.GetWaitingMealOrder;
        }

        #endregion

        #region Properties

        public ObservableCollection<Order> WaitingMeals
        {
            get
            {
                return _waitingMeals;
            }

            set
            {
                if (_waitingMeals == value)
                {
                    return;
                }

                _waitingMeals = value;
                OnPropertyChanged("WaitingMeals");
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

        public ObservableCollection<Order> GetWaitingMealOrder
        {
            get
            {
                List<Order> waitingMeals;
                if (!string.IsNullOrWhiteSpace(SearchValue))
                {
                    const byte MaxLengthOrderNo = 3;
                    waitingMeals = SearchValue.Length <= MaxLengthOrderNo
                        ? GetWaitingMealsByOrderNo(int.Parse(SearchValue))
                        : GetWaitingMealsByOrderId(int.Parse(SearchValue));
                }
                else
                {
                    var criteria = new GetWaitingMealOrderCriteria();
                    waitingMeals = KitchenService.GetWaitngMealOrder(criteria);
                }

                return new ObservableCollection<Order>(waitingMeals);
            }
        }

        public void EditDataInWaitingMeals(Order order)
        {
            if (WaitingMeals.Any(x => x.OrderId == order.OrderId))
            {
                var oldOrder = WaitingMeals.FirstOrDefault(x => x.OrderId == order.OrderId);
                WaitingMeals.Remove(oldOrder);

                if (order.CurrentStatusId != (int)OrderStatus.DeletedOrder)
                {
                    NewOrderDataAndSort(order);
                }
            }
        }

        public void DeleteDataInWaitingMeals(int orderId)
        {
            if (WaitingMeals.Any(x => x.OrderId == orderId))
            {
                Order oldOrder = WaitingMeals.FirstOrDefault(x => x.OrderId == orderId);
                WaitingMeals.Remove(oldOrder);
            }
        }

        public void NewOrderDataInWaitingMeals(Order order)
        {
            if (!WaitingMeals.Any(x => x.OrderId == order.OrderId))
            {
                WaitingMeals.Add(order);
            }
        }

        public void GetNewDataForApp()
        {
            WaitingMeals = this.GetWaitingMealOrder;
        }

        public void NewOrderDataAndSort(Order order)
        {
            if (!WaitingMeals.Any(x => x.OrderId == order.OrderId))
            {
                if (order.CurrentStatusId == (int)OrderStatus.NewOrder)
                {
                    Order lastNewOrder = WaitingMeals.LastOrDefault(x => x.CurrentStatusId == (int)OrderStatus.NewOrder);
                    if (lastNewOrder != null)
                    {
                        int index = WaitingMeals.IndexOf(lastNewOrder);
                        WaitingMeals.Insert(index + 1, order);
                    }
                    else
                    {
                        WaitingMeals.Insert(0, order);
                    }
                }
                else
                {
                    WaitingMeals.Add(order);
                }
            }
        }

        private List<Order> GetWaitingMealsByOrderNo(int orderNo)
        {
            var criteria = new GetOrderDetailsByOrderNoCriteria { OrderNo = orderNo };
            IList<Order> orders = KitchenService.GetOrdersByOrderNo(criteria);
            return orders.Where(item => item.CurrentStatusId == (int)OrderStatus.NewOrder
                || item.CurrentStatusId == (int)OrderStatus.ReceivedOrder
                || item.CurrentStatusId == (int)OrderStatus.ReducedOrder).ToList();
        }

        private List<Order> GetWaitingMealsByOrderId(int orderId)
        {
            var waitingMeals = new List<Order>();
            var criteria = new GetMealOrderByIdCriteria { OrderId = orderId };
            Order order = KitchenService.GetMealOrderById(criteria);
            if (order != null &&
                (order.CurrentStatusId == (int)OrderStatus.NewOrder
                || order.CurrentStatusId == (int)OrderStatus.ReceivedOrder
                || order.CurrentStatusId == (int)OrderStatus.ReducedOrder))
            {
                waitingMeals.Add(order);
            }

            return waitingMeals;
        }

        #endregion
    }
}