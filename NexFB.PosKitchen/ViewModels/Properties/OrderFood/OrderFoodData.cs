namespace NexFB.PosKitchen.ViewModels.Properties.OrderFood
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using AutoMapper;
    using NexFB.Criteria.DeliveryArea;
    using NexFB.Criteria.Kitchen;
    using NexFB.Models.Entities;
    using NexFB.Models.Kitchen;
    using NexFB.Pos.Core.Models;
    using NexFB.Resources;

    public class OrderFoodData : ViewModelBase
    {
        public ObservableCollection<Order> GetWaitingMealOrder
        {
            get
            {
                var criteria = new GetWaitingMealOrderCriteria();
                List<Order> waitingMeals = KitchenService.GetWaitngMealOrder(criteria);
                return new ObservableCollection<Order>(waitingMeals);
            }
        }

        public ObservableCollection<CommonKitchenOrder> GetAllLocation
        {
            get
            {
                var criteria = new GetAllDeliveryAreaCriteria();
                return this.GetDeliveryArea(criteria);
            }
        }

        public ObservableCollection<CommonKitchenOrder> GetAllActiveLocation
        {
            get
            {
                var criteria = new GetAllDeliveryAreaCriteria { IsActive = true };
                return this.GetDeliveryArea(criteria);
            }
        }

        public ObservableCollection<CommonKitchenOrder> GetAllFood
        {
            get
            {
                var getAlternativeFood = new GetAlternativeFoodCriteria();
                List<FoodOrder> foods = this.KitchenService.GetAllAlternativeFoodOrder(getAlternativeFood).ToList();
                if (foods != null)
                {
                    List<CommonKitchenOrder> data = Mapper.Map<List<FoodOrder>, List<CommonKitchenOrder>>(foods);
                    data = data.Where(x => !string.IsNullOrEmpty(x.ItemName)).ToList();
                    return new ObservableCollection<CommonKitchenOrder>(data);
                }

                return new ObservableCollection<CommonKitchenOrder>();
            }
        }

        public ObservableCollection<CommonKitchenOrder> GetAllAlternativeFood
        {
            get
            {
                var getAllFood = new GetFoodNameCriteria();
                List<FoodOrder> foods = this.KitchenService.GetAllFoodOrder(getAllFood);
                if (foods != null)
                {
                    List<CommonKitchenOrder> data = Mapper.Map<List<FoodOrder>, List<CommonKitchenOrder>>(foods);
                    data = data.Where(x => !string.IsNullOrEmpty(x.ItemName)).ToList();
                    return new ObservableCollection<CommonKitchenOrder>(data);
                }

                return new ObservableCollection<CommonKitchenOrder>();
            }
        }

        public ObservableCollection<CommonKitchenOrder> GetAllGroup
        {
            get
            {
                var criteria = new GetGroupNameCriteria();
                List<GroupOrder> groups = this.KitchenService.GetAllGroupOrder(criteria);
                if (groups != null)
                {
                    List<CommonKitchenOrder> data = Mapper.Map<List<GroupOrder>, List<CommonKitchenOrder>>(groups);
                    data = data.Where(x => !string.IsNullOrEmpty(x.ItemName)).ToList();
                    return new ObservableCollection<CommonKitchenOrder>(data);
                }

                return new ObservableCollection<CommonKitchenOrder>();
            }
        }

        public string DisplayEmployeeOrderSummary(KitchenEmployeeInfo currentSelectedMember)
        {
            return currentSelectedMember != null ? EmployeeOrderData(currentSelectedMember) : string.Empty;
        }

        public string EmployeeOrderData(KitchenEmployeeInfo currentSelectedMember)
        {
            List<EmployeeOrderDetails> listMainOrder = currentSelectedMember.ListMainMenuOrderDeatails;
            List<EmployeeOrderDetails> listAlternativeOrder = currentSelectedMember.ListAlternativeMenuOrderDeatails;
            int sumMainOrder = listMainOrder.Sum(x => x.SelectedMenuItemQuantity);
            int sumAlternativeOrder = listAlternativeOrder.Sum(x => x.SelectedMenuItemQuantity);
            var orderInfoSummary = string.Empty;

            if (listMainOrder.Count > 0)
            {
                orderInfoSummary += CombineDataOrderSummary(listMainOrder);
            }

            if (listAlternativeOrder.Count > 0)
            {
                orderInfoSummary += " - ";
                orderInfoSummary += CombineDataOrderSummary(listAlternativeOrder);
            }

            if (orderInfoSummary.Count() > 100)
            {
                orderInfoSummary = string.Empty;
                if (listAlternativeOrder.Count > 0)
                {
                    orderInfoSummary += listMainOrder.Count > 0 ?
                        WinFormMainResource.Parentheses1 + sumMainOrder + WinFormMainResource.Main + WinFormMainResource.CommaSeparated
                            + sumAlternativeOrder + WinFormMainResource.Snack + WinFormMainResource.Parentheses2
                            :
                            WinFormMainResource.Parentheses1 + sumAlternativeOrder + WinFormMainResource.Snack + WinFormMainResource.Parentheses2;
                }
                else
                {
                    if (listMainOrder.Count > 0)
                    {
                        orderInfoSummary += WinFormMainResource.Parentheses1 + sumMainOrder + WinFormMainResource.Main + WinFormMainResource.Parentheses2;
                    }
                }
            }

            return orderInfoSummary;
        }

        public string CombineDataOrderSummary(List<EmployeeOrderDetails> listOrderDetails)
        {
            var orderInfoSummary = new StringBuilder();
            var lastItem = listOrderDetails.Last();
            foreach (var item in listOrderDetails)
            {
                orderInfoSummary.Append(item.SelectedMenuItemQuantity > 1 ?
                    item.SelectedMenuItemQuantity.ToString() + WinFormMainResource.QuantityPerOrder
                        + WinFormMainResource.Parentheses1 + item.SelectedMenuItemName + WinFormMainResource.Parentheses2
                        :
                        WinFormMainResource.Parentheses1 + item.SelectedMenuItemName + WinFormMainResource.Parentheses2);

                if (item != lastItem)
                {
                    orderInfoSummary.Append(WinFormMainResource.CommaSeparated);
                }
            }

            return orderInfoSummary.ToString();
        }

        private ObservableCollection<CommonKitchenOrder> GetDeliveryArea(GetAllDeliveryAreaCriteria getAllDeliveryAreaCriteria)
        {
            List<Location> locations = this.DeliveryAreaService.GetAllDeliveryArea(getAllDeliveryAreaCriteria).ToList();
            if (locations != null)
            {
                List<CommonKitchenOrder> data = Mapper.Map<List<Location>, List<CommonKitchenOrder>>(locations);
                data = data.Where(x => !string.IsNullOrEmpty(x.ItemName)).ToList();
                return new ObservableCollection<CommonKitchenOrder>(data);
            }

            return new ObservableCollection<CommonKitchenOrder>();
        }
    }
}