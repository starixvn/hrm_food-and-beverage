// --------------------------------------------------------------------------
// <copyright file="OrderHub.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.15">Created</change>
// </history>

namespace NexFB.Hub
{
    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;
    using NexFB.Criteria.Kitchen;
    using NexFB.Criteria.Ordering;
    using NexFB.Models.Kitchen;
    using NexFB.Models.Ordering;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
using System.Collections.ObjectModel;

    /// <summary>
    /// The Order hub.
    /// </summary>
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

        public void AppNotifyReceiveNewOrder(ObservableCollection<Order> waitingMeals)
        {
            Clients.All.appNotifyReceiveNewOrder(waitingMeals);
        }

        public void AppNotifyExportAwaitingOrder(ObservableCollection<Order> awaitingDeliverMeals)
        {
            Clients.All.appNotifyExportAwaitingOrder(awaitingDeliverMeals);
        }

        #endregion

        public void SendMessage(string message)
        {
            Clients.All.newMessage(message);
        }

        public void KitchenNotify(int orderId)
        {
            Clients.All.kitchenNotify(orderId);
        }

        public void SendNewOrder(int orderId)
        {
            SendNewOrderForWeb(orderId);
            SendNewOrderForApp(orderId);
        }

        private void SendNewOrderForWeb(int orderId)
        {
            var criteria = new GetMyOrderByIdCriteria()
            {
                OrderId = orderId
            };

            MealOrderingModel order = _orderService.GetMyOrderById(criteria);
            Clients.All.newOrderForWeb(order);
        }

        private void SendNewOrderForApp(int orderId)
        {
            var criteria = new GetMealOrderByIdCriteria()
            {
                OrderId = orderId
            };

            Order order = _kitchenService.GetMealOrderById(criteria);
            Clients.All.newOrderForApp(order);
        }
    }
}
