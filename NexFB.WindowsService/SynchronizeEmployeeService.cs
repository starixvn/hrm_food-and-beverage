// --------------------------------------------------------------------------
// <copyright file="SynchronizeEmployeeService.cs" company="Nexcel Solutions Vietnam">
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
    using NexFB.Core.Enums;
    using NexFB.Core.Models;
    using NexFB.Core.Services;
    using NexFB.WindowsService.Criteria;
    using NexFB.WindowsService.SynchronizeDataService;

    public class SynchronizeEmployeeService : BaseService
    {
        private SynchronizeDataServiceClient _synchronizeDataService = null;

        public SynchronizeEmployeeService()
        {
            SynchronizeDataService = new SynchronizeDataServiceClient();
        }

        protected SynchronizeDataServiceClient SynchronizeDataService
        {
            get { return _synchronizeDataService; }
            set { _synchronizeDataService = value; }
        }

        public CommandResult UpdateUser()
        {
            CommandResult result = null;
            DateTime fromDate = GetMaxModifiedDateUser();
            IList<UserModel> users = SynchronizeDataService.GetUser(fromDate);
            if (users != null && users.Any())
            {
                var criteria = new InsertAndUpdateUserCriteria()
                {
                    Users = users
                };

                result = Repository.Update(criteria);
            }

            return result;
        }

        public CommandResult UpdateDepartment()
        {
            CommandResult result = null;
            DateTime fromDate = GetMaxModifiedDateDepartment();
            IList<DepartmentModel> departments = SynchronizeDataService.GetDepartment(fromDate);
            if (departments != null && departments.Any())
            {
                var criteria = new InsertAndUpdateDepartmentCriteria()
                {
                    Departments = departments
                };

                result = Repository.Update(criteria);
            }

            return result;
        }

        public CommandResult UpdateTeam()
        {
            CommandResult result = null;
            DateTime fromDate = GetMaxModifiedDateTeam();
            IList<TeamModel> teams = SynchronizeDataService.GetTeam(fromDate);
            if (teams != null && teams.Any())
            {
                var criteria = new InsertAndUpdateTeamCriteria()
                {
                    Teams = teams
                };

                result = Repository.Update(criteria);
            }

            return result;
        }

        public CommandResult UpdateLeaveType()
        {
            CommandResult result = null;
            IList<LeaveTypeModel> leaveTypes = SynchronizeDataService.GetLeaveType();
            if (leaveTypes != null && leaveTypes.Any())
            {
                var criteria = new InsertAndUpdateLeaveTypeCriteria()
                {
                    LeaveTypes = leaveTypes
                };

                result = Repository.Update(criteria);
            }

            return result;
        }

        public CommandResult UpdateJobTitle()
        {
            CommandResult result = null;
            IList<JobTitleModel> jobTitles = SynchronizeDataService.GetJobTitle();
            if (jobTitles != null && jobTitles.Any())
            {
                var criteria = new InsertAndUpdateJobTitleCriteria()
                {
                    JobTitles = jobTitles
                };

                result = Repository.Update(criteria);
            }

            return result;
        }

        public CommandResult UpdateShift()
        {
            CommandResult result = null;
            IList<ShiftModel> shifts = SynchronizeDataService.GetShift();
            if (shifts != null && shifts.Any())
            {
                var criteria = new InsertAndUpdateShiftCriteria()
                {
                    Shifts = shifts
                };

                result = Repository.Update(criteria);
            }

            return result;
        }

        public CommandResult UpdateEmployeeCalendar()
        {
            CommandResult result  = null;
            DateTime fromDate = GetMaxModifiedDateCalendar();
            IList<EmployeeCalendarModel> calendars = SynchronizeDataService.GetEmployeeCalendar(fromDate);
            if (calendars != null && calendars.Any())
            {
                var criteria = new InsertAndUpdateEmployeeCalendarCriteria()
                {
                    EmployeeCalendars = calendars
                };

                result = Repository.Update(criteria);
            }

            return result;
        }

        public CommandResult UpdateEmployeeRequest()
        {
            CommandResult result = null;
            DateTime fromDate = GetMaxModifiedDateRequest();
            IList<EmployeeRequestModel> requests = SynchronizeDataService.GetEmployeeRequest(fromDate);
            if (requests != null && requests.Any())
            {
                var criteria = new InsertAndUpdateRequestCriteria()
                {
                    EmployeeRequests = requests
                };

                result = Repository.Update(criteria);
            }

            return result;
        }

        public CommandResult UpdateEmployeeRequestHistory()
        {
            CommandResult result = null;
            DateTime fromDate = GetMaxModifiedDateRequestHistory();
            IList<EmployeeRequestHistoryModel> requests = SynchronizeDataService.GetEmployeeRequestHistory(fromDate);
            if (requests != null && requests.Any())
            {
                var criteria = new InsertAndUpdateRequestHistoryCriteria()
                {
                    RequestHistories = requests
                };

                result = Repository.Update(criteria);
            }

            return result;
        }

        public CommandResult UpdateEmployeeAttendance()
        {
            CommandResult result = null;
            DateTime fromDate = GetMaxModifiedDateAttendance();
            IList<EmployeeAttendanceModel> attendances = SynchronizeDataService.GetEmployeeAttendance(fromDate);
            if (attendances != null && attendances.Any())
            {
                var criteria = new InsertAndUpdateAttendanceCriteria()
                {
                    EmployeeAttendances = attendances
                };

                result = Repository.Update(criteria);
            }

            return result;
        }

        public CommandResult UpdateEmployeeAttendanceViolation()
        {
            CommandResult result = null;
            DateTime lastModifiedDate = GetMaxModifiedDateAttendanceViolation();
            DateTime fromDate = GetFromDateAttendanceViolation();
            IList<EmployeeAttendanceViolationModel> violations = SynchronizeDataService.GetEmployeeAttendanceViolation(fromDate, lastModifiedDate);
            if (violations != null && violations.Any())
            {
                var criteria = new InsertAndUpdateAttendanceViolationCriteria()
                {
                    AttendanceViolations = violations
                };

                result = Repository.Update(criteria);
            }

            return result;
        }

        public CommandResult UpdateEmployeeSupervisor()
        {
            CommandResult result = null;
            DateTime fromDate = GetMaxModifiedDateEmployeeSupervisor();
            IList<EmployeeSupervisorModel> supervisors = SynchronizeDataService.GetEmployeeSupervisor(fromDate);
            if (supervisors != null && supervisors.Any())
            {
                var criteria = new InsertAndUpdateEmployeeSupervisorCriteria()
                {
                    Supervisors = supervisors
                };

                result = Repository.Update(criteria);
            }

            return result;
        }

        public CommandResult UpdateGroupSupervisor()
        {
            CommandResult result = null;
            DateTime fromDate = GetMaxModifiedDateGroupSupervisor();
            IList<GroupSupervisorModel> supervisors = SynchronizeDataService.GetGroupSupervisor(fromDate);
            if (supervisors != null && supervisors.Any())
            {
                var criteria = new InsertAndUpdateGroupSupervisorCriteria()
                {
                    Supervisors = supervisors
                };

                result = Repository.Update(criteria);
            }

            return result;
        }

        public void UpdatePhoto()
        {
            string photoPath = ConfigurationManager.AppSettings["PhotoPathSMSEPA"];
            DateTime lastModifiedDate = GetMaxModifiedDatePhoto(photoPath);
            IList<PhotoModel> photos = SynchronizeDataService.GetPhoto(lastModifiedDate);

            string fullPath = string.Empty;
            foreach (PhotoModel photo in photos)
            {
                fullPath = photoPath + "\\" + photo.Code;
                File.WriteAllBytes(@fullPath, photo.Photo);
            }
        }

        public DateTime GetMaxModifiedDateUser()
        {
            var criteria = new GetMaxModifiedDateForUserCriteria();

            DateTime fromDate = this.GetDataEntity<DateTime>(criteria);
            return fromDate;
        }

        public DateTime GetMaxModifiedDateDepartment()
        {
            var criteria = new GetMaxModifiedDateForDepartmentCriteria();

            DateTime fromDate = this.GetDataEntity<DateTime>(criteria);
            return fromDate;
        }

        public DateTime GetMaxModifiedDateTeam()
        {
            var criteria = new GetMaxModifiedDateForTeamCriteria();

            DateTime fromDate = this.GetDataEntity<DateTime>(criteria);
            return fromDate;
        }

        public DateTime GetMaxModifiedDateCalendar()
        {
            var criteria = new GetMaxModifiedDateForCalendarCriteria();

            DateTime fromDate = this.GetDataEntity<DateTime>(criteria);
            return fromDate;
        }

        public DateTime GetMaxModifiedDateRequest()
        {
            var criteria = new GetMaxModifiedDateForRequestCriteria();

            DateTime fromDate = this.GetDataEntity<DateTime>(criteria);
            return fromDate;
        }

        public DateTime GetMaxModifiedDateRequestHistory()
        {
            var criteria = new GetMaxModifiedDateForRequestHistoryCriteria();

            DateTime fromDate = this.GetDataEntity<DateTime>(criteria);
            return fromDate;
        }

        public DateTime GetMaxModifiedDateAttendance()
        {
            var criteria = new GetMaxModifiedDateForAttendanceCriteria();

            DateTime fromDate = this.GetDataEntity<DateTime>(criteria);
            return fromDate;
        }

        public DateTime GetMaxModifiedDateAttendanceViolation()
        {
            var criteria = new GetMaxModifiedDateForAttendanceViolationCriteria();

            DateTime fromDate = this.GetDataEntity<DateTime>(criteria);
            return fromDate;
        }

        public DateTime GetFromDateAttendanceViolation()
        {
            var criteria = new GetFromDateForAttendanceViolationCriteria();

            DateTime fromDate = this.GetDataEntity<DateTime>(criteria);
            return fromDate;
        }

        public DateTime GetMaxModifiedDateEmployeeSupervisor()
        {
            var criteria = new GetMaxModifiedDateForEmployeeSupervisorCriteria();

            DateTime fromDate = this.GetDataEntity<DateTime>(criteria);
            return fromDate;
        }

        public DateTime GetMaxModifiedDateGroupSupervisor()
        {
            var criteria = new GetMaxModifiedDateForGroupSupervisorCriteria();

            DateTime fromDate = this.GetDataEntity<DateTime>(criteria);
            return fromDate;
        }

        private DateTime GetMaxModifiedDatePhoto(string path)
        {
            var lastModifiedDate = new DateTime(1990, 01, 01);
            
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
