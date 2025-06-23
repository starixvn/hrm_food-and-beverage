namespace NexFB.Services.Interface
{
    using System.Collections.Generic;
    using NexFB.Core.Models;
    using NexFB.Criteria.MealRegistration;
    using NexFB.Models.Entities;
    using NexFB.Models.MealRegistration;

    /// <summary>
    /// The interface of meal registration service.
    /// </summary>
    public interface IMealRegistrationService
    {
        IList<MealRegisterModel> GetRegisterMealByRangeDate(GetRegisterMealByRangeDateCriteria criteria);
        IEnumerable<EmployeeRegisterFood> GetEmployeeRegisterFoodByRangeDate(GetEmployeeRegisterFoodByRangeDateCriteria criteria);
        IEnumerable<VegetarianManagement> GetVegetarianManagementByRangeDate(GetVegetarianManagementByRangeDateCriteria criteria);

        CommandResult RegisterMealByRangeDate(RegisterMealByRangeDateCriteria criteria);

        CommandResult SetVegetarianByDate(SetVegetarianByDateCriteria criteria);

        CommandResult SetVegetarianManagementBySingleDate(SetVegetarianManagementBySingleDateCriteria criteria);
        CommandResult SetDefaultVegetarianManagementAnnual(SetDefaultVegetarianManagementAnnualCriteria criteria);

        IList<MealDateModel> GetDefaultRegisterMeal(GetRegisterMealByDefaultCriteria criteria);

        CommandResult RegisterMealByDefault(SetRegisterMealByDefaultCriteria criteria);

        CommandResult ApplyRegisterMealFromTemplate(ApplyRegisterMealFromTemplateCriteria criteria);

        RegisterMealSetting GetRegisterMealSetting(GetRegisterMealSettingCriteria criteria);

        CommandResult CancelRegisterMealByDate(CancelRegisterMealByDateCriteria criteria);

        IList<EmployeeRegister> GetEmployeeRegisterByDate(GetEmployeeRegisterMealByDateCriteria criteria);
    }
}
