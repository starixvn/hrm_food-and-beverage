namespace NexFB.WCFService.Services
{
    using NexFB.WCFService.Models;
    [ServiceContract]
    public interface ISynchronizeDataService
    {
        [OperationContract]
        Task<IEnumerable<UserModel>> GetUser(DateTime? fromDate);

        [OperationContract]
        Task<IEnumerable<TeamModel>> GetTeam(DateTime? fromDate);

        [OperationContract]
        Task<IEnumerable<SectionModel>> GetSection(DateTime? fromDate);

        [OperationContract]
        Task<IEnumerable<DepartmentModel>> GetDepartment(DateTime? fromDate);

        [OperationContract]
        Task<IEnumerable<UnitModel>> GetUnit(DateTime? fromDate);

        [OperationContract]
        Task<IEnumerable<ShiftModel>> GetShift();

        [OperationContract]
        Task<IEnumerable<EmployeeCalendarModel>> GetEmployeeCalendar(DateTime? fromDate);

        [OperationContract]
        IList<PhotoModel> GetPhoto(DateTime? fromDate);

        [OperationContract]
        string GetTextTranslation(string sourceText);
    }
}
