// --------------------------------------------------------------------------
// <copyright file="CoreMappingRegister.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.20">Created</change>
// </history>

namespace NexFB.WinAppSmartCard.Core.Mapping
{
    using AutoMapper;
    using NexFB.Models.Kitchen;
    using NexFB.WinAppSmartCard.Core.Models;

    /// <summary>
    /// Core Mapping Register.
    /// </summary>
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
