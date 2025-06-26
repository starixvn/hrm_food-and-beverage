namespace NexFB.Repositories.Interface
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using NexFB.Models.MealRegistration;

    /// <summary>
    /// The interface of Meal Registration repository.
    /// </summary>
    public interface IMealRegistrationRepository
    {
        IList<MealRegisterModel> GetRegisterMealByRangeDate(ICriteria criteria);
    }
}
