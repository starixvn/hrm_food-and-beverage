namespace NexFB.Models.Report.SideDishReport
{
    using System.Collections.Generic;

    /// <summary>
    /// The SideDishReportModel class.
    /// </summary>
    public class SideDishReportModel
    {
        public List<SideDishItemModel> SideDishItems { get; set; }

        public decimal TotalQuantity { get; set; }

        public decimal TotalAmount { get; set; }

        public int TotalItem { get; set; }
    }
}
