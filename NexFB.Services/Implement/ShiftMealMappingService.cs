namespace NexFB.Services.Implement
{
    using System.Collections.Generic;
    using NexFB.Core.Models;
    using NexFB.Core.Services;
    using NexFB.Core.Utilities;
    using NexFB.Criteria.ShiftMealMapping;
    using NexFB.Models.ShiftMealMapping;
    using NexFB.Resources;
    using NexFB.Services.Interface;

    public class ShiftMealMappingService : BaseService, IShiftMealMappingService
    {
        public IEnumerable<ShiftMealMappingModel> GetAllShiftMeal(GetAllShiftMealMappingCriteria criteria)
        {
            IEnumerable<ShiftMealMappingModel> shiftMeals = null;
            if (criteria.IsValid())
            {
                shiftMeals = GetData<ShiftMealMappingModel>(criteria);
            }

            return shiftMeals;
        }

        public void ExportShiftMealMapping(IEnumerable<ShiftMealMappingModel> report, string templatePath)
        {
            string reportTitle = Lang.ShiftMealMappingReport;
            var writer = new ExcelTemplateWriter(templatePath, reportTitle, reportTitle);

            // Create report title.
            writer.CopyRows(0, 1);

            // Create report header.
            writer.CopyRows(1, 2);

            FillContentRow(report, writer);

            writer.SyncColumnsWidth(13);
            writer.Render();
        }

        private void FillContentRow(IEnumerable<ShiftMealMappingModel> report, ExcelTemplateWriter writer)
        {
            int counter = 1;

            foreach (ShiftMealMappingModel item in report)
            {
                var subItem = new
                {
                    No = counter,
                    CompanyName = item.CompanyName,
                    Shift = item.ShiftName,
                    CheckIn = item.BeginCheckInTime,
                    CheckOut = item.BeginCheckOutTime,
                    MealPreviousSupper = item.MealPreviousSupper ? "x" : string.Empty,
                    MealBreakfast = item.MealBreakfast ? "x" : string.Empty,
                    MealLunch = item.MealLunch ? "x" : string.Empty,
                    MealDinner = item.MealDinner ? "x" : string.Empty,
                    MealSupper = item.MealSupper ? "x" : string.Empty,
                    MealNextBreakfast = item.MealNextBreakfast ? "x" : string.Empty,
                    MealNextLunch = item.MealNextLunch ? "x" : string.Empty,
                    MealNextDinner = item.MealNextDinner ? "x" : string.Empty,
                };

                writer.WriteRow(4, subItem);
                counter++;
            }
        }

        public CommandResult UpdateShiftMeal(UpdateShiftMealByShiftIdCriteria criteria)
        {
            var commandResult = new CommandResult();
            if (criteria.IsValid())
            {
                commandResult = Repository.Update(criteria);
            }

            return commandResult;
        }
    }
}