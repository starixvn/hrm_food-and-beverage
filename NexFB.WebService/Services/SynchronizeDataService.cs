// --------------------------------------------------------------------------
// <copyright file="SynchronizeDataService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2015.8.28">Created</change>
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
    using NexFB.WebService.Translator;

    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class SynchronizeDataService : BaseService, ISynchronizeDataService
    {
        private string _imagePath;

        public SynchronizeDataService()
        {
            DbSettingProviderManager.Current.Start();
            _imagePath = ConfigurationManager.AppSettings["ImagePath"];
        }

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

        public IEnumerable<DepartmentModel> GetDepartment(DateTime? fromDate)
        {
            IEnumerable<DepartmentModel> departments = null;
            var criteria = new GetDepartmentCriteria()
            {
                FromDate = fromDate
            };

            if (criteria.IsValid())
            {
                departments = GetData<DepartmentModel>(criteria);
            }

            return departments;
        }

        public IEnumerable<ShiftModel> GetShift()
        {
            IEnumerable<ShiftModel> shifts = null;
            var criteria = new GetShiftCriteria();

            if (criteria.IsValid())
            {
                shifts = GetData<ShiftModel>(criteria);
            }

            return shifts;
        }

        public IEnumerable<LeaveTypeModel> GetLeaveType()
        {
            IEnumerable<LeaveTypeModel> leaveTypes = null;
            var criteria = new GetLeaveTypeCriteria();

            if (criteria.IsValid())
            {
                leaveTypes = GetData<LeaveTypeModel>(criteria);
            }

            return leaveTypes;
        }

        public IEnumerable<JobTitleModel> GetJobTitle()
        {
            IEnumerable<JobTitleModel> jobTitles = null;
            var criteria = new GetJobTitleCriteria();

            if (criteria.IsValid())
            {
                jobTitles = GetData<JobTitleModel>(criteria);
            }

            return jobTitles;
        }

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

        public IEnumerable<EmployeeRequestModel> GetEmployeeRequest(DateTime? fromDate)
        {
            IEnumerable<EmployeeRequestModel> requests = null;
            var criteria = new GetEmployeeRequestCriteria()
            {
                FromDate = fromDate
            };

            if (criteria.IsValid())
            {
                requests = GetData<EmployeeRequestModel>(criteria);
            }

            return requests;
        }

        public IEnumerable<EmployeeAttendanceModel> GetEmployeeAttendance(DateTime? fromDate)
        {
            IEnumerable<EmployeeAttendanceModel> attendances = null;
            var criteria = new GetEmployeeAttendanceCriteria()
            {
                FromDate = fromDate
            };

            if (criteria.IsValid())
            {
                attendances = GetData<EmployeeAttendanceModel>(criteria);
            }

            return attendances;
        }

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

        public string GetTextTranslation(string sourceText)
        {
            return FBTranslator.AutoTranslation(sourceText);
        }
    }
}
