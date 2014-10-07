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
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.ServiceModel;
    using Fanex.Data.Configuration;
    using NexFB.Core.Services;
    using NexFB.WebService.Criteria;
    using NexFB.WebService.Models;

    /// <summary>
    /// User Service.
    /// </summary>
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class UserService : BaseService, IUserService
    {
        private string _imagePath;

        public UserService()
        {
            DbSettingProviderManager.Current.Start();
            _imagePath = ConfigurationManager.AppSettings["ImagePath"];
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
                users = GetData<UserModel>(criteria);
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
                teams = GetData<TeamModel>(criteria);
            }

            return teams;
        }

        /// <summary>
        /// Function to get all shifts.
        /// </summary>
        /// <param name="fromDate">The From date.</param>
        /// <returns>List of shifts model.</returns>
        public IEnumerable<ShiftModel> GetShift(DateTime? fromDate)
        {
            IEnumerable<ShiftModel> shifts = null;
            var criteria = new GetShiftCriteria();

            if (criteria.IsValid())
            {
                shifts = GetData<ShiftModel>(criteria);
            }

            return shifts;
        }

        /// <summary>
        /// Function to get teams by last modified date.
        /// </summary>
        /// <param name="fromDate">The From date.</param>
        /// <returns>List of teams model.</returns>
        public IEnumerable<EmployeeCalendarModel> GetEmployeeCalendar(DateTime? fromDate)
        {
            IEnumerable<EmployeeCalendarModel> calendars = null;
            var criteria = new GetEmployeeCalendarCriteria()
            {
                FromDate = fromDate
            };

            if (criteria.IsValid())
            {
                calendars = GetData<EmployeeCalendarModel>(criteria);
            }

            return calendars;
        }

        /// <summary>
        /// Function to get photo by modified date.
        /// </summary>
        /// <param name="fromDate">Modified date.</param>
        /// <returns>List of Photo Model.</returns>
        public IList<PhotoModel> GetPhoto(DateTime? fromDate)
        {
            var photos = new List<PhotoModel>();
            DirectoryInfo directory = new DirectoryInfo(_imagePath);
            DateTime modifiedDate = fromDate.HasValue ? fromDate.Value : new DateTime(2010, 1, 1);
            IEnumerable<FileInfo> files = directory.GetFiles().Where(file => file.LastWriteTime >= modifiedDate 
                && file.Extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase));
            foreach (FileInfo file in files)
            {
                photos.Add(
                    new PhotoModel()
                    { 
                        Code = file.Name,
                        Photo = File.ReadAllBytes(file.FullName)
                    });
            }

            return photos;
        }
    }
}
