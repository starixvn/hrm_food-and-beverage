namespace NexFB.WinAppSmartCard.Core.Mapping
{
    using AutoMapper;
    using NexFB.Models.Kitchen;
    using NexFB.WinAppSmartCard.Core.Models;

    public class CoreMappingRegister : IMappingRegister
    {
        public void Register()
        {
            MapTwoWay<Employee, EmployeeInfo>();
            MapTwoWay<FoodOrder, Food>();
        }

        private void MapTwoWay<TLeft, TRight>()
        {
            Mapper.CreateMap<TLeft, TRight>();
            Mapper.CreateMap<TRight, TLeft>();
        }
    }
}