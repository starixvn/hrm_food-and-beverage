namespace NexFB.WindowsService
{
    using Fanex.Data.DNAide;
    using NexFB.Core.Enums;
    using NexFB.Core.Models;
    using NexFB.Core.Services;
    using NexFB.Criteria.MealRegistration;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.WindowsService.Criteria;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Web.UI;

    public class CommonService : BaseService
    {
        private NexcelService.SynchronizeDataServiceClient _nexcelService = null;
        private LuminetService.SynchronizeDataServiceClient _luminetService = null;
        private readonly IMealRegistrationService _mealRegistrationService;

        public CommonService()
        {
            NexcelService = new NexcelService.SynchronizeDataServiceClient();
            LuminetService = new LuminetService.SynchronizeDataServiceClient();
            _mealRegistrationService = new MealRegistrationService();
        }

        protected NexcelService.SynchronizeDataServiceClient NexcelService
        {
            get { return _nexcelService; }
            set { _nexcelService = value; }
        }

        protected LuminetService.SynchronizeDataServiceClient LuminetService
        {
            get { return _luminetService; }
            set { _luminetService = value; }
        }

        public CommandResult SetDefaultVegetarianManagement()
        {
            CommandResult result = null;
            if (!IsHasDataVegetarianManagement())
            {
                var dateApplyAllMeal = "";
                var dateApplySupperMeal = "";
                GetDateApply(ref dateApplyAllMeal, ref dateApplySupperMeal);

                var criteria = new SetDefaultVegetarianManagementAnnualCriteria
                {
                    DateApplyAllMeal = dateApplyAllMeal,
                    DateApplySupperMeal = dateApplySupperMeal
                };

                result = _mealRegistrationService.SetDefaultVegetarianManagementAnnual(criteria);
            }
            return result;
        }

        public CommandResult UpdateBreakRuleOrder()
        {
            var criteria = new UpdateBreakRuleOrderCriteria();
            CommandResult result = Repository.Update(criteria);
            return result;
        }

        private void GetDateApply(ref string dateApplyAllMeal, ref string dateApplySupperMeal)
        {
            List<DateTime> listDateApplyAllMeal = new List<DateTime>();
            List<DateTime> listDateApplySupperMeal = new List<DateTime>();
            var firstDayOfYear = new DateTimeOffset(DateTime.Now.Year, 1, 1, 0, 0, 0, TimeSpan.Zero).DateTime;
            var lastDayOfYear = new DateTimeOffset(DateTime.Now.Year, 12, 31, 0, 0, 0, TimeSpan.Zero).DateTime;
            var tempDate = firstDayOfYear;

            while (tempDate <= lastDayOfYear)
            {
                if (IsVegetarianDate(tempDate))
                {
                    listDateApplyAllMeal.Add(tempDate);
                    listDateApplySupperMeal.Add(tempDate.AddDays(-1));
                }
                tempDate = tempDate.AddDays(1);
            }

            listDateApplySupperMeal.RemoveAll(x => listDateApplyAllMeal.Contains(x));

            dateApplyAllMeal = string.Join(",", listDateApplyAllMeal.Select(date => date.ToString("yyyy/MM/dd")));
            dateApplySupperMeal = string.Join(",", listDateApplySupperMeal.Select(date => date.ToString("yyyy/MM/dd")));
        }

        private bool IsVegetarianDate(DateTime date)
        {
            var solarDate = new Solar
            {
                solarDay = date.Day,
                solarMonth = date.Month,
                solarYear = date.Year
            };
            var lunarDate = LunarSolarConverter.SolarToLunar(solarDate);

            var isVegetarianMonth = lunarDate.lunarMonth == 7;
            var isVegetarianDay = lunarDate.lunarDay == 1 || lunarDate.lunarDay == 15;

            return isVegetarianMonth || isVegetarianDay;
        }

        private bool IsHasDataVegetarianManagement()
        {
            var criteria = new GetVegetarianManagementByRangeDateCriteria
            {
                MealId = null,
                FromDate = new DateTimeOffset(DateTime.Now.Year, 1, 1, 0, 0, 0, TimeSpan.Zero).DateTime,
                ToDate = new DateTimeOffset(DateTime.Now.Year, 12, 31, 0, 0, 0, TimeSpan.Zero).DateTime
            };

            return _mealRegistrationService.GetVegetarianManagementByRangeDate(criteria).Any();
        }

        public CommandResult UpdateUserForNexcel()
        {
            CommandResult result = null;
            var fromDate = GetDateFromBeforeUpdateUser((int)CompanyIdentity.Nexcel);
            IList<NexcelService.UserModel> users = NexcelService.GetUser(fromDate);
            if (users != null && users.Any())
            {
                var insertCriteria = new InsertAndUpdateUserNexcelCriteria
                {
                    CompanyId = (int)CompanyIdentity.Nexcel,
                    Users = users
                };

                result = Repository.Update(insertCriteria);
            }

            return result;
        }

        public CommandResult UpdateTeamForNexcel()
        {
            CommandResult result = null;
            var fromDate = GetDateFromBeforeUpdateTeam((int)CompanyIdentity.Nexcel);
            IList<NexcelService.TeamModel> teams = NexcelService.GetTeam(fromDate);
            if (teams != null && teams.Any())
            {
                var insertCriteria = new InsertAndUpdateTeamNexcelCriteria
                {
                    CompanyId = (int)CompanyIdentity.Nexcel,
                    Teams = teams
                };

                result = Repository.Update(insertCriteria);
            }

            return result;
        }

        public CommandResult UpdateShiftForNexcel()
        {
            CommandResult result = null;
            IList<NexcelService.ShiftModel> shifts = NexcelService.GetShift();
            if (shifts != null && shifts.Any())
            {
                var criteria = new InsertAndUpdateShiftNexcelCriteria
                {
                    CompanyId = (int)CompanyIdentity.Nexcel,
                    Shifts = shifts
                };

                result = Repository.Update(criteria);
            }

            return result;
        }

        public CommandResult UpdateEmployeeCalendarForNexcel()
        {
            CommandResult result = null;
            var fromDate = GetDateFromBeforeUpdateEmployeeCalendar((int)CompanyIdentity.Nexcel);
            IList<NexcelService.EmployeeCalendarModel> calendars = NexcelService.GetEmployeeCalendar(fromDate);
            if (calendars != null && calendars.Any())
            {
                var criteria = new InsertAndUpdateEmployeeCalendarNexcelCriteria
                {
                    CompanyId = (int)CompanyIdentity.Nexcel,
                    EmployeeCalendars = calendars
                };

                result = Repository.Update(criteria);
            }

            return result;
        }

        public void UpdatePhotoForNexcel()
        {
            var photoPath = ConfigurationManager.AppSettings["PhotoPathNexcel"];
            var lastModifiedDate = GetLastModifiedDateOfPhoto(photoPath);
            IList<NexcelService.PhotoModel> photos = NexcelService.GetPhoto(lastModifiedDate);
            foreach (NexcelService.PhotoModel photo in photos)
            {
                var fullPath = photoPath + "\\" + photo.Code;
                File.WriteAllBytes(@fullPath, photo.Photo);
            }
        }

        public DateTime GetDateFromBeforeUpdateUser(int companyId)
        {
            var criteria = new GetLastUserModifiedDateCriteria
            {
                CompanyId = companyId
            };
            return this.GetDataEntity<DateTime>(criteria);
        }

        public DateTime GetDateFromBeforeUpdateTeam(int companyId)
        {
            var criteria = new GetLastTeamModifiedDateCriteria
            {
                CompanyId = companyId
            };
            return this.GetDataEntity<DateTime>(criteria);
        }

        public DateTime GetDateFromBeforeUpdateEmployeeCalendar(int companyId)
        {
            var criteria = new GetLastEmployeeCalendarModifiedDateCriteria
            {
                CompanyId = companyId
            };
            return this.GetDataEntity<DateTime>(criteria);
        }

        public CommandResult UpdateUserForLuminet()
        {
            CommandResult result = null;
            var fromDate = GetDateFromBeforeUpdateUser((int)CompanyIdentity.Luminet);
            IList<LuminetService.UserModel> users = LuminetService.GetUser(fromDate);
            if (users != null && users.Any())
            {
                var insertCriteria = new InsertAndUpdateUserLuminetCriteria
                {
                    CompanyId = (int)CompanyIdentity.Luminet,
                    Users = users
                };

                result = Repository.Update(insertCriteria);
            }

            return result;
        }

        public CommandResult UpdateTeamForLuminet()
        {
            CommandResult result = null;
            var fromDate = GetDateFromBeforeUpdateTeam((int)CompanyIdentity.Luminet);
            IList<LuminetService.TeamModel> teams = LuminetService.GetTeam(fromDate);
            if (teams != null && teams.Any())
            {
                var insertCriteria = new InsertAndUpdateTeamLuminetCriteria
                {
                    CompanyId = (int)CompanyIdentity.Luminet,
                    Teams = teams
                };

                result = Repository.Update(insertCriteria);
            }

            return result;
        }

        public CommandResult UpdateShiftForLuminet()
        {
            CommandResult result = null;
            IList<LuminetService.ShiftModel> shifts = LuminetService.GetShift();
            if (shifts != null && shifts.Any())
            {
                var criteria = new InsertAndUpdateShiftLuminetCriteria
                {
                    CompanyId = (int)CompanyIdentity.Luminet,
                    Shifts = shifts
                };

                result = Repository.Update(criteria);
            }

            return result;
        }

        public CommandResult UpdateEmployeeCalendarForLuminet()
        {
            CommandResult result = null;
            var fromDate = GetDateFromBeforeUpdateEmployeeCalendar((int)CompanyIdentity.Luminet);
            IList<LuminetService.EmployeeCalendarModel> calendars = LuminetService.GetEmployeeCalendar(fromDate);
            if (calendars != null && calendars.Any())
            {
                var criteria = new InsertAndUpdateEmployeeCalendarLuminetCriteria
                {
                    CompanyId = (int)CompanyIdentity.Luminet,
                    EmployeeCalendars = calendars
                };

                result = Repository.Update(criteria);
            }

            return result;
        }

        public void UpdatePhotoForLuminet()
        {
            var photoPath = ConfigurationManager.AppSettings["PhotoPathLuminet"];
            var lastModifiedDate = GetLastModifiedDateOfPhoto(photoPath);
            IList<LuminetService.PhotoModel> photos = LuminetService.GetPhoto(lastModifiedDate);
            foreach (LuminetService.PhotoModel photo in photos)
            {
                var fullPath = photoPath + "\\" + photo.Code;
                File.WriteAllBytes(@fullPath, photo.Photo);
            }
        }

        private static DateTime GetLastModifiedDateOfPhoto(string path)
        {
            var lastModifiedDate = new DateTimeOffset(1990, 01, 01, 0, 0, 0, TimeSpan.Zero).DateTime;

            var directory = new DirectoryInfo(path);
            FileInfo lastModifiedFile = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).FirstOrDefault();

            if (lastModifiedFile != null)
            {
                lastModifiedDate = lastModifiedFile.LastWriteTime;
            }

            return lastModifiedDate;
        }
    }
}