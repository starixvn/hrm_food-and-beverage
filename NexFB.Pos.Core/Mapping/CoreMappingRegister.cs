// --------------------------------------------------------------------------
// <copyright file="CoreMappingRegister.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.8.22">Created</change>
// </history>

namespace NexFB.Pos.Core.Mapping
{
    using System.Configuration;
    using AutoMapper;
    using NexFB.Models.Entities;
    using NexFB.Models.Kitchen;
    using NexFB.Pos.Core.Constants;
    using NexFB.Pos.Core.Models;

    /// <summary>
    /// Core Mapping Register.
    /// </summary>
    public class CoreMappingRegister : IMappingRegister
    {
        /// <summary>
        /// Method Register Mapper.
        /// </summary>
        public void Register()
        {
            string defaultLanguage = ConfigurationManager.AppSettings[Constant.DefaultLanguage];
            if (string.IsNullOrEmpty(defaultLanguage) || defaultLanguage == Constant.DefaultCulture)
            {
                Mapper.CreateMap<Location, CommonKitchenOrder>()
                   .ForMember(x => x.ItemId, x => x.MapFrom(y => y.LocationId))
                   .ForMember(x => x.ItemName, x => x.MapFrom(y => y.NameVN));

                Mapper.CreateMap<FoodOrder, CommonKitchenOrder>()
                   .ForMember(x => x.ItemId, x => x.MapFrom(y => y.FoodId))
                   .ForMember(x => x.ItemName, x => x.MapFrom(y => y.FoodVnmName))
                   .ForMember(x => x.Price, x => x.MapFrom(y => y.Price));

                Mapper.CreateMap<OrderMenu, KitchenEmployeeInfo>()
                  .ForMember(x => x.GroupOrderId, x => x.MapFrom(y => y.OrderId));
            }
            else
            {
                Mapper.CreateMap<Location, CommonKitchenOrder>()
                 .ForMember(x => x.ItemId, x => x.MapFrom(y => y.LocationId))
                 .ForMember(x => x.ItemName, x => x.MapFrom(y => y.NameEN));

                Mapper.CreateMap<FoodOrder, CommonKitchenOrder>()
                  .ForMember(x => x.ItemId, x => x.MapFrom(y => y.FoodId))
                  .ForMember(x => x.ItemName, x => x.MapFrom(y => y.FoodEngName))
                  .ForMember(x => x.Price, x => x.MapFrom(y => y.Price));
            }

            Mapper.CreateMap<GroupOrder, CommonKitchenOrder>()
              .ForMember(x => x.ItemId, x => x.MapFrom(y => y.GroupOrderId))
              .ForMember(x => x.ItemName, x => x.MapFrom(y => y.GroupName));

            MapTwoWay<EmployeeInfo, KitchenEmployeeInfo>();
        }

        private void MapTwoWay<TLeft, TRight>()
        {
            Mapper.CreateMap<TLeft, TRight>();
            Mapper.CreateMap<TRight, TLeft>();
        }
    }
}
