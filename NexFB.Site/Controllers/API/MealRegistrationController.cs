namespace NexFB.Site.Controllers.API
{
    using NexFB.Core.Constants;
    using NexFB.Core.Enums;
    using NexFB.Core.Models;
    using NexFB.Criteria.MealRegistration;
    using NexFB.Models.Authority;
    using NexFB.Models.MealRegistration;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.Web.Attributes;
    using NexFB.Web.Services.Implement;
    using NexFB.Web.Services.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using System.Xml;

    public class MealRegistrationController : BaseController
    {
        private readonly IMealRegistrationService _mealRegistrationService;
        private readonly IWebCommonService _webCommonService;
        private const string IsVeg = "IsVeg";

        public MealRegistrationController()
            : this(null, null)
        {
        }

        public MealRegistrationController(
            IMealRegistrationService mealRegistrationService,
            IWebCommonService webCommonService)
        {
            _mealRegistrationService = mealRegistrationService ?? new MealRegistrationService();
            _webCommonService = webCommonService ?? new WebCommonService();
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.Registration, Policy = Policy.Read)]
        public IList<MealRegisterModel> GetRegisterMealByRangeDate(
            DateTime fromDate,
            DateTime toDate,
            short? mealId = null)
        {
            var criteria = new GetRegisterMealByRangeDateCriteria
            {
                SystemId = this.CurrentUser.UserId,
                MealId = mealId,
                FromDate = fromDate,
                ToDate = toDate
            };

            return _mealRegistrationService.GetRegisterMealByRangeDate(criteria);
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.Registration, Policy = Policy.Read)]
        public IEnumerable<EmployeeRegisterFood> GetEmployeeRegisterFoodByRangeDate(
            DateTime fromDate,
            DateTime toDate,
            short? mealId = null)
        {
            var criteria = new GetEmployeeRegisterFoodByRangeDateCriteria
            {
                SystemId = this.CurrentUser.UserId,
                MealId = mealId,
                FromDate = fromDate,
                ToDate = toDate
            };

            return _mealRegistrationService.GetEmployeeRegisterFoodByRangeDate(criteria);
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.Registration, Policy = Policy.Read)]
        public IEnumerable<VegetarianManagement> GetVegetarianManagementByRangeDate(
            DateTime fromDate,
            DateTime toDate,
            short? mealId = null)
        {
            var criteria = new GetVegetarianManagementByRangeDateCriteria
            {
                MealId = mealId,
                FromDate = fromDate,
                ToDate = toDate
            };

            return _mealRegistrationService.GetVegetarianManagementByRangeDate(criteria);
        }

        [HttpPost]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Registration, Policy = Policy.Host)]
        public CommandResult RegisterMealByRangeDate(RegisterMealByRangeDateCriteria criteria)
        {
            bool isValidRegister = true;

            bool isValidDate = DateTime.TryParse(criteria.OnDate, out var onDate);
            if (isValidDate)
            {
                var isAllowRegisterVegetarian = GetVegetarianManagementByRangeDate(onDate, onDate).ToList<VegetarianManagement>();
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(criteria.Meal);
                XmlNodeList mealList = xml.GetElementsByTagName("Meal");

                for (int i = 0; i < 4; i++)
                {
                    if (!CheckRegisterMeal(mealList, isAllowRegisterVegetarian, i))
                    {
                        isValidRegister = false;
                    }
                }
            }

            if (isValidRegister)
            {
                criteria.SystemId = this.CurrentUser.UserId;
                criteria.ModifiedBy = this.CurrentUser.UserId;
                criteria.ModifiedDate = DateTime.Now;
            }

            return _mealRegistrationService.RegisterMealByRangeDate(criteria);
        }

        public static bool CheckRegisterMeal(XmlNodeList mealList, List<VegetarianManagement> isAllowRegisterVegetarian, int index)
        {
            if (mealList[index].Attributes?[IsVeg]?.Value == "1")
            {
                if (isAllowRegisterVegetarian.Count == 0)
                {
                    return false;
                }
                else if ((bool)!isAllowRegisterVegetarian[index].IsAllowVegetarian)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return true;
        }

        [HttpPost]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Registration, Policy = Policy.Host)]
        public CommandResult SetVegetarianManagementBySingleDate(SetVegetarianManagementBySingleDateCriteria criteria)
        {
            criteria.ModifiedBy = this.CurrentUser.UserId;
            criteria.ModifiedDate = DateTime.Now;

            return _mealRegistrationService.SetVegetarianManagementBySingleDate(criteria);
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.Registration, Policy = Policy.Read)]
        public IList<MealDateModel> GetDefaultRegisterMeal()
        {
            var criteria = new GetRegisterMealByDefaultCriteria
            {
                SystemId = this.CurrentUser.UserId
            };

            return _mealRegistrationService.GetDefaultRegisterMeal(criteria);
        }

        [HttpPost]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Registration, Policy = Policy.Host)]
        public CommandResult RegisterMealByDefault(SetRegisterMealByDefaultCriteria criteria)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(criteria.Meal);
            XmlNodeList mealList = xml.GetElementsByTagName("Meal");

            bool isVeg = false;
            for (int i = 0; i < 4; i++)
            {
                if (mealList[i].Attributes?[IsVeg]?.Value == "1")
                {
                    isVeg = true;
                    break;
                }
            }

            if (isVeg)
            {
                return _mealRegistrationService.RegisterMealByDefault(criteria);
            }

            if (criteria != null)
            {
                criteria.UserId = this.CurrentUser.UserId;
                criteria.ModifiedBy = this.CurrentUser.UserId;
                criteria.ModifiedDate = DateTime.Now;
            }

            return _mealRegistrationService.RegisterMealByDefault(criteria);
        }

        [HttpPut]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Registration, Policy = Policy.Host)]
        public CommandResult ApplyRegisterMealFromTemplate()
        {
            var criteria = new ApplyRegisterMealFromTemplateCriteria
            {
                UserId = this.CurrentUser.UserId,
                ModifiedBy = this.CurrentUser.UserId,
                ModifiedDate = DateTime.Now
            };

            return _mealRegistrationService.ApplyRegisterMealFromTemplate(criteria);
        }

        public int GetUserPermissionOnRegistration()
        {
            int policy = _webCommonService.GetPermission(FBFunctionKey.Registration);
            return policy;
        }

        public RegisterMealSetting GetRegisterMealSetting()
        {
            var criteria = new GetRegisterMealSettingCriteria();
            RegisterMealSetting setting = _mealRegistrationService.GetRegisterMealSetting(criteria);

            return setting;
        }

        public UserSession GetCurrentUser()
        {
            return this.CurrentUser;
        }
    }
}