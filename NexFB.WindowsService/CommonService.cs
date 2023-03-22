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
        private HongMaService.SynchronizeDataServiceClient _hongmaService = null;
        private SMSService.SynchronizeDataServiceClient _smsService = null;
        private LuminetService.SynchronizeDataServiceClient _luminetService = null;
        private readonly IMealRegistrationService _mealRegistrationService;

        public CommonService()
        {
            NexcelService = new NexcelService.SynchronizeDataServiceClient();
            HongmaService = new HongMaService.SynchronizeDataServiceClient();
            SmsService = new SMSService.SynchronizeDataServiceClient();
            LuminetService = new LuminetService.SynchronizeDataServiceClient();
            _mealRegistrationService = new MealRegistrationService();
        }

        protected NexcelService.SynchronizeDataServiceClient NexcelService
        {
            get { return _nexcelService; }
            set { _nexcelService = value; }
        }

        protected HongMaService.SynchronizeDataServiceClient HongmaService
        {
            get { return _hongmaService; }
            set { _hongmaService = value; }
        }

        protected SMSService.SynchronizeDataServiceClient SmsService
        {
            get { return _smsService; }
            set { _smsService = value; }
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
                string dateApplyAllMeal = "";
                string dateApplySupperMeal = "";
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

        private void GetDateApply(ref string dateApplyAllMeal, ref string dateApplySupperMeal)
        {
            List<DateTime> listDateApplyAllMeal = new List<DateTime>();
            List<DateTime> listDateApplySupperMeal = new List<DateTime>();
            DateTime firstDayOfYear = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime lastDayOfYear = new DateTime(DateTime.Now.Year, 12, 31);
            DateTime tempDate = firstDayOfYear;

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

            dateApplyAllMeal = String.Join(",", listDateApplyAllMeal.Select(date => date.ToString("yyyy/MM/dd")));
            dateApplySupperMeal = String.Join(",", listDateApplySupperMeal.Select(date => date.ToString("yyyy/MM/dd")));
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

            bool isVegetarianMonth = lunarDate.lunarMonth == 7;
            bool isVegetarianDay = lunarDate.lunarDay == 1 || lunarDate.lunarDay == 15;

            return isVegetarianMonth || isVegetarianDay;
        }

        private bool IsHasDataVegetarianManagement()
        {
            var criteria = new GetVegetarianManagementByRangeDateCriteria
            {
                MealId = null,
                FromDate = new DateTime(DateTime.Now.Year, 1, 1),
                ToDate = new DateTime(DateTime.Now.Year, 12, 31)
            };

            return _mealRegistrationService.GetVegetarianManagementByRangeDate(criteria).Any();
        }

        public CommandResult UpdateUserForNexcel()
        {
            CommandResult result = null;
            DateTime fromDate = GetDateFromBeforeUpdateUser((int)CompanyIdentity.Nexcel);
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
            DateTime fromDate = GetDateFromBeforeUpdateTeam((int)CompanyIdentity.Nexcel);
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
            DateTime fromDate = GetDateFromBeforeUpdateEmployeeCalendar((int)CompanyIdentity.Nexcel);
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
            string photoPath = ConfigurationManager.AppSettings["PhotoPathNexcel"];
            DateTime lastModifiedDate = GetLastModifiedDateOfPhoto(photoPath);
            IList<NexcelService.PhotoModel> photos = NexcelService.GetPhoto(lastModifiedDate);
            foreach (NexcelService.PhotoModel photo in photos)
            {
                string fullPath = photoPath + "\\" + photo.Code;
                File.WriteAllBytes(@fullPath, photo.Photo);
            }
        }

        public CommandResult UpdateUserForHM()
        {
            CommandResult result = null;
            DateTime fromDate = GetDateFromBeforeUpdateUser((int)CompanyIdentity.HongMA);
            IList<HongMaService.UserModel> users = HongmaService.GetUser(fromDate);

            if (users != null && users.Any())
            {
                var insertCriteria = new InsertAndUpdateUserHMCriteria
                {
                    CompanyId = (int)CompanyIdentity.HongMA,
                    Users = users
                };

                result = Repository.Update(insertCriteria);
            }

            return result;
        }

        public CommandResult UpdateTeamForHM()
        {
            CommandResult result = null;
            DateTime fromDate = GetDateFromBeforeUpdateTeam((int)CompanyIdentity.HongMA);
            IList<HongMaService.TeamModel> teams = HongmaService.GetTeam(fromDate);
            if (teams != null && teams.Any())
            {
                var insertCriteria = new InsertAndUpdateTeamHMCriteria
                {
                    CompanyId = (int)CompanyIdentity.HongMA,
                    Teams = teams
                };

                result = Repository.Update(insertCriteria);
            }

            return result;
        }

        public CommandResult UpdateSectionForHM()
        {
            CommandResult result = null;
            DateTime fromDate = GetDateFromBeforeUpdateSection((int)CompanyIdentity.HongMA);
            IList<HongMaService.SectionModel> sections = HongmaService.GetSection(fromDate);
            if (sections != null && sections.Any())
            {
                var insertCriteria = new InsertAndUpdateSectionHMCriteria
                {
                    CompanyId = (int)CompanyIdentity.HongMA,
                    Sections = sections
                };

                result = Repository.Update(insertCriteria);
            }

            return result;
        }

        public CommandResult UpdateDepartmentForHM()
        {
            CommandResult result = null;
            DateTime fromDate = GetDateFromBeforeUpdateDepartment((int)CompanyIdentity.HongMA);
            IList<HongMaService.DepartmentModel> departments = HongmaService.GetDepartment(fromDate);
            if (departments != null && departments.Any())
            {
                var insertCriteria = new InsertAndUpdateDepartmentHMCriteria
                {
                    CompanyId = (int)CompanyIdentity.HongMA,
                    Departments = departments
                };

                result = Repository.Update(insertCriteria);
            }

            return result;
        }

        public CommandResult UpdateUnitForHM()
        {
            CommandResult result = null;
            DateTime fromDate = GetDateFromBeforeUpdateUnit((int)CompanyIdentity.HongMA);
            IList<HongMaService.UnitModel> units = HongmaService.GetUnit(fromDate);
            if (units != null && units.Any())
            {
                var insertCriteria = new InsertAndUpdateUnitHMCriteria
                {
                    CompanyId = (int)CompanyIdentity.HongMA,
                    Units = units
                };

                result = Repository.Update(insertCriteria);
            }

            return result;
        }

        public CommandResult UpdateShiftForHM()
        {
            CommandResult result = null;
            IList<HongMaService.ShiftModel> shifts = HongmaService.GetShift();
            if (shifts != null && shifts.Any())
            {
                var criteria = new InsertAndUpdateShiftHMCriteria
                {
                    CompanyId = (int)CompanyIdentity.HongMA,
                    Shifts = shifts
                };

                result = Repository.Update(criteria);
            }

            return result;
        }

        public CommandResult UpdateEmployeeCalendarForHM()
        {
            CommandResult result = null;
            DateTime fromDate = GetDateFromBeforeUpdateEmployeeCalendar((int)CompanyIdentity.HongMA);
            IList<HongMaService.EmployeeCalendarModel> calendars = HongmaService.GetEmployeeCalendar(fromDate);
            if (calendars != null && calendars.Any())
            {
                var criteria = new InsertAndUpdateEmployeeCalendarHMCriteria
                {
                    CompanyId = (int)CompanyIdentity.HongMA,
                    EmployeeCalendars = calendars
                };

                result = Repository.Update(criteria);
            }

            return result;
        }

        public void UpdatePhotoForHM()
        {
            string photoPath = ConfigurationManager.AppSettings["PhotoPathHongMa"];
            DateTime lastModifiedDate = GetLastModifiedDateOfPhoto(photoPath);
            IList<HongMaService.PhotoModel> photos = HongmaService.GetPhoto(lastModifiedDate);
            foreach (HongMaService.PhotoModel photo in photos)
            {
                string fullPath = photoPath + "\\" + photo.Code;
                File.WriteAllBytes(@fullPath, photo.Photo);
            }
        }

        public CommandResult UpdateUserForSms()
        {
            CommandResult result = null;
            DateTime fromDate = GetDateFromBeforeUpdateUser((int)CompanyIdentity.Sms);
            IList<SMSService.UserModel> users = SmsService.GetUser(fromDate);
            if (users != null && users.Any())
            {
                var insertCriteria = new InsertAndUpdateUserSmsCriteria
                {
                    CompanyId = (int)CompanyIdentity.Sms,
                    Users = users
                };

                result = Repository.Update(insertCriteria);
            }

            return result;
        }

        public CommandResult UpdateTeamForSms()
        {
            CommandResult result = null;
            DateTime fromDate = GetDateFromBeforeUpdateTeam((int)CompanyIdentity.Sms);
            IList<SMSService.TeamModel> teams = SmsService.GetTeam(fromDate);
            if (teams != null && teams.Any())
            {
                var insertCriteria = new InsertAndUpdateTeamSmsCriteria
                {
                    CompanyId = (int)CompanyIdentity.Sms,
                    Teams = teams
                };

                result = Repository.Update(insertCriteria);
            }

            return result;
        }

        public CommandResult UpdateSectionForSMS()
        {
            CommandResult result = null;
            DateTime fromDate = GetDateFromBeforeUpdateSection((int)CompanyIdentity.Sms);
            IList<SMSService.SectionModel> sections = SmsService.GetSection(fromDate);
            if (sections != null && sections.Any())
            {
                var insertCriteria = new InsertAndUpdateSectionSMSCriteria
                {
                    CompanyId = (int)CompanyIdentity.Sms,
                    Sections = sections
                };

                result = Repository.Update(insertCriteria);
            }

            return result;
        }

        public CommandResult UpdateDepartmentForSMS()
        {
            CommandResult result = null;
            DateTime fromDate = GetDateFromBeforeUpdateDepartment((int)CompanyIdentity.Sms);
            IList<SMSService.DepartmentModel> departments = SmsService.GetDepartment(fromDate);
            if (departments != null && departments.Any())
            {
                var insertCriteria = new InsertAndUpdateDepartmentSMSCriteria
                {
                    CompanyId = (int)CompanyIdentity.Sms,
                    Departments = departments
                };

                result = Repository.Update(insertCriteria);
            }

            return result;
        }

        public CommandResult UpdateUnitForSMS()
        {
            CommandResult result = null;
            DateTime fromDate = GetDateFromBeforeUpdateUnit((int)CompanyIdentity.Sms);
            IList<SMSService.UnitModel> units = SmsService.GetUnit(fromDate);
            if (units != null && units.Any())
            {
                var insertCriteria = new InsertAndUpdateUnitSMSCriteria
                {
                    CompanyId = (int)CompanyIdentity.Sms,
                    Units = units
                };

                result = Repository.Update(insertCriteria);
            }

            return result;
        }

        public CommandResult UpdateShiftForSms()
        {
            CommandResult result = null;
            IList<SMSService.ShiftModel> shifts = SmsService.GetShift();
            if (shifts != null && shifts.Any())
            {
                var criteria = new InsertAndUpdateShiftSmsCriteria
                {
                    CompanyId = (int)CompanyIdentity.Sms,
                    Shifts = shifts
                };

                result = Repository.Update(criteria);
            }

            return result;
        }

        public CommandResult UpdateEmployeeCalendarForSms()
        {
            CommandResult result = null;
            DateTime fromDate = GetDateFromBeforeUpdateEmployeeCalendar((int)CompanyIdentity.Sms);
            IList<SMSService.EmployeeCalendarModel> calendars = SmsService.GetEmployeeCalendar(fromDate);
            if (calendars != null && calendars.Any())
            {
                var criteria = new InsertAndUpdateEmployeeCalendarSmsCriteria
                {
                    CompanyId = (int)CompanyIdentity.Sms,
                    EmployeeCalendars = calendars
                };

                result = Repository.Update(criteria);
            }

            return result;
        }

        public void UpdatePhotoForSms()
        {
            string photoPath = ConfigurationManager.AppSettings["PhotoPathSMS"];
            DateTime lastModifiedDate = GetLastModifiedDateOfPhoto(photoPath);
            IList<SMSService.PhotoModel> photos = SmsService.GetPhoto(lastModifiedDate);
            foreach (SMSService.PhotoModel photo in photos)
            {
                string fullPath = photoPath + "\\" + photo.Code;
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

        public DateTime GetDateFromBeforeUpdateSection(int companyId)
        {
            var criteria = new GetLastSectionModifiedDateCriteria
            {
                CompanyId = companyId
            };
            return this.GetDataEntity<DateTime>(criteria);
        }

        public DateTime GetDateFromBeforeUpdateDepartment(int companyId)
        {
            var criteria = new GetLastDepartmentModifiedDateCriteria
            {
                CompanyId = companyId
            };
            return this.GetDataEntity<DateTime>(criteria);
        }

        public DateTime GetDateFromBeforeUpdateUnit(int companyId)
        {
            var criteria = new GetLastUnitModifiedDateCriteria
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
            DateTime fromDate = GetDateFromBeforeUpdateUser((int)CompanyIdentity.Luminet);
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
            DateTime fromDate = GetDateFromBeforeUpdateTeam((int)CompanyIdentity.Luminet);
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
            DateTime fromDate = GetDateFromBeforeUpdateEmployeeCalendar((int)CompanyIdentity.Luminet);
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
            string photoPath = ConfigurationManager.AppSettings["PhotoPathLuminet"];
            DateTime lastModifiedDate = GetLastModifiedDateOfPhoto(photoPath);
            IList<LuminetService.PhotoModel> photos = LuminetService.GetPhoto(lastModifiedDate);
            foreach (LuminetService.PhotoModel photo in photos)
            {
                string fullPath = photoPath + "\\" + photo.Code;
                File.WriteAllBytes(@fullPath, photo.Photo);
            }
        }

        private static DateTime GetLastModifiedDateOfPhoto(string path)
        {
            var lastModifiedDate = new DateTime(1990, 01, 01);

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