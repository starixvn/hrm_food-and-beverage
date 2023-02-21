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
            const byte HeaderRowIndex = 2;
            writer.CopyRows(1, HeaderRowIndex);

            FillContentRow(report, writer);

            const byte TotalColumns = 13;
            writer.SyncColumnsWidth(TotalColumns);
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
                    item.CompanyName,
                    Shift = item.ShiftName,
                    CheckIn = item.BeginCheckInTime,
                    CheckOut = item.BeginCheckOutTime,
                    MealPreviousSupper = GetCheckbox(item.MealPreviousSupper),
                    MealBreakfast = GetCheckbox(item.MealBreakfast),
                    MealLunch = GetCheckbox(item.MealLunch),
                    MealDinner = GetCheckbox(item.MealDinner),
                    MealSupper = GetCheckbox(item.MealSupper),
                    MealNextBreakfast = GetCheckbox(item.MealNextBreakfast),
                    MealNextLunch = GetCheckbox(item.MealNextLunch),
                    MealNextDinner = GetCheckbox(item.MealNextDinner),
                };

                const byte StartBodyRowIndex = 4;
                writer.WriteRow(StartBodyRowIndex, subItem);
                counter++;
            }
        }

        private static string GetCheckbox(bool isChecked)
        {
            return isChecked ? "x" : string.Empty;
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