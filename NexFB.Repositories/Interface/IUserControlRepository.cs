// ----------------------------------------------------------------------
// <copyright file="IUserControlRepository.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/28/2014 2:27:20 PM">Created.</change>
// </history>

namespace NexFB.Repositories.Interface
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using NexFB.Models.Employee;

    /// <summary>
    /// The interface of User Control repository.
    /// </summary>
    public interface IUserControlRepository
    {
        IEnumerable<EmployeeModel> GetAllEmployeeRole(ICriteria criteria);
    }
}
