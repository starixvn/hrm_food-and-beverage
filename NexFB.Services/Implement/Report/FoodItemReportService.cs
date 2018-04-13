namespace NexFB.Services.Implement.Report
{
    using System.Collections.Generic;
    using Core.Utilities;
    using Interface.Report;
    using Models.FoodItemManagement;

    public class FoodItemReportService : IFoodItemReportService
    {
        public void ExportFoodItemReport(IList<FoodItemModel> report, string templatePath)
        {
            string reportTitle = "Masterlist_fooditem";
            var writer = new ExcelTemplateWriter(templatePath, reportTitle, reportTitle);

            // Create report title.
            writer.CopyRows(0, 2);

            // Create report header.
            writer.CopyRows(2, 1);

            FillContentRow(report, ref writer);

            writer.SyncColumnsWidth(4);
            writer.Render();
        }

        private void FillContentRow(IList<FoodItemModel> report, ref ExcelTemplateWriter writer)
        {
            int counter = 1;
            foreach (FoodItemModel item in report)
            {
                var subItem = new
                {
                    Count = counter,
                    FoodCode = item.FoodCode,
                    FoodNameVietnamese = item.FoodVnmName,
                    FoodNameEnglish = item.FoodEngName
                };

                writer.WriteRow(1, subItem);
                counter++;
            }
        }
    }
}