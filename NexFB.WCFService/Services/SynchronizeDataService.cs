namespace NexFB.WCFService.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using NexFB.WCFService.Criteria;
    using NexFB.WCFService.Models;

    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class SynchronizeDataService(
        IBaseService baseService,
        IFBTranslatorService fbTranslatorService,
        IConfiguration configuration) : ISynchronizeDataService
    {
        public async Task<IEnumerable<UserModel>> GetUser(DateTime? fromDate)
        {
            var criteria = new GetEmployeeCriteria
            {
                FromDate = fromDate
            };
            return await baseService.GetDataAsync<UserModel>(criteria);
        }

        public async Task<IEnumerable<TeamModel>> GetTeam(DateTime? fromDate)
        {
            var criteria = new GetTeamCriteria
            {
                FromDate = fromDate
            };
            return await baseService.GetDataAsync<TeamModel>(criteria);
        }

        public async Task<IEnumerable<SectionModel>> GetSection(DateTime? fromDate)
        {
            var criteria = new GetSectionCriteria
            {
                FromDate = fromDate
            };
            return await baseService.GetDataAsync<SectionModel>(criteria);
        }

        public async Task<IEnumerable<DepartmentModel>> GetDepartment(DateTime? fromDate)
        {
            var criteria = new GetDepartmentCriteria
            {
                FromDate = fromDate
            };
            return await baseService.GetDataAsync<DepartmentModel>(criteria);
        }

        public async Task<IEnumerable<UnitModel>> GetUnit(DateTime? fromDate)
        {
            var criteria = new GetUnitCriteria
            {
                FromDate = fromDate
            };
            return await baseService.GetDataAsync<UnitModel>(criteria);
        }

        public async Task<IEnumerable<ShiftModel>> GetShift()
        {
            var criteria = new GetShiftCriteria();
            return await baseService.GetDataAsync<ShiftModel>(criteria);
        }

        public async Task<IEnumerable<EmployeeCalendarModel>> GetEmployeeCalendar(DateTime? fromDate)
        {
            var criteria = new GetEmployeeCalendarCriteria
            {
                FromDate = fromDate
            };
            return await baseService.GetDataAsync<EmployeeCalendarModel>(criteria);
        }

        public IList<PhotoModel> GetPhoto(DateTime? fromDate)
        {
            var imagePath = configuration["AppSettings:ImagePath"];
            var photos = new List<PhotoModel>();
            var directory = new DirectoryInfo(imagePath);
            var modifiedDate = fromDate ?? new DateTimeOffset(2010, 1, 1, 0, 0, 0, TimeSpan.Zero);
            var files = directory.GetFiles()
                .Where(file => file.LastWriteTime >= modifiedDate && file.Extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase)).ToList();
            foreach (var file in files)
            {
                photos.Add(new PhotoModel
                {
                    Code = file.Name,
                    Photo = File.ReadAllBytes(file.FullName)
                });
            }

            return photos;
        }

        public async Task<string> GetTextTranslation(string sourceText)
        {
            return await fbTranslatorService.AutoTranslation(sourceText);
        }
    }
}
