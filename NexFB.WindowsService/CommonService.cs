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
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using NexFB.Core.Constants;
    using NexFB.Core.Models;
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
        /// <returns>Command Result.</returns>
        public CommandResult UpdateUserForNexcel()
        {
            CommandResult result = null;
            DateTime fromDate = GetDateFromBeforeUpdateUser(Constants.NexcelDepartment);
            IList<NexcelService.UserModel> users = NexcelService.GetUser(fromDate);
            if (users != null && users.Any())
            {
                var insertCriteria = new InsertAndUpdateUserNexcelCriteria()
                {
                    DepartmentId = Constants.NexcelDepartment,
                    Users = users
                };

                result = Repository.Update(insertCriteria);
            }

            return result;
        }

        /// <summary>
        /// Function to update teams of NEXCEL.
        /// </summary>
        /// <returns>Command Result.</returns>
        public CommandResult UpdateTeamForNexcel()
        {
            CommandResult result = null;
            DateTime fromDate = GetDateFromBeforeUpdateTeam(Constants.NexcelDepartment);
            IList<NexcelService.TeamModel> teams = NexcelService.GetTeam(fromDate);
            if (teams != null && teams.Any())
            {
                var insertCriteria = new InsertAndUpdateTeamNexcelCriteria()
                {
                    DepartmentId = Constants.NexcelDepartment,
                    Teams = teams
                };

                result = Repository.Update(insertCriteria);
            }

            return result;
        }

        /// <summary>
        /// Function to update shift of NEXCEL.
        /// </summary>
        /// <returns>Command Result.</returns>
        public CommandResult UpdateShiftForNexcel()
        {
            CommandResult result = null;
            IList<NexcelService.ShiftModel> shifts = NexcelService.GetShift(null);
            if (shifts != null && shifts.Any())
            {
                var criteria = new InsertAndUpdateShiftNexcelCriteria()
                {
                    DepartmentId = Constants.NexcelDepartment,
                    Shifts = shifts
                };

                result = Repository.Update(criteria);
            }

            return result;
        }

        /// <summary>
        /// Function to update employee calendar of NEXCEL.
        /// </summary>
        /// <returns>Command Result.</returns>
        public CommandResult UpdateEmployeeCalendarForNexcel()
        {
            CommandResult result = null;
            DateTime fromDate = GetDateFromBeforeUpdateEmployeeCalendar(Constants.NexcelDepartment);
            IList<NexcelService.EmployeeCalendarModel> calendars = NexcelService.GetEmployeeCalendar(fromDate);
            if (calendars != null && calendars.Any())
            {
                var criteria = new InsertAndUpdateEmployeeCalendarNexcelCriteria()
                {
                    DepartmentId = Constants.NexcelDepartment,
                    EmployeeCalendars = calendars
                };

                result = Repository.Update(criteria);
            }

            return result;
        }

        /// <summary>
        /// Function to update photo of employee for NEXCEL.
        /// </summary>
        public void UpdatePhotoForNexcel()
        {
            string photoPath = ConfigurationManager.AppSettings["PhotoPathNexcel"];
            DateTime lastModifiedDate = GetLastModifiedDateOfPhoto(photoPath);
            IList<NexcelService.PhotoModel> photos = NexcelService.GetPhoto(lastModifiedDate);

            string fullPath = string.Empty;
            foreach (NexcelService.PhotoModel photo in photos)
            {
                fullPath = photoPath + "\\" + photo.Code;
                File.WriteAllBytes(@fullPath, photo.Photo);
            }
        }

        /// <summary>
        /// Function to update users of Hong Ma.
        /// </summary>
        /// <returns>Command Result.</returns>
        public CommandResult UpdateUserForHM()
        {
            CommandResult result = null;
            DateTime fromDate = GetDateFromBeforeUpdateUser(Constants.HMDepartment);
            IList<HongMaService.UserModel> users = HongmaService.GetUser(fromDate);

            if (users != null && users.Any())
            {
                var insertCriteria = new InsertAndUpdateUserHMCriteria()
                {
                    DepartmentId = Constants.HMDepartment,
                    Users = users
                };

                result = Repository.Update(insertCriteria);
            }

            return result;
        }

        /// <summary>
        /// Function to update teams of Hong Ma.
        /// </summary>
        /// <returns>Command Result.</returns>
        public CommandResult UpdateTeamForHM()
        {
            CommandResult result = null;
            DateTime fromDate = GetDateFromBeforeUpdateTeam(Constants.HMDepartment);
            IList<HongMaService.TeamModel> teams = HongmaService.GetTeam(fromDate);
            if (teams != null && teams.Any())
            {
                var insertCriteria = new InsertAndUpdateTeamHMCriteria()
                {
                    DepartmentId = Constants.HMDepartment,
                    Teams = teams
                };

                result = Repository.Update(insertCriteria);
            }

            return result;
        }

        /// <summary>
        /// Function to update shift of Hong Ma.
        /// </summary>
        /// <returns>Command Result.</returns>
        public CommandResult UpdateShiftForHM()
        {
            CommandResult result = null;
            IList<HongMaService.ShiftModel> shifts = HongmaService.GetShift(null);
            if (shifts != null && shifts.Any())
            {
                var criteria = new InsertAndUpdateShiftHMCriteria()
                {
                    DepartmentId = Constants.HMDepartment,
                    Shifts = shifts
                };

                result = Repository.Update(criteria);
            }

            return result;
        }

        /// <summary>
        /// Function to update employee calendar of Hong Ma.
        /// </summary>
        /// <returns>Command Result.</returns>
        public CommandResult UpdateEmployeeCalendarForHM()
        {
            CommandResult result = null;
            DateTime fromDate = GetDateFromBeforeUpdateEmployeeCalendar(Constants.HMDepartment);
            IList<HongMaService.EmployeeCalendarModel> calendars = HongmaService.GetEmployeeCalendar(fromDate);
            if (calendars != null && calendars.Any())
            {
                var criteria = new InsertAndUpdateEmployeeCalendarHMCriteria()
                {
                    DepartmentId = Constants.HMDepartment,
                    EmployeeCalendars = calendars
                };

                result = Repository.Update(criteria);
            }

            return result;
        }

        /// <summary>
        /// Function to update photo of employee for Hong Ma.
        /// </summary>
        public void UpdatePhotoForHM()
        {
            string photoPath = ConfigurationManager.AppSettings["PhotoPathHongMa"];
            DateTime lastModifiedDate = GetLastModifiedDateOfPhoto(photoPath);
            IList<HongMaService.PhotoModel> photos = HongmaService.GetPhoto(lastModifiedDate);

            string fullPath = string.Empty;
            foreach (HongMaService.PhotoModel photo in photos)
            {
                fullPath = photoPath + "\\" + photo.Code;
                File.WriteAllBytes(@fullPath, photo.Photo);
            }
        }

        /// <summary>
        /// Function to update users of SMS.
        /// </summary>
        /// <returns>Command Result.</returns>
        public CommandResult UpdateUserForSms()
        {
            CommandResult result = null;
            DateTime fromDate = GetDateFromBeforeUpdateUser(Constants.SmsDepartment);
            IList<SMSService.UserModel> users = SmsService.GetUser(fromDate);
            if (users != null && users.Any())
            {
                var insertCriteria = new InsertAndUpdateUserSmsCriteria()
                {
                    DepartmentId = Constants.SmsDepartment,
                    Users = users
                };

                result = Repository.Update(insertCriteria);
            }

            return result;
        }

        /// <summary>
        /// Function to update teams of SMS.
        /// </summary>
        /// <returns>Command Result.</returns>
        public CommandResult UpdateTeamForSms()
        {
            CommandResult result = null;
            DateTime fromDate = GetDateFromBeforeUpdateTeam(Constants.SmsDepartment);
            IList<SMSService.TeamModel> teams = SmsService.GetTeam(fromDate);
            if (teams != null && teams.Any())
            {
                var insertCriteria = new InsertAndUpdateTeamSmsCriteria()
                {
                    DepartmentId = Constants.SmsDepartment,
                    Teams = teams
                };

                result = Repository.Update(insertCriteria);
            }

            return result;
        }

        /// <summary>
        /// Function to update shift of SMS.
        /// </summary>
        /// <returns>Command Result.</returns>
        public CommandResult UpdateShiftForSms()
        {
            CommandResult result = null;
            IList<SMSService.ShiftModel> shifts = SmsService.GetShift(null);
            if (shifts != null && shifts.Any())
            {
                var criteria = new InsertAndUpdateShiftSmsCriteria()
                {
                    DepartmentId = Constants.SmsDepartment,
                    Shifts = shifts
                };

                result = Repository.Update(criteria);
            }

            return result;
        }

        /// <summary>
        /// Function to update employee calendar of SMS.
        /// </summary>
        /// <returns>Command Result.</returns>
        public CommandResult UpdateEmployeeCalendarForSms()
        {
            CommandResult result  = null;
            DateTime fromDate = GetDateFromBeforeUpdateEmployeeCalendar(Constants.SmsDepartment);
            IList<SMSService.EmployeeCalendarModel> calendars = SmsService.GetEmployeeCalendar(fromDate);
            if (calendars != null && calendars.Any())
            {
                var criteria = new InsertAndUpdateEmployeeCalendarSmsCriteria()
                {
                    DepartmentId = Constants.SmsDepartment,
                    EmployeeCalendars = calendars
                };

                result = Repository.Update(criteria);
            }

            return result;
        }

        /// <summary>
        /// Function to update photo of employee for SMS.
        /// </summary>
        public void UpdatePhotoForSms()
        {
            string photoPath = ConfigurationManager.AppSettings["PhotoPathSMS"];
            DateTime lastModifiedDate = GetLastModifiedDateOfPhoto(photoPath);
            IList<SMSService.PhotoModel> photos = SmsService.GetPhoto(lastModifiedDate);

            string fullPath = string.Empty;
            foreach (SMSService.PhotoModel photo in photos)
            {
                fullPath = photoPath + "\\" + photo.Code;
                File.WriteAllBytes(@fullPath, photo.Photo);
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

        /// <summary>
        /// Function to get last modified date of folder contain photos.
        /// </summary>
        /// <param name="path">Folder path.</param>
        /// <returns>Modified date.</returns>
        private DateTime GetLastModifiedDateOfPhoto(string path)
        {
            var lastModifiedDate = new DateTime(2010, 01, 01);
            
            DirectoryInfo directory = new DirectoryInfo(path);
            FileInfo lastModifiedFile = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).FirstOrDefault();
            
            if (lastModifiedFile != null)
            {
                lastModifiedDate = lastModifiedFile.LastWriteTime;
            }

            return lastModifiedDate;
        }
    }
}
