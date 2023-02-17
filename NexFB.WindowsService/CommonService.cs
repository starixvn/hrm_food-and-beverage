namespace NexFB.WindowsService
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using NexFB.Core.Enums;
    using NexFB.Core.Models;
    using NexFB.Core.Services;
    using NexFB.WindowsService.Criteria;

    public class CommonService : BaseService
    {
        private NexcelService.SynchronizeDataServiceClient _nexcelService = null;
        private HongMaService.SynchronizeDataServiceClient _hongmaService = null;
        private SMSService.SynchronizeDataServiceClient _smsService = null;
        private LuminetService.SynchronizeDataServiceClient _luminetService = null;

        public CommonService()
        {
            NexcelService = new NexcelService.SynchronizeDataServiceClient();
            HongmaService = new HongMaService.SynchronizeDataServiceClient();
            SmsService = new SMSService.SynchronizeDataServiceClient();
            LuminetService = new LuminetService.SynchronizeDataServiceClient();
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

        public CommandResult UpdateUserForNexcel()
        {
            CommandResult result = null;
            DateTime fromDate = GetDateFromBeforeUpdateUser((int)DepartmentIdentity.Nexcel);
            IList<NexcelService.UserModel> users = NexcelService.GetUser(fromDate);
            if (users != null && users.Any())
            {
                var insertCriteria = new InsertAndUpdateUserNexcelCriteria
                {
                    CompanyId = (int)DepartmentIdentity.Nexcel,
                    Users = users
                };

                result = Repository.Update(insertCriteria);
            }

            return result;
        }

        public CommandResult UpdateTeamForNexcel()
        {
            CommandResult result = null;
            DateTime fromDate = GetDateFromBeforeUpdateTeam((int)DepartmentIdentity.Nexcel);
            IList<NexcelService.TeamModel> teams = NexcelService.GetTeam(fromDate);
            if (teams != null && teams.Any())
            {
                var insertCriteria = new InsertAndUpdateTeamNexcelCriteria
                {
                    CompanyId = (int)DepartmentIdentity.Nexcel,
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
                    CompanyId = (int)DepartmentIdentity.Nexcel,
                    Shifts = shifts
                };

                result = Repository.Update(criteria);
            }

            return result;
        }

        public CommandResult UpdateEmployeeCalendarForNexcel()
        {
            CommandResult result = null;
            DateTime fromDate = GetDateFromBeforeUpdateEmployeeCalendar((int)DepartmentIdentity.Nexcel);
            IList<NexcelService.EmployeeCalendarModel> calendars = NexcelService.GetEmployeeCalendar(fromDate);
            if (calendars != null && calendars.Any())
            {
                var criteria = new InsertAndUpdateEmployeeCalendarNexcelCriteria
                {
                    CompanyId = (int)DepartmentIdentity.Nexcel,
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

            string fullPath = string.Empty;
            foreach (NexcelService.PhotoModel photo in photos)
            {
                fullPath = photoPath + "\\" + photo.Code;
                File.WriteAllBytes(@fullPath, photo.Photo);
            }
        }

        public CommandResult UpdateUserForHM()
        {
            CommandResult result = null;
            DateTime fromDate = GetDateFromBeforeUpdateUser((int)DepartmentIdentity.HongMA);
            IList<HongMaService.UserModel> users = HongmaService.GetUser(fromDate);

            if (users != null && users.Any())
            {
                var insertCriteria = new InsertAndUpdateUserHMCriteria
                {
                    CompanyId = (int)DepartmentIdentity.HongMA,
                    Users = users
                };

                result = Repository.Update(insertCriteria);
            }

            return result;
        }

        public CommandResult UpdateTeamForHM()
        {
            CommandResult result = null;
            DateTime fromDate = GetDateFromBeforeUpdateTeam((int)DepartmentIdentity.HongMA);
            IList<HongMaService.TeamModel> teams = HongmaService.GetTeam(fromDate);
            if (teams != null && teams.Any())
            {
                var insertCriteria = new InsertAndUpdateTeamHMCriteria
                {
                    CompanyId = (int)DepartmentIdentity.HongMA,
                    Teams = teams
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
                    CompanyId = (int)DepartmentIdentity.HongMA,
                    Shifts = shifts
                };

                result = Repository.Update(criteria);
            }

            return result;
        }

        public CommandResult UpdateEmployeeCalendarForHM()
        {
            CommandResult result = null;
            DateTime fromDate = GetDateFromBeforeUpdateEmployeeCalendar((int)DepartmentIdentity.HongMA);
            IList<HongMaService.EmployeeCalendarModel> calendars = HongmaService.GetEmployeeCalendar(fromDate);
            if (calendars != null && calendars.Any())
            {
                var criteria = new InsertAndUpdateEmployeeCalendarHMCriteria
                {
                    CompanyId = (int)DepartmentIdentity.HongMA,
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

            string fullPath = string.Empty;
            foreach (HongMaService.PhotoModel photo in photos)
            {
                fullPath = photoPath + "\\" + photo.Code;
                File.WriteAllBytes(@fullPath, photo.Photo);
            }
        }

        public CommandResult UpdateUserForSms()
        {
            CommandResult result = null;
            DateTime fromDate = GetDateFromBeforeUpdateUser((int)DepartmentIdentity.Sms);
            IList<SMSService.UserModel> users = SmsService.GetUser(fromDate);
            if (users != null && users.Any())
            {
                var insertCriteria = new InsertAndUpdateUserSmsCriteria
                {
                    CompanyId = (int)DepartmentIdentity.Sms,
                    Users = users
                };

                result = Repository.Update(insertCriteria);
            }

            return result;
        }

        public CommandResult UpdateTeamForSms()
        {
            CommandResult result = null;
            DateTime fromDate = GetDateFromBeforeUpdateTeam((int)DepartmentIdentity.Sms);
            IList<SMSService.TeamModel> teams = SmsService.GetTeam(fromDate);
            if (teams != null && teams.Any())
            {
                var insertCriteria = new InsertAndUpdateTeamSmsCriteria
                {
                    CompanyId = (int)DepartmentIdentity.Sms,
                    Teams = teams
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
                    CompanyId = (int)DepartmentIdentity.Sms,
                    Shifts = shifts
                };

                result = Repository.Update(criteria);
            }

            return result;
        }

        public CommandResult UpdateEmployeeCalendarForSms()
        {
            CommandResult result  = null;
            DateTime fromDate = GetDateFromBeforeUpdateEmployeeCalendar((int)DepartmentIdentity.Sms);
            IList<SMSService.EmployeeCalendarModel> calendars = SmsService.GetEmployeeCalendar(fromDate);
            if (calendars != null && calendars.Any())
            {
                var criteria = new InsertAndUpdateEmployeeCalendarSmsCriteria
                {
                    CompanyId = (int)DepartmentIdentity.Sms,
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

            string fullPath = string.Empty;
            foreach (SMSService.PhotoModel photo in photos)
            {
                fullPath = photoPath + "\\" + photo.Code;
                File.WriteAllBytes(@fullPath, photo.Photo);
            }
        }

        public DateTime GetDateFromBeforeUpdateUser(int departmentId)
        {
            var criteria = new GetLastUserModifiedDateCriteria
            {
                CompanyId = departmentId
            };

            DateTime fromDate = this.GetDataEntity<DateTime>(criteria);
            return fromDate;
        }

        public DateTime GetDateFromBeforeUpdateTeam(int departmentId)
        {
            var criteria = new GetLastTeamModifiedDateCriteria()
            {
                CompanyId = departmentId
            };

            DateTime fromDate = this.GetDataEntity<DateTime>(criteria);
            return fromDate;
        }

        public DateTime GetDateFromBeforeUpdateEmployeeCalendar(int departmentId)
        {
            var criteria = new GetLastEmployeeCalendarModifiedDateCriteria
            {
                CompanyId = departmentId
            };

            DateTime fromDate = this.GetDataEntity<DateTime>(criteria);
            return fromDate;
        }

        public CommandResult UpdateUserForLuminet()
        {
            CommandResult result = null;
            DateTime fromDate = GetDateFromBeforeUpdateUser((int)DepartmentIdentity.Luminet);
            IList<LuminetService.UserModel> users = LuminetService.GetUser(fromDate);
            if (users != null && users.Any())
            {
                var insertCriteria = new InsertAndUpdateUserLuminetCriteria
                {
                    CompanyId = (int)DepartmentIdentity.Luminet,
                    Users = users
                };

                result = Repository.Update(insertCriteria);
            }

            return result;
        }

        public CommandResult UpdateTeamForLuminet()
        {
            CommandResult result = null;
            DateTime fromDate = GetDateFromBeforeUpdateTeam((int)DepartmentIdentity.Luminet);
            IList<LuminetService.TeamModel> teams = LuminetService.GetTeam(fromDate);
            if (teams != null && teams.Any())
            {
                var insertCriteria = new InsertAndUpdateTeamLuminetCriteria
                {
                    CompanyId = (int)DepartmentIdentity.Luminet,
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
                    CompanyId = (int)DepartmentIdentity.Luminet,
                    Shifts = shifts
                };

                result = Repository.Update(criteria);
            }

            return result;
        }

        public CommandResult UpdateEmployeeCalendarForLuminet()
        {
            CommandResult result = null;
            DateTime fromDate = GetDateFromBeforeUpdateEmployeeCalendar((int)DepartmentIdentity.Luminet);
            IList<LuminetService.EmployeeCalendarModel> calendars = LuminetService.GetEmployeeCalendar(fromDate);
            if (calendars != null && calendars.Any())
            {
                var criteria = new InsertAndUpdateEmployeeCalendarLuminetCriteria
                {
                    CompanyId = (int)DepartmentIdentity.Luminet,
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

            string fullPath = string.Empty;
            foreach (LuminetService.PhotoModel photo in photos)
            {
                fullPath = photoPath + "\\" + photo.Code;
                File.WriteAllBytes(@fullPath, photo.Photo);
            }
        }

        private DateTime GetLastModifiedDateOfPhoto(string path)
        {
            var lastModifiedDate = new DateTime(1990, 01, 01);
            
            DirectoryInfo directory = new DirectoryInfo(path);
            FileInfo lastModifiedFile = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).FirstOrDefault();
            
            if (lastModifiedFile != null)
            {
                lastModifiedDate = lastModifiedFile.LastWriteTime;
            }

            return lastModifiedDate;
        }
    }
}