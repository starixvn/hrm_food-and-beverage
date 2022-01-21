namespace NexFB.PosKitchen.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using NexFB.Core.Enums;
    using NexFB.Criteria.Kitchen;
    using NexFB.Models.Kitchen;

    public class CanceledFoodViewModel : ViewModelBase
    {
        private ObservableCollection<Order> _canceledMeals = new ObservableCollection<Order>();
        private string _searchValue;

        public CanceledFoodViewModel()
            : this(null)
        {
        }

        public CanceledFoodViewModel(string searchValue)
        {
            SearchValue = searchValue;
            CanceledMeals = this.GetCanceledMealOrder;
        }

        #region Properties

        public ObservableCollection<Order> CanceledMeals
        {
            get
            {
                return _canceledMeals;
            }

            set
            {
                if (_canceledMeals == value)
                {
                    return;
                }

                _canceledMeals = value;
                OnPropertyChanged("CanceledMeals");
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

        public ObservableCollection<Order> GetCanceledMealOrder
        {
            get
            {
                List<Order> canceledMeals;
                if (!string.IsNullOrWhiteSpace(SearchValue))
                {
                    const byte MaxLengthOrderNo = 3;
                    canceledMeals = SearchValue.Length <= MaxLengthOrderNo
                        ? GetCanceledMealsByOrderNo(int.Parse(SearchValue))
                        : GetCanceledMealsByOrderId(int.Parse(SearchValue));
                }
                else
                {
                    var criteria = new GetCanceledMealOrderCriteria();
                    canceledMeals = KitchenService.GetCanceledMealOrder(criteria);
                }

                return new ObservableCollection<Order>(canceledMeals);
            }
        }

        #endregion

        #region Methods

        public void GetNewDataForApp()
        {
            CanceledMeals = this.GetCanceledMealOrder;
        }

        public void AddRejectedOrderToList(Order order)
        {
            if (order != null)
            {
                if (!CanceledMeals.Any(x => x.OrderId == order.OrderId))
                {
                    CanceledMeals.Add(order);
                }

                CanceledMeals = new ObservableCollection<Order>(this.CanceledMeals);
            }
        }

        private List<Order> GetCanceledMealsByOrderNo(int orderNo)
        {
            var criteria = new GetOrderDetailsByOrderNoCriteria { OrderNo = orderNo };
            IList<Order> orders = KitchenService.GetOrdersByOrderNo(criteria);
            return orders.Where(item => item.CurrentStatusId == (int)OrderStatus.CanceledOrder).ToList();
        }

        private List<Order> GetCanceledMealsByOrderId(int orderId)
        {
            var canceledMeals = new List<Order>();
            var criteria = new GetMealOrderByIdCriteria { OrderId = orderId };
            Order order = KitchenService.GetMealOrderById(criteria);
            if (order != null && order.CurrentStatusId == (int)OrderStatus.CanceledOrder)
            {
                canceledMeals.Add(order);
            }

            return canceledMeals;
        }

        #endregion
    }
}