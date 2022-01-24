namespace NexFB.Pos.Core.Mapping
{
    using System.Configuration;
    using AutoMapper;
    using NexFB.Models.Entities;
    using NexFB.Models.Kitchen;
    using NexFB.Pos.Core.Constants;
    using NexFB.Pos.Core.Models;

    public class CoreMappingRegister : IMappingRegister
    {
        public void Register()
        {
            string defaultLanguage = ConfigurationManager.AppSettings[Constant.DefaultLanguage];
            if (string.IsNullOrEmpty(defaultLanguage) || defaultLanguage == Constant.DefaultCulture)
            {
                MappingDataVietNamese();
            }
            else
            {
                MappingDataEnglish();
            }

            Mapper.CreateMap<GroupOrder, CommonKitchenOrder>()
              .ForMember(x => x.ItemId, x => x.MapFrom(y => y.GroupOrderId))
              .ForMember(x => x.ItemName, x => x.MapFrom(y => y.GroupName));

            MapTwoWay<EmployeeInfo, KitchenEmployeeInfo>();
        }

        private static void MapTwoWay<TLeft, TRight>()
        {
            Mapper.CreateMap<TLeft, TRight>();
            Mapper.CreateMap<TRight, TLeft>();
        }

        private static void MappingDataVietNamese()
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

        private static void MappingDataEnglish()
        {
            Mapper.CreateMap<Location, CommonKitchenOrder>()
             .ForMember(x => x.ItemId, x => x.MapFrom(y => y.LocationId))
             .ForMember(x => x.ItemName, x => x.MapFrom(y => y.NameEN));

            Mapper.CreateMap<FoodOrder, CommonKitchenOrder>()
              .ForMember(x => x.ItemId, x => x.MapFrom(y => y.FoodId))
              .ForMember(x => x.ItemName, x => x.MapFrom(y => y.FoodEngName))
              .ForMember(x => x.Price, x => x.MapFrom(y => y.Price));
        }
    }
}