namespace NexFB.Services.Implement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using NexFB.Core.Models;
    using NexFB.Core.Services;
    using NexFB.Criteria.MealRegistration;
    using NexFB.Models.MealRegistration;
    using NexFB.Repositories.Implement;
    using NexFB.Repositories.Interface;
    using NexFB.Services.Interface;

    public class MealRegistrationService : BaseService, IMealRegistrationService
    {
        private readonly IMealRegistrationRepository _mealRegistrationRepository;

        public MealRegistrationService()
            : this(null)
        {
        }

        public MealRegistrationService(IMealRegistrationRepository mealRegistrationRepository)
        {
            _mealRegistrationRepository = mealRegistrationRepository ?? new MealRegistrationRepository();
        }

        public IList<MealRegisterModel> GetRegisterMealByRangeDate(GetRegisterMealByRangeDateCriteria criteria)
        {
            IList<MealRegisterModel> mealDates = new List<MealRegisterModel>();
            if (criteria.IsValid())
            {
                mealDates = _mealRegistrationRepository.GetRegisterMealByRangeDate(criteria);
                CheckDisableDate(mealDates);
            }

            return mealDates;
        }

        public IEnumerable<EmployeeRegisterFood> GetEmployeeRegisterFoodByRangeDate(GetEmployeeRegisterFoodByRangeDateCriteria criteria)
        {
            IEnumerable<EmployeeRegisterFood> listRegisterFood = null;
            if (criteria.IsValid())
            {
                listRegisterFood = GetData<EmployeeRegisterFood>(criteria);
            }

            return listRegisterFood;
        }

        public IEnumerable<VegetarianManagement> GetVegetarianManagementByRangeDate(GetVegetarianManagementByRangeDateCriteria criteria)
        {
            IEnumerable<VegetarianManagement> listResult = null;
            if (criteria.IsValid())
            {
                listResult = GetData<VegetarianManagement>(criteria);
            }

            return listResult;
        }

        public CommandResult RegisterMealByRangeDate(RegisterMealByRangeDateCriteria criteria)
        {
            var commandResult = new CommandResult();
            if (criteria.IsValid())
            {
                commandResult = Repository.Update(criteria);
            }

            return commandResult;
        }

        public CommandResult SetVegetarianByDate(SetVegetarianByDateCriteria criteria)
        {
            var commandResult = new CommandResult();
            if (criteria.IsValid())
            {
                commandResult = Repository.Update(criteria);
            }

            return commandResult;
        }

        public CommandResult SetVegetarianManagementBySingleDate(SetVegetarianManagementBySingleDateCriteria criteria)
        {
            var commandResult = new CommandResult();
            if (criteria.IsValid())
            {
                commandResult = Repository.Update(criteria);
            }

            return commandResult;
        }

        public CommandResult SetDefaultVegetarianManagementAnnual(SetDefaultVegetarianManagementAnnualCriteria criteria)
        {
            var commandResult = new CommandResult();
            if (criteria.IsValid())
            {
                commandResult = Repository.Update(criteria);
            }

            return commandResult;
        }

        public IList<MealDateModel> GetDefaultRegisterMeal(GetRegisterMealByDefaultCriteria criteria)
        {
            IList<MealDateModel> defaultMeals = null;
            if (criteria.IsValid())
            {
                IList<DefaultMealModel> rawDefaultMeals = GetData<DefaultMealModel>(criteria).ToList();
                defaultMeals = GroupMealByDate(rawDefaultMeals);
            }

            return defaultMeals;
        }

        public CommandResult RegisterMealByDefault(SetRegisterMealByDefaultCriteria criteria)
        {
            var commandResult = new CommandResult();
            if (criteria.IsValid())
            {
                commandResult = Repository.Update(criteria);
            }

            return commandResult;
        }

        public CommandResult ApplyRegisterMealFromTemplate(ApplyRegisterMealFromTemplateCriteria criteria)
        {
            var commandResult = new CommandResult();
            if (criteria.IsValid())
            {
                commandResult = Repository.Update(criteria);
            }

            return commandResult;
        }

        public RegisterMealSetting GetRegisterMealSetting(GetRegisterMealSettingCriteria criteria)
        {
            var setting = new RegisterMealSetting();
            if (criteria.IsValid())
            {
                setting = GetDataEntity<RegisterMealSetting>(criteria);
            }

            return setting;
        }

        private void CheckDisableDate(IList<MealRegisterModel> allWeeks)
        {
            foreach (MealRegisterModel week in allWeeks)
            {
                foreach (MealDateModel date in week.WeekMeals)
                {
                    if (date.WorkDate.Date < DateTime.Now.Date)
                    {
                        date.IsDisabled = true;
                    }
                    else
                    {
                        var criteria = new GetRegisterMealSettingCriteria();
                        RegisterMealSetting setting = GetRegisterMealSetting(criteria);

                        CheckDisableMeal(date, setting.Order);
                    }
                }
            }
        }

        private static void CheckDisableMeal(MealDateModel mealDate, int limitDay)
        {
            var today = DateTime.Now;
            var endDay = DateTime.Now.AddDays(limitDay);

            foreach (MealModel meal in mealDate.Meals)
            {
                if (mealDate.WorkDate.Date >= today.Date && mealDate.WorkDate.Date < endDay.Date)
                {
                    meal.IsDisableVegetarian = true;

                    if (mealDate.WorkDate.Date == today.Date)
                    {
                        meal.IsDisableNonRegistration = true;
                        meal.IsDisableNonVegetarian = true;
                    }
                }
            }
        }

        private IList<MealDateModel> GroupMealByDate(IList<DefaultMealModel> rawDefaultMeals)
        {
            var defaultMeals = new List<MealDateModel>();

            for (int i = 0; i < 7; i++)
            {
                int weekDay = i + 1;
                MealDateModel date = GetDateMeal(weekDay, rawDefaultMeals);
                defaultMeals.Add(date);
            }

            return defaultMeals;
        }

        private MealDateModel GetDateMeal(int weekDay, IList<DefaultMealModel> rawDefaultMeals)
        {
            var date = new MealDateModel
            {
                WeekDay = weekDay
            };

            for (int i = 0; i < date.Meals.Count(); i++)
            {
                MealModel meal = date.Meals[i];

                for (int j = 0; j < rawDefaultMeals.Count(); j++)
                {
                    if (meal.MealId == rawDefaultMeals[j].MealId)
                    {
                        switch (weekDay)
                        {
                            case 1:
                                meal.IsVegetarian = ConvertTypeMeal(rawDefaultMeals[j].SunMeal);
                                break;

                            case 2:
                                meal.IsVegetarian = ConvertTypeMeal(rawDefaultMeals[j].MonMeal);
                                break;

                            case 3:
                                meal.IsVegetarian = ConvertTypeMeal(rawDefaultMeals[j].TueMeal);
                                break;

                            case 4:
                                meal.IsVegetarian = ConvertTypeMeal(rawDefaultMeals[j].WedMeal);
                                break;

                            case 5:
                                meal.IsVegetarian = ConvertTypeMeal(rawDefaultMeals[j].ThuMeal);
                                break;

                            case 6:
                                meal.IsVegetarian = ConvertTypeMeal(rawDefaultMeals[j].FriMeal);
                                break;

                            case 7:
                                meal.IsVegetarian = ConvertTypeMeal(rawDefaultMeals[j].SatMeal);
                                break;
                        }
                    }
                }
            }

            return date;
        }

        private static int? ConvertTypeMeal(bool? value)
        {
            if (value.HasValue)
            {
                return value == true ? 1 : 0;
            }

            return null;
        }
    }
}