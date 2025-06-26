using NexFB.Models.Ordering;

namespace NexFB.Services.Interface
{
    using System.Collections.Generic;
    using NexFB.Core.Models;
    using NexFB.Criteria.ShiftMealMapping;
    using NexFB.Models.ShiftMealMapping;

    /// <summary>
    /// The interface of Shift-Meal mapping service.
    /// </summary>
    public interface IShiftMealMappingService
    {
        IEnumerable<ShiftMealMappingModel> GetAllShiftMeal(GetAllShiftMealMappingCriteria criteria);

        void ExportShiftMealMapping(IEnumerable<ShiftMealMappingModel> report, string templatePath);

        CommandResult UpdateShiftMeal(UpdateShiftMealByShiftIdCriteria criteria);
    }
}
