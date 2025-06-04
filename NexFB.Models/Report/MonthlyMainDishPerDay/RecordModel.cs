namespace NexFB.Models.Report.MonthlyMainDishPerDay
{
    using System.Collections.Generic;

    /// <summary>
    /// The record model.
    /// </summary>
    public class RecordModel
    {
        public IEnumerable<object> Records { get; set; }

        public int RowIndex { get; set; }

        public List<string> DaysOfRedOrder { get; set; }
        public List<string> DaysOfGreenOrder { get; set; }
    }
}
