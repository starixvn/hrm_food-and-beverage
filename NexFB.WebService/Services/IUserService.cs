// --------------------------------------------------------------------------
// <copyright file="IUserService.cs" company="Nexcel Solutions Vietnam">
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
    public interface IUserService
    {
        [OperationContract]
        IEnumerable<UserModel> GetUser(DateTime? fromDate);

        [OperationContract]
        IEnumerable<TeamModel> GetTeam(DateTime? fromDate);

        [OperationContract]
        IEnumerable<ShiftModel> GetShift(DateTime? fromDate);

        [OperationContract]
        IEnumerable<EmployeeCalendarModel> GetEmployeeCalendar(DateTime? fromDate);

        [OperationContract]
        IList<PhotoModel> GetPhoto(DateTime? fromDate);

        [OperationContract]
        string GetTextTranslation(string sourceText);
    }
}
