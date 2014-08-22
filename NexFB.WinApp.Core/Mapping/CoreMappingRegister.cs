// --------------------------------------------------------------------------
// <copyright file="CoreMappingRegister.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.8.22">Created</change>
// </history>

namespace NexFB.WinApp.Core.Mapping
{
    using AutoMapper;

    /// <summary>
    /// Core Mapping Register.
    /// </summary>
    public class CoreMappingRegister : IMappingRegister
    {
        public void Register()
        {
        }

        private void MapTwoWay<TLeft, TRight>()
        {
            Mapper.CreateMap<TLeft, TRight>();
            Mapper.CreateMap<TRight, TLeft>();
        }
    }
}
