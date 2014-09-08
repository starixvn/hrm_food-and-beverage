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
    using System.Diagnostics.CodeAnalysis;
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
            NexcelService = new NexcelService.UserServiceClient();
            HongmaService = new HongMaService.UserServiceClient();
        }

        protected NexcelService.UserServiceClient NexcelService
        {
            get { return _nexcelService; }
            set { _nexcelService = value; }
        }

        protected HongMaService.UserServiceClient HongmaService
        {
            get { return _hongmaService; }
            set { _hongmaService = value; }
        }

        /// <summary>
        /// Function to update users of NEXCEL.
        /// </summary>
        public void UpdateUserForNexcel()
        {
            DateTime fromDate = GetDateFromBeforeUpdateUser(Constants.NexcelDepartment);
            IList<NexcelService.UserModel> users = NexcelService.GetUser(fromDate);
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
        [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "Reviewed. Suppression is OK here.")]
        public void UpdateTeamForNexcel()
        {
            DateTime fromDate = GetDateFromBeforeUpdateTeam(Constants.NexcelDepartment);
            IList<NexcelService.TeamModel> teams = NexcelService.GetTeam(fromDate);
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
        public void UpdateUserForHM()
        {
            DateTime fromDate = GetDateFromBeforeUpdateUser(Constants.HMDepartment);
            IList<HongMaService.UserModel> users = HongmaService.GetUser(fromDate);

            if (users != null && users.Any())
            {
                var insertCriteria = new InsertAndUpdateUserHMCriteria()
                {
                    DepartmentId = Constants.HMDepartment,
                    Users = users
                };

                Repository.Update(insertCriteria);
            }
        }

        /// <summary>
        /// Function to update teams of Hong Ma.
        /// </summary>
        public void UpdateTeamForHM()
        {
            DateTime fromDate = GetDateFromBeforeUpdateTeam(Constants.HMDepartment);
            IList<HongMaService.TeamModel> teams = HongmaService.GetTeam(fromDate);
            if (teams != null && teams.Any())
            {
                var insertCriteria = new InsertAndUpdateTeamHMCriteria()
                {
                    DepartmentId = Constants.HMDepartment,
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
