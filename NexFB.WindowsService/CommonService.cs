// --------------------------------------------------------------------------
// <copyright file="CommonService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.4">Created</change>
// </history>

namespace NexFB.WindowsService
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using NexFB.Core.Constants;
    using NexFB.Core.Services;
    using NexFB.WindowsService.Criteria;

    /// <summary>
    /// Common Service.
    /// </summary>
    public class CommonService : BaseService
    {
        private NexcelService.UserServiceClient _nexcelService = null;
        private HongMaService.UserServiceClient _hongmaService = null;

        public CommonService()
        {
            _nexcelService = new NexcelService.UserServiceClient();
            _hongmaService = new HongMaService.UserServiceClient();
        }

        /// <summary>
        /// Function to update users of NEXCEL.
        /// </summary>
        public void UpdateUserForNexcel()
        {
            DateTime fromDate = GetDateFromBeforeUpdateUser(Constants.NexcelDepartment);
            IList<NexcelService.UserModel> users = _nexcelService.GetUser(fromDate);
            if (users != null && users.Any())
            {
                var insertCriteria = new InsertAndUpdateUserNexcelCriteria()
                {
                    DepartmentId = Constants.NexcelDepartment,
                    Users = users
                };

                Repository.Update(insertCriteria);
            }
        }

        /// <summary>
        /// Function to update teams of NEXCEL.
        /// </summary>
        public void UpdateTeamForNexcel()
        {
            DateTime fromDate = GetDateFromBeforeUpdateTeam(Constants.NexcelDepartment);
            IList<NexcelService.TeamModel> teams = _nexcelService.GetTeam(fromDate);
            if (teams != null && teams.Any())
            {
                var insertCriteria = new InsertAndUpdateTeamNexcelCriteria()
                {
                    DepartmentId = Constants.NexcelDepartment,
                    Teams = teams
                };

                Repository.Update(insertCriteria);
            }
        }

        /// <summary>
        /// Function to update users of Hong Ma.
        /// </summary>
        public void UpdateUserForHongMa()
        {
            DateTime fromDate = GetDateFromBeforeUpdateUser(Constants.HongMaDepartment);
            IList<HongMaService.UserModel> users = _hongmaService.GetUser(fromDate);

            if (users != null && users.Any())
            {
                var insertCriteria = new InsertAndUpdateUserHongMaCriteria()
                {
                    DepartmentId = Constants.HongMaDepartment,
                    Users = users
                };

                Repository.Update(insertCriteria);
            }
        }

        /// <summary>
        /// Function to update teams of Hong Ma.
        /// </summary>
        public void UpdateTeamForHongMa()
        {
            DateTime fromDate = GetDateFromBeforeUpdateTeam(Constants.HongMaDepartment);
            IList<HongMaService.TeamModel> teams = _hongmaService.GetTeam(fromDate);
            if (teams != null && teams.Any())
            {
                var insertCriteria = new InsertAndUpdateTeamHongMaCriteria()
                {
                    DepartmentId = Constants.HongMaDepartment,
                    Teams = teams
                };

                Repository.Update(insertCriteria);
            }
        }

        public DateTime GetDateFromBeforeUpdateUser(int departmentId)
        {
            var criteria = new GetLastUserModifiedDateCriteria()
            {
                DepartmentId = departmentId
            };

            DateTime fromDate = this.GetDataEntity<DateTime>(criteria);
            return fromDate;
        }

        public DateTime GetDateFromBeforeUpdateTeam(int departmentId)
        {
            var criteria = new GetLastTeamModifiedDateCriteria()
            {
                DepartmentId = departmentId
            };

            DateTime fromDate = this.GetDataEntity<DateTime>(criteria);
            return fromDate;
        }
    }
}
