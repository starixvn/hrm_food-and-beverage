// --------------------------------------------------------------------------
// <copyright file="ISynchronizeDataService.cs" company="Nexcel Solutions Vietnam">
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
    using System.ServiceModel;
    using NexFB.WebService.Models;

    [ServiceContract]
    public interface ISynchronizeDataService
    {
        [OperationContract]
        IEnumerable<UserModel> GetUser(DateTime? fromDate);

        [OperationContract]
        IEnumerable<TeamModel> GetTeam(DateTime? fromDate);

        [OperationContract]
        IEnumerable<DepartmentModel> GetDepartment(DateTime? fromDate);

        [OperationContract]
        IEnumerable<ShiftModel> GetShift();

        [OperationContract]
        IEnumerable<LeaveTypeModel> GetLeaveType();

        [OperationContract]
        IEnumerable<JobTitleModel> GetJobTitle();

        [OperationContract]
        IEnumerable<EmployeeCalendarModel> GetEmployeeCalendar(DateTime? fromDate);

        [OperationContract]
        IEnumerable<EmployeeRequestModel> GetEmployeeRequest(DateTime? fromDate);

        [OperationContract]
        IEnumerable<EmployeeRequestHistoryModel> GetEmployeeRequestHistory(DateTime? fromDate);

        [OperationContract]
        IEnumerable<EmployeeAttendanceModel> GetEmployeeAttendance(DateTime? fromDate);

        [OperationContract]
        IEnumerable<EmployeeAttendanceViolationModel> GetEmployeeAttendanceViolation(DateTime fromDate, DateTime lastModifiedDate);

        [OperationContract]
        IEnumerable<EmployeeSupervisorModel> GetEmployeeSupervisor(DateTime? fromDate);

        [OperationContract]
        IEnumerable<GroupSupervisorModel> GetGroupSupervisor(DateTime? fromDate);

        [OperationContract]
        IList<PhotoModel> GetPhoto(DateTime? fromDate);

        [OperationContract]
        string GetTextTranslation(string sourceText);
    }
}
