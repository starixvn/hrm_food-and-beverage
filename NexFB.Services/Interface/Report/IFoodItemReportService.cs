using System.Collections.Generic;
using NexFB.Models.FoodItemManagement;

namespace NexFB.Services.Interface.Report
{
    public interface IFoodItemReportService
    {
        void ExportFoodItemReport(IList<FoodItemModel> report, string templatePath);
    }
}