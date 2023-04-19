namespace NexFB.Site.Controllers.API
{
    using NexFB.Core.Models;
    using NexFB.Criteria.MealRegistration;
    using NexFB.Models.MealRegistration;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Http;
    using System.Xml;

    public class MealRegistrationVegetarianController : ApiController
    {
        private const int numMeals = 4;
        private readonly IMealRegistrationService _mealRegistrationService;

        public MealRegistrationVegetarianController()
             : this(null)
        {
        }

        public MealRegistrationVegetarianController(
                    IMealRegistrationService mealRegistrationService)
        {
            _mealRegistrationService = mealRegistrationService ?? new MealRegistrationService();
        }

        [HttpPost]
        public CommandResult RegisterMealByRangeDate(RegisterMealByRangeDateCriteria criteria)
        {

            bool isValidRegister = true;

            bool isValidDate = DateTime.TryParse(criteria.OnDate, out var onDate);
            if (isValidDate)
            {
                var isAllowRegisterVegetarian = GetMealVegetarianManagementByRangeDate(onDate, onDate).ToList();
                criteria.Meal = CreateXMLMeal(criteria.SystemId);
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(criteria.Meal);
                XmlNodeList mealList = xml.GetElementsByTagName("Meal");

                for (int i = 0; i < numMeals; i++)
                {
                    if (!MealRegistrationController.CheckRegisterMeal(mealList, isAllowRegisterVegetarian, i))
                    {
                        isValidRegister = false;
                    }
                }
            }

            if (isValidRegister)
            {
                criteria.ModifiedBy = criteria.SystemId;
            }

            return _mealRegistrationService.RegisterMealByRangeDate(criteria);
        }

        [HttpPost]
        public string RegistVegetarian(int EmpId, int CompanyId, DateTime OnDate)
        {
            var criteria = new SetVegetarianByDateCriteria
            {
                EmpId = EmpId,
                CompanyId = CompanyId,
                OnDate = OnDate
            };

            _mealRegistrationService.SetVegetarianByDate(criteria);
            return "Ok";
        }

        private IEnumerable<VegetarianManagement> GetMealVegetarianManagementByRangeDate(
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

        private string CreateXMLMeal(int userId)
        {
            var meals = GetEmployeeRegisterFoodByRangeDate(userId);
            const string root = "<ROOT>";
            const string endRoot = "</ROOT>";
            const string mealId = "<Meal MealID=\"";
            const string isVegetarian = " IsVeg=\"1\" ";
            const string endTask = "/>";
            const string tile = "\"";
            StringBuilder mealsTemplate = new StringBuilder();
            mealsTemplate.Append(root);
            for (int i = 0; i < numMeals; i++)
            {
                mealsTemplate.Append(mealId + (i + 1) + tile);
                if (meals[i].IsVegetarian != null)
                {
                    mealsTemplate.Append(isVegetarian);
                }
                mealsTemplate.Append(endTask);
            }
            mealsTemplate.Append(endRoot);
            return mealsTemplate.ToString();
        }

        private IList<EmployeeRegisterFood> GetEmployeeRegisterFoodByRangeDate(int userId)
        {
            var criteria = new GetEmployeeRegisterFoodByRangeDateCriteria
            {
                MealId = null,
                SystemId = userId,
                ToDate = DateTime.Now.Date,
                FromDate = DateTime.Now.Date
            };

            return _mealRegistrationService.GetEmployeeRegisterFoodByRangeDate(criteria).ToList();
        }
    }
}
