namespace NexFB.Hub
{
    using System.Collections.ObjectModel;
    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;
    using NexFB.Criteria.Kitchen;
    using NexFB.Criteria.Ordering;
    using NexFB.Models.Kitchen;
    using NexFB.Models.Ordering;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;

    [HubName("order")]
    public class OrderHub : Hub
    {
        private readonly IOrderingService _orderService;
        private readonly IKitchenService _kitchenService;

        public OrderHub()
            : this(null, null)
        {
        }

        public OrderHub(IOrderingService orderService, IKitchenService kitchenService)
        {
            _orderService = orderService ?? new OrderingService();
            _kitchenService = kitchenService ?? new KitchenService();
        }

        #region MyRegion

        public void RefeshAwaitingDeliverOrderList(ObservableCollection<Order> awaitingDeliverMeals, int currentOrderDelivered)
        {
            var criteria = new GetMealOrderByIdCriteria
            {
                OrderId = currentOrderDelivered
            };

            Order order = _kitchenService.GetMealOrderById(criteria);
            Clients.All.refeshAwaitingDeliverOrderList(awaitingDeliverMeals);
            Clients.All.addOrderToDeliveredMeals(order);
        }

        #endregion

        public void KitchenNotify(int orderId, int screenControl)
        {
            SendEditOrderForWeb(orderId);
            switch (screenControl)
            {
                case 1:
                    Clients.All.appNotifyReceiveNewOrder(orderId);
                    break;
                case 2:
                    Clients.All.appNotifyAwaitingDeliverOrder(orderId);
                    break;
                case 3:
                    Clients.All.appNotifyUndoDeliveredOrder(orderId);
                    break;
                case 4:
                    Clients.All.appNotifyRejectedOrder(orderId);
                    break;
                default:
                    // do nothing
                    break;
            }
        }

        public void SendNewOrder(int orderId)
        {
            SendNewOrderForWeb(orderId);
            SendNewOrderForApp(orderId);
        }

        public void SendEditOrder(int orderId)
        {
            SendEditOrderForWeb(orderId);
            SendEditOrderForApp(orderId);
        }

        public void SendDeleteOrder(int orderId)
        {
            Clients.All.deleteOrderForWeb(orderId);
            Clients.All.deleteOrderForApp(orderId);
        }

        private void SendNewOrderForWeb(int orderId)
        {
            MealOrderingModel order = GetOrderById(orderId);
            Clients.All.newOrderForWeb(order);
        }

        private void SendEditOrderForWeb(int orderId)
        {
            MealOrderingModel order = GetOrderById(orderId);
            Clients.All.editOrderForWeb(order);
        }

        private void SendNewOrderForApp(int orderId)
        {
            Clients.All.newOrderForApp(orderId);
        }

        private void SendEditOrderForApp(int orderId)
        {
            Clients.All.editOrderForApp(orderId);
        }

        private MealOrderingModel GetOrderById(int orderId)
        {
            var criteria = new GetOrderByIdCriteria
            {
                OrderId = orderId
            };

            MealOrderingModel order = _orderService.GetOrderById(criteria);
            return order;
        }
    }
}