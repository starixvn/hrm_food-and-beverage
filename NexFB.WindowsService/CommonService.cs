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
        private SMSService.UserServiceClient _smsService = null;

        public CommonService()
        {
            NexcelService = new NexcelService.UserServiceClient();
            HongmaService = new HongMaService.UserServiceClient();
            SmsService = new SMSService.UserServiceClient();
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

        protected SMSService.UserServiceClient SmsService
        {
            get { return _smsService; }
            set { _smsService = value; }
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
        /// Function to update shift of NEXCEL.
        /// </summary>
        public void UpdateShiftForNexcel()
        {
            IList<NexcelService.ShiftModel> shifts = NexcelService.GetShift();
            if (shifts != null && shifts.Any())
            {
                var criteria = new InsertAndUpdateShiftNexcelCriteria()
                {
                    DepartmentId = Constants.NexcelDepartment,
                    Shifts = shifts
                };

                Repository.Update(criteria);
            }
        }

        /// <summary>
        /// Function to update employee calendar of NEXCEL.
        /// </summary>
        public void UpdateEmployeeCalendarForNexcel()
        {
            DateTime fromDate = GetDateFromBeforeUpdateEmployeeCalendar(Constants.NexcelDepartment);
            ////var fromDate = new DateTime(2014, 6, 30);
            IList<NexcelService.EmployeeCalendarModel> calendars = NexcelService.GetEmployeeCalendar(fromDate);
            if (calendars != null && calendars.Any())
            {
                var criteria = new InsertAndUpdateEmployeeCalendarNexcelCriteria()
                {
                    DepartmentId = Constants.NexcelDepartment,
                    EmployeeCalendars = calendars
                };

                Repository.Update(criteria);
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

        /// <summary>
        /// Function to update shift of Hong Ma.
        /// </summary>
        public void UpdateShiftForHM()
        {
            IList<HongMaService.ShiftModel> shifts = HongmaService.GetShift();
            if (shifts != null && shifts.Any())
            {
                var criteria = new InsertAndUpdateShiftHMCriteria()
                {
                    DepartmentId = Constants.HMDepartment,
                    Shifts = shifts
                };

                Repository.Update(criteria);
            }
        }

        /// <summary>
        /// Function to update employee calendar of Hong Ma.
        /// </summary>
        public void UpdateEmployeeCalendarForHM()
        {
            DateTime fromDate = GetDateFromBeforeUpdateEmployeeCalendar(Constants.HMDepartment);
            IList<HongMaService.EmployeeCalendarModel> calendars = HongmaService.GetEmployeeCalendar(fromDate);
            if (calendars != null && calendars.Any())
            {
                var criteria = new InsertAndUpdateEmployeeCalendarHMCriteria()
                {
                    DepartmentId = Constants.HMDepartment,
                    EmployeeCalendars = calendars
                };

                Repository.Update(criteria);
            }
        }

        /// <summary>
        /// Function to update users of SMS.
        /// </summary>
        public void UpdateUserForSms()
        {
            DateTime fromDate = GetDateFromBeforeUpdateUser(Constants.SmsDepartment);
            IList<SMSService.UserModel> users = SmsService.GetUser(fromDate);
            if (users != null && users.Any())
            {
                var insertCriteria = new InsertAndUpdateUserSmsCriteria()
                {
                    DepartmentId = Constants.SmsDepartment,
                    Users = users
                };

                Repository.Update(insertCriteria);
            }
        }

        /// <summary>
        /// Function to update teams of SMS.
        /// </summary>
        public void UpdateTeamForSms()
        {
            DateTime fromDate = GetDateFromBeforeUpdateTeam(Constants.SmsDepartment);
            IList<SMSService.TeamModel> teams = SmsService.GetTeam(fromDate);
            if (teams != null && teams.Any())
            {
                var insertCriteria = new InsertAndUpdateTeamSmsCriteria()
                {
                    DepartmentId = Constants.SmsDepartment,
                    Teams = teams
                };

                Repository.Update(insertCriteria);
            }
        }

        /// <summary>
        /// Function to update shift of SMS.
        /// </summary>
        public void UpdateShiftForSms()
        {
            IList<SMSService.ShiftModel> shifts = SmsService.GetShift();
            if (shifts != null && shifts.Any())
            {
                var criteria = new InsertAndUpdateShiftSmsCriteria()
                {
                    DepartmentId = Constants.SmsDepartment,
                    Shifts = shifts
                };

                Repository.Update(criteria);
            }
        }

        /// <summary>
        /// Function to update employee calendar of SMS.
        /// </summary>
        public void UpdateEmployeeCalendarForSms()
        {
            DateTime fromDate = GetDateFromBeforeUpdateEmployeeCalendar(Constants.SmsDepartment);
            IList<SMSService.EmployeeCalendarModel> calendars = SmsService.GetEmployeeCalendar(fromDate);
            if (calendars != null && calendars.Any())
            {
                var criteria = new InsertAndUpdateEmployeeCalendarSmsCriteria()
                {
                    DepartmentId = Constants.SmsDepartment,
                    EmployeeCalendars = calendars
                };

                Repository.Update(criteria);
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

        public DateTime GetDateFromBeforeUpdateEmployeeCalendar(int departmentId)
        {
            var criteria = new GetLastEmployeeCalendarModifiedDateCriteria()
            {
                DepartmentId = departmentId
            };

            DateTime fromDate = this.GetDataEntity<DateTime>(criteria);
            return fromDate;
        }
    }
}
