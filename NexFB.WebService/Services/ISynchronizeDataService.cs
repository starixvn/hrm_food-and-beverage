namespace NexFB.WebService.Services
{
    using NexFB.WebService.Models;
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;

    [ServiceContract]
    public interface ISynchronizeDataService
    {
        [OperationContract]
        IEnumerable<UserModel> GetUser(DateTime? fromDate);

        [OperationContract]
        IEnumerable<TeamModel> GetTeam(DateTime? fromDate);

        [OperationContract]
        IEnumerable<SectionModel> GetSection(DateTime? fromDate);

        [OperationContract]
        IEnumerable<DepartmentModel> GetDepartment(DateTime? fromDate);

        [OperationContract]
        IEnumerable<UnitModel> GetUnit(DateTime? fromDate);

        [OperationContract]
        IEnumerable<ShiftModel> GetShift();

        [OperationContract]
        IEnumerable<EmployeeCalendarModel> GetEmployeeCalendar(DateTime? fromDate);

        [OperationContract]
        IList<PhotoModel> GetPhoto(DateTime? fromDate);

        [OperationContract]
        string GetTextTranslation(string sourceText);
    }
}
