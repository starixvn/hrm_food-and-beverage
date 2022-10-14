// ----------------------------------------------------------------------
// <copyright file="MealRegistrationService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/4/2014 3:29:02 PM">Created.</change>
// </history>

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

    /// <summary>
    /// The meal registration service.
    /// </summary>
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

        /// <summary>
        /// Get register meal by range date.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list meal in week model.</returns>
        public IList<MealRegisterModel> GetRegisterMealByRangeDate(GetRegisterMealByRangeDateCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IList<MealRegisterModel> mealDates = new List<MealRegisterModel>();

            if (criteria.IsValid())
            {
                mealDates = _mealRegistrationRepository.GetRegisterMealByRangeDate(criteria);
                CheckDisableDate(mealDates);
            }

            return mealDates;
        }

        /// <summary>
        /// Get register meal by range date.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list meal in week model.</returns>
        public IEnumerable<EmployeeRegisterFood> GetEmployeeRegisterFoodByRangeDate(GetEmployeeRegisterFoodByRangeDateCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IEnumerable<EmployeeRegisterFood> listRegisterFood = null;

            if (criteria.IsValid())
            {
                listRegisterFood = GetData<EmployeeRegisterFood>(criteria);
            }

            return listRegisterFood;
        }

        /// <summary>
        /// Get vegetarian management by range date.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list meal in week model.</returns>
        public IEnumerable<VegetarianManagement> GetVegetarianManagementByRangeDate(GetVegetarianManagementByRangeDateCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IEnumerable<VegetarianManagement> listResult = null;

            if (criteria.IsValid())
            {
                listResult = GetData<VegetarianManagement>(criteria);
            }

            return listResult;
        }

        /// <summary>
        /// Register meal by range date.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult RegisterMealByRangeDate(RegisterMealByRangeDateCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult commandResult = new CommandResult();

            if (criteria.IsValid())
            {
                commandResult = Repository.Update(criteria);
            }

            return commandResult;
        }


        /// <summary>
        /// set vegetarian management by range date.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult SetVegetarianManagementBySingleDate(SetVegetarianManagementBySingleDateCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult commandResult = new CommandResult();

            if (criteria.IsValid())
            {
                commandResult = Repository.Update(criteria);
            }

            return commandResult;
        }

        /// <summary>
        /// Get default register meal.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>THe enumerator default meal model.</returns>
        public IList<MealDateModel> GetDefaultRegisterMeal(GetRegisterMealByDefaultCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IList<MealDateModel> defaultMeals = null;

            if (criteria.IsValid())
            {
                IList<DefaultMealModel> rawDefaultMeals = GetData<DefaultMealModel>(criteria).ToList();
                defaultMeals = GroupMealByDate(rawDefaultMeals);
            }

            return defaultMeals;
        }

        /// <summary>
        /// Register meal by default.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult RegisterMealByDefault(SetRegisterMealByDefaultCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult commandResult = new CommandResult();

            if (criteria.IsValid())
            {
                commandResult = Repository.Update(criteria);
            }

            return commandResult;
        }

        /// <summary>
        /// Apply register meal from template.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult ApplyRegisterMealFromTemplate(ApplyRegisterMealFromTemplateCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult commandResult = new CommandResult();

            if (criteria.IsValid())
            {
                commandResult = Repository.Update(criteria);
            }

            return commandResult;
        }

        /// <summary>
        /// Get setting of register meal.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The setting model.</returns>
        public RegisterMealSetting GetRegisterMealSetting(GetRegisterMealSettingCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            RegisterMealSetting setting = new RegisterMealSetting();

            if (criteria.IsValid())
            {
                setting = GetDataEntity<RegisterMealSetting>(criteria);
            }

            return setting;
        }

        /// <summary>
        /// Check date is disable or not.
        /// </summary>
        /// <param name="allWeeks">The list all weeks in month.</param>
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

        /// <summary>
        /// Check disable vegetarian and non-vegetarian.
        /// </summary>
        /// <param name="mealDate">The meal in date.</param>
        /// <param name="limitDay">The limit day.</param>
        private void CheckDisableMeal(MealDateModel mealDate, int limitDay)
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

        /// <summary>
        /// Group meal by date.
        /// </summary>
        /// <param name="rawDefaultMeals">The list meal.</param>
        /// <returns>The list meal date model.</returns>
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

        /// <summary>
        /// Get meal of date.
        /// </summary>
        /// <param name="weekDay">The week day.</param>
        /// <param name="rawDefaultMeals">The list default meals.</param>
        /// <returns>The meal date model.</returns>
        private MealDateModel GetDateMeal(int weekDay, IList<DefaultMealModel> rawDefaultMeals)
        {
            MealDateModel date = new MealDateModel
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

        private int? ConvertTypeMeal(bool? value)
        {
            if (value.HasValue)
            {
                return value == true ? 1 : 0;
            }

            return null;
        }
    }
}
