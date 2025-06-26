namespace NexFB.Repositories.Interface
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using NexFB.Models;
    using NexFB.Models.Employee;
    using NexFB.Models.Entities;

    /// <summary>
    /// The interface of User Control repository.
    /// </summary>
    public interface IUserControlRepository
    {
        IEnumerable<EmployeeModel> GetAllEmployeeRole(ICriteria criteria);

        EmployeeModel GetEmployeeRoleById(ICriteria criteria);

        IEnumerable<Company> GetCompanyUserHasPermission(ICriteria criteria);
    }
}
