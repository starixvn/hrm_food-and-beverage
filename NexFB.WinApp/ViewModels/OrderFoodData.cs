// --------------------------------------------------------------------------
// <copyright file="OrderFoodData.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.15">Created</change>
// </history>

namespace NexFB.WinApp.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using AutoMapper;
    using NexFB.Criteria.Department;
    using NexFB.Criteria.Kitchen;
    using NexFB.Models.Entities;
    using NexFB.Models.Kitchen;
    using NexFB.WinApp.Core.Models;

    /// <summary>
    /// Order Food Data.
    /// </summary>
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
                var getAllLocation = new GetAllLocationCriteria();
                List<Location> locations = this.CommonService.GetAllLocation(getAllLocation).ToList();
                if (locations != null)
                {
                    List<CommonKitchenOrder> data = Mapper.Map<List<Location>, List<CommonKitchenOrder>>(locations);
                    data = data.Where(x => !string.IsNullOrEmpty(x.ItemName)).ToList();
                    return new ObservableCollection<CommonKitchenOrder>(data);
                }

                return new ObservableCollection<CommonKitchenOrder>();
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
                var getAllGroup = new GetGroupNameCriteria();
                List<GroupOrder> groups = this.KitchenService.GetAllGroupOrder(getAllGroup);
                if (groups != null)
                {
                    List<CommonKitchenOrder> data = Mapper.Map<List<GroupOrder>, List<CommonKitchenOrder>>(groups);
                    data = data.Where(x => !string.IsNullOrEmpty(x.ItemName)).ToList();
                    return new ObservableCollection<CommonKitchenOrder>(data);
                }

                return new ObservableCollection<CommonKitchenOrder>();
            }
        }
    }
}
