// --------------------------------------------------------------------------
// <copyright file="UserService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.4">Created</change>
// </history>

namespace NexFB.WebService.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Configuration;
    using NexFB.Core.Services;
    using NexFB.WebService.Criteria;
    using NexFB.WebService.Models;

    /// <summary>
    /// User Service.
    /// </summary>
    public class UserService : BaseService, IUserService
    {
        public UserService()
        {
            DbSettingProviderManager.Current.Start();
        }

        /// <summary>
        /// Function to get users by last modified date.
        /// </summary>
        /// <param name="fromDate">The From date.</param>
        /// <returns>List of user model.</returns>
        public IEnumerable<UserModel> GetUser(DateTime? fromDate)
        {
            var criteria = new GetEmployeeCriteria() 
            {
                FromDate = fromDate
            };

            IEnumerable<UserModel> users = null;
            if (criteria.IsValid())
            {
                users = GetData<UserModel>(criteria).ToList();
            }

            return users;
        }

        /// <summary>
        /// Function to get teams by last modified date.
        /// </summary>
        /// <param name="fromDate">The From date.</param>
        /// <returns>List of teams model.</returns>
        public IEnumerable<TeamModel> GetTeam(DateTime? fromDate)
        {
            IEnumerable<TeamModel> teams = null;
            var criteria = new GetTeamCriteria() 
            { 
                FromDate = fromDate
            };

            if (criteria.IsValid())
            {
                teams = GetData<TeamModel>(criteria).ToList();
            }

            return teams;
        }
    }
}
