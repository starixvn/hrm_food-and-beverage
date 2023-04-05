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
    using System.Web.Http;
    using System.Xml;

    public class MealRegistrationVegetarianController : ApiController
    {
        private readonly IMealRegistrationService _mealRegistrationService;
        private const string IsVeg = "IsVeg";

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
                var isAllowRegisterVegetarian = GetMealVegetarianManagementByRangeDate(onDate, onDate).ToList<VegetarianManagement>();
                criteria.Meal = CreateXMLMeal(criteria.SystemId);
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
                criteria.ModifiedDate = DateTime.Now;
            }

            return _mealRegistrationService.RegisterMealByRangeDate(criteria);
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
            string mealsTemplate = "<ROOT>";
            for (int i = 0; i < 4; i++)
            {
                mealsTemplate += "<Meal MealID=\"" + (i + 1) + "\"";
                if (meals[i].IsVegetarian != null)
                {
                    mealsTemplate += " IsVeg=\"1\" ";
                }
                mealsTemplate += "/>";

            }
            mealsTemplate += "</ROOT>";
            return mealsTemplate;
        }

        private IList<EmployeeRegisterFood> GetEmployeeRegisterFoodByRangeDate(int userId)
        {
            var criteria = new GetEmployeeRegisterFoodByRangeDateCriteria
            {
                SystemId = userId,
                MealId = null,
                FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
            };

            return _mealRegistrationService.GetEmployeeRegisterFoodByRangeDate(criteria).ToList();
        }


        private static bool CheckRegisterMeal(XmlNodeList mealList, List<VegetarianManagement> isAllowRegisterVegetarian, int index)
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
    }
}
