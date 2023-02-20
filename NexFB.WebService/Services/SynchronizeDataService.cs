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
        private readonly string _imagePath;

        public SynchronizeDataService()
        {
            DbSettingProviderManager.Current.Start();
            _imagePath = ConfigurationManager.AppSettings["ImagePath"];
        }

        public IEnumerable<UserModel> GetUser(DateTime? fromDate)
        {
            var criteria = new GetEmployeeCriteria 
            {
                FromDate = fromDate
            };
            return GetData<UserModel>(criteria);
        }

        public IEnumerable<TeamModel> GetTeam(DateTime? fromDate)
        {
            var criteria = new GetTeamCriteria 
            { 
                FromDate = fromDate
            };
            return GetData<TeamModel>(criteria);
        }

        public IEnumerable<SectionModel> GetSection(DateTime? fromDate)
        {
            var criteria = new GetSectionCriteria
            {
                FromDate = fromDate
            };
            return GetData<SectionModel>(criteria);
        }

        public IEnumerable<DepartmentModel> GetDepartment(DateTime? fromDate)
        {
            var criteria = new GetDepartmentCriteria
            {
                FromDate = fromDate
            };
            return GetData<DepartmentModel>(criteria);
        }

        public IEnumerable<UnitModel> GetUnit(DateTime? fromDate)
        {
            var criteria = new GetUnitCriteria
            {
                FromDate = fromDate
            };
            return GetData<UnitModel>(criteria);
        }

        public IEnumerable<ShiftModel> GetShift()
        {
            var criteria = new GetShiftCriteria();
            return GetData<ShiftModel>(criteria);
        }

        public IEnumerable<EmployeeCalendarModel> GetEmployeeCalendar(DateTime? fromDate)
        {
            var criteria = new GetEmployeeCalendarCriteria
            {
                FromDate = fromDate
            };
            return GetData<EmployeeCalendarModel>(criteria);
        }

        public IList<PhotoModel> GetPhoto(DateTime? fromDate)
        {
            var photos = new List<PhotoModel>();
            var directory = new DirectoryInfo(_imagePath);
            var modifiedDate = fromDate ?? new DateTime(2010, 1, 1);
            IEnumerable<FileInfo> files = directory.GetFiles()
                .Where(file => file.LastWriteTime >= modifiedDate && file.Extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase));
            foreach (FileInfo file in files)
            {
                photos.Add(new PhotoModel
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