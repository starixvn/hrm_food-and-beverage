namespace NexFB.WebService.Services
{
    using Fanex.Data.Configuration;
    using NexFB.Core.Services;
    using NexFB.WebService.Criteria;
    using NexFB.WebService.Models;
    using NexFB.WebService.Translator;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.ServiceModel;

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
